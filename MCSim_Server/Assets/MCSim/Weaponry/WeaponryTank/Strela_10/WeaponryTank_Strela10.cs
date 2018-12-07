using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using MilitaryCombatSimulator;

public class WeaponryTank_Strela10 : WeaponryTank, IWeaponryControl
{
    public Strela10_TowerHandler TowerHandler;

    #region �������

    Hashtable _resources;
    
    public override Hashtable Resources
    {
        get { return _resources; }
    }

    private void LoadResources()
    {
        _resources = new Hashtable();

        _resources.Add("Icon_WeaponryList", "Icon_WL_Strela10"); // ������ ��� ������ ����������
    }

    #endregion

    #region ���� � ��������

    // ������ ����� ������
    private const string PrefabHullPath = "WeaponryModel/WeaponryTank/Strela10/Hull/Prefab_strela_10";

    // ������ ������ ���������
    private const string OperatorCabPath = "WeaponryModel/WeaponryTank/Strela10/Cabin/Strela10_Cabin";

    #endregion
       
    /// <summary>
    /// �����
    /// </summary>
    public GameObject Hull;

    /// <summary>
    /// ������, � �������� ���� ��������
    /// </summary>
    public GameObject Cabin;

    /// <summary>
    /// �����
    /// </summary>
    public GameObject Tower;

    /// <summary>
    /// ����������
    /// </summary>
    public GameObject Containers;

    public Strela10_Arms Arms;
    public TankTrackController TankTrackController;
    public static bool towerBlocker;
    public override void OnWeaponryInstantiate()
    {
		base.OnWeaponryInstantiate();

        Debug.Log("�������� " + Name + " [" + ID + "]");
        Arms.LoadAll<WeaponryRocket_9M37>();
    }


    // Use this for initialization
    void Awake()
    {
        LoadResources();
        LoadCores();

        TowerRotationSpeed.X = 50f;
        TowerRotationSpeed.Y = 35f;

        TowerLimitAngle.Y.Max = 80;
        TowerLimitAngle.Y.Min = -5;

        if (Network.isServer)
        {
            Virtualize();
        }
    }

    void Start()
    {
        
    }

    /// <summary>
    /// �������� ���� ����������
    /// </summary>
    public void LoadCores()
    {
        if (gameObject)
        {
            string name;
            int i = 1;

            foreach (CoreToolkit coreToolkit in gameObject.GetComponentsInChildren<CoreToolkit>())
            {
                name = coreToolkit.Name;

                while (Core.ContainsKey(name)) name = coreToolkit.Name + "_" + i++;

                //Debug.Log("��������� ���� [" + name + "]");
                Core.Add(name, coreToolkit);
            }
        }
    }

    public override string PrefabPath {
        get { return PrefabHullPath; }
    }

    public override void Execute(MCSCommand cmd)
    {
        Debug.Log("������ Execute: " + cmd.Args[1]);

        switch (cmd.Args[1].ToString()) // 0 �������� - ID Weaponry
        {
            case "ChangeTowerState":
                Strela10_TowerHandler.WorkMode workMode = (Strela10_TowerHandler.WorkMode)cmd.Args[2];
                bool showAnimation = (bool) cmd.Args[3];

				Debug.Log("WorkMode: " + workMode + " anim: " + showAnimation);
                TowerHandler.SwitchWorkMode(workMode, showAnimation);
                break;
            
            case "AutoAimTarget": 
                try {

                    /*
                     * 0 - Weaponry ID 
                     * 1 - Command
                     * 2 - Start [true]/Stop [false]
                     * 3 - ID ������������ �������
                    */

                    bool start;
                    start = (bool) cmd.Args[2];

                    Model.StopUCoroutine("autoAimTarget");

                    if (start)
                    {
                        //Tower.GetComponent<NetworkInterpolatedRotation>().StreamStatus = BitStreamStatus.Read;
                        //Containers.GetComponent<NetworkInterpolatedRotation>().StreamStatus = BitStreamStatus.Read; ;
                        Model.UCoroutine(TowerHandler,
                                         TowerHandler.AutoAimTarget(
                                             MCSGlobalSimulation.Weapons.List[(int)cmd.Args[3]].gameObject), "autoAimTarget");
                    }
                    else
                    {
                        //Tower.GetComponent<NetworkInterpolatedRotation>().StreamStatus = BitStreamStatus.Write;
                        //Containers.GetComponent<NetworkInterpolatedRotation>().StreamStatus = BitStreamStatus.Write; ;
                    }
                }
                catch {
                    Debug.LogError("�� ������ Weaponry � ID [" + cmd.Args[2] + "] ��� ��������� �����.");
                }
                break;
            case "ACUBattle":
                Debug.LogWarning("ACUBattle");
                try
                {

                    bool start;

                    var target = MCSGlobalSimulation.Weapons.List.Values.FirstOrDefault(weaponry => weaponry is WeaponryPlane);
                    start = (bool)cmd.Args[2];
                    //Debug.LogWarning((bool)cmd.Args[2]);

                    //Debug.Log(target+"start --"+start);
                    
                    TowerHandler.StopCoroutine("ACUBattle");

                    if (start && target != null)
                    {
                        if (towerBlocker)
                        {
                            //MCSGlobalSimulation.Weapons.List[(int)cmd.Args[3]].gameObject
                            StartCoroutine(TowerHandler.ACUBattle(target.gameObject));
                        }
                        else {
                            //Model.StopUCoroutine(TowerHandler.ACUBattle(target.gameObject));
                            StopAllCoroutines();
                        }
                        
                        //Tower.GetComponent<NetworkInterpolatedRotation>().StreamStatus = BitStreamStatus.Read;
                        //Containers.GetComponent<NetworkInterpolatedRotation>().StreamStatus = BitStreamStatus.Read;
                    }
                    else
                    {
                        
                        //Tower.GetComponent<NetworkInterpolatedRotation>().StreamStatus = BitStreamStatus.Write;
                        //Containers.GetComponent<NetworkInterpolatedRotation>().StreamStatus = BitStreamStatus.Write; ;
                        StopAllCoroutines();
                       // TowerHandler.StopCoroutine("ACUBattle");

                    }
                }
                catch
                {
                    Debug.LogError("�� ������ Weaponry � ID [" + cmd.Args[2] + "] ��� ��������� �����.");
                }
                break;
        }
    }

