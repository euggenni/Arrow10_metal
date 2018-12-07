using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;


/// <summary>
/// Событие на размещение Weaponry
/// </summary>
public delegate void OnWeaponryInstantiated(Weaponry weaponry);


public struct SimulationStruct_Players {
    private static Dictionary<NetworkPlayer, MCSPlayer> _list = new Dictionary<NetworkPlayer, MCSPlayer>();

    /// <summary>
    /// Участники симуляции
    /// </summary>
    public Dictionary<NetworkPlayer, MCSPlayer> List {
        get { return _list; }
        set { _list = value; }
    }

    /// <summary>
    /// Возвращает MCSPlayer игроков с указанным статусом
    /// </summary>
    public List<MCSPlayer> WithStatus(MCSPlayer.PlayerStatus status) {
        return (List<MCSPlayer>)(from player in _list.Values
                                 where player.Status == status
                                 select player);
    }
}

public struct SimulationStruct_Weaponry {
    // Индекс последнего Weaponry в сцене
    private static int _lastIndex = 0;

    /// <summary>
    /// ИД Weaponry в сцене / Ссылка на скрипт
    /// </summary>
    private static Dictionary<int, Weaponry> _list = new Dictionary<int, Weaponry>();

    /// <summary>
    /// Список количества присланных команд "синхронизировано" для каждого Weaponry
    /// </summary>
    private static Dictionary<int, int> _synchronizations = new Dictionary<int, int>();

    /// <summary>
    /// Выделить ID для нового Weaponry
    /// </summary>
    public int AllocateWeaponryID() {
        return _lastIndex++;
    }

    /// <summary>
    /// Список объектов Weaponry в сцене
    /// </summary>
    public Dictionary<int, Weaponry> List {
        get { return _list; }
        set { _list = value; }
    }

    /// <summary>
    /// Список количества присланных команд "синхронизировано" для каждого Weaponry
    /// </summary>
    public Dictionary<int, int> Synchronizations {
        get { return _synchronizations; }
        set { _synchronizations = value; }
    }

    /// <summary>
    /// Возвращает Weaponry указанного типа
    /// </summary>
    public List<Weaponry> ByType<WeaponryT>() {
        return ByType(typeof(WeaponryT).FullName);
    }

    /// <summary>
    /// Возвращает Weaponry указанного типа
    /// </summary>
    public List<Weaponry> ByType(string weaponryType) {
        return (List<Weaponry>)(from weaponry in _list.Values
                                where weaponry.GetType() == Type.GetType(weaponryType)
                                select weaponry);
    }
}

/// <summary>
/// Статический класс для работы с симуляцией
/// </summary>
public class MCSGlobalSimulation : MonoBehaviour {


    public static event OnWeaponryInstantiated WeaponryInstantiatedEvent;

    ///<summary>
    ///Событие на размещение Weaponry
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
    /// Игроки
    /// </summary>
    public static SimulationStruct_Players Players;

    /// <summary>
    /// Вооружения
    /// </summary>
    public static SimulationStruct_Weaponry Weapons;

    /// <summary>
    /// Лог команд
    /// </summary>
    public static MCSCommandLog Log = new MCSCommandLog();

    /// <summary>
    /// Состояние симуляции
    /// </summary>
    public static SimulationStatus Status {
        get { return _status; }
        private set { _status = value; }
    }
    private static SimulationStatus _status = SimulationStatus.Stopped;


    #region Отсчет времени

    private static float _pauseStartTime = 0f;

    /// <summary>
    /// Время старта симуляции
    /// </summary>
    public static float SimulationStartedTime // Впринципе, всегда равен нулю, если мы начинаем симуляцию с момента загрузки уровня
    {
        get { return _simulationStartedTime; }
        private set { _simulationStartedTime = value; }
    }
    private static float _simulationStartedTime = -1f;

    /// <summary>
    /// Время, прошедшее со старта симуляции
    /// </summary>
    public static float SimulationLifeTime {
        get {
            if (SimulationStartedTime >= 0) // Время с загрузки минус время с запуска минус время пауз 
            {
                return Time.timeSinceLevelLoad - SimulationStartedTime - PauseAmendment;
            }

            return -1f;
        }
    }


    private static float _summaryPauseTime = 0f;
    /// <summary>
    /// Суммарное время, затраченное на паузы
    /// </summary>
    public static float PauseAmendment {
        get { return _summaryPauseTime; }
    }
    #endregion

    private static MCSCommandCenter _commandCenter;
    public static MCSCommandCenter CommandCenter {
        get {
            if (!_commandCenter) Debug.LogError("Отсутствует CommandCenter.");
            return _commandCenter;
        }
        set { _commandCenter = value; }
    }

