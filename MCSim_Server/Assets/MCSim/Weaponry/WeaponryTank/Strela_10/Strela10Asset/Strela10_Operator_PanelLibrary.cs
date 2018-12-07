using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.ComponentModel;

[Serializable]
public class Strela10_Operator_PanelLibrary : Library
{
    // Список всех панелей
    List<PanelLibrary> list_panels = new List<PanelLibrary>();

    [EnumDescription("Панель оператора")]
    public Strela10_OperatorPanel operatorpanel = new Strela10_OperatorPanel();

    [EnumDescription("Панель оперативного управления")]
    public Strela10_OperationalPanel operationalpanel = new Strela10_OperationalPanel();

    [EnumDescription("Блок управления")]
    public Strela10_ControlBlock controlblockpanel = new Strela10_ControlBlock();

    [EnumDescription("Указатель Азимута")]
    public Strela10_AzimuthIndicator azimuth_indicator = new Strela10_AzimuthIndicator();

    [EnumDescription("Панель настройки")]
    public Strela10_SupportPanel support_panel = new Strela10_SupportPanel();

    [EnumDescription("Панель джойстиков")]
    public Strela10_GuidancePanel joystick_panel = new Strela10_GuidancePanel();
    
    [EnumDescription("Панель Визира")]
    public Strela10_VizorPanel vizor_panel = new Strela10_VizorPanel();

    [EnumDescription("Панель АРЦ")]
    public Strela10_ARC arc_panel = new Strela10_ARC();
    
    public override string GetRole()
    {
        return "Оператор";
    }

    public override string Name
    {
       // return "Strela-10 Panels Store";
	    get { return "Strela-10_Operator"; }
    }

    public List<PanelLibrary> GetPanels()
    {
        return list_panels;
    }
    public List<string> GetPanelNames()
    {
        List<string> list = new List<string>();

        foreach (PanelLibrary pnl in list_panels)
        {
            list.Add(pnl.ToString());
        }

        return list;
    }
    public List<string> GetPanelDescriptions()
    {
        List<string> list = new List<string>();

        foreach (FieldInfo fieldInfo in typeof(Strela10_Operator_PanelLibrary).GetFields())
        {
            if (GetPanelNames().Contains(fieldInfo.FieldType.ToString()))
            {
                EnumDescription[] attrs = (EnumDescription[])fieldInfo.GetCustomAttributes(typeof(EnumDescription), false);
                if (attrs.Length > 0) {
                    list.Add(attrs[0].Text);
                }

                //list.Add(fieldInfo.FieldType.ToString());
            }
        }
        return list;
    }

    public List<Tumbler> GetTumlbers(PanelLibrary panel)
    {
        return list_panels[list_panels.IndexOf(panel)].GetTumblers();
    }
    public List<Tumbler> GetIndicators(PanelLibrary panel)
    {
        return list_panels[list_panels.IndexOf(panel)].GetIndicators();
    }
    public List<Spinner> GetSpinners(PanelLibrary panel)
    {
        return list_panels[list_panels.IndexOf(panel)].GetSpinners();
    }
    public List<Joystick> GetJoysticks(PanelLibrary panel)
    {
        return list_panels[list_panels.IndexOf(panel)].GetJoysticks();
    }

    #region Добавление всех панелей в списки в КОНСТРУКТОРЕ

    public Strela10_Operator_PanelLibrary()
    {
        list_panels.Add(operatorpanel);
        list_panels.Add(operationalpanel);
        list_panels.Add(controlblockpanel);
        list_panels.Add(azimuth_indicator);
        list_panels.Add(support_panel);
        list_panels.Add(joystick_panel);
        list_panels.Add(vizor_panel);
        list_panels.Add(arc_panel);
    }

