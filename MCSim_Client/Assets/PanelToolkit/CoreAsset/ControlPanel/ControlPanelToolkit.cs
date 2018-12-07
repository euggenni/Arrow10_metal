using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

[SerializeField]
public enum ControlType {
    [EnumDescription("Тумблеры")]
    Tumbler = 0,

    [EnumDescription("Регуляторы")]
    Spinner = 1,

    [EnumDescription("Индикаторы")]
    Indicator = 2,

    [EnumDescription("Джойстики")]
    Joystick = 3
}

[Serializable]
public class ControlPanelToolkit : MonoBehaviour, Panel {
    [SerializeField]
    public GameObject CoreSource;

    private CoreLibrary.Core _core;

    public CoreLibrary.Core Core {
        get { return _core; }
        set { _core = value; }
    }

    public string GetName() {
        return panelName;
    }

    public List<Tumbler> GetTumblers() {
        if (Panel == null) {
            return new List<Tumbler>();
        }
        return Library.GetTumlbers(currentPanel);
    }

    public List<Tumbler> GetIndicators() {
        if (Panel == null) {
            return new List<Tumbler>();
        }
        return Library.GetIndicators(currentPanel);
    }

    public List<Spinner> GetSpinners() {
        if (Panel == null) {
            return new List<Spinner>();
        }
        //Debug.Log("pl: " + panellibrary + "Library: " + Library + " cp: " + currentPanel);
        return Library.GetSpinners(currentPanel);
    }

    public List<Joystick> GetJoysticks() {
        if (Panel == null) {
            return new List<Joystick>();
        }
        return Library.GetJoysticks(currentPanel);
    }


    [SerializeField]
    private Object panellibraryobject;

    [SerializeField]
    public Library panellibrary;

    public Library Library {
        get {
            //Debug.Log("plo: " + panellibraryobject.name);
            // Мы не можем сериализовать (запомнить) объект нашего класса, но можем запомнить скрипт как объект
            // и потом по нему восстановить объект нашего класса
            if (panellibrary == null && panellibraryobject != null)
                LoadPanelLibrary();

            return panellibrary;
        }

        set {
            panellibrary = value;
            //Debug.Log("Library: " + panellibrary.GetName());
        }
    }

    public void SetPanelLibraryObject(Object obj) {
        if (obj == null) return;
        if (obj.Equals(panellibraryobject)) {
            return;
        }

        panellibraryobject = obj;
        LoadPanelLibrary();
    }

    public Object GetPanelLibraryObject() {
        return (Object) panellibraryobject;
    }

    public void LoadPanelLibrary() {
        if (panellibraryobject == null) {
            Debug.Log("Не можем получить библиотеку, т.к. отстутствует объект, содержащий ее");
            return;
        }

        List<Type> list;

        foreach (Type t in Assembly.GetExecutingAssembly().GetTypes()) {
            // Находим объекты того же типа что и PanelLibrary - библиотеки пультов
            if (t.BaseType == typeof(Library) && t.GetConstructor(Type.EmptyTypes) != null) {
                //Если имя вставляемого скрипта совпадает с именем найденного типа
                if (t.Name.Equals(panellibraryobject.name)) {
                    panellibrary = (Library) Assembly.GetExecutingAssembly().CreateInstance(t.FullName);
                    return;
                }
            }
        }

        panellibraryobject = null;
    }

    public void ClearPanelLibrary() {
        Debug.Log("Очищаем библиотеку панели и саму панель");
        ClearAllConnections(); // Удаляем у всех тумблеров связь с этой панелью

        panellibraryobject = null;
        panellibrary = null;

        Panel = null;
        panelName = null;
        currentPanel = null;
        PanelIndex = 0;
    } // Очистить все, включая библиотеку с панелями

    public void ClearPanel() {
        ClearAllConnections(); // Удаляем у всех тумблеров связь с этой панелью

        Panel = null;
        panelName = null;
        currentPanel = null;
        PanelIndex = 0;
    }

