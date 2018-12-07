using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

[SerializeField]
public enum ControlType
{
    [EnumDescription("��������")]
    Tumbler = 0,

    [EnumDescription("����������")]
    Spinner = 1,

    [EnumDescription("����������")]
    Indicator = 2,

    [EnumDescription("���������")]
    Joystick = 3
}

[Serializable]
public class ControlPanelToolkit : MonoBehaviour, Panel {

    [SerializeField]
    public GameObject CoreSource;
    private CoreLibrary.Core _core;
    public CoreLibrary.Core Core
    {
        get { return _core; }
        set { _core = value; }
    }

    public string GetName()
    {
        return panelName;
    }

    public List<Tumbler> GetTumblers()
    {
        if (Panel == null) {
            return new List<Tumbler>();
        }        
        return Library.GetTumlbers(currentPanel);
    }
    public List<Tumbler> GetIndicators()
    {
        if (Panel == null) {
            return new List<Tumbler>();
        }
        return Library.GetIndicators(currentPanel);
    }
    public List<Spinner> GetSpinners()
    {
        if (Panel == null) {
            return new List<Spinner>();
        }
        //Debug.Log("pl: " + panellibrary + "Library: " + Library + " cp: " + currentPanel);
        return Library.GetSpinners(currentPanel);
    }
    public List<Joystick> GetJoysticks()
    {
        if (Panel == null) {
            return new List<Joystick>();
        }
        return Library.GetJoysticks(currentPanel);
    }


    [SerializeField]
    private UnityEngine.Object  panellibraryobject;

    [SerializeField]
    public Library         panellibrary;
    public Library         Library
    {
        get
        {
            //Debug.Log("plo: " + panellibraryobject.name);
            // �� �� ����� ������������� (���������) ������ ������ ������, �� ����� ��������� ������ ��� ������
            // � ����� �� ���� ������������ ������ ������ ������
            if (panellibrary == null && panellibraryobject != null)
                LoadPanelLibrary();

            return panellibrary;
        }

        set 
        {
            panellibrary = value;
            //Debug.Log("Library: " + panellibrary.GetName());
        }
    }

    public void                 SetPanelLibraryObject(UnityEngine.Object obj)
    {
        if (obj == null) return;
        if (obj.Equals(panellibraryobject)) {  return; }

        panellibraryobject = obj;
        LoadPanelLibrary();
    }
    public UnityEngine.Object   GetPanelLibraryObject()
    {
        return (UnityEngine.Object)panellibraryobject;
    }
    public void                 LoadPanelLibrary()
    {
        if (panellibraryobject == null)
        {
            Debug.Log("�� ����� �������� ����������, �.�. ������������ ������, ���������� ��");
            return;
        }

        List<Type> list;

        foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
        {
            // ������� ������� ���� �� ���� ��� � PanelLibrary - ���������� �������
            if (t.BaseType == typeof(Library) && t.GetConstructor(Type.EmptyTypes) != null)
            {
                //���� ��� ������������ ������� ��������� � ������ ���������� ����
                if (t.Name.Equals(panellibraryobject.name))
                {
                    panellibrary = (Library) Assembly.GetExecutingAssembly().CreateInstance(t.FullName);
                    return;
                }
            }
        }

        panellibraryobject = null;
    }
    public void                 ClearPanelLibrary()
    {
        Debug.Log("������� ���������� ������ � ���� ������");
        ClearAllConnections(); // ������� � ���� ��������� ����� � ���� �������

        panellibraryobject = null;
        panellibrary = null;

        Panel = null;
        panelName = null;
        currentPanel = null;
        PanelIndex = 0;
    }              // �������� ���, ������� ���������� � ��������
    public void                 ClearPanel()
    {
        ClearAllConnections(); // ������� � ���� ��������� ����� � ���� �������

        Panel = null;
        panelName = null;
        currentPanel = null;
        PanelIndex = 0;
    }                   