    public PanelLibrary GetPanelByName(string panel)
    {
        foreach (PanelLibrary pnl in list_panels)
        {
            if (pnl.ToString().Equals(panel)) return pnl;
        }
        return null; 
    }
    public PanelLibrary GetPanelByIndex(int index)
    {
        return list_panels[index];
    }

    #endregion
}

[Serializable]
public class Strela10_OperatorPanel : PanelLibrary
{
    List<Tumbler> list_tumblers     = new List<Tumbler>();
    List<Tumbler> list_indicators   = new List<Tumbler>();
    List<Spinner> list_spinners = new List<Spinner>();
    List<Joystick> list_joysticks = new List<Joystick>();

    public Strela10_OperatorPanel()
    {
        foreach (FieldInfo fieldInfo in this.GetType().GetFields())
        {
            if (fieldInfo.FieldType.ToString().Contains("EnumTumbler"))
                list_tumblers.Add((Tumbler)fieldInfo.GetValue(this));

            if (fieldInfo.FieldType.ToString().Contains("EnumIndicator"))
                list_indicators.Add((Tumbler)fieldInfo.GetValue(this));
            
            if (fieldInfo.FieldType.ToString().Contains("Spinner"))
                list_spinners.Add((Spinner)fieldInfo.GetValue(this));
        }
    }

    #region Тумблеры ENUM