    public CoreLibrary.Core GetCore() {
        if (_core == null) {
            if (transform.parent.GetComponent<CoreToolkit>() != null) CoreSource = transform.parent.gameObject;

            if (CoreSource != null) {
                CoreLibrary.Core core_temp = CoreSource.GetComponent<CoreToolkit>();

                if (core_temp != null) {
                    this._core = core_temp;
                } else {
                    Debug.Log("Cant find Core scrpit on object [" + CoreSource.name + "]");
                }
            } else {
                //if(panelName != null)
                //    if(panelName.Length > 0)
                //Debug.LogWarning("Have no Core for panel [" + panelName + "]");
            }
        }

        return this._core;
    }

    public void SetCoreObject(GameObject core_source) {
        // Если ядро не задано
        if (_core == null) {
            this.CoreSource = core_source;

            CoreLibrary.Core core_temp = (CoreLibrary.Core) core_source.GetComponent<CoreToolkit>();

            if (core_temp != null) {
                this._core = core_temp;
            } else {
                Debug.Log("Cant find Core scrpit on this object");
            }
        }
    }

    public void RemoveCore() {
        this._core = null;
        this.CoreSource = null;
    }

    // Текущая панель, если меняем - загружаем в нее все нужные данные (перечисление тумблеров)
    [SerializeField]
    private string panelName;

    private PanelLibrary currentPanel;

    public PanelLibrary Panel {
        get {
            if (panelName == null) {
                //Debug.Log("Нету имени панели, нечего возвращать"); 
                return null;
            }

            if (panellibraryobject == null && Library == null)
                Debug.LogWarning("Library is not set for this ControlPanelToolkit");
            if (currentPanel == null && Library != null) {
                currentPanel = Library.GetPanelByName(panelName);
            }
            return currentPanel;
        }
        set {
            if (value == null) {
                panelName = null;
                return;
            }
            if (value.Equals(panelName)) return; // Если то же самое значение - выходим, иначе обнуляются массивы

            panelName = value.ToString();
            Debug.Log("Загрузили " + panelName);
            currentPanel = Library.GetPanelByName(panelName);

            int TumblersCount = Library.GetTumlbers(currentPanel).Count;
            int SpinnersCount = Library.GetSpinners(currentPanel).Count;
            int IndicatorsCount = Library.GetIndicators(currentPanel).Count;
            int JoysticksCount = Library.GetJoysticks(currentPanel).Count;

            /*  Как только загружается панель и мы получаем список ее переключателей
                Создаем массив GameObject в котором будут храниться объекты со скриптом SwitcherToolit
                И массив скриптов SwitcherToolkit, куда они будут извлекаться из массива с GameObject   */

            PanelGameObjects = new PanelObjectsList(TumblersCount, SpinnersCount, IndicatorsCount, JoysticksCount);


            SwitcherScripts = new SwitcherToolkit[TumblersCount];
            SpinnerScripts = new SpinnerToolkit[SpinnersCount];
            IndicatorScripts = new IndicatorToolkit[IndicatorsCount];
            JoystickScripts = new JoystickToolkit[JoysticksCount];
        }
    }

    // Здесь хранятся все объекты, на которых висят скрипты переключателей
    [SerializeField]
    private PanelObjectsList panelgameobjects;