    public CoreLibrary.Core GetCore()
    {
        if (_core == null)
        {
            if (transform.parent.GetComponent<CoreToolkit>() != null) CoreSource = transform.parent.gameObject;

            if (CoreSource != null)
            {
                CoreLibrary.Core core_temp = CoreSource.GetComponent<CoreToolkit>();

                if (core_temp != null)
                {
                    this._core = core_temp;
                }
                else
                {
                    Debug.Log("Cant find Core scrpit on object [" + CoreSource.name + "]");
                }
            }
            else
            {
                //if(panelName != null)
                //    if(panelName.Length > 0)
                //Debug.LogWarning("Have no Core for panel [" + panelName + "]");
            }
        }

        return this._core;
    }
    public void SetCoreObject(GameObject core_source)
    {
        // ���� ���� �� ������
        if (_core == null)
        {
            this.CoreSource = core_source;

            CoreLibrary.Core core_temp = (CoreLibrary.Core) core_source.GetComponent<CoreToolkit>();

            if (core_temp != null)
            {
                this._core = core_temp;
            }
            else
            {
                Debug.Log("Cant find Core scrpit on this object");
            }
        }
        
    }
    public void RemoveCore()
    {
        this._core = null;
        this.CoreSource = null;
    }

    // ������� ������, ���� ������ - ��������� � ��� ��� ������ ������ (������������ ���������)
    [SerializeField]
    private string panelName;
    private PanelLibrary currentPanel;
    public  PanelLibrary Panel
    {
        get {
            if (panelName == null)
            { 
                //Debug.Log("���� ����� ������, ������ ����������"); 
                return null; 
            }

            if (panellibraryobject == null && Library == null) Debug.LogWarning("Library is not set for this ControlPanelToolkit");
            if (currentPanel == null && Library != null) { currentPanel = Library.GetPanelByName(panelName); }
            return currentPanel;
        }
        set {
            if (value == null) { panelName = null; return; }
            if (value.Equals(panelName)) return; // ���� �� �� ����� �������� - �������, ����� ���������� �������

            panelName = value.ToString();
            //Debug.Log("��������� " + panelName);
            currentPanel = Library.GetPanelByName(panelName);

            int TumblersCount = Library.GetTumlbers(currentPanel).Count;
            int SpinnersCount = Library.GetSpinners(currentPanel).Count;
            int IndicatorsCount = Library.GetIndicators(currentPanel).Count;
            int JoysticksCount = Library.GetJoysticks(currentPanel).Count;

            /*  ��� ������ ����������� ������ � �� �������� ������ �� ��������������
                ������� ������ GameObject � ������� ����� ��������� ������� �� �������� SwitcherToolit
                � ������ �������� SwitcherToolkit, ���� ��� ����� ����������� �� ������� � GameObject   */

            PanelGameObjects = new PanelObjectsList(TumblersCount, SpinnersCount, IndicatorsCount, JoysticksCount);

            
            SwitcherScripts = new SwitcherToolkit[TumblersCount];
            SpinnerScripts = new SpinnerToolkit[SpinnersCount];
            IndicatorScripts = new IndicatorToolkit[IndicatorsCount];
            JoystickScripts = new JoystickToolkit[JoysticksCount];
        }
    }
        
