using System;
using System.Collections;
using System.Collections.Generic;
using MilitaryCombatSimulator;
using UnityEngine;

public class WeaponryTank_Strela10 : WeaponryTank, IWeaponryControl {
    #region Ресурсы

    Hashtable _resources;

    public override Hashtable Resources {
        get { return _resources; }
    }

    private void LoadResources() {
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

    private float _oldAccelerate, _oldSteer, _accelerate, _steer;

    public Strela10_TowerHandler TowerHandler;

    public bool IsTargetInsideLaunchZone;


    /// <summary>
    /// Вызов происходит при готовности системы
    /// </summary>
    void OnSystemReady() {
        Debug.Log("System [" + name + "] ready");

        // Подключаем к башне CoreHandler и запускаем ее
        TowerHandler.Handler = (Strela10_Operator_CoreHandler) (Core["Strela-10_Operator"] as CoreToolkit).Handler;
        TowerHandler.OnSystemReady();
        //Arms.ChargeProjectile();

        // Передаем с клиента на сервер походное или боевое состояние башни
        MCSGlobalSimulation.CommandCenter.Execute(new MCSCommand(MCSCommandType.Weaponry, "Execute", true, ID,
            "ChangeTowerState",
            (int) Strela10_TowerHandler.WorkMode.Marching,
            false
        ));

        TowerHandler.SwitchWorkMode(Strela10_TowerHandler.WorkMode.Marching, false);
    }

    void Start() {
        // Корутина на проверку систему к готовности
        StartCoroutine(CheckSystemReady());
    }

    void FixedUpdate() {
        if (_isOperator) {
            _accelerate = 0f;
            _steer = 0f;

            if (Input.GetKey(KeyCode.W))
                _accelerate = 1;
            if (Input.GetKey(KeyCode.S))
                _accelerate = -1;
            if (Input.GetKey(KeyCode.A))
                _steer = -1;
            if (Input.GetKey(KeyCode.D))
                _steer = 1;
            if (Input.GetKey(KeyCode.Q))
                _steer = 1;
            if (_accelerate != _oldAccelerate || _steer != _oldSteer) {
                _oldAccelerate = _accelerate;
                _oldSteer = _steer;
                networkView.RPC("UpdateOperator", RPCMode.Server, _accelerate, _steer);
            }
        }
    }

    [RPC]
    public void UpdateOperator(float accelerate, float steer) {
    }

    public override void OnWeaponryInstantiate() {
        base.OnWeaponryInstantiate();
        //Debug.Log("Клиентов: " + MCSGlobalSimulation.Weapons.Synchronizations[ID]);
        //Debug.Log("Размещен " + Name + " [" + ID + "]");
    }

    public Strela10_Arms Arms;

    // Use this for initialization
    void Awake() {
        LoadResources();

        TowerRotationSpeed.X = 50f;
        TowerRotationSpeed.Y = 35f;

        TowerLimitAngle.Y.Max = 80;
        TowerLimitAngle.Y.Min = -5;

        if (Network.isServer) {
            Virtualize();
        }
    }

    /// <summary>
    /// Загрузка ядер управления
    /// </summary>
    public void LoadCores() {
        if (gameObject) {
            string name;
            int i = 1;

            foreach (CoreToolkit coreToolkit in gameObject.GetComponentsInChildren<CoreToolkit>()) {
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

    #region IWeaponryControl Members

    private Dictionary<NetworkPlayer, string> _crew = new Dictionary<NetworkPlayer, string>();

    public Dictionary<NetworkPlayer, string> Crew {
        get { return _crew; }
        set {
            // Если список владельцев не содерижт этого члена экипажа - выходим
            foreach (NetworkPlayer player in value.Keys) {
                if (!Owners.Contains(player)) {
                    Debug.LogWarning("Список владельцеов данного Weaponry не содержит члена экипажа [" + player + "]");
                    return;
                }
            }
            _crew = value;
        }
    }

    List<NetworkPlayer> _owners = new List<NetworkPlayer>();

    public List<NetworkPlayer> Owners {
        get { return _owners; }
    }

    public void SetControl(string panelname, string controlname, object value) {
        throw new NotImplementedException();
    }

    public void Virtualize() {
    }

    public void SetRole(NetworkPlayer to, string roleName) {
        //try
        //{
        if (Network.isServer) {
            Core[roleName].Virtualize();
            CoreToolkit ct = Core[roleName] as CoreToolkit;
            ct.Handler.Weaponry = this;
            ct.Handler.Core = ct;

            //Debug.Log("Успешно виртуализировали ядро [" + Core[roleName].Name + "] его значение isVurtual=" + Core[roleName].isVirtual);

            MCSGlobalSimulation.Players.List[to].Weaponry = this;
            MCSCommand cmd = new MCSCommand(MCSCommandType.Weaponry, "SetRole", false, ID, roleName);

            MCSGlobalSimulation.CommandCenter.Execute(to, cmd);
        }

        if (Network.isClient) {
            // Оператор
            if (roleName.Equals("Strela-10_Operator")) {
                try {
                    Camera.mainCamera.GetComponent<AudioListener>().enabled = false;
                    Camera.mainCamera.enabled = false;
                } catch {
                }

                // Новый корпус
                GameObject newHull =
                    (GameObject)
                    Instantiate(UnityEngine.Resources.Load(OperatorCabPath), Hull.transform.position,
                        Hull.transform.rotation);

                // Отключаем видимость компонентов
                Hull.GetComponent<MeshRenderer>().enabled = false;
                foreach (MeshRenderer mr in Cabin.GetComponentsInChildren<MeshRenderer>()) {
                    mr.enabled = false;
                }

                // Новая кабина
                GameObject newCab = newHull.transform.FindChild("Cab").gameObject;

                // Загружаем Core
                Debug.Log("Включаем Handler");
                Core["Strela-10_Operator"] = newCab.GetComponent<CoreToolkit>();
                newCab.GetComponent<CoreToolkit>().Handler.enabled = true;

                newHull.transform.parent = gameObject.transform;
                newCab.transform.parent = Cabin.transform;

                // Устанавливаем связь
                newHull.GetComponent<Strela10_Operator_Node>().Host = this;
                Debug.Log("Прописали хоста с ид " + this.ID);
                newHull.GetComponent<Strela10_Operator_Node>().Initialize();

                UniSkyAPI uniSky = GameObject.Find("UniSkyAPI").GetComponent("UniSkyAPI") as UniSkyAPI;
                //uniSky.mainCamera = ;

                Camera.mainCamera.enabled = false;
                Camera operatorCamera = newHull.GetComponent<Strela10_Operator_Node>().Camera;
                operatorCamera.tag = "MainCamera";
                operatorCamera.enabled = true;
                //operatorCamera.GetComponent<MouseAround>().enabled = false;
                //Debug.Log("camera position " + operatorCamera.transform.position.x + " " + operatorCamera.transform.position.y);
                uniSky.mainCamera = operatorCamera;
                //uniSky.mainCamera.transform.rotation = Quaternion.Euler(-22.59001f, -1.813258e-13f, -4.508065e-20f);
                //foreach (Camera camera in newHull.GetComponentsInChildren<Camera>())
                //{
                //    Debug.Log(">>>>>" + camera.gameObject.name);

                //}

                _crew.Add(Network.player, "Strela-10_Operator");
                _isOwner = true; // Овнер
                _isOperator = true;
            }
        }
        //}
        //catch(Exception e) { Debug.LogError("Ошибка при задании роли: " + e.Message);}
    }

    private bool _isOperator, _isMechanic, isCommander;

    private bool _isOwner;

    public bool isOwner {
        get { return _isOwner; }
    }

    public override string Name {
        //  get { return "ZRK Strela-10"; }
        get { return "ЗРК Стрела-10"; }
    }

    public override WeaponryCategory Category {
        get { return WeaponryCategory.Ground; }
    }


    public NetworkViewID[] InitializeNetworkViews() {
        throw new NotImplementedException();
    }

    public void SetNetworkViews(NetworkViewID[] viewId) {
        throw new NotImplementedException();
    }

    public void AddNetworkViewID(NetworkViewID viewId) {
        throw new NotImplementedException();
    }

    #endregion

    public override void Destroy() {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Проверка системы на готовность к работе
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckSystemReady() {
        int i;
        while (true) {
            i = 0;
            // Проверяем все ли ракеты загружены

            foreach (WeaponryArms_Strela10_Launcher launcher in Arms.Launchers) {
                if (launcher.Projectile) i++;
            }

            // Если подключены все ракеты
            if (i == 4) {
                OnSystemReady();
                break;
            } else yield return new WaitForSeconds(1f);
        }
    }


    public override void Execute(MCSCommand cmd) {
        switch (cmd.Args[1].ToString()) {
            case "ChangeTowerState":
                Strela10_TowerHandler.WorkMode workMode = (Strela10_TowerHandler.WorkMode) cmd.Args[2];
                bool showAnimation = (bool) cmd.Args[3];

                TowerHandler.SwitchWorkMode(workMode, showAnimation);
                break;
        }
    }
}