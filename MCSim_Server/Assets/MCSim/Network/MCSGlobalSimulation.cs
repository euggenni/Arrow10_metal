using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;


/// <summary>
/// ������� �� ���������� Weaponry
/// </summary>
public delegate void OnWeaponryInstantiated(Weaponry weaponry);


public struct SimulationStruct_Players {
    private static Dictionary<NetworkPlayer, MCSPlayer> _list = new Dictionary<NetworkPlayer, MCSPlayer>();

    /// <summary>
    /// ��������� ���������
    /// </summary>
    public Dictionary<NetworkPlayer, MCSPlayer> List {
        get { return _list; }
        set { _list = value; }
    }

    /// <summary>
    /// ���������� MCSPlayer ������� � ��������� ��������
    /// </summary>
    public List<MCSPlayer> WithStatus(MCSPlayer.PlayerStatus status) {
        return (List<MCSPlayer>)(from player in _list.Values
                                 where player.Status == status
                                 select player);
    }
}

public struct SimulationStruct_Weaponry {
    // ������ ���������� Weaponry � �����
    private static int _lastIndex = 0;

    /// <summary>
    /// �� Weaponry � ����� / ������ �� ������
    /// </summary>
    private static Dictionary<int, Weaponry> _list = new Dictionary<int, Weaponry>();

    /// <summary>
    /// ������ ���������� ���������� ������ "����������������" ��� ������� Weaponry
    /// </summary>
    private static Dictionary<int, int> _synchronizations = new Dictionary<int, int>();

    /// <summary>
    /// �������� ID ��� ������ Weaponry
    /// </summary>
    public int AllocateWeaponryID() {
        return _lastIndex++;
    }

    /// <summary>
    /// ������ �������� Weaponry � �����
    /// </summary>
    public Dictionary<int, Weaponry> List {
        get { return _list; }
        set { _list = value; }
    }

    /// <summary>
    /// ������ ���������� ���������� ������ "����������������" ��� ������� Weaponry
    /// </summary>
    public Dictionary<int, int> Synchronizations {
        get { return _synchronizations; }
        set { _synchronizations = value; }
    }

    /// <summary>
    /// ���������� Weaponry ���������� ����
    /// </summary>
    public List<Weaponry> ByType<WeaponryT>() {
        return ByType(typeof(WeaponryT).FullName);
    }

    /// <summary>
    /// ���������� Weaponry ���������� ����
    /// </summary>
    public List<Weaponry> ByType(string weaponryType) {
        return (List<Weaponry>)(from weaponry in _list.Values
                                where weaponry.GetType() == Type.GetType(weaponryType)
                                select weaponry);
    }
}

/// <summary>
/// ����������� ����� ��� ������ � ����������
/// </summary>
public class MCSGlobalSimulation : MonoBehaviour {


    public static event OnWeaponryInstantiated WeaponryInstantiatedEvent;

    ///<summary>
    ///������� �� ���������� Weaponry
    ///</summary>
    public static void OnWeaponryInstantiatedEvent(Weaponry weaponry) {
        OnWeaponryInstantiated handler = WeaponryInstantiatedEvent;
        if (handler != null) handler(weaponry);
    }

    void Awake() {
        MCSGlobalEnvironment.InitializeEnvironment();
    }

    public enum SimulationStatus {
        Stopped,
        Started,
        Paused
    }

    /// <summary>
    /// ������
    /// </summary>
    public static SimulationStruct_Players Players;

    /// <summary>
    /// ����������
    /// </summary>
    public static SimulationStruct_Weaponry Weapons;

    /// <summary>
    /// ��� ������
    /// </summary>
    public static MCSCommandLog Log = new MCSCommandLog();

    /// <summary>
    /// ��������� ���������
    /// </summary>
    public static SimulationStatus Status {
        get { return _status; }
        private set { _status = value; }
    }
    private static SimulationStatus _status = SimulationStatus.Stopped;


    #region ������ �������

    private static float _pauseStartTime = 0f;

    /// <summary>
    /// ����� ������ ���������
    /// </summary>
    public static float SimulationStartedTime // ���������, ������ ����� ����, ���� �� �������� ��������� � ������� �������� ������
    {
        get { return _simulationStartedTime; }
        private set { _simulationStartedTime = value; }
    }
    private static float _simulationStartedTime = -1f;

    /// <summary>
    /// �����, ��������� �� ������ ���������
    /// </summary>
    public static float SimulationLifeTime {
        get {
            if (SimulationStartedTime >= 0) // ����� � �������� ����� ����� � ������� ����� ����� ���� 
            {
                return Time.timeSinceLevelLoad - SimulationStartedTime - PauseAmendment;
            }

            return -1f;
        }
    }


    private static float _summaryPauseTime = 0f;
    /// <summary>
    /// ��������� �����, ����������� �� �����
    /// </summary>
    public static float PauseAmendment {
        get { return _summaryPauseTime; }
    }
    #endregion

