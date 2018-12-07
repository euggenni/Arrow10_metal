using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using System.Reflection;
using System.ComponentModel;


public enum TOGGLE_STANDART
{
    [Description("����")]
    OFF,
    [Description("���")]
    ON,
}



// �������� � Enum
public class EnumDescription : Attribute
{

    public string Text
    {

        get { return _text; }

    } private string _text;



    public EnumDescription(string text)
    {

        _text = text;

    }
}


/// <summary>
/// ����� ��� �������� GameObjects ���� �������� PanelObjects
/// </summary>
[Serializable]
public class PanelObjectsList : System.Object
{
    [SerializeField]
    public List<GameObject> tumblers;

    [SerializeField]
    public List<GameObject> spinners;

    [SerializeField]
    public List<GameObject> indicators;

    [SerializeField]
    public List<GameObject> joysticks;

    /// <summary>
    /// ����������� ������
    /// </summary>
    /// <param name="tumblersCount">���������� ���������</param>
    /// <param name="spinnersCount">���������� �����������</param>
    /// <param name="indicatorsCount">���������� �����������</param>
    /// <param name="joysticksCount">���������� ����������</param>
    public PanelObjectsList(int tumblersCount, int spinnersCount, int indicatorsCount, int joysticksCount)
    {
        tumblers    = new List<GameObject>(tumblersCount);
        spinners    = new List<GameObject>(spinnersCount);
        indicators = new List<GameObject>(indicatorsCount);
        joysticks = new List<GameObject>(joysticksCount);

        FillByNulls(tumblers);
        FillByNulls(spinners);
        FillByNulls(indicators);
        FillByNulls(joysticks);

        //Debug.Log("Tumblers:" + tumblers.Capacity + " Spinners:" + spinners.Capacity + " Indicators:" + indicators.Capacity + " Joysticks:" + joysticks.Capacity);
    }

    /// <summary>
    /// ���������� GameObject
    /// </summary>
    /// <param name="objectType">��� ��������</param>
    /// <param name="index">������ ��������</param>
    public GameObject this[ControlType objectType, int index]
    {        
        get
        {
            switch (objectType)
            {
                case ControlType.Tumbler:
                    return tumblers[index];

                case ControlType.Spinner:
                    return spinners[index];

                case ControlType.Indicator:
                    return indicators[index];

                case ControlType.Joystick:
                    return joysticks[index];

                default:
                    Debug.Log("Cant recognize panel object type [" + objectType + "]");
                    return null;
            }
        }

        set
        {
            switch (objectType)
            {
                case ControlType.Tumbler:
                    tumblers[index] = value; break;

                case ControlType.Spinner:
                    spinners[index] = value; break;

                case ControlType.Indicator:
                    indicators[index] = value; break;

                case ControlType.Joystick:
                    joysticks[index] = value; break;

                default:
                    Debug.Log("Cant recognize panel object type [" + objectType + "]");
                    break;
            }
        }
    }

    public List<GameObject> this[ControlType objectType]
    {
        get
        {
            switch (objectType)
            {
                case ControlType.Tumbler:
                    return tumblers;

                case ControlType.Spinner:
                    return spinners;

                case ControlType.Indicator:
                    return indicators;

                case ControlType.Joystick:
                    return joysticks;

                default:
                    Debug.Log("Cant recognize panel object type [" + objectType + "]");
                    return new List<GameObject>();
            }
        }

        set
        {
            switch (objectType)
            {
                case ControlType.Tumbler:
                    tumblers = value;
                    break;

                case ControlType.Spinner:
                    spinners = value;
                    break;

                case ControlType.Indicator:
                    indicators = value;
                    break;

                case ControlType.Joystick:
                    joysticks = value;
                    break;

                default:
                    Debug.Log("Cant recognize panel object type [" + objectType + "]");
                    break;
            }
        }
    }

    public void FillByNulls(List<GameObject> list)
    {
        for (int i = 0; i < list.Capacity; i++)
            list.Add(null);
    }
}

/// <summary>
/// ��������� ���������� �������
/// </summary>
public abstract class Library
{
    // ������ ���� �������
    public List<PanelLibrary> Panels = new List<PanelLibrary>();