    // Тумблер АОЗ - Аппаратура Оценки Зоны (Equipment of Area Assessment)
    [Description("Аппаратура оценки зоны")]
    public enum TUMBLER_AOZ
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON
    }

    [Description("Привод - Выкл. - Ручное")]
    public enum TUMBLER_DRIVE_HANDLE_OFF
    {
        [Description("Ручное")]
        MANUAL,
        [Description("Привод")]
        DRIVE,
        [Description("Выкл")]
        OFF
    }

    [Description("Фон")]
    public enum TUMBLER_FON
    {
        [Description("I")]
        STATE_1,
        [Description("II")]
        STATE_2
    }

    [Description("Род работы")]
    public enum TUMBLER_WORK_TYPE
    {
        [Description("Авт.")]
        AUTO,
        [Description("I")]
        MODE_1,
        [Description("II")]
        MODE_2,
        [Description("III")]
        MODE_3,
        [Description("IV")]
        MODE_4
    }

    [Description("Питание 24В")]
    public enum TUMBLER_POWER_24B
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    [Description("Питание 28В")]
    public enum TUMBLER_POWER_28B
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    [Description("Питание 30В")]
    public enum TUMBLER_POWER_30B
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    [Description("Режим работы")]
    public enum TUMBLER_MODE
    {
        [Description("Боевой")]
        COMBAT,
        [Description("Учебный")]
        TRAINING,
    }

    [Description("ПСП")]
    public enum TUMBLER_PSP
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    [Description("Режим ПСП")]
    public enum TUMBLER_PSP_MODE
    {
        [Description("НП")]
        NP,
        [Description("ДП")]
        DP,
    }

    [Description("Кнопка Сброс")]
    public enum TUMBLER_DROP
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    [Description("Кнопка АЦУ")]
    public enum TUMBLER_ACU
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    [Description("БЛ по 1РЛ246")]
    public enum TUMBLER_BL
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

      #endregion
    
    public enum INDICATOR_STANDART
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    #region Тумблеры

    public EnumTumbler<TUMBLER_AOZ> tumbler_eaa = new EnumTumbler<TUMBLER_AOZ>(TUMBLER_AOZ.OFF);
    public EnumTumbler<TUMBLER_FON> tumbler_fon = new EnumTumbler<TUMBLER_FON>(TUMBLER_FON.STATE_1);

    public EnumTumbler<TUMBLER_DRIVE_HANDLE_OFF>    tumbler_mdo = new EnumTumbler<TUMBLER_DRIVE_HANDLE_OFF>(TUMBLER_DRIVE_HANDLE_OFF.DRIVE);
    public EnumTumbler<TUMBLER_WORK_TYPE>           tumbler_workmode = new EnumTumbler<TUMBLER_WORK_TYPE>(TUMBLER_WORK_TYPE.AUTO);

    public EnumTumbler<TUMBLER_POWER_24B> tumbler_power_24b = new EnumTumbler<TUMBLER_POWER_24B>(TUMBLER_POWER_24B.OFF);
    public EnumTumbler<TUMBLER_POWER_28B> tumbler_power_28b = new EnumTumbler<TUMBLER_POWER_28B>(TUMBLER_POWER_28B.OFF);
    public EnumTumbler<TUMBLER_POWER_30B> tumbler_power_30b = new EnumTumbler<TUMBLER_POWER_30B>(TUMBLER_POWER_30B.OFF);
    
    public EnumTumbler<TUMBLER_MODE>        tumbler_mode = new EnumTumbler<TUMBLER_MODE>(TUMBLER_MODE.TRAINING);
    public EnumTumbler<TUMBLER_PSP>         tumbler_psp = new EnumTumbler<TUMBLER_PSP>(TUMBLER_PSP.OFF);
    public EnumTumbler<TUMBLER_PSP_MODE>    tumbler_psp_mode = new EnumTumbler<TUMBLER_PSP_MODE>(TUMBLER_PSP_MODE.NP);

    public EnumTumbler<TUMBLER_DROP>    tumbler_drop = new EnumTumbler<TUMBLER_DROP>(TUMBLER_DROP.OFF);
    public EnumTumbler<TUMBLER_ACU>     tumbler_acu = new EnumTumbler<TUMBLER_ACU>(TUMBLER_ACU.OFF);
    public EnumTumbler<TUMBLER_BL>      tumbler_bl = new EnumTumbler<TUMBLER_BL>(TUMBLER_BL.OFF);
    #endregion

    #region Регуляторы
    public Spinner spinner_power_28B    = new Spinner("SPINNER_POWER_28B", "Рег.28В", 0, 100);
    public Spinner spinner_brightness   = new Spinner("SPINNER_BRIGHTNESS", "Яркость", 0, 100);
    public Spinner spinner_nvu_mode     = new Spinner("SPINNER_MVU_MODE", "Режим НВУ", 0, 360);
    public Spinner spinner_voltmeter    = new Spinner("SPINNER_VOLTMETER", "Вольтметр", 0, 50);
    #endregion

    #region Индикаторы
    public EnumIndicator<INDICATOR_STANDART> indicator_voltmeter_backlight = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "VOLTMETER_BACKLIGHT", "Подсветка вольтметра");

    public EnumIndicator<INDICATOR_STANDART> indicator_i = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_I", "Инфракрасный канал (I)");

    public EnumIndicator<INDICATOR_STANDART> indicator_combat = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_COMBAT", "Индикатор 'Боевой'");
    public EnumIndicator<INDICATOR_STANDART> indicator_training = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_TRAINING", "Индикатор 'Учебный'");

    public EnumIndicator<INDICATOR_STANDART> indicator_board = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_BOARD", "Индикатор 'Борт'");

    public EnumIndicator<INDICATOR_STANDART> indicator_launcher_1 = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "LAUNCHER_1", "Индикатор 'Пост 1'");
    public EnumIndicator<INDICATOR_STANDART> indicator_launcher_2 = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "LAUNCHER_2", "Индикатор 'Пост 2'");
    public EnumIndicator<INDICATOR_STANDART> indicator_launcher_3 = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "LAUNCHER_3", "Индикатор 'Пост 3'");
    public EnumIndicator<INDICATOR_STANDART> indicator_launcher_4 = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "LAUNCHER_4", "Индикатор 'Пост 4'");

    public EnumIndicator<INDICATOR_STANDART> indicator_bl = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_BL", "Индикатор 'БЛ по 1РЛ246'");

    public EnumIndicator<INDICATOR_STANDART> indicator_pz_ohl_1 = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "CONTR_PZ_OHL_1", "Контр.Пз.Охл. I");
    public EnumIndicator<INDICATOR_STANDART> indicator_pz_ohl_2 = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "CONTR_PZ_OHL_2", "Контр.Пз.Охл. II");

    public EnumIndicator<INDICATOR_STANDART> indicator_stowed = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_STOWED", "Индикатор 'Поход'");
    #endregion

    public List<Tumbler> GetTumblers()
    {
        return list_tumblers;
    }

    public List<Tumbler> GetIndicators()
    {
        return list_indicators;
    }

    public List<Spinner> GetSpinners()
    {
        return list_spinners;
    }

    public List<Joystick> GetJoysticks()
    {
        return list_joysticks;
    } 
}