    #region Инстанцирование объектов

    //// МЕТОД, ПРИНИМАЮЩИЙ ШАБЛОН ИЗ РЕДАКТОРА КАРТ, ПОЛУЧАЕТ ЕГО АЙДИ, СПИСОК ИГРОКОВ И СОЗДАЕТ В СООТВЕТСТВИИ С ПАРАМЕТРАМИ

    /// <summary>
    /// Инстанцирование уже находящегося в сцене GameObject с Weaponry на нем. Объект не может быть размещен в сети повторно, если у него есть ID
    /// </summary>
    /// <param name="go">Игровой объект со скриптом Weaponry</param>
    public static Weaponry Instantiate(GameObject go) {
        Weaponry weaponry = null;
        if (go != null) {
            if (weaponry = go.GetComponent<Weaponry>()) {
                if (weaponry.ID > 0) {
                    Debug.LogWarning("Попытка повторного размещения Weaponry.");
                    return null;
                }

                weaponry.ID = Weapons.AllocateWeaponryID(); // Выделяем новый ID

                Weapons.List.Add(weaponry.ID, weaponry); // Добавляем в список

                NetworkView[] nwlist = weaponry.gameObject.GetComponentsInChildren<NetworkView>();

                // Выделяем ViewID каждому NetworkView на объекте
                foreach (NetworkView networkView in nwlist) {
                    networkView.viewID = Network.AllocateViewID();
                    networkView.stateSynchronization = NetworkStateSynchronization.Unreliable;
                }

                // Если экземпляр создан успешно
                if (weaponry) {
                    Debug.Log("Instantiating");
                    CommandCenter.Execute(new MCSCommand(MCSCommandType.Simulation, "Instantiate", true, weaponry.GetType().FullName, weaponry.ID));
                    //CommandCenter.Execute(new MCSCommand(MCSCommandType.Simulation, "WeatherPosted", true, 3f));
                }
            } else {
                Debug.LogWarning("Указанный объект не содержит объект класса [Weaponry]");
            }
        } else {
            Debug.LogWarning("Невозможно создать объект, т.к. переданный объект [null]");
        }

        return weaponry;
    }

    /// <summary>
    /// Уничтожить представленный образец Weaponry у всех участников
    /// </summary>
    /// <param name="weaponry"></param>
    public static void Destroy(Weaponry weaponry) {
        CommandCenter.Execute(RPCMode.All, new MCSCommand(MCSCommandType.Simulation, "DestroyWeaponry", true, weaponry.ID));
    }

    /// <summary>
    /// Размещение указанного Weaponry для всех игроков. Возвращает ID нового объекта в случае успеха определения Weaponry
    /// </summary>
    public static Weaponry Instantiate<WeaponryT>() {
        return Instantiate(typeof(WeaponryT).FullName);
    }

    /// <summary>
    /// Размещение указанного Weaponry для конкретного игрока. Возвращает ID нового объекта в случае успеха определения Weaponry
    /// </summary>
    public static Weaponry Instantiate<WeaponryT>(NetworkPlayer player) {
        return Instantiate(typeof(WeaponryT).FullName, player);
    }

    /// <summary>
    /// Размещение указанного Weaponry для всех игроков. Возвращает ID нового объекта в случае успеха определения Weaponry
    /// </summary>
    /// <param name="weaponryType">Имя типа Weaponry</param>
    public static Weaponry Instantiate(string weaponryType) {
        Weaponry weaponry = null;

        //try
        //{
        weaponry = MCSGlobalFactory.InstantiateWeaponry(weaponryType).GetComponent<Weaponry>();
        weaponry.ID = Weapons.AllocateWeaponryID(); // Выделяем новый ID

        Weapons.List.Add(weaponry.ID, weaponry); // Добавляем в список

        NetworkView[] nwlist = weaponry.gameObject.GetComponentsInChildren<NetworkView>();

        // Выделяем ViewID каждому NetworkView на объекте
        foreach (NetworkView networkView in nwlist) {
            networkView.viewID = Network.AllocateViewID();
            networkView.stateSynchronization = NetworkStateSynchronization.Unreliable;
        }

        // Если экземпляр создан успешно
        if (weaponry) CommandCenter.Execute(new MCSCommand(MCSCommandType.Simulation, "Instantiate", true, weaponry.GetType().FullName, weaponry.ID));
        //}
        //catch(Exception e)
        //{
        //    Debug.LogWarning("Ошибка при создании экземпляра Weaponry: " + e.Message);
        //    throw;
        //    return null;
        //}
        return weaponry;
    }

