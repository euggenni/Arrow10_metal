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
    [Description("Выкл")]
    OFF,
    [Description("Вкл")]
    ON,
}



// Подсказа к Enum
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
/// Класс для хранения GameObjects всех объектов PanelObjects
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
    /// Конструктор класса
    /// </summary>
    /// <param name="tumblersCount">Количество тумблеров</param>
    /// <param name="spinnersCount">Количество регуляторов</param>
    /// <param name="indicatorsCount">Количество индикаторов</param>
    /// <param name="joysticksCount">Количество джойстиков</param>
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
    /// Возвращает GameObject
    /// </summary>
    /// <param name="objectType">Тип контрола</param>
    /// <param name="index">Индекс контрола</param>
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
/// Интерфейс библиотеки панелей
/// </summary>
public abstract class Library
{
    // Список всех панелей
    public List<PanelLibrary> Panels = new List<PanelLibrary>();

    /// <summary>
    /// Возвращает название библиотеки
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Возвращает имя роли для этой библиотеки
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
    /// Возвращает список тумблеров панели
    /// </summary>
    public List<Tumbler> GetTumlbers(PanelLibrary panel)
    {
        return Panels[Panels.IndexOf(panel)].GetTumblers();
    }

    /// <summary>
    /// Возвращает список индикаторов панели
    /// </summary>
    public List<Tumbler> GetIndicators(PanelLibrary panel)
    {
        return Panels[Panels.IndexOf(panel)].GetIndicators();
    }
       
    /// <summary>
    /// Возвращает список регуляторов панели
    /// </summary>
    public List<Spinner> GetSpinners(PanelLibrary panel)
    {
        return Panels[Panels.IndexOf(panel)].GetSpinners();
    }

    /// <summary>
    /// Возвращает список джойстиков панели
    /// </summary>
    public List<Joystick> GetJoysticks(PanelLibrary panel)
    {
        return Panels[Panels.IndexOf(panel)].GetJoysticks();
    }

    //------------------------------------------------------------------

    
    /// <summary>
    /// Возвращает панель по ее имени
    /// </summary>
    /// <param name="panel">Имя панели</param>
    public PanelLibrary GetPanelByName(string panel)
    {
        return Panels.FirstOrDefault(pnl => pnl.ToString().Equals(panel));
    }

    /// <summary>
    /// Возвращает список имен панелей в этой библиотеке (англ.)
    /// </summary>
    public List<string> GetPanelNames()
    {
        return Panels.Select(pnl => pnl.ToString()).ToList();
    }

    /// <summary>
    /// Возвращает список имен панелей в этой библиотеке (рус.)
    /// </summary>
    public List<string> GetPanelDescriptions()
    {
        var list = new List<string>();

        Debug.Log("Получение описание панелей для " + GetType());

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
/// Интерфейс для библиотеки панели
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
    /// Возвращает список тумблеров панели
    /// </summary>
    public List<Tumbler> GetTumblers()
    {
        return TumblersList;
    }

    /// <summary>
    /// Возвращает список индикаторов панели
    /// </summary>
    public List<Tumbler> GetIndicators()
    {
        return IndicatorsList;
    }

    /// <summary>
    /// Возвращает список рычагов управления панели
    /// </summary>
    public List<Spinner> GetSpinners()
    {
        return SpinnersList;
    }

    /// <summary>
    /// Возвращает список рычагов управления панели
    /// </summary>
    public List<Joystick> GetJoysticks()
    {
        return JoysticksList;
    }
}

/// <summary>
/// Интерфейс панелей
/// </summary>
public interface Panel 
{
    /// <summary>
    /// Возвращает контрол с указанным именем
    /// </summary>
    /// <param name="ControlType">Тип контрола</param>
    /// <param name="name">Имя контрола</param>
    PanelControl GetControl(ControlType ControlType, string ControlName);

    /// <summary>
    /// Передать информацию об изменении состояния контрола в ядро
    /// </summary>
    /// <param name="control">Контрол</param>
    void ControlChanged(PanelControl control);

    /// <summary>
    /// Возвращает список тумблеров панели
    /// </summary>
    List<Tumbler> GetTumblers();

    /// <summary>
    /// Возвращает список индикаторов панели
    /// </summary>
    List<Tumbler> GetIndicators();

    /// <summary>
    /// Возвращает список регуляторов панели
    /// </summary>
    List<Spinner> GetSpinners();

    /// <summary>
    /// Возвращает список рычагов управления панели
    /// </summary>
    List<Joystick> GetJoysticks();
}

/// <summary>
/// Интерфейс контролов панели
/// </summary>
public abstract class PanelControl : MonoBehaviour
{
    /// <summary>
    /// Возвращает тип контрола (Tumbler, Spinner, Indicator)
    /// </summary>
    public abstract ControlType ControlType { get; }

    /// <summary>
    /// Возвращает ядро, с которым связан данный контрол
    /// </summary>
    public abstract CoreLibrary.Core Core { get; }

    /// <summary>
    /// Возвращает имя родительской панели
    /// </summary>
    public abstract string GetPanelName();

    /// <summary>
    /// Возвращает имя контрола
    /// </summary>
    public abstract string GetName();

    /// <summary>
    /// Передача в панель управления этого контрола информации о его новом состоянии
    /// </summary>
    public abstract System.Object State { get; set; }

    /// <summary>
    /// Передача в панель управления этого контрола информации о его новом состоянии
    /// </summary>
    public abstract void ControlChanged();
}

// Интерфейс переключателей
public interface Tumbler
{
    /// <summary>
    /// Состояние переключателя
    /// </summary>
    string State { get; set; }

    /// <summary>
    /// Возвращает имя переключателя
    /// </summary>
    string GetName();

    /// <summary>
    /// Возвращает текущее состояние переключателя
    /// </summary>
    /// <param name="panel">Панель</param>
    string GetCurState();

    /// <summary>
    /// Устанавливает состояние переключателя
    /// </summary>
    /// <param name="new_state">Новое состояние переключателя</param>
    void SetState(string new_state);

    /// <summary>
    /// Возвращает массив строк с возможными состояниями переключателя
    /// </summary>
    string[] GetStatesList();

    /// <summary>
    /// Возвращает список строк с возможными состояниями переключателя
    /// </summary>
    List<string> GetStatesListAsList();

    /// <summary>
    /// Возвращает список строк с описаниями состояний тумблера
    /// </summary>
    List<string> GetStatesDescriptions();

    /// <summary>
    /// Возвращает описание тумблера
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