    public PanelObjectsList PanelGameObjects {
        get { return panelgameobjects; }
        set {
            panelgameobjects = value;

            //  Если на каком то из объектов в массиве нет нужного скрипта - удаляем из массива
            foreach (ControlType enumvalue in Enum.GetValues(typeof(ControlType))) {
                switch ((int) enumvalue) {
                    case (int) ControlType.Tumbler:
                        for (int i = 0; i < panelgameobjects[enumvalue].Count; i++) {
                            if (panelgameobjects[enumvalue][i] == null) continue;

                            if (panelgameobjects[enumvalue][i].GetComponent<SwitcherToolkit>() == null)
                                continue; // Если на объекте висит SwitcherToolit, идем к следующему
                            else {
                                Debug.Log("GameObject was succesfully delievered to HELL");
                            } // Иначе отправляем его в АД
                        }

                        // Если изменяем массив с геймобъектами, сохраняем в массив со скриптами все скрипты этих объектов
                        for (int i = 0; i < panelgameobjects[enumvalue].Count; i++) {
                            if (panelgameobjects[enumvalue][i] == null) continue;
                            _switcherScripts[i] = panelgameobjects[enumvalue][i].GetComponent<SwitcherToolkit>();
                        }
                        break;

                    case (int) ControlType.Spinner:
                        for (int i = 0; i < panelgameobjects[enumvalue].Count; i++) {
                            if (panelgameobjects[enumvalue][i] == null) continue;

                            if (panelgameobjects[enumvalue][i].GetComponent<SpinnerToolkit>() == null)
                                continue; // Если на объекте висит SwitcherToolit, идем к следующему
                            else {
                                Debug.Log("GameObject was succesfully delievered to HELL");
                            } // Иначе отправляем его в АД
                        }

                        // Если изменяем массив с геймобъектами, сохраняем в массив со скриптами все скрипты этих объектов
                        for (int i = 0; i < panelgameobjects[enumvalue].Count; i++) {
                            if (panelgameobjects[enumvalue][i] == null) continue;
                            _spinnerScripts[i] = panelgameobjects[enumvalue][i].GetComponent<SpinnerToolkit>();
                        }
                        break;

                    case (int) ControlType.Indicator:
                        for (int i = 0; i < panelgameobjects[enumvalue].Count; i++) {
                            if (panelgameobjects[enumvalue][i] == null) continue;

                            if (panelgameobjects[enumvalue][i].GetComponent<IndicatorToolkit>() == null)
                                continue; // Если на объекте висит IndicatorToolkit, идем к следующему
                            else {
                                Debug.Log("GameObject was succesfully delievered to HELL");
                            } // Иначе отправляем его в АД
                        }

                        // Если изменяем массив с геймобъектами, сохраняем в массив со скриптами все скрипты этих объектов
                        for (int i = 0; i < panelgameobjects[enumvalue].Count; i++) {
                            if (panelgameobjects[enumvalue][i] == null) continue;
                            _indicatorScripts[i] = panelgameobjects[enumvalue][i].GetComponent<IndicatorToolkit>();
                        }
                        break;

                    case (int) ControlType.Joystick:
                        for (int i = 0; i < panelgameobjects[enumvalue].Count; i++) {
                            if (panelgameobjects[enumvalue][i] == null) continue;

                            if (panelgameobjects[enumvalue][i].GetComponent<JoystickToolkit>() == null)
                                continue; // Если на объекте висит JoystickToolkit, идем к следующему
                            else {
                                Debug.Log("GameObject was succesfully delievered to HELL");
                            } // Иначе отправляем его в АД
                        }

                        // Если изменяем массив с геймобъектами, сохраняем в массив со скриптами все скрипты этих объектов
                        for (int i = 0; i < panelgameobjects[enumvalue].Count; i++) {
                            if (panelgameobjects[enumvalue][i] == null) continue;
                            _joystickScripts[i] = panelgameobjects[enumvalue][i].GetComponent<JoystickToolkit>();
                        }
                        break;

                    default:
                        Debug.LogWarning("Cant recognize PanelObject type");
                        return;
                }
            }
        }
    }

    #region Массивы скриптов

    // Здесь хранятся объекты (тумблеры) со скриптами SwitcherToolit
    [SerializeField]
    private SwitcherToolkit[] _switcherScripts;

    public SwitcherToolkit[] SwitcherScripts {
        get { return _switcherScripts; }
        set { _switcherScripts = value; }
    }

    // Здесь хранятся объекты (тумблеры) со скриптами SpinnerToolkit
    [SerializeField]
    private SpinnerToolkit[] _spinnerScripts;

    public SpinnerToolkit[] SpinnerScripts {
        get { return _spinnerScripts; }
        set { _spinnerScripts = value; }
    }

    [SerializeField]
    private IndicatorToolkit[] _indicatorScripts;

    public IndicatorToolkit[] IndicatorScripts {
        get { return _indicatorScripts; }
        set { _indicatorScripts = value; }
    }

    [SerializeField]
    private JoystickToolkit[] _joystickScripts;

    public JoystickToolkit[] JoystickScripts {
        get { return _joystickScripts; }
        set { _joystickScripts = value; }
    }