[Serializable]
public class Strela10_OperationalPanel : PanelLibrary
{
    List<Tumbler> list_tumblers = new List<Tumbler>();
    List<Tumbler> list_indicators = new List<Tumbler>();
    List<Spinner> list_spinners = new List<Spinner>();

    public Strela10_OperationalPanel()
    {
        foreach (FieldInfo fieldInfo in this.GetType().GetFields())
        {
            if (fieldInfo.FieldType.ToString().Contains("EnumTumbler"))
                list_tumblers.Add((Tumbler)fieldInfo.GetValue(this));

            if (fieldInfo.FieldType.ToString().Contains("EnumIndicator"))
                list_indicators.Add((Tumbler)fieldInfo.GetValue(this));

            if (fieldInfo.FieldType.ToString().Contains("Spinner"))
                list_spinners.Add((Spinner)fieldInfo.GetValue(this));
        }
    }

    #region Тумблеры

    [Description("Тумблер состояния АОЗ")]
    public enum TUMBLER_AOZ
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    [Description("Режим")]
    public enum TUMBLER_MODE
    {
        [Description("Боевой")]
        COMBAT,
        [Description("Учебный")]
        TRAINING,
    }

    #endregion

    public enum INDICATOR_STANDART
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    #region Тумблеры

    public EnumTumbler<TUMBLER_AOZ> tumbler_eaa = new EnumTumbler<TUMBLER_AOZ>(TUMBLER_AOZ.OFF);
    public EnumTumbler<TUMBLER_MODE> tumbler_fon = new EnumTumbler<TUMBLER_MODE>(TUMBLER_MODE.TRAINING);
    #endregion

    #region Индикаторы
    public EnumIndicator<INDICATOR_STANDART> indicator_aoz = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_AOZ", "Индикатор состояния АОЗ");
    public EnumIndicator<INDICATOR_STANDART> indicator_radiation = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_RADIATION", "Индикатор 'Излучение'");
    public EnumIndicator<INDICATOR_STANDART> indicator_battle = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_BATTLE", "Индикатор 'Боевой'");
    public EnumIndicator<INDICATOR_STANDART> indicator_training = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_TRAINING", "Индикатор 'Учебный'");
    #endregion

    public List<Tumbler> GetTumblers()
    {
        return list_tumblers;
    }

    public List<Tumbler> GetIndicators()
    {
        return list_indicators;
    }

    public List<Spinner> GetSpinners()
    {
        return list_spinners;
    }

    public List<Joystick> GetJoysticks()
    {
        return new List<Joystick>();
    } 
}

[Serializable]
public class Strela10_ControlBlock : PanelLibrary
{
    List<Tumbler> list_tumblers = new List<Tumbler>();
    List<Tumbler> list_indicators = new List<Tumbler>();
    List<Spinner> list_spinners = new List<Spinner>();