    /// <summary>
    /// Размещение указанного Weaponry для конкретного игрока. Возвращает ID нового объекта в случае успеха определения Weaponry
    /// </summary>
    /// <param name="weaponryType">Имя типа Weaponry</param>
    /// <param name="player">Целевой игрок</param>
    public static Weaponry Instantiate(string weaponryType, NetworkPlayer player) {
        Weaponry weaponry = null;

        try {
            weaponry = MCSGlobalFactory.InstantiateWeaponry(weaponryType).GetComponent<Weaponry>();
            weaponry.ID = Weapons.AllocateWeaponryID(); // Выделяем новый ID

            Weapons.List.Add(weaponry.ID, weaponry); // Добавляем в список

            // Если экземпляр создан успешно
            if (weaponry) CommandCenter.Execute(player, new MCSCommand(MCSCommandType.Simulation, "Instantiate", true, weaponry.GetType().FullName, weaponry.ID));
        } catch {
            Debug.LogWarning("Ошибка при создании экземпляра Weaponry.");
            return null;
        }

        return weaponry;
    }

    /// <summary>
    /// Вызывается на стороне клиента
    /// </summary>
    /// <param name="weaponryID">ID добавляемого Weaponry</param>
    /// <param name="weaponryType">Тип добавляемого Weaponry</param>
    public static Weaponry ClientInstantiate(int weaponryID, string weaponryType) {
        Weaponry weaponry = null;

        try {
            weaponry = MCSGlobalFactory.InstantiateWeaponry(weaponryType).GetComponent<Weaponry>();
            weaponry.ID = weaponryID; // Выделяем новый ID

            Weapons.List.Add(weaponry.ID, weaponry); // Добавляем в список

            // Удаляем Rigidbody на стороне клиента
            Rigidbody rgdb = weaponry.GetComponent<Rigidbody>();
            GameObject.Destroy(rgdb);

            // Сообщение о готовности и запрос ViewID'шек
            MCSCommand newCmd = new MCSCommand();
            newCmd.CommandType = MCSCommandType.Simulation;
            newCmd.Command = "WeaponryPosted";

            newCmd.Args = new object[] { weaponryID }; // ID Weaponry

            MCSGlobalSimulation.CommandCenter.Execute(RPCMode.Server, newCmd);
        } catch {
            Debug.LogWarning("Ошибка при создании экземпляра Weaponry.");
            return null;
        }
        return weaponry;
    }

    #endregion

    #region Инстанцирование AI

    /// <summary>
    /// Размещение указанного Weaponry c AI для всех игроков. Возвращает true в случае успеха определения Weaponry
    /// </summary>
    /// <param name="weaponryType">Имя типа Weaponry</param>
    public static Weaponry InstantiateAI(string weaponryType) {
        Weaponry weaponry = Instantiate(weaponryType);

        try {
            (weaponry as AIControllable).InitializeAI(); // Инициализируем AI на объекте
        } catch {
            Debug.LogError("Ошибка при создании AI для Weaponry [" + weaponryType + "]. Для данного Weaponry не определен интерфейс AIControllable.");
            return null;
        }

        return weaponry;
    }

    #endregion

    /// <summary>
    /// Инициализация сервера. Запуск командного центра.
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
    /// Запуск симуляции
    /// </summary>
    public static void Start() {
        //Network.
        if (Status == SimulationStatus.Started) return;

        // Если стояли на паузе - считаем суммарное время пауз
        if (Status == SimulationStatus.Paused) {
            _summaryPauseTime += Time.timeSinceLevelLoad - _pauseStartTime;
        } else // Если запускаем впервые
        {
            // Устанавливаем время старта симуляции
            Application.LoadLevel(Application.loadedLevel);
            Debug.Log("Устанавливаем время старта " + Time.timeSinceLevelLoad);
            SimulationStartedTime = Time.timeSinceLevelLoad;
        }

        Debug.Log("-= Симуляция запущена =-");
        Debug.Log("Время пауз: " + PauseAmendment);
        Status = SimulationStatus.Started;
    }

    /// <summary>
    /// Прекращение симуляции
    /// </summary>
    public static void Stop() {
        if (Status == SimulationStatus.Stopped) return;

        Debug.Log("-= Симуляция прекращена =-");
        Status = SimulationStatus.Stopped;

        // Устанавливаем время старта симуляции
        SimulationStartedTime = Time.timeSinceLevelLoad;
    }

    /// <summary>
    /// Приостановка симуляции
    /// </summary>
    public static void Pause() {
        if (Status == SimulationStatus.Paused) return;

        // Запоминаем время, когда поставили на паузу
        _pauseStartTime = Time.timeSinceLevelLoad;


        Debug.Log("-= Симуляция приостановлена =-");
        Status = SimulationStatus.Paused;
    }
}