    #endregion

    // Работа с объектами
    public void SetGameObject(ControlType ObjectType, GameObject obj, int index) {
        if (!obj) return;

        Debug.Log("INDEX - " + index);
        //Debug.Log("ObjectType = " + ObjectType.ToString());

        Debug.Log("obj = " + obj.ToString());

        //for (int i = 0; i < panelgameobjects[ObjectType].Count; i++) {
        //    Debug.Log("panelgameobjects = " + panelgameobjects[ObjectType][i].ToString());
        //}

        
        if (panelgameobjects[ObjectType][index] != null)
            if (panelgameobjects[ObjectType][index].name == obj.name)
                return; // Если на этой позиции есть этот объект              

        //Debug.Log("Начинаем искать на совпадения");

        switch (ObjectType) {
            case ControlType.Tumbler:

                #region Добавление тумблера

                if (obj.GetComponent("SwitcherToolkit") == null)
                    // Если на объекте висит SwitcherToolkit, идем к следующему
                {
                    obj.AddComponent<SwitcherToolkit>();
                    Debug.Log("Добавлен объект SwitcherToolkit");
                }
                // Иначе отправляем его в АД
                SwitcherToolkit switcher_script;

                // Проверяем, если ли уже объект с таким именем в коллекции (чтобы исключить двойное использование объекта)
                for (int i = 0; i < panelgameobjects[ObjectType].Count; i++) {
                    if (panelgameobjects[ObjectType][i] == null) continue; // Если нет объекта идем дальше
                    if (panelgameobjects[ObjectType][i].name == obj.name) {
                        if (i == index) return; // Если вставляем тот же объект в ту же позиция
                        else {
                            SwitcherScripts[i] = null;
                            PanelGameObjects[ObjectType][i] = null;
                            //.......................................
                            panelgameobjects[ObjectType][index] = obj;
                            switcher_script =
                                (SwitcherToolkit) panelgameobjects[ObjectType][index].GetComponent("SwitcherToolkit");

                            // Запоминаем скрипт SwitcherToolkit этого тумблера
                            SwitcherScripts[index] = switcher_script;
                            SwitcherScripts[index].TumblerID = index;
                            SwitcherScripts[index].Tumbler = Library.GetTumlbers(currentPanel)[index];

                            return;
                        }
                    }
                }

                panelgameobjects[ObjectType][index] = obj;

                // Помещаем в массив скриптов и указываем родителя для того чтобы передать тумблеры
                switcher_script = (SwitcherToolkit) panelgameobjects[ObjectType][index].GetComponent("SwitcherToolkit");
                SwitcherScripts[index] = switcher_script;
                SwitcherScripts[index].SetParentPanelScript(this);

                //Указываем ID этого тумблера в списке на панели
                SwitcherScripts[index].TumblerID = index;

                SwitcherScripts[index].Tumbler = Library.GetTumlbers(currentPanel)[index];

                #endregion

                break;

            case ControlType.Spinner:

                #region Добавление регулятора

                if (obj.GetComponent("SpinnerToolkit") == null)
                    // Если на объекте висит SpinnerToolkit, идем к следующему
                {
                    obj.AddComponent<SpinnerToolkit>();
                    Debug.Log("Добавлен объект SpinnerToolkit");
                } // Иначе отправляем его в АД

                SpinnerToolkit spinner_script;

                // Проверяем, если ли уже объект с таким именем в коллекции (чтобы исключить двойное использование объекта)
                for (int i = 0; i < panelgameobjects[ObjectType].Count; i++) {
                    if (panelgameobjects[ObjectType][i] == null) continue; // Если нет объекта идем дальше
                    if (panelgameobjects[ObjectType][i].name == obj.name) {
                        if (i == index) return; // Если вставляем тот же объект в ту же позиция
                        else {
                            SpinnerScripts[i] = null;
                            PanelGameObjects[ObjectType][i] = null;
                            //.......................................
                            PanelGameObjects[ObjectType][index] = obj;
                            spinner_script =
                                (SpinnerToolkit) PanelGameObjects[ObjectType][index].GetComponent("SpinnerToolkit");

                            // Запоминаем скрипт SwitcherToolkit этого тумблера
                            SpinnerScripts[index] = spinner_script;
                            SpinnerScripts[index].SetParentPanelScript(this);

                            SpinnerScripts[index].SpinnerID = index;
                            SpinnerScripts[index].MinimalValue = Library.GetSpinners(currentPanel)[index].GetMinValue();
                            SpinnerScripts[index].MaximalValue = Library.GetSpinners(currentPanel)[index].GetMaxValue();

                            return;
                        }
                    }
                }

                PanelGameObjects[ObjectType][index] = obj;

                // Помещаем в массив скриптов и указываем родителя для того чтобы передать тумблеры
                spinner_script = (SpinnerToolkit) PanelGameObjects[ObjectType][index].GetComponent("SpinnerToolkit");
                SpinnerScripts[index] = spinner_script;
                SpinnerScripts[index].SetParentPanelScript(this);

                //Указываем ID этого тумблера в списке на панели
                SpinnerScripts[index].SpinnerID = index;
                SpinnerScripts[index].MinimalValue = Library.GetSpinners(currentPanel)[index].GetMinValue();
                SpinnerScripts[index].MaximalValue = Library.GetSpinners(currentPanel)[index].GetMaxValue();

                #endregion

                break;

            case ControlType.Indicator:

                #region Добавление индикатора

                if (obj.GetComponent("IndicatorToolkit") == null)
                    // Если на объекте висит SpinnerToolkit, идем к следующему
                {
                    obj.AddComponent<IndicatorToolkit>();
                }
                IndicatorToolkit indicator_script;

                // Проверяем, если ли уже объект с таким именем в коллекции (чтобы исключить двойное использование объекта)
                for (int i = 0; i < panelgameobjects[ObjectType].Count; i++) {
                    if (panelgameobjects[ObjectType][i] == null) continue; // Если нет объекта идем дальше
                    if (panelgameobjects[ObjectType][i].name == obj.name) {
                        if (i == index) return; // Если вставляем тот же объект в ту же позиция
                        else {
                            IndicatorScripts[i] = null;
                            PanelGameObjects[ObjectType][i] = null;
                            //.......................................
                            PanelGameObjects[ObjectType][index] = obj;
                            indicator_script =
                                (IndicatorToolkit) PanelGameObjects[ObjectType][index].GetComponent("IndicatorToolkit");


                            // Запоминаем скрипт IndicatorToolkit этого тумблера
                            IndicatorScripts[index] = indicator_script;
                            IndicatorScripts[index].setParentPanelScript(this);

                            //Указываем ID этого индикатора в списке на панели
                            IndicatorScripts[index].IndicatorID = index;
                            IndicatorScripts[index].Indicator = Library.GetIndicators(currentPanel)[index];


                            return;
                        }
                    }
                }

                PanelGameObjects[ObjectType][index] = obj;

                // Помещаем в массив скриптов и указываем родителя для того чтобы передать тумблеры
                indicator_script =
                    (IndicatorToolkit) PanelGameObjects[ObjectType][index].GetComponent("IndicatorToolkit");
                IndicatorScripts[index] = indicator_script;
                IndicatorScripts[index].setParentPanelScript(this);

                //Указываем ID этого индикатора в списке на панели
                IndicatorScripts[index].IndicatorID = index;
                IndicatorScripts[index].Indicator = Library.GetIndicators(currentPanel)[index];

                #endregion

                break;

            case ControlType.Joystick:

                #region Добавление джойстика

                if (obj.GetComponent<JoystickToolkit>() == null)
                    // Если на объекте висит SpinnerToolkit, идем к следующему
                {
                    obj.AddComponent<JoystickToolkit>();
                }
                JoystickToolkit joystick_script;

                // Проверяем, если ли уже объект с таким именем в коллекции (чтобы исключить двойное использование объекта)
                for (int i = 0; i < panelgameobjects[ObjectType].Count; i++) {
                    if (panelgameobjects[ObjectType][i] == null) continue; // Если нет объекта идем дальше
                    if (panelgameobjects[ObjectType][i].name == obj.name) {
                        if (i == index) return; // Если вставляем тот же объект в ту же позиция

                        JoystickScripts[i] = null;
                        PanelGameObjects[ObjectType][i] = null;
                        //.......................................
                        PanelGameObjects[ObjectType][index] = obj;
                        joystick_script = PanelGameObjects[ObjectType][index].GetComponent<JoystickToolkit>();


                        // Запоминаем скрипт JoystickToolkit этого тумблера
                        JoystickScripts[index] = joystick_script;
                        JoystickScripts[index].SetParentPanelScript(this);

                        //Указываем ID этого индикатора в списке на панели
                        JoystickScripts[index].ControlID = index;
                        JoystickScripts[index].SetName(this.Library.GetJoysticks(currentPanel)[index].GetName());
                        return;
                    }
                }

                PanelGameObjects[ObjectType][index] = obj;

                // Помещаем в массив скриптов и указываем родителя для того чтобы передать тумблеры
                joystick_script = PanelGameObjects[ObjectType][index].GetComponent<JoystickToolkit>();
                JoystickScripts[index] = joystick_script;
                JoystickScripts[index].SetParentPanelScript(this);

                //Указываем ID этого индикатора в списке на панели
                JoystickScripts[index].ControlID = index;
                JoystickScripts[index].SetName(this.Library.GetJoysticks(currentPanel)[index].GetName());

                #endregion

                break;

            default:
                Debug.Log("Cant recognize PanelObject type [" + ObjectType + "]");
                return;
        }
    }