    public Strela10_ControlBlock()
    {
        foreach (FieldInfo fieldInfo in this.GetType().GetFields())
        {
            if (fieldInfo.FieldType.ToString().Contains("EnumTumbler"))
                list_tumblers.Add((Tumbler)fieldInfo.GetValue(this));

            if (fieldInfo.FieldType.ToString().Contains("EnumIndicator"))
                list_indicators.Add((Tumbler)fieldInfo.GetValue(this));

            if (fieldInfo.FieldType.ToString().Contains("Spinner"))
                list_spinners.Add((Spinner)fieldInfo.GetValue(this));
        }
    }

    #region Тумблеры ENUM

    [Description("Кнопка включения")]
    public enum TUMBLER_STATUS
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    [Description("Переключатель 'Работа/Контроль-I/Контроль-II'")]
    public enum TUMBLER_MODE
    {
        [Description("Работа")]
        WORK,
        [Description("Контроль-I")]
        CHECK_I,
        [Description("Контроль-II")]
        CHECK_II,
    }

    [Description("Тумблер 'Ступени-I'")]
    public enum TUMBLER_STAGE_1
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    [Description("Тумблер 'Ступени-II'")]
    public enum TUMBLER_STAGE_2
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    [Description("Тумблер 'Ступени-III'")]
    public enum TUMBLER_STAGE_3
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    [Description("Тумблер 'Ступени-IV'")]
    public enum TUMBLER_STAGE_4
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    #endregion

    public enum INDICATOR_STANDART
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    #region Тумблеры

    public EnumTumbler<TUMBLER_STATUS> tumbler_status = new EnumTumbler<TUMBLER_STATUS>(TUMBLER_STATUS.OFF);
    public EnumTumbler<TUMBLER_MODE>   tumbler_mode      = new EnumTumbler<TUMBLER_MODE>(TUMBLER_MODE.WORK);

    public EnumTumbler<TUMBLER_STAGE_1> tumbler_stage_1 = new EnumTumbler<TUMBLER_STAGE_1>(TUMBLER_STAGE_1.OFF);
    public EnumTumbler<TUMBLER_STAGE_2> tumbler_stage_2 = new EnumTumbler<TUMBLER_STAGE_2>(TUMBLER_STAGE_2.OFF);
    public EnumTumbler<TUMBLER_STAGE_3> tumbler_stage_3 = new EnumTumbler<TUMBLER_STAGE_3>(TUMBLER_STAGE_3.OFF);
    public EnumTumbler<TUMBLER_STAGE_4> tumbler_stage_4 = new EnumTumbler<TUMBLER_STAGE_4>(TUMBLER_STAGE_4.OFF);

    #endregion

    #region Индикаторы
    public EnumIndicator<INDICATOR_STANDART> indicator_status = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_STATUS", "Индикатор состояния прибора");
    #endregion

    #region Регуляторы
    public Spinner spinner_brightess = new Spinner("SPINNER_BRIGHTNESS", "Рег. яркости свечения ламп", 0, 100);    
    #endregion

    public List<Tumbler> GetTumblers()
    {
        return list_tumblers;
    }

    public List<Tumbler> GetIndicators()
    {
        return list_indicators;
    }

    public List<Spinner> GetSpinners()
    {
        return list_spinners;
    }

    public List<Joystick> GetJoysticks()
    {
        return new List<Joystick>();
    } 
}

[Serializable]
public class Strela10_ARC : PanelLibrary
{
    List<Tumbler> list_tumblers = new List<Tumbler>();
    List<Tumbler> list_indicators = new List<Tumbler>();
    List<Spinner> list_spinners = new List<Spinner>();

    public Strela10_ARC()
    {
        foreach (FieldInfo fieldInfo in this.GetType().GetFields())
        {
            if (fieldInfo.FieldType.ToString().Contains("EnumTumbler"))
                list_tumblers.Add((Tumbler)fieldInfo.GetValue(this));

            if (fieldInfo.FieldType.ToString().Contains("EnumIndicator"))
                list_indicators.Add((Tumbler)fieldInfo.GetValue(this));

            if (fieldInfo.FieldType.ToString().Contains("Spinner"))
                list_spinners.Add((Spinner)fieldInfo.GetValue(this));
        }
    }