    #region IWeaponryControl Members

    private Dictionary<NetworkPlayer, string> _crew = new Dictionary<NetworkPlayer, string>();
    public Dictionary<NetworkPlayer, string> Crew
    {
        get { return _crew; }
        set
        {
            // ���� ������ ���������� �� �������� ����� ����� ������� - �������
            foreach (NetworkPlayer player in value.Keys) {
             if(!Owners.Contains(player)) { Debug.LogWarning("������ ����������� ������� Weaponry �� �������� ����� ������� [" + player + "]"); return;}
            }
            _crew = value;
        }
    }

	protected List<NetworkPlayer> _owners = new List<NetworkPlayer>();

    public System.Collections.Generic.List<NetworkPlayer> Owners
    {
        get { return _owners; }
    }

    public void SetControl(string panelname, string controlname, object value)
    {
        throw new System.NotImplementedException();
    }

    public void Virtualize()
    {
        foreach (CoreToolkit core in Core.Values) {
            core.Virtualize();
        }
    }

    public void SetRole(NetworkPlayer to, string roleName)
    {
        //try
        //{
            if(Network.isServer)
            {
                // �������������� ���� ��� ������ ������
                Core[roleName].Virtualize();

                // ����������� ����
                CoreToolkit ct = Core[roleName] as CoreToolkit;
                ct.Handler.Weaponry = this; // ����������� ���������� ������� ����
                ct.Handler.Core = ct; 

                // ������� ���������� �����
                TowerHandler.enabled = true;
                
                //Debug.Log("������� ���������������� ���� [" + Core[roleName].Name + "] ��� �������� isVurtual=" + Core[roleName].isVirtual);
				var player = MCSGlobalSimulation.Players.List[to];
                player.Weaponry = this;
                player.Role = roleName;
				Owners.Add(player.NetworkPlayer);

                MCSCommand cmd = new MCSCommand(MCSCommandType.Weaponry, "SetRole", false, ID, roleName);

                MCSGlobalSimulation.CommandCenter.Execute(to, cmd);
            }

            if (Network.isClient)
            {
                // ��������
                if (roleName.Equals(new Strela10_Operator_PanelLibrary().Name))
                {
                    Camera.mainCamera.enabled = false;

                    // ����� ������
                    GameObject newHull =
                        (GameObject)
                        Instantiate(UnityEngine.Resources.Load(OperatorCabPath), Hull.transform.position,
                                    Hull.transform.rotation);

                    // ��������� ��������� �����������
                    Hull.GetComponent<MeshRenderer>().enabled = false;
                    foreach (MeshRenderer mr in Cabin.GetComponentsInChildren<MeshRenderer>())
                    {
                        mr.enabled = false;
                    }

                    // ����� ������
                    GameObject newCab = newHull.transform.FindChild("Cab").gameObject;

                    // ��������� Core
                    Core["Strela-10_Operator"] = newCab.GetComponent<CoreToolkit>();
                    newHull.transform.parent = gameObject.transform;
                    newCab.transform.parent = Cabin.transform;
                }
            }
        //}
        //catch(Exception e) { Debug.LogError("������ ��� ������� ����: " + e.Message);}
    }


    public bool isOwner
    {
        get { throw new System.NotImplementedException(); }
    }

    public override string Name
    {
        get { return "��� ������-10"; }
    }

    public override WeaponryCategory Category
    {
        get { return WeaponryCategory.Ground; }
    }


    public NetworkViewID[] InitializeNetworkViews()
    {
        throw new System.NotImplementedException();
    }

    public void SetNetworkViews(NetworkViewID[] viewId)
    {
        throw new System.NotImplementedException();
    }

    #endregion

    #region IWeaponryControl Members


    public void AddNetworkViewID(NetworkViewID viewId)
    {
        throw new System.NotImplementedException();
    }

    #endregion

    public override void Destroy()
    {
        throw new NotImplementedException();
    }
    [RPC]
    public void UpdateOperator(float accelerate, float steer)
    {
        TankTrackController.accelerate = accelerate;
        TankTrackController.steer = steer;
    }
}