    public GameObject GetGameObject(ControlType ObjectType, int index) {
        try {
            return panelgameobjects[ObjectType][index];
        } catch {
            return null;
        }
    }

    public void RemoveTumbler(ControlType objectType, int index) {
        if (index >= PanelGameObjects[objectType].Count) {
            Debug.Log("Пытаемся удалить тумблер за пределами массива тумблеров [" + objectType + "]");
            return;
        }

        panelgameobjects[objectType][index] = null;
        RemoveScript(objectType, index);
    }

    public void RemoveScript(ControlType objectType, int index) {
        switch (objectType) {
            case ControlType.Tumbler:
                _switcherScripts[index] = null;
                break;

            case ControlType.Spinner:
                _spinnerScripts[index] = null;
                break;

            case ControlType.Indicator:
                _indicatorScripts[index] = null;
                break;

            case ControlType.Joystick:
                _joystickScripts[index] = null;
                break;

            default:
                Debug.Log("Cant recognize PanelObject type");
                return;
        }
    }

    public void ClearAllConnections() {
        //Debug.Log("SwitcherScripts Length: " + SwitcherScripts.Length);
        if (SwitcherScripts != null)
            foreach (SwitcherToolkit st in SwitcherScripts) {
                if (st != null)
                    st.removeParentPanelScript();
            }

        if (SpinnerScripts != null)
            foreach (SpinnerToolkit st in SpinnerScripts) {
                if (st != null)
                    st.removeParentPanelScript();
            }

        if (IndicatorScripts != null)
            foreach (IndicatorToolkit it in IndicatorScripts) {
                if (it != null)
                    it.removeParentPanelScript();
            }

        if (JoystickScripts != null)
            foreach (JoystickToolkit jt in JoystickScripts) {
                if (jt != null)
                    jt.RemoveParentPanelScript();
            }
    }