    /// <summary>
    /// ���������� �������� ����������
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// ���������� ��� ���� ��� ���� ����������
    /// </summary>
    public abstract string  GetRole();

    protected Library()
    {
        foreach (FieldInfo fieldInfo in this.GetType().GetFields())
        {
            if (fieldInfo.FieldType.BaseType == typeof(PanelLibrary))
                Panels.Add((PanelLibrary)fieldInfo.GetValue(this));
        }
    }

    /// <summary>
    /// ���������� ������ ��������� ������
    /// </summary>
    public List<Tumbler> GetTumlbers(PanelLibrary panel)
    {
        return Panels[Panels.IndexOf(panel)].GetTumblers();
    }

    /// <summary>
    /// ���������� ������ ����������� ������
    /// </summary>
    public List<Tumbler> GetIndicators(PanelLibrary panel)
    {
        return Panels[Panels.IndexOf(panel)].GetIndicators();
    }
       
    /// <summary>
    /// ���������� ������ ����������� ������
    /// </summary>
    public List<Spinner> GetSpinners(PanelLibrary panel)
    {
        return Panels[Panels.IndexOf(panel)].GetSpinners();
    }

    /// <summary>
    /// ���������� ������ ���������� ������
    /// </summary>
    public List<Joystick> GetJoysticks(PanelLibrary panel)
    {
        return Panels[Panels.IndexOf(panel)].GetJoysticks();
    }

    //------------------------------------------------------------------

    
    /// <summary>
    /// ���������� ������ �� �� �����
    /// </summary>
    /// <param name="panel">��� ������</param>
    public PanelLibrary GetPanelByName(string panel)
    {
        return Panels.FirstOrDefault(pnl => pnl.ToString().Equals(panel));
    }

    /// <summary>
    /// ���������� ������ ���� ������� � ���� ���������� (����.)
    /// </summary>
    public List<string> GetPanelNames()
    {
        return Panels.Select(pnl => pnl.ToString()).ToList();
    }

    /// <summary>
    /// ���������� ������ ���� ������� � ���� ���������� (���.)
    /// </summary>
    public List<string> GetPanelDescriptions()
    {
        var list = new List<string>();

        Debug.Log("��������� �������� ������� ��� " + GetType());

        foreach (FieldInfo fieldInfo in GetType().GetFields())
        {
            if (GetPanelNames().Contains(fieldInfo.FieldType.ToString()))
            {
                EnumDescription[] attrs = (EnumDescription[])fieldInfo.GetCustomAttributes(typeof(EnumDescription), false);
                if (attrs.Length > 0)
                {
                    list.Add(attrs[0].Text);
                }
            }
        }
        return list;
    }
}

/// <summary>
/// ��������� ��� ���������� ������
/// </summary>
public class PanelLibrary
{
    protected List<Tumbler> TumblersList = new List<Tumbler>();
    protected List<Tumbler> IndicatorsList = new List<Tumbler>();
    protected List<Spinner> SpinnersList = new List<Spinner>();
    protected List<Joystick> JoysticksList = new List<Joystick>();

    public PanelLibrary()
    {
        foreach (FieldInfo fieldInfo in this.GetType().GetFields())
        {
            if (fieldInfo.FieldType.ToString().Contains("EnumTumbler"))
                TumblersList.Add((Tumbler)fieldInfo.GetValue(this));

            if (fieldInfo.FieldType.ToString().Contains("EnumIndicator"))
                IndicatorsList.Add((Tumbler)fieldInfo.GetValue(this));

            if (fieldInfo.FieldType.ToString().Contains("Spinner"))
                SpinnersList.Add((Spinner)fieldInfo.GetValue(this));

            if (fieldInfo.FieldType.ToString().Contains("Joystick"))
                JoysticksList.Add((Joystick) fieldInfo.GetValue(this));

        }
    }

    /// <summary>
    /// ���������� ������ ��������� ������
    /// </summary>
    public List<Tumbler> GetTumblers()
    {
        return TumblersList;
    }

    /// <summary>
    /// ���������� ������ ����������� ������
    /// </summary>
    public List<Tumbler> GetIndicators()
    {
        return IndicatorsList;
    }

