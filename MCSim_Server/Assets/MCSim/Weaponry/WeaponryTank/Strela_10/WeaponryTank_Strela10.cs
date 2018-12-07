using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using MilitaryCombatSimulator;

public class WeaponryTank_Strela10 : WeaponryTank, IWeaponryControl
{
    public Strela10_TowerHandler TowerHandler;

    #region Ресурсы

    Hashtable _resources;
    
    public override Hashtable Resources
    {
        get { return _resources; }
    }

    private void LoadResources()
    {
        _resources = new Hashtable();

        _resources.Add("Icon_WeaponryList", "Icon_WL_Strela10"); // Иконка для списка вооружений
    }

    #endregion

    #region Пути к префабам

    // Префаб целой модели
    private const string PrefabHullPath = "WeaponryModel/WeaponryTank/Strela10/Hull/Prefab_strela_10";

    // Префаб кабины оператора
    private const string OperatorCabPath = "WeaponryModel/WeaponryTank/Strela10/Cabin/Strela10_Cabin";

    #endregion
       
    /// <summary>
    /// Кузов
    /// </summary>
    public GameObject Hull;

    /// <summary>
    /// Кабина, к котороый идет привязка
    /// </summary>
    public GameObject Cabin;

    /// <summary>
    /// Башня
    /// </summary>
    public GameObject Tower;

    /// <summary>
    /// Контейнеры
    /// </summary>
    public GameObject Containers;

    public Strela10_Arms Arms;
    public TankTrackController TankTrackController;
    public static bool towerBlocker;
    public override void OnWeaponryInstantiate()
    {
		base.OnWeaponryInstantiate();

        Debug.Log("Размещен " + Name + " [" + ID + "]");
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
    /// Загрузка ядер управления
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

                //Debug.Log("Добавляем ядро [" + name + "]");
                Core.Add(name, coreToolkit);
            }
        }
    }

    public override string PrefabPath {
        get { return PrefabHullPath; }
    }

    public override void Execute(MCSCommand cmd)
    {
        Debug.Log("Пришел Execute: " + cmd.Args[1]);

        switch (cmd.Args[1].ToString()) // 0 аргумент - ID Weaponry
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
                     * 3 - ID захваченного объекта
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
                    Debug.LogError("Не найден Weaponry с ID [" + cmd.Args[2] + "] для наведения башни.");
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
                    Debug.LogError("Не найден Weaponry с ID [" + cmd.Args[2] + "] для наведения башни.");
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
            // Если список владельцев не содерижт этого члена экипажа - выходим
            foreach (NetworkPlayer player in value.Keys) {
             if(!Owners.Contains(player)) { Debug.LogWarning("Список владельцеов данного Weaponry не содержит члена экипажа [" + player + "]"); return;}
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
                // Виртуализируем ядро для приема команд
                Core[roleName].Virtualize();

                // Настраиваем ядро
                CoreToolkit ct = Core[roleName] as CoreToolkit;
                ct.Handler.Weaponry = this; // Настраиваем обработчик событий ядра
                ct.Handler.Core = ct; 

                // Включае обработчик башни
                TowerHandler.enabled = true;
                
                //Debug.Log("Успешно виртуализировали ядро [" + Core[roleName].Name + "] его значение isVurtual=" + Core[roleName].isVirtual);
				var player = MCSGlobalSimulation.Players.List[to];
                player.Weaponry = this;
                player.Role = roleName;
				Owners.Add(player.NetworkPlayer);

                MCSCommand cmd = new MCSCommand(MCSCommandType.Weaponry, "SetRole", false, ID, roleName);

                MCSGlobalSimulation.CommandCenter.Execute(to, cmd);
            }

            if (Network.isClient)
            {
                // Оператор
                if (roleName.Equals(new Strela10_Operator_PanelLibrary().Name))
                {
                    Camera.mainCamera.enabled = false;

                    // Новый корпус
                    GameObject newHull =
                        (GameObject)
                        Instantiate(UnityEngine.Resources.Load(OperatorCabPath), Hull.transform.position,
                                    Hull.transform.rotation);

                    // Отключаем видимость компонентов
                    Hull.GetComponent<MeshRenderer>().enabled = false;
                    foreach (MeshRenderer mr in Cabin.GetComponentsInChildren<MeshRenderer>())
                    {
                        mr.enabled = false;
                    }

                    // Новая кабина
                    GameObject newCab = newHull.transform.FindChild("Cab").gameObject;

                    // Загруажем Core
                    Core["Strela-10_Operator"] = newCab.GetComponent<CoreToolkit>();
                    newHull.transform.parent = gameObject.transform;
                    newCab.transform.parent = Cabin.transform;
                }
            }
        //}
        //catch(Exception e) { Debug.LogError("Ошибка при задании роли: " + e.Message);}
    }


    public bool isOwner
    {
        get { throw new System.NotImplementedException(); }
    }

    public override string Name
    {
        get { return "ЗРК Стрела-10"; }
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