    private static MCSCommandCenter _commandCenter;
    public static MCSCommandCenter CommandCenter {
        get {
            if (!_commandCenter) Debug.LogError("����������� CommandCenter.");
            return _commandCenter;
        }
        set { _commandCenter = value; }
    }

    #region ��������������� ��������

    //// �����, ����������� ������ �� ��������� ����, �������� ��� ����, ������ ������� � ������� � ������������ � �����������

    /// <summary>
    /// ��������������� ��� ������������ � ����� GameObject � Weaponry �� ���. ������ �� ����� ���� �������� � ���� ��������, ���� � ���� ���� ID
    /// </summary>
    /// <param name="go">������� ������ �� �������� Weaponry</param>
    public static Weaponry Instantiate(GameObject go) {
        Weaponry weaponry = null;
        if (go != null) {
            if (weaponry = go.GetComponent<Weaponry>()) {
                if (weaponry.ID > 0) {
                    Debug.LogWarning("������� ���������� ���������� Weaponry.");
                    return null;
                }

                weaponry.ID = Weapons.AllocateWeaponryID(); // �������� ����� ID

                Weapons.List.Add(weaponry.ID, weaponry); // ��������� � ������

                NetworkView[] nwlist = weaponry.gameObject.GetComponentsInChildren<NetworkView>();

                // �������� ViewID ������� NetworkView �� �������
                foreach (NetworkView networkView in nwlist) {
                    networkView.viewID = Network.AllocateViewID();
                    networkView.stateSynchronization = NetworkStateSynchronization.Unreliable;
                }

                // ���� ��������� ������ �������
                if (weaponry) {
                    Debug.Log("Instantiating");
                    CommandCenter.Execute(new MCSCommand(MCSCommandType.Simulation, "Instantiate", true, weaponry.GetType().FullName, weaponry.ID));
                    //CommandCenter.Execute(new MCSCommand(MCSCommandType.Simulation, "WeatherPosted", true, 3f));
                }
            } else {
                Debug.LogWarning("��������� ������ �� �������� ������ ������ [Weaponry]");
            }
        } else {
            Debug.LogWarning("���������� ������� ������, �.�. ���������� ������ [null]");
        }

        return weaponry;
    }

    /// <summary>
    /// ���������� �������������� ������� Weaponry � ���� ����������
    /// </summary>
    /// <param name="weaponry"></param>
    public static void Destroy(Weaponry weaponry) {
        CommandCenter.Execute(RPCMode.All, new MCSCommand(MCSCommandType.Simulation, "DestroyWeaponry", true, weaponry.ID));
    }

    /// <summary>
    /// ���������� ���������� Weaponry ��� ���� �������. ���������� ID ������ ������� � ������ ������ ����������� Weaponry
    /// </summary>
    public static Weaponry Instantiate<WeaponryT>() {
        return Instantiate(typeof(WeaponryT).FullName);
    }

    /// <summary>
    /// ���������� ���������� Weaponry ��� ����������� ������. ���������� ID ������ ������� � ������ ������ ����������� Weaponry
    /// </summary>
    public static Weaponry Instantiate<WeaponryT>(NetworkPlayer player) {
        return Instantiate(typeof(WeaponryT).FullName, player);
    }

    /// <summary>
    /// ���������� ���������� Weaponry ��� ���� �������. ���������� ID ������ ������� � ������ ������ ����������� Weaponry
    /// </summary>
    /// <param name="weaponryType">��� ���� Weaponry</param>
    public static Weaponry Instantiate(string weaponryType) {
        Weaponry weaponry = null;

        //try
        //{
        weaponry = MCSGlobalFactory.InstantiateWeaponry(weaponryType).GetComponent<Weaponry>();
        weaponry.ID = Weapons.AllocateWeaponryID(); // �������� ����� ID

        Weapons.List.Add(weaponry.ID, weaponry); // ��������� � ������

        NetworkView[] nwlist = weaponry.gameObject.GetComponentsInChildren<NetworkView>();

        // �������� ViewID ������� NetworkView �� �������
        foreach (NetworkView networkView in nwlist) {
            networkView.viewID = Network.AllocateViewID();
            networkView.stateSynchronization = NetworkStateSynchronization.Unreliable;
        }

        // ���� ��������� ������ �������
        if (weaponry) CommandCenter.Execute(new MCSCommand(MCSCommandType.Simulation, "Instantiate", true, weaponry.GetType().FullName, weaponry.ID));
        //}
        //catch(Exception e)
        //{
        //    Debug.LogWarning("������ ��� �������� ���������� Weaponry: " + e.Message);
        //    throw;
        //    return null;
        //}
        return weaponry;
    }