    public List<string> GetPanelObjectsNames(ControlType ObjectType) {
        List<string> list = new List<string>();

        switch (ObjectType) {
            case ControlType.Tumbler:
                List<Tumbler> tumblers = GetTumblers();
                for (int i = 0; i < tumblers.Count; i++) {
                    list.Add(tumblers[i].GetName());
                }
                return list;

            case ControlType.Spinner:
                List<Spinner> spinners = GetSpinners();
                for (int i = 0; i < spinners.Count; i++) {
                    list.Add(spinners[i].GetName());
                }
                return list;

            case ControlType.Indicator:
                List<Tumbler> indicators = GetIndicators();
                for (int i = 0; i < indicators.Count; i++) {
                    list.Add(indicators[i].GetName());
                }
                return list;

            case ControlType.Joystick:
                List<Joystick> joysticks = GetJoysticks();
                for (int i = 0; i < joysticks.Count; i++) {
                    list.Add(joysticks[i].GetName());
                }
                return list;
        }

        //Debug.Log("Cant recognize PanelObject type [" + ObjectType + "]");
        return new List<string>();
    }

    public List<string> GetPanelObjectsDecriptions(ControlType ObjectType) {
        List<string> list = new List<string>();

        switch (ObjectType) {
            case ControlType.Tumbler:
                List<Tumbler> tumblers = GetTumblers();
                for (int i = 0; i < tumblers.Count; i++) {
                    list.Add(tumblers[i].GetDescription());
                }
                return list;

            case ControlType.Spinner:
                List<Spinner> spinners = GetSpinners();
                for (int i = 0; i < spinners.Count; i++) {
                    list.Add(spinners[i].GetDescription());
                }
                return list;

            case ControlType.Indicator:
                List<Tumbler> indicators = GetIndicators();
                for (int i = 0; i < indicators.Count; i++) {
                    list.Add(indicators[i].GetDescription());
                }
                return list;

            case ControlType.Joystick:
                List<Joystick> joysticks = GetJoysticks();
                for (int i = 0; i < joysticks.Count; i++) {
                    list.Add(joysticks[i].GetDescription());
                }
                return list;
        }

        return new List<string>();
    }