    #region Тумблеры ENUM

    [Description("Кнопка включения")]
    public enum TUMBLER_A3
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    #endregion

    #region Тумблеры

    public EnumTumbler<TUMBLER_A3> tumbler_a3 = new EnumTumbler<TUMBLER_A3>(TUMBLER_A3.OFF);

    #endregion


    public enum INDICATOR_STANDART
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }


    #region Индикаторы
    public EnumIndicator<INDICATOR_STANDART> indicator_status = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_STATUS", "Индикатор состояния АРЦ");
    #endregion

    #region Регуляторы
    public Spinner spinner_brightess = new Spinner("SPINNER_BRIGHTNESS", "Рег. АРЦ", 0, 100);
    #endregion

    public List<Tumbler> GetTumblers()
    {
        return list_tumblers;
    }

    public List<Tumbler> GetIndicators()
    {
        return list_indicators;
    }

    public List<Spinner> GetSpinners()
    {
        return list_spinners;
    }

    public List<Joystick> GetJoysticks()
    {
        return new List<Joystick>();
    }
}

[Serializable]
public class Strela10_AzimuthIndicator : PanelLibrary
{
    List<Tumbler> list_tumblers = new List<Tumbler>();
    List<Tumbler> list_indicators = new List<Tumbler>();
    List<Spinner> list_spinners = new List<Spinner>();

    public Strela10_AzimuthIndicator()
    {
        foreach (FieldInfo fieldInfo in this.GetType().GetFields())
        {
            if (fieldInfo.FieldType.ToString().Contains("EnumTumbler"))
                list_tumblers.Add((Tumbler)fieldInfo.GetValue(this));

            if (fieldInfo.FieldType.ToString().Contains("EnumIndicator"))
                list_indicators.Add((Tumbler)fieldInfo.GetValue(this));

            if (fieldInfo.FieldType.ToString().Contains("Spinner"))
                list_spinners.Add((Spinner)fieldInfo.GetValue(this));
        }
    }

    #region Тумблеры ENUM

    [Description("Тумблер 'Подсветка шкалы'")]
    public enum TUMBLER_BACKLIGHT
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    [Description("Тумблер включения связи 'УА - ТНА3'")]
    public enum TUMBLER_C
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }
    #endregion

    public enum INDICATOR_STANDART
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    #region Тумблеры

    public EnumTumbler<TUMBLER_BACKLIGHT> tumbler_backlight = new EnumTumbler<TUMBLER_BACKLIGHT>(TUMBLER_BACKLIGHT.OFF);
    public EnumTumbler<TUMBLER_C> tumbler_c = new EnumTumbler<TUMBLER_C>(TUMBLER_C.OFF);

    #endregion

    #region Индикаторы
    public EnumIndicator<INDICATOR_STANDART> indicator_left = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_LEFT", "Указатель 'Цель слева'");
    public EnumIndicator<INDICATOR_STANDART> indicator_right = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_RIGHT", "Указатель 'Цель справа'");
    public EnumIndicator<INDICATOR_STANDART> indicator_forward = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_FORWARD", "Указатель 'Цель прямо'");
    
    public EnumIndicator<INDICATOR_STANDART> indicator_backlight = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_BACKLIGHT", "Подсветка шкалы");
    #endregion

    #region Регуляторы
    public Spinner spinner_tower_angle = new Spinner("SPINNER_TOWERANGLE", "Стрелка-указатель поворота башни (белая)", 0, 360);
    public Spinner spinner_launchers_pointer= new Spinner("SPINNER_ORIENTATION", "Стрелка-указатель ориентирования пусковой установки (черная)", 0, 360);

    public Spinner spinner_amplifier = new Spinner("SPINNER_AMPLIFIER", "Регулятор-усилитель", 0, 360);
    #endregion

    public List<Tumbler> GetTumblers()
    {
        return list_tumblers;
    }

    public List<Tumbler> GetIndicators()
    {
        return list_indicators;
    }

    public List<Spinner> GetSpinners()
    {
        return list_spinners;
    }

    public List<Joystick> GetJoysticks()
    {
        return new List<Joystick>();
    } 
}