    /// <summary>
    /// ���������� ������ ������� ���������� ������
    /// </summary>
    public List<Spinner> GetSpinners()
    {
        return SpinnersList;
    }

    /// <summary>
    /// ���������� ������ ������� ���������� ������
    /// </summary>
    public List<Joystick> GetJoysticks()
    {
        return JoysticksList;
    }
}

/// <summary>
/// ��������� �������
/// </summary>
public interface Panel 
{
    /// <summary>
    /// ���������� ������� � ��������� ������
    /// </summary>
    /// <param name="ControlType">��� ��������</param>
    /// <param name="name">��� ��������</param>
    PanelControl GetControl(ControlType ControlType, string ControlName);

    /// <summary>
    /// �������� ���������� �� ��������� ��������� �������� � ����
    /// </summary>
    /// <param name="control">�������</param>
    void ControlChanged(PanelControl control);

    /// <summary>
    /// ���������� ������ ��������� ������
    /// </summary>
    List<Tumbler> GetTumblers();

    /// <summary>
    /// ���������� ������ ����������� ������
    /// </summary>
    List<Tumbler> GetIndicators();

    /// <summary>
    /// ���������� ������ ����������� ������
    /// </summary>
    List<Spinner> GetSpinners();

    /// <summary>
    /// ���������� ������ ������� ���������� ������
    /// </summary>
    List<Joystick> GetJoysticks();
}

/// <summary>
/// ��������� ��������� ������
/// </summary>
public abstract class PanelControl : MonoBehaviour
{
    /// <summary>
    /// ���������� ��� �������� (Tumbler, Spinner, Indicator)
    /// </summary>
    public abstract ControlType ControlType { get; }

    /// <summary>
    /// ���������� ����, � ������� ������ ������ �������
    /// </summary>
    public abstract CoreLibrary.Core Core { get; }

    /// <summary>
    /// ���������� ��� ������������ ������
    /// </summary>
    public abstract string GetPanelName();

    /// <summary>
    /// ���������� ��� ��������
    /// </summary>
    public abstract string GetName();

    /// <summary>
    /// �������� � ������ ���������� ����� �������� ���������� � ��� ����� ���������
    /// </summary>
    public abstract System.Object State { get; set; }

    /// <summary>
    /// �������� � ������ ���������� ����� �������� ���������� � ��� ����� ���������
    /// </summary>
    public abstract void ControlChanged();
}

// ��������� ��������������
public interface Tumbler
{
    /// <summary>
    /// ��������� �������������
    /// </summary>
    string State { get; set; }

    /// <summary>
    /// ���������� ��� �������������
    /// </summary>
    string GetName();

    /// <summary>
    /// ���������� ������� ��������� �������������
    /// </summary>
    /// <param name="panel">������</param>
    string GetCurState();

    /// <summary>
    /// ������������� ��������� �������������
    /// </summary>
    /// <param name="new_state">����� ��������� �������������</param>
    void SetState(string new_state);

    /// <summary>
    /// ���������� ������ ����� � ���������� ����������� �������������
    /// </summary>
    string[] GetStatesList();

    /// <summary>
    /// ���������� ������ ����� � ���������� ����������� �������������
    /// </summary>
    List<string> GetStatesListAsList();

    /// <summary>
    /// ���������� ������ ����� � ���������� ��������� ��������
    /// </summary>
    List<string> GetStatesDescriptions();

    /// <summary>
    /// ���������� �������� ��������
    /// </summary>
    string GetDescription();
}


[Serializable]
public class EnumTumbler<EnumT> : Tumbler
{
    EnumT _value;
    string _name;
    string _description;

    public string State
    {
        get { return _value.ToString(); }
        set { _value = (EnumT)Enum.Parse(typeof(EnumT), value); }
    }

    public EnumTumbler()
    {
        //this._value = (Enum) 0;
        this._name = typeof(EnumT).ToString().Split('+')[1];
    }

    public EnumTumbler(EnumT value)
    {
        this._value = value;
        this._name = typeof(EnumT).ToString().Split('+')[1];

        var attributes = (DescriptionAttribute[])typeof(EnumT).GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (attributes.Length > 0)
        {
            _description = attributes[0].Description;
        }
    }