    // ����� �������� ��� �������, �� ������� ����� ������� ��������������
    [SerializeField]
    private PanelObjectsList panelgameobjects;
    public  PanelObjectsList PanelGameObjects
    {
        get { return panelgameobjects; }
        set {
            panelgameobjects = value;

            //  ���� �� ����� �� �� �������� � ������� ��� ������� ������� - ������� �� �������
            foreach (ControlType enumvalue in Enum.GetValues(typeof(ControlType)))
            {
                switch ((int)enumvalue)
                {
                    case (int)ControlType.Tumbler:
                        for (int i = 0; i < panelgameobjects[enumvalue].Count; i++)
                        {
                            if (panelgameobjects[enumvalue][i] == null) continue;

                            if (panelgameobjects[enumvalue][i].GetComponent<SwitcherToolkit>() == null) continue; // ���� �� ������� ����� SwitcherToolit, ���� � ����������
                            else
                            {
                                Debug.Log("GameObject was succesfully delievered to HELL");
                            } // ����� ���������� ��� � ��
                        }

                        // ���� �������� ������ � �������������, ��������� � ������ �� ��������� ��� ������� ���� ��������
                        for (int i = 0; i < panelgameobjects[enumvalue].Count; i++)
                        {
                            if (panelgameobjects[enumvalue][i] == null) continue;
                            _switcherScripts[i] = panelgameobjects[enumvalue][i].GetComponent<SwitcherToolkit>();
                        }
                        break;

                    case (int)ControlType.Spinner:
                        for (int i = 0; i < panelgameobjects[enumvalue].Count; i++)
                        {
                            if (panelgameobjects[enumvalue][i] == null) continue;

                            if (panelgameobjects[enumvalue][i].GetComponent<SpinnerToolkit>() == null) continue; // ���� �� ������� ����� SwitcherToolit, ���� � ����������
                            else
                            {
                                Debug.Log("GameObject was succesfully delievered to HELL");
                            } // ����� ���������� ��� � ��
                        }

                        // ���� �������� ������ � �������������, ��������� � ������ �� ��������� ��� ������� ���� ��������
                        for (int i = 0; i < panelgameobjects[enumvalue].Count; i++)
                        {
                            if (panelgameobjects[enumvalue][i] == null) continue;
                            _spinnerScripts[i] =  panelgameobjects[enumvalue][i].GetComponent<SpinnerToolkit>();
                        }
                        break;

                    case (int)ControlType.Indicator:
                        for (int i = 0; i < panelgameobjects[enumvalue].Count; i++)
                        {
                            if (panelgameobjects[enumvalue][i] == null) continue;

                            if (panelgameobjects[enumvalue][i].GetComponent<IndicatorToolkit>() == null) continue; // ���� �� ������� ����� IndicatorToolkit, ���� � ����������
                            else
                            {
                                Debug.Log("GameObject was succesfully delievered to HELL");
                            } // ����� ���������� ��� � ��
                        }

                        // ���� �������� ������ � �������������, ��������� � ������ �� ��������� ��� ������� ���� ��������
                        for (int i = 0; i < panelgameobjects[enumvalue].Count; i++)
                        {
                            if (panelgameobjects[enumvalue][i] == null) continue;
                            _indicatorScripts[i] = panelgameobjects[enumvalue][i].GetComponent<IndicatorToolkit>();
                        }
                        break;

                    case (int)ControlType.Joystick:
                        for (int i = 0; i < panelgameobjects[enumvalue].Count; i++)
                        {
                            if (panelgameobjects[enumvalue][i] == null) continue;

                            if (panelgameobjects[enumvalue][i].GetComponent<JoystickToolkit>() == null) continue; // ���� �� ������� ����� JoystickToolkit, ���� � ����������
                            else
                            {
                                Debug.Log("GameObject was succesfully delievered to HELL");
                            } // ����� ���������� ��� � ��
                        }

                        // ���� �������� ������ � �������������, ��������� � ������ �� ��������� ��� ������� ���� ��������
                        for (int i = 0; i < panelgameobjects[enumvalue].Count; i++)
                        {
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

    #region ������� ��������
    // ����� �������� ������� (��������) �� ��������� SwitcherToolit
    [SerializeField]
    private SwitcherToolkit[] _switcherScripts;
    public  SwitcherToolkit[] SwitcherScripts
    {
        get { return _switcherScripts; }
        set { _switcherScripts = value; }
    }

    // ����� �������� ������� (��������) �� ��������� SpinnerToolkit
    [SerializeField]
    private SpinnerToolkit[] _spinnerScripts;
    public  SpinnerToolkit[] SpinnerScripts
    {
        get { return _spinnerScripts; }
        set { _spinnerScripts = value; }
    }

    [SerializeField]
    private IndicatorToolkit[] _indicatorScripts;
    public  IndicatorToolkit[] IndicatorScripts
    {
        get { return _indicatorScripts; }
        set { _indicatorScripts = value; }
    }

    [SerializeField]
    private JoystickToolkit[] _joystickScripts;
    public JoystickToolkit[] JoystickScripts
    {
        get { return _joystickScripts; }
        set { _joystickScripts = value; }
    }
    #endregion

    // ������ � ���������
    public void         SetGameObject(ControlType ObjectType, GameObject obj, int index)
    {

        if (!obj) return;

        if (panelgameobjects[ObjectType][index] != null)
            if (panelgameobjects[ObjectType][index].name == obj.name) return; // ���� �� ���� ������� ���� ���� ������              

        //Debug.Log("�������� ������ �� ����������");

        switch (ObjectType)
        {
            case ControlType.Tumbler:

                #region ���������� ��������

                if (obj.GetComponent("SwitcherToolkit") == null)
                    // ���� �� ������� ����� SwitcherToolkit, ���� � ����������
                {
                    obj.AddComponent<SwitcherToolkit>();
                    Debug.Log("�������� ������ SwitcherToolkit");
                }
                // ����� ���������� ��� � ��
                SwitcherToolkit switcher_script;

                // ���������, ���� �� ��� ������ � ����� ������ � ��������� (����� ��������� ������� ������������� �������)
                for (int i = 0; i < panelgameobjects[ObjectType].Count; i++)
                {
                    if (panelgameobjects[ObjectType][i] == null) continue; // ���� ��� ������� ���� ������
                    if (panelgameobjects[ObjectType][i].name == obj.name)
                    {
                        if (i == index) return; // ���� ��������� ��� �� ������ � �� �� �������
                        else
                        {
                            SwitcherScripts[i] = null;
                            PanelGameObjects[ObjectType][i] = null;
                            //.......................................
                            panelgameobjects[ObjectType][index] = obj;
                            switcher_script = (SwitcherToolkit) panelgameobjects[ObjectType][index].GetComponent("SwitcherToolkit");

                            // ���������� ������ SwitcherToolkit ����� ��������
                            SwitcherScripts[index] = switcher_script;
                            SwitcherScripts[index].TumblerID = index;
                            SwitcherScripts[index].Tumbler = Library.GetTumlbers(currentPanel)[index];

                            return;
                        }
                    }
                }

                panelgameobjects[ObjectType][index] = obj;

                // �������� � ������ �������� � ��������� �������� ��� ���� ����� �������� ��������
                switcher_script = (SwitcherToolkit) panelgameobjects[ObjectType][index].GetComponent("SwitcherToolkit");
                SwitcherScripts[index] = switcher_script;
                SwitcherScripts[index].SetParentPanelScript(this);

                //��������� ID ����� �������� � ������ �� ������
                SwitcherScripts[index].TumblerID = index;

                SwitcherScripts[index].Tumbler = Library.GetTumlbers(currentPanel)[index];


                #endregion

                break;

            case ControlType.Spinner:

                #region ���������� ����������

                if (obj.GetComponent("SpinnerToolkit") == null)
                    // ���� �� ������� ����� SpinnerToolkit, ���� � ����������
                {
                    obj.AddComponent<SpinnerToolkit>();
                    Debug.Log("�������� ������ SpinnerToolkit");
                } // ����� ���������� ��� � ��

                SpinnerToolkit spinner_script;

                // ���������, ���� �� ��� ������ � ����� ������ � ��������� (����� ��������� ������� ������������� �������)
                for (int i = 0; i < panelgameobjects[ObjectType].Count; i++)
                {
                    if (panelgameobjects[ObjectType][i] == null) continue; // ���� ��� ������� ���� ������
                    if (panelgameobjects[ObjectType][i].name == obj.name)
                    {
                        if (i == index) return; // ���� ��������� ��� �� ������ � �� �� �������
                        else
                        {
                            SpinnerScripts[i] = null;
                            PanelGameObjects[ObjectType][i] = null;
                            //.......................................
                            PanelGameObjects[ObjectType][index] = obj;
                            spinner_script =
                                (SpinnerToolkit) PanelGameObjects[ObjectType][index].GetComponent("SpinnerToolkit");

                            // ���������� ������ SwitcherToolkit ����� ��������
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

                // �������� � ������ �������� � ��������� �������� ��� ���� ����� �������� ��������
                spinner_script = (SpinnerToolkit) PanelGameObjects[ObjectType][index].GetComponent("SpinnerToolkit");
                SpinnerScripts[index] = spinner_script;
                SpinnerScripts[index].SetParentPanelScript(this);

                //��������� ID ����� �������� � ������ �� ������
                SpinnerScripts[index].SpinnerID = index;
                SpinnerScripts[index].MinimalValue = Library.GetSpinners(currentPanel)[index].GetMinValue();
                SpinnerScripts[index].MaximalValue = Library.GetSpinners(currentPanel)[index].GetMaxValue();

                #endregion

                break;

            case ControlType.Indicator:

                #region ���������� ����������

                if (obj.GetComponent("IndicatorToolkit") == null)
                    // ���� �� ������� ����� SpinnerToolkit, ���� � ����������
                {
                    obj.AddComponent<IndicatorToolkit>();
                }
                IndicatorToolkit indicator_script;

                // ���������, ���� �� ��� ������ � ����� ������ � ��������� (����� ��������� ������� ������������� �������)
                for (int i = 0; i < panelgameobjects[ObjectType].Count; i++)
                {
                    if (panelgameobjects[ObjectType][i] == null) continue; // ���� ��� ������� ���� ������
                    if (panelgameobjects[ObjectType][i].name == obj.name)
                    {
                        if (i == index) return; // ���� ��������� ��� �� ������ � �� �� �������
                        else
                        {
                            IndicatorScripts[i] = null;
                            PanelGameObjects[ObjectType][i] = null;
                            //.......................................
                            PanelGameObjects[ObjectType][index] = obj;
                            indicator_script =
                                (IndicatorToolkit) PanelGameObjects[ObjectType][index].GetComponent("IndicatorToolkit");


                            // ���������� ������ IndicatorToolkit ����� ��������
                            IndicatorScripts[index] = indicator_script;
                            IndicatorScripts[index].setParentPanelScript(this);

                            //��������� ID ����� ���������� � ������ �� ������
                            IndicatorScripts[index].IndicatorID = index;
                            IndicatorScripts[index].Indicator = Library.GetIndicators(currentPanel)[index];


                            return;
                        }
                    }
                }

                PanelGameObjects[ObjectType][index] = obj;

                // �������� � ������ �������� � ��������� �������� ��� ���� ����� �������� ��������
                indicator_script =
                    (IndicatorToolkit) PanelGameObjects[ObjectType][index].GetComponent("IndicatorToolkit");
                IndicatorScripts[index] = indicator_script;
                IndicatorScripts[index].setParentPanelScript(this);

                //��������� ID ����� ���������� � ������ �� ������
                IndicatorScripts[index].IndicatorID = index;
                IndicatorScripts[index].Indicator = Library.GetIndicators(currentPanel)[index];


                #endregion

                break;

            case ControlType.Joystick:

                #region ���������� ���������

                if (obj.GetComponent<JoystickToolkit>() == null)
                    // ���� �� ������� ����� SpinnerToolkit, ���� � ����������
                {
                    obj.AddComponent<JoystickToolkit>();
                }
                JoystickToolkit joystick_script;

                // ���������, ���� �� ��� ������ � ����� ������ � ��������� (����� ��������� ������� ������������� �������)
                for (int i = 0; i < panelgameobjects[ObjectType].Count; i++)
                {
                    if (panelgameobjects[ObjectType][i] == null) continue; // ���� ��� ������� ���� ������
                    if (panelgameobjects[ObjectType][i].name == obj.name)
                    {
                        if (i == index) return; // ���� ��������� ��� �� ������ � �� �� �������

                        JoystickScripts[i] = null;
                        PanelGameObjects[ObjectType][i] = null;
                        //.......................................
                        PanelGameObjects[ObjectType][index] = obj;
                        joystick_script = PanelGameObjects[ObjectType][index].GetComponent<JoystickToolkit>();


                        // ���������� ������ JoystickToolkit ����� ��������
                        JoystickScripts[index] = joystick_script;
                        JoystickScripts[index].SetParentPanelScript(this);

                        //��������� ID ����� ���������� � ������ �� ������
                        JoystickScripts[index].ControlID = index;
                        JoystickScripts[index].SetName(this.Library.GetJoysticks(currentPanel)[index].GetName());
                        return;

                    }
                }

                PanelGameObjects[ObjectType][index] = obj;

                // �������� � ������ �������� � ��������� �������� ��� ���� ����� �������� ��������
                joystick_script = PanelGameObjects[ObjectType][index].GetComponent<JoystickToolkit>();
                JoystickScripts[index] = joystick_script;
                JoystickScripts[index].SetParentPanelScript(this);

                //��������� ID ����� ���������� � ������ �� ������
                JoystickScripts[index].ControlID = index;
                JoystickScripts[index].SetName(this.Library.GetJoysticks(currentPanel)[index].GetName());

                #endregion

                break;

            default:
                Debug.Log("Cant recognize PanelObject type [" + ObjectType + "]");
                return;
        }
    }

    public GameObject   GetGameObject(ControlType ObjectType, int index)
    {
        try
        {
            return panelgameobjects[ObjectType][index];
        }
        catch { return null; }
    }

    public void RemoveTumbler(ControlType objectType, int index)
    {
        if (index >= PanelGameObjects[objectType].Count)
        {
            Debug.Log("�������� ������� ������� �� ��������� ������� ��������� [" + objectType + "]");
            return;
        }

        panelgameobjects[objectType][index] = null;
        RemoveScript(objectType, index);
    }
    public void RemoveScript (ControlType objectType, int index)
    {
        switch (objectType)
        {
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

    public void ClearAllConnections()
    {
        //Debug.Log("SwitcherScripts Length: " + SwitcherScripts.Length);
        if(SwitcherScripts != null)
        foreach (SwitcherToolkit st in SwitcherScripts)
        {
            if(st != null)
            st.removeParentPanelScript();
        }

        if (SpinnerScripts != null)
            foreach (SpinnerToolkit st in SpinnerScripts)
            {
                if (st != null)
                    st.removeParentPanelScript();
            }

        if (IndicatorScripts != null)
            foreach (IndicatorToolkit it in IndicatorScripts)
            {
                if (it != null)
                    it.removeParentPanelScript();
            }

        if (JoystickScripts != null)
            foreach (JoystickToolkit jt in JoystickScripts)
            {
                if (jt != null)
                    jt.RemoveParentPanelScript();
            }
    }

    public List<string> GetPanelObjectsNames(ControlType ObjectType)
    {
        List<string> list = new List<string>();

        switch (ObjectType)
        {
            case ControlType.Tumbler:
                List<Tumbler> tumblers = GetTumblers();
                for (int i = 0; i < tumblers.Count; i++)
                {
                    list.Add(tumblers[i].GetName());
                }
                return list;

            case ControlType.Spinner:
                List<Spinner> spinners = GetSpinners();
                for (int i = 0; i < spinners.Count; i++)
                {
                    list.Add(spinners[i].GetName());
                }
                return list;

            case ControlType.Indicator:
                List<Tumbler> indicators = GetIndicators();
                for (int i = 0; i < indicators.Count; i++)
                {
                    list.Add(indicators[i].GetName());
                }
                return list;

            case ControlType.Joystick:
                List<Joystick> joysticks = GetJoysticks();
                for (int i = 0; i < joysticks.Count; i++)
                {
                    list.Add(joysticks[i].GetName());
                }
                return list;
        }        

        //Debug.Log("Cant recognize PanelObject type [" + ObjectType + "]");
        return new List<string>();
    }

    public List<string> GetPanelObjectsDecriptions(ControlType ObjectType)
    {
        List<string> list = new List<string>();

        switch (ObjectType)
        {
            case ControlType.Tumbler:
                List<Tumbler> tumblers = GetTumblers();
                for (int i = 0; i < tumblers.Count; i++)
                {
                    list.Add(tumblers[i].GetDescription());
                }
                return list;

            case ControlType.Spinner:
                List<Spinner> spinners = GetSpinners();
                for (int i = 0; i < spinners.Count; i++)
                {
                    list.Add(spinners[i].GetDescription());
                }
                return list;

            case ControlType.Indicator:
                List<Tumbler> indicators = GetIndicators();
                for (int i = 0; i < indicators.Count; i++)
                {
                    list.Add(indicators[i].GetDescription());
                }
                return list;

            case ControlType.Joystick:
                List<Joystick> joysticks = GetJoysticks();
                for (int i = 0; i < joysticks.Count; i++)
                {
                    list.Add(joysticks[i].GetDescription());
                }
                return list;
        }

        return new List<string>();
    }

    /// <summary>
    /// �������� ���������� �� ��������� ��������� �������� � ����
    /// </summary>
    /// <param name="control">�������</param>
    public void ControlChanged(PanelControl control)
    {
        if (GetCore() != null)
        {
            GetCore().ControlChanged(control); 
        }
        else
        {
            Debug.Log("Cannot send ControlChanged message, because Core was not found for [" + GetName() + "]");
        }
    }

    /// <summary>
    /// ���������� ������� � ��������� ������
    /// </summary>
    /// <param name="ControlType">��� ��������</param>
    /// <param name="name">��� ��������</param>
    public PanelControl GetControl(ControlType ControlType, string ControlName)
    {
        int index = -1;

        switch (ControlType)
        {
            case ControlType.Tumbler:
                for (int i = 0; i < GetTumblers().Count; i++)
                    if (GetTumblers()[i].GetName().Equals(ControlName)) { index = i; break; }
                break;

            case ControlType.Spinner:
                for (int i = 0; i < GetSpinners().Count; i++)
                    if (GetSpinners()[i].GetName().Equals(ControlName)) { index = i; break; }
                break;

            case ControlType.Indicator:
                for (int i = 0; i < GetIndicators().Count; i++)
                    if (GetIndicators()[i].GetName().Equals(ControlName)) { index = i; break; }
                break;

            case ControlType.Joystick:
                for (int i = 0; i < GetJoysticks().Count; i++)
                    if (GetJoysticks()[i].GetName().Equals(ControlName)) { index = i; break; }
                break;

            default:
                Debug.Log("ControlType [" + ControlType + "] is not recognized");
                break;
        }

        if(index == -1) {
            Debug.Log("Control with name [" + ControlName + "] was not found for [" + GetName() + "]");
            return null;
        }

        if (PanelGameObjects[ControlType, index] != null || Core.isVirtual)
        {
            switch (ControlType)
            {
                case ControlType.Tumbler:
                    return (PanelControl)SwitcherScripts[index];

                case ControlType.Spinner:
                    return (PanelControl)SpinnerScripts[index];

                case ControlType.Indicator:
                    return (PanelControl)IndicatorScripts[index];

                case ControlType.Joystick:
                    return (PanelControl)JoystickScripts[index];

                default:
                    Debug.Log("ControlType [" + ControlType + "] is not recognized");
                    return null;
            }
        }
        else
        {
            Debug.Log("Control with name [" + ControlName + "] was not defined yet for [" + GetName() + "]");
            return null;
        }
    }

    [SerializeField]
    private int panelIndex;
    public  int PanelIndex
    {
        get { return panelIndex; }
        set { panelIndex = value; }
    }

}