[Serializable]
public class Strela10_SupportPanel : PanelLibrary
{
    List<Tumbler> list_tumblers = new List<Tumbler>();
    List<Tumbler> list_indicators = new List<Tumbler>();
    List<Spinner> list_spinners = new List<Spinner>();

    public Strela10_SupportPanel()
    {
        foreach (FieldInfo fieldInfo in this.GetType().GetFields())
        {
            if (fieldInfo.FieldType.ToString().Contains("EnumTumbler"))
                list_tumblers.Add((Tumbler)fieldInfo.GetValue(this));

            if (fieldInfo.FieldType.ToString().Contains("EnumIndicator"))
                list_indicators.Add((Tumbler)fieldInfo.GetValue(this));

            if (fieldInfo.FieldType.ToString().Contains("Spinner"))
                list_spinners.Add((Spinner)fieldInfo.GetValue(this));
        }
    }

    #region Тумблеры ENUM

    [Description("Тумблер 'Вентилятор'")]
    public enum TUMBLER_FAN
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    [Description("Тумблер 'Обогрев стекла'")]
    public enum TUMBLER_GLASS_HEATING
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    [Description("Тумблер 'Освещение'")]
    public enum TUMBLER_LIGHT
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    [Description("Тумблер 'Положение'")]
    public enum TUMBLER_POSITION
    {
        [Description("Боевой")]
        BATTLE,
        [Description("Походный")]
        STOWED,
    }

    [Description("Тумблер 'Слежение'")]
    public enum TUMBLER_TRACKING
    {
        [Description("Автомат.")]
        AUTO,
        [Description("Ручной")]
        MANUAL,
    }

    [Description("Тумблер 'Омыватель'")]
    public enum TUMBLER_CLEANER
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }
    #endregion

    //-------------------------------------------------------

    public enum INDICATOR_STANDART
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    #region Тумблеры

    public EnumTumbler<TUMBLER_GLASS_HEATING> tumbler_glass_heating = new EnumTumbler<TUMBLER_GLASS_HEATING>(TUMBLER_GLASS_HEATING.OFF);
    public EnumTumbler<TUMBLER_LIGHT> tumbler_light = new EnumTumbler<TUMBLER_LIGHT>(TUMBLER_LIGHT.OFF);
    public EnumTumbler<TUMBLER_FAN> tumbler_fan = new EnumTumbler<TUMBLER_FAN>(TUMBLER_FAN.OFF);
    public EnumTumbler<TUMBLER_CLEANER> tumbler_cleaner = new EnumTumbler<TUMBLER_CLEANER>(TUMBLER_CLEANER.OFF);


    public EnumTumbler<TUMBLER_POSITION> tumbler_position = new EnumTumbler<TUMBLER_POSITION>(TUMBLER_POSITION.BATTLE);
    public EnumTumbler<TUMBLER_TRACKING> tumbler_tracking = new EnumTumbler<TUMBLER_TRACKING>(TUMBLER_TRACKING.AUTO);
    #endregion

    #region Индикаторы
    public EnumIndicator<INDICATOR_STANDART> indicator_heating = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_GLASS_HEATING", "Обогрев стекла");
    #endregion


    public List<Tumbler> GetTumblers()
    {
        return list_tumblers;
    }

    public List<Tumbler> GetIndicators()
    {
        return list_indicators;
    }

    public List<Spinner> GetSpinners()
    {
        return list_spinners;
    }

    public List<Joystick> GetJoysticks()
    {
        return new List<Joystick>();
    } 
}