    public EnumTumbler(EnumT value, string name)
    {
        this._value = value;
        this._name = name;

        var attributes = (DescriptionAttribute[])typeof(EnumT).GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (attributes.Length > 0)
        {
            _description = attributes[0].Description;
        }
    }

    public string GetName()
    {
        return this._name;
    }

    public string GetDescription()
    {
        return _description;
    }

    public string GetCurState()
    {
        return _value.ToString();
    }

    public void SetState(string new_state)
    {
        _value = (EnumT)Enum.Parse(typeof(EnumT), new_state);
    }
    public void SetState(EnumT value)
    {
        this._value = value;
    }

    public string[] GetStatesList()
    {
        return Enum.GetNames(typeof(EnumT));
    }

    public List<string> GetStatesListAsList()
    {
        List<string> list = new List<string>();
        for (int i = 0; i < Enum.GetNames(typeof(EnumT)).Length; i++)
        {
            list.Add(Enum.GetNames(typeof(EnumT))[i]);
        }
        return list;
    }


    public List<string> GetStatesDescriptions()
    {
        List<string> list = new List<string>();
        //typeof(EnumT)
        //Type type = EnumT.GetControlType();
        string description = "";

        foreach (FieldInfo fieldInfo in typeof(EnumT).GetFields())
        {
                var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    description = attributes[0].Description;
                    list.Add(description);  
                }
          
        }

        return list;
    }
}

[Serializable]
public class EnumIndicator<EnumT> : Tumbler
{
    EnumT _value;
    string _name;
    string _description;

    public string State
    {
        get { return _value.ToString(); }
        set { _value = (EnumT)Enum.Parse(typeof(EnumT), value); }
    }

    public EnumIndicator(EnumT value)
    {
        this._value = value;
        this._name = typeof(EnumT).ToString().Split('+')[1];

        var attributes = (DescriptionAttribute[])typeof(EnumT).GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (attributes.Length > 0)
        {
            _description = attributes[0].Description;
        }
    }

    public EnumIndicator(EnumT value, string name)
    {
        this._value = value;
        this._name = name;

        var attributes = (DescriptionAttribute[])typeof(EnumT).GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (attributes.Length > 0)
        {
            _description = attributes[0].Description;
        }
    }

    public EnumIndicator(EnumT value, string name, string description)
    {
        this._value = value;
        this._name = name;

        _description = description;
    }

    public string GetName()
    {
        return this._name;
    }

    public string GetDescription()
    {
        return _description;
    }

    public string GetCurState()
    {
        return _value.ToString();
    }

    public void SetState(string new_state)
    {
        _value = (EnumT)Enum.Parse(typeof(EnumT), new_state);
    }
    public void SetState(EnumT value)
    {
        this._value = value;
    }

    public string[] GetStatesList()
    {
        return Enum.GetNames(typeof(EnumT));
    }

    public List<string> GetStatesListAsList()
    {
        List<string> list = new List<string>();
        for (int i = 0; i < Enum.GetNames(typeof(EnumT)).Length; i++)
        {
            list.Add(Enum.GetNames(typeof(EnumT))[i]);
        }
        return list;
    }


    public List<string> GetStatesDescriptions()
    {
        List<string> list = new List<string>();
        //typeof(EnumT)
        //Type type = EnumT.GetControlType();
        string description = "";

        foreach (FieldInfo fieldInfo in typeof(EnumT).GetFields())
        {
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                description = attributes[0].Description;
                list.Add(description);
            }

        }

        return list;
    }
}


[Serializable]
public class Spinner
{
    private string name = "None";
    private int minValue = 0;
    private int maxValue = 1;

    private string description;

    public Spinner(string name, string description, int minValue, int maxValue)
    {
        this.name = name;
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.description = description;
    }

    public string GetName()
    {
        return this.name;
    }

    public int GetMinValue()
    {
        return this.minValue;
    }

    public int GetMaxValue()
    {
        return this.maxValue;
    }

    public string GetDescription()
    {
        return this.description;
    }
}

[Serializable]
public class Joystick
{
    private string name = "None";
    private string description = "None";

    public Joystick(string name, string description)
    {
        this.name = name;
        this.description = description;
    }

    public string GetName()
    {
        return this.name;
    }

    public string GetDescription()
    {
        return this.description;
    }
}