    /// <summary>
    /// ���������� ���������� Weaponry ��� ����������� ������. ���������� ID ������ ������� � ������ ������ ����������� Weaponry
    /// </summary>
    /// <param name="weaponryType">��� ���� Weaponry</param>
    /// <param name="player">������� �����</param>
    public static Weaponry Instantiate(string weaponryType, NetworkPlayer player) {
        Weaponry weaponry = null;

        try {
            weaponry = MCSGlobalFactory.InstantiateWeaponry(weaponryType).GetComponent<Weaponry>();
            weaponry.ID = Weapons.AllocateWeaponryID(); // �������� ����� ID

            Weapons.List.Add(weaponry.ID, weaponry); // ��������� � ������

            // ���� ��������� ������ �������
            if (weaponry) CommandCenter.Execute(player, new MCSCommand(MCSCommandType.Simulation, "Instantiate", true, weaponry.GetType().FullName, weaponry.ID));
        } catch {
            Debug.LogWarning("������ ��� �������� ���������� Weaponry.");
            return null;
        }

        return weaponry;
    }

    /// <summary>
    /// ���������� �� ������� �������
    /// </summary>
    /// <param name="weaponryID">ID ������������ Weaponry</param>
    /// <param name="weaponryType">��� ������������ Weaponry</param>
    public static Weaponry ClientInstantiate(int weaponryID, string weaponryType) {
        Weaponry weaponry = null;

        try {
            weaponry = MCSGlobalFactory.InstantiateWeaponry(weaponryType).GetComponent<Weaponry>();
            weaponry.ID = weaponryID; // �������� ����� ID

            Weapons.List.Add(weaponry.ID, weaponry); // ��������� � ������

            // ������� Rigidbody �� ������� �������
            Rigidbody rgdb = weaponry.GetComponent<Rigidbody>();
            GameObject.Destroy(rgdb);

            // ��������� � ���������� � ������ ViewID'���
            MCSCommand newCmd = new MCSCommand();
            newCmd.CommandType = MCSCommandType.Simulation;
            newCmd.Command = "WeaponryPosted";

            newCmd.Args = new object[] { weaponryID }; // ID Weaponry

            MCSGlobalSimulation.CommandCenter.Execute(RPCMode.Server, newCmd);
        } catch {
            Debug.LogWarning("������ ��� �������� ���������� Weaponry.");
            return null;
        }
        return weaponry;
    }

    #endregion

    #region ��������������� AI

    /// <summary>
    /// ���������� ���������� Weaponry c AI ��� ���� �������. ���������� true � ������ ������ ����������� Weaponry
    /// </summary>
    /// <param name="weaponryType">��� ���� Weaponry</param>
    public static Weaponry InstantiateAI(string weaponryType) {
        Weaponry weaponry = Instantiate(weaponryType);

        try {
            (weaponry as AIControllable).InitializeAI(); // �������������� AI �� �������
        } catch {
            Debug.LogError("������ ��� �������� AI ��� Weaponry [" + weaponryType + "]. ��� ������� Weaponry �� ��������� ��������� AIControllable.");
            return null;
        }

        return weaponry;
    }

    #endregion

    /// <summary>
    /// ������������� �������. ������ ���������� ������.
    /// </summary>
    public static void InitializeServer() {
        GameObject go = (GameObject)Network.Instantiate(Resources.Load("MCSim/Network/MCSCommandCenter"), Vector3.zero, Quaternion.identity, 0);

        if (Network.isServer) {
            go.name = "MCSCommandCenter(Server)";
        } else {
            go.name = "MCSCommandCenter(Client)";
        }
    }

    /// <summary>
    /// ������ ���������
    /// </summary>
    public static void Start() {
        //Network.
        if (Status == SimulationStatus.Started) return;

        // ���� ������ �� ����� - ������� ��������� ����� ����
        if (Status == SimulationStatus.Paused) {
            _summaryPauseTime += Time.timeSinceLevelLoad - _pauseStartTime;
        } else // ���� ��������� �������
        {
            // ������������� ����� ������ ���������
            Application.LoadLevel(Application.loadedLevel);
            Debug.Log("������������� ����� ������ " + Time.timeSinceLevelLoad);
            SimulationStartedTime = Time.timeSinceLevelLoad;
        }

        Debug.Log("-= ��������� �������� =-");
        Debug.Log("����� ����: " + PauseAmendment);
        Status = SimulationStatus.Started;
    }

    /// <summary>
    /// ����������� ���������
    /// </summary>
    public static void Stop() {
        if (Status == SimulationStatus.Stopped) return;

        Debug.Log("-= ��������� ���������� =-");
        Status = SimulationStatus.Stopped;

        // ������������� ����� ������ ���������
        SimulationStartedTime = Time.timeSinceLevelLoad;
    }

    /// <summary>
    /// ������������ ���������
    /// </summary>
    public static void Pause() {
        if (Status == SimulationStatus.Paused) return;

        // ���������� �����, ����� ��������� �� �����
        _pauseStartTime = Time.timeSinceLevelLoad;


        Debug.Log("-= ��������� �������������� =-");
        Status = SimulationStatus.Paused;
    }
}