[Serializable]
public class Strela10_GuidancePanel : PanelLibrary
{
    List<Tumbler>  list_tumblers = new List<Tumbler>();
    List<Joystick> list_joysticks = new List<Joystick>();

    public Strela10_GuidancePanel()
    {
        foreach (FieldInfo fieldInfo in this.GetType().GetFields())
        {
            if (fieldInfo.FieldType.ToString().Contains("EnumTumbler"))
                list_tumblers.Add((Tumbler)fieldInfo.GetValue(this));

            if (fieldInfo.FieldType.ToString().Contains("Joystick"))
                list_joysticks.Add((Joystick)fieldInfo.GetValue(this));
        }
    }


    [Description("Тумблер 'Борт'")]
    public enum TUMBLER_BOARD
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    [Description("Тумблер 'Охлаждение'")]
    public enum TUMBLER_COOL
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    [Description("Тумблер 'Слежение-Пуск'")]
    public enum TUMBLER_TRACK_LAUNCH
    {
        [Description("Выкл")]
        OFF,
        [Description("Слежение")]
        TRACKING,
        [Description("Пуск")]
        LAUNCH,
    }

    [Description("Гашетка включения приводов наведения")]
    public enum TUMBLER_TRIGGER_DRIVE
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }

    public EnumTumbler<TUMBLER_COOL> tumbler_cool = new EnumTumbler<TUMBLER_COOL>(TUMBLER_COOL.OFF);
    public EnumTumbler<TUMBLER_BOARD> tumbler_board = new EnumTumbler<TUMBLER_BOARD>(TUMBLER_BOARD.OFF);
    public EnumTumbler<TUMBLER_TRACK_LAUNCH> tumbler_tl = new EnumTumbler<TUMBLER_TRACK_LAUNCH>(TUMBLER_TRACK_LAUNCH.OFF);
    public EnumTumbler<TUMBLER_TRIGGER_DRIVE> tumbler_trigger = new EnumTumbler<TUMBLER_TRIGGER_DRIVE>(TUMBLER_TRIGGER_DRIVE.OFF);

    public Joystick Operator_Joystick_Tower_Horizontal = new Joystick("OperatorJoystickHorizontal", "Джойстик горизонтального поворота башни");
    public Joystick Operator_Joystick_Tower_Vertical = new Joystick("OperatorJoystickVertical", "Джойстик вертикального поворота башни");
  

    public List<Tumbler> GetTumblers()
    {
        return list_tumblers;
    }

    public List<Tumbler> GetIndicators()
    {
        return new List<Tumbler>();
    }

    public List<Spinner> GetSpinners()
    {
        return new List<Spinner>();
    }

    public List<Joystick> GetJoysticks()
    {
        return list_joysticks;
    } 
}

[Serializable]
public class Strela10_VizorPanel : PanelLibrary
{
    List<Tumbler> list_tumblers = new List<Tumbler>();

    public Strela10_VizorPanel()
    {
        foreach (FieldInfo fieldInfo in this.GetType().GetFields())
        {
            if (fieldInfo.FieldType.ToString().Contains("EnumTumbler"))
                list_tumblers.Add((Tumbler)fieldInfo.GetValue(this));
        }
    }


    [Description("Тумблер 'Подсветка'")]
    public enum TUMBLER_ILLUM
    {
        [Description("Выкл")]
        OFF,
        [Description("Вкл")]
        ON,
    }
    public EnumTumbler<TUMBLER_ILLUM> tumbler_illum = new EnumTumbler<TUMBLER_ILLUM>(TUMBLER_ILLUM.OFF);


    public List<Tumbler> GetTumblers()
    {
        return list_tumblers;
    }

    public List<Tumbler> GetIndicators()
    {
        return new List<Tumbler>();
    }

    public List<Spinner> GetSpinners()
    {
        return new List<Spinner>();
    }

    public List<Joystick> GetJoysticks()
    {
        return new List<Joystick>();
    }
}