    /// <summary>
    /// Передать информацию об изменении состояния контрола в ядро
    /// </summary>
    /// <param name="control">Контрол</param>
    public void ControlChanged(PanelControl control) {
        if (GetCore() != null) {
            GetCore().ControlChanged(control);
        } else {
            Debug.Log("Cannot send ControlChanged message, because Core was not found for [" + GetName() + "]");
        }
    }

    /// <summary>
    /// Возвращает контрол с указанным именем
    /// </summary>
    /// <param name="ControlType">Тип контрола</param>
    /// <param name="name">Имя контрола</param>
    public PanelControl GetControl(ControlType ControlType, string ControlName) {
        int index = -1;

        switch (ControlType) {
            case ControlType.Tumbler:
                for (int i = 0; i < GetTumblers().Count; i++)
                    if (GetTumblers()[i].GetName().Equals(ControlName)) {
                        index = i;
                        break;
                    }
                break;

            case ControlType.Spinner:
                for (int i = 0; i < GetSpinners().Count; i++)
                    if (GetSpinners()[i].GetName().Equals(ControlName)) {
                        index = i;
                        break;
                    }
                break;

            case ControlType.Indicator:
                for (int i = 0; i < GetIndicators().Count; i++)
                    if (GetIndicators()[i].GetName().Equals(ControlName)) {
                        index = i;
                        break;
                    }
                break;

            case ControlType.Joystick:
                for (int i = 0; i < GetJoysticks().Count; i++)
                    if (GetJoysticks()[i].GetName().Equals(ControlName)) {
                        index = i;
                        break;
                    }
                break;

            default:
                Debug.Log("ControlType [" + ControlType + "] is not recognized");
                break;
        }

        if (index == -1) {
            Debug.Log("Control with name [" + ControlName + "] was not found for [" + GetName() + "]");
            return null;
        }

        if (PanelGameObjects[ControlType, index] != null) {
            switch (ControlType) {
                case ControlType.Tumbler:
                    return (PanelControl) SwitcherScripts[index];

                case ControlType.Spinner:
                    return (PanelControl) SpinnerScripts[index];

                case ControlType.Indicator:
                    return (PanelControl) IndicatorScripts[index];

                case ControlType.Joystick:
                    return (PanelControl) JoystickScripts[index];

                default:
                    Debug.Log("ControlType [" + ControlType + "] is not recognized");
                    return null;
            }
        } else {
            Debug.Log("Control with name [" + ControlName + "] was not defined yet for [" + GetName() + "]");
            return null;
        }
    }

    [SerializeField]
    private int panelIndex;

    public int PanelIndex {
        get { return panelIndex; }
        set { panelIndex = value; }
    }
}