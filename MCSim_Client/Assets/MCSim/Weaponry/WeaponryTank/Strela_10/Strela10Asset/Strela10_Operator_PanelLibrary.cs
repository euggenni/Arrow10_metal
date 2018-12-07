using System;
using System.ComponentModel;

[Serializable]
public class Strela10_Operator_PanelLibrary : Library {
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

    [EnumDescription("Суммирующий усилитель")]
    public Strela10_SoundPanel sound_panel = new Strela10_SoundPanel();

    [EnumDescription("Общая панель")]
    public Strela10_CommonPanel common_panel = new Strela10_CommonPanel();

    public override string Name {
        get { return "Strela-10_Operator"; }
    }
}

[Serializable]
public class Strela10_OperatorPanel : PanelLibrary {
    #region Тумблеры ENUM

    // Тумблер АОЗ - Аппаратура Оценки Зоны (Equipment of Area Assessment)
    [Description("Аппаратура оценки зоны")]
    public enum TUMBLER_AOZ {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON
    }

    [Description("Привод - Выкл. - Ручное")]
    public enum TUMBLER_DRIVE_HANDLE_OFF {
        [Description("Ручное")]
        MANUAL,

        [Description("Привод")]
        DRIVE,

        [Description("Выкл")]
        OFF
    }

    [Description("Фон")]
    public enum TUMBLER_FON {
        [Description("I")]
        STATE_1,

        [Description("II")]
        STATE_2
    }

    [Description("Активная ракета")]
    public enum TUMBLER_WORK_TYPE {
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
    public enum TUMBLER_POWER_24B {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Питание 28В")]
    public enum TUMBLER_POWER_28B {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Питание 30В")]
    public enum TUMBLER_POWER_30B {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Режим работы")]
    public enum TUMBLER_MODE {
        [Description("Боевой")]
        COMBAT,

        [Description("Учебный")]
        TRAINING,
    }

    [Description("ПСП")]
    public enum TUMBLER_PSP {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Режим ПСП")]
    public enum TUMBLER_PSP_MODE {
        [Description("НП")]
        NP,

        [Description("ДП")]
        DP,
    }

    [Description("Кнопка Сброс")]
    public enum TUMBLER_DROP {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Кнопка АЦУ")]
    public enum TUMBLER_ACU {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("БЛ по 1РЛ246")]
    public enum TUMBLER_BL {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Тумблер СС")]
    public enum TUMBLER_SS {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Тумбер 'Испыт.-Основ.'")]
    public enum TUMBLER_TEST {
        [Description("Испыт")]
        OFF,

        [Description("Основ")]
        ON
    }

    #endregion

    #region Тумблеры

    public EnumTumbler<TUMBLER_AOZ> tumbler_eaa = new EnumTumbler<TUMBLER_AOZ>(TUMBLER_AOZ.ON);
    public EnumTumbler<TUMBLER_FON> tumbler_fon = new EnumTumbler<TUMBLER_FON>(TUMBLER_FON.STATE_1);

    public EnumTumbler<TUMBLER_DRIVE_HANDLE_OFF> tumbler_mdo =
        new EnumTumbler<TUMBLER_DRIVE_HANDLE_OFF>(TUMBLER_DRIVE_HANDLE_OFF.DRIVE);

    public EnumTumbler<TUMBLER_WORK_TYPE> tumbler_workmode = new EnumTumbler<TUMBLER_WORK_TYPE>(TUMBLER_WORK_TYPE.AUTO);

    public EnumTumbler<TUMBLER_POWER_24B> tumbler_power_24b = new EnumTumbler<TUMBLER_POWER_24B>(TUMBLER_POWER_24B.OFF);
    public EnumTumbler<TUMBLER_POWER_28B> tumbler_power_28b = new EnumTumbler<TUMBLER_POWER_28B>(TUMBLER_POWER_28B.OFF);
    public EnumTumbler<TUMBLER_POWER_30B> tumbler_power_30b = new EnumTumbler<TUMBLER_POWER_30B>(TUMBLER_POWER_30B.OFF);

    public EnumTumbler<TUMBLER_MODE> tumbler_mode = new EnumTumbler<TUMBLER_MODE>(TUMBLER_MODE.TRAINING);
    public EnumTumbler<TUMBLER_PSP> tumbler_psp = new EnumTumbler<TUMBLER_PSP>(TUMBLER_PSP.OFF);
    public EnumTumbler<TUMBLER_PSP_MODE> tumbler_psp_mode = new EnumTumbler<TUMBLER_PSP_MODE>(TUMBLER_PSP_MODE.NP);

    public EnumTumbler<TUMBLER_DROP> tumbler_drop = new EnumTumbler<TUMBLER_DROP>(TUMBLER_DROP.OFF);
    public EnumTumbler<TUMBLER_ACU> tumbler_acu = new EnumTumbler<TUMBLER_ACU>(TUMBLER_ACU.OFF);
    public EnumTumbler<TUMBLER_BL> tumbler_bl = new EnumTumbler<TUMBLER_BL>(TUMBLER_BL.OFF);
    public EnumTumbler<TUMBLER_SS> tumbler_ss = new EnumTumbler<TUMBLER_SS>(TUMBLER_SS.OFF);
    public EnumTumbler<TUMBLER_TEST> tumbler_test = new EnumTumbler<TUMBLER_TEST>(TUMBLER_TEST.OFF);

    #endregion

    #region Регуляторы

    //public Spinner spinner_power_28B    = new Spinner("SPINNER_POWER_28B", "Рег.28В", 0, 100);
    public Spinner spinner_brightness = new Spinner("SPINNER_BRIGHTNESS", "Яркость", 0, 100);

    //public Spinner spinner_nvu_mode     = new Spinner("SPINNER_MVU_MODE", "Режим НВУ", 0, 360);
    public Spinner spinner_voltmeter = new Spinner("SPINNER_VOLTMETER", "Вольтметр", 0, 50);

    #endregion

    #region Индикаторы

    public EnumIndicator<TOGGLE_STANDART> indicator_voltmeter_backlight =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "VOLTMETER_BACKLIGHT", "Подсветка вольтметра");

    public EnumIndicator<TOGGLE_STANDART> indicator_nvu =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "NVU_MODE", "Режим НВУ");

    public EnumIndicator<TOGGLE_STANDART> indicator_i =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_I", "Инфракрасный канал (I)");

    public EnumIndicator<TOGGLE_STANDART> indicator_combat =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_COMBAT", "Индикатор 'Боевой'");

    public EnumIndicator<TOGGLE_STANDART> indicator_training =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_TRAINING", "Индикатор 'Учебный'");

    public EnumIndicator<TOGGLE_STANDART> indicator_board =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_BOARD", "Индикатор 'Борт'");

    public EnumIndicator<TOGGLE_STANDART> indicator_launcher_1 =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "LAUNCHER_1", "Индикатор 'Пост 1'");

    public EnumIndicator<TOGGLE_STANDART> indicator_launcher_2 =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "LAUNCHER_2", "Индикатор 'Пост 2'");

    public EnumIndicator<TOGGLE_STANDART> indicator_launcher_3 =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "LAUNCHER_3", "Индикатор 'Пост 3'");

    public EnumIndicator<TOGGLE_STANDART> indicator_launcher_4 =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "LAUNCHER_4", "Индикатор 'Пост 4'");

    public EnumIndicator<TOGGLE_STANDART> indicator_bl =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_BL", "Индикатор 'БЛ по 1РЛ246'");

    public EnumIndicator<TOGGLE_STANDART> indicator_check =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "CHECK", "Контроль");

    public EnumIndicator<TOGGLE_STANDART> indicator_pz_ohl =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "PZ_OHL", "Пз. Охл");

    public EnumIndicator<TOGGLE_STANDART> indicator_stowed =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_STOWED", "Индикатор 'Поход'");

    #endregion
}

[Serializable]
public class Strela10_OperationalPanel : PanelLibrary {
    #region Тумблеры

    [Description("Тумблер состояния АОЗ")]
    public enum TUMBLER_AOZ {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Режим")]
    public enum TUMBLER_MODE {
        [Description("Боевой")]
        COMBAT,

        [Description("Дежурный")]
        TRAINING,
    }

    #endregion

    #region Тумблеры

    public EnumTumbler<TUMBLER_AOZ> tumbler_eaa = new EnumTumbler<TUMBLER_AOZ>(TUMBLER_AOZ.OFF);
    public EnumTumbler<TUMBLER_MODE> tumbler_fon = new EnumTumbler<TUMBLER_MODE>(TUMBLER_MODE.TRAINING);

    #endregion

    #region Индикаторы

    public EnumIndicator<TOGGLE_STANDART> indicator_aoz =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_AOZ", "Индикатор состояния АОЗ");

    public EnumIndicator<TOGGLE_STANDART> indicator_radiation =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_RADIATION", "Индикатор 'Излучение'");

    public EnumIndicator<TOGGLE_STANDART> indicator_battle =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_BATTLE", "Индикатор 'Боевой'");

    public EnumIndicator<TOGGLE_STANDART> indicator_training =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_TRAINING", "Индикатор 'Учебный'");

    #endregion
}

[Serializable]
public class Strela10_ControlBlock : PanelLibrary {
    #region Тумблеры ENUM

    [Description("Кнопка включения")]
    public enum TUMBLER_STATUS {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Переключатель 'Работа/Контроль-I/Контроль-II'")]
    public enum TUMBLER_MODE {
        [Description("Работа")]
        WORK,

        [Description("Контроль-I")]
        CHECK_I,

        [Description("Контроль-II")]
        CHECK_II,
    }

    [Description("Тумблер 'Ступени-I'")]
    public enum TUMBLER_STAGE_1 {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Тумблер 'Ступени-II'")]
    public enum TUMBLER_STAGE_2 {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Тумблер 'Ступени-III'")]
    public enum TUMBLER_STAGE_3 {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Тумблер 'Ступени-IV'")]
    public enum TUMBLER_STAGE_4 {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    #endregion

    #region Тумблеры

    public EnumTumbler<TUMBLER_STATUS> tumbler_status = new EnumTumbler<TUMBLER_STATUS>(TUMBLER_STATUS.OFF);
    public EnumTumbler<TUMBLER_MODE> tumbler_mode = new EnumTumbler<TUMBLER_MODE>(TUMBLER_MODE.WORK);

    public EnumTumbler<TUMBLER_STAGE_1> tumbler_stage_1 = new EnumTumbler<TUMBLER_STAGE_1>(TUMBLER_STAGE_1.OFF);
    public EnumTumbler<TUMBLER_STAGE_2> tumbler_stage_2 = new EnumTumbler<TUMBLER_STAGE_2>(TUMBLER_STAGE_2.OFF);
    public EnumTumbler<TUMBLER_STAGE_3> tumbler_stage_3 = new EnumTumbler<TUMBLER_STAGE_3>(TUMBLER_STAGE_3.OFF);
    public EnumTumbler<TUMBLER_STAGE_4> tumbler_stage_4 = new EnumTumbler<TUMBLER_STAGE_4>(TUMBLER_STAGE_4.OFF);

    #endregion

    #region Индикаторы

    public EnumIndicator<TOGGLE_STANDART> indicator_status =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_STATUS", "Индикатор состояния прибора");

    #endregion

    #region Регуляторы

    public Spinner spinner_brightess = new Spinner("SPINNER_BRIGHTNESS", "Рег. яркости свечения ламп", 0, 100);

    #endregion
}

[Serializable]
public class Strela10_ARC : PanelLibrary {
    #region Тумблеры ENUM

    [Description("Кнопка включения")]
    public enum TUMBLER_A3 {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    #endregion

    #region Тумблеры

    public EnumTumbler<TUMBLER_A3> tumbler_a3 = new EnumTumbler<TUMBLER_A3>(TUMBLER_A3.OFF);

    #endregion

    #region Индикаторы

    public EnumIndicator<TOGGLE_STANDART> indicator_status =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_STATUS", "Индикатор состояния АРЦ");

    #endregion

    #region Регуляторы

    public Spinner spinner_brightess = new Spinner("SPINNER_BRIGHTNESS", "Рег. АРЦ", 0, 100);

    #endregion
}

[Serializable]
public class Strela10_AzimuthIndicator : PanelLibrary {
    #region Тумблеры ENUM

    [Description("Тумблер 'Подсветка шкалы'")]
    public enum TUMBLER_BACKLIGHT {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Тумблер включения связи 'УА - ТНА3'")]
    public enum TUMBLER_C {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    #endregion


    #region Тумблеры

    public EnumTumbler<TUMBLER_BACKLIGHT> tumbler_backlight = new EnumTumbler<TUMBLER_BACKLIGHT>(TUMBLER_BACKLIGHT.OFF);
    public EnumTumbler<TUMBLER_C> tumbler_c = new EnumTumbler<TUMBLER_C>(TUMBLER_C.OFF);

    #endregion

    #region Индикаторы

    public EnumIndicator<TOGGLE_STANDART> indicator_left =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_LEFT", "Указатель 'Цель слева'");

    public EnumIndicator<TOGGLE_STANDART> indicator_right =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_RIGHT", "Указатель 'Цель справа'");

    public EnumIndicator<TOGGLE_STANDART> indicator_forward =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_FORWARD", "Указатель 'Цель прямо'");

    public EnumIndicator<TOGGLE_STANDART> indicator_backlight =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_BACKLIGHT", "Подсветка шкалы");

    #endregion

    #region Регуляторы

    public Spinner spinner_tower_angle =
        new Spinner("SPINNER_TOWERANGLE", "Стрелка-указатель поворота башни (белая)", 0, 360);

    public Spinner spinner_launchers_pointer = new Spinner("SPINNER_ORIENTATION",
        "Стрелка-указатель ориентирования пусковой установки (черная)", 0, 360);

    public Spinner spinner_amplifier = new Spinner("SPINNER_AMPLIFIER", "Регулятор-усилитель", 0, 360);

    #endregion
}

[Serializable]
public class Strela10_SupportPanel : PanelLibrary {
    #region Тумблеры ENUM

    //[Description("Тумблер 'Вентилятор'")]
    [Description("Оптический визир")]
    public enum TUMBLER_FAN {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Тумблер 'Обогрев стекла'")]
    public enum TUMBLER_GLASS_HEATING {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Тумблер 'Освещение'")]
    public enum TUMBLER_LIGHT {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Тумблер 'Положение'")]
    public enum TUMBLER_POSITION {
        [Description("Боевой")]
        BATTLE,

        [Description("Походный")]
        STOWED,
    }

    [Description("Тумблер 'Слежение'")]
    public enum TUMBLER_TRACKING {
        [Description("Автомат.")]
        AUTO,

        [Description("Ручной")]
        MANUAL,
    }

    [Description("Тумблер 'Омыватель'")]
    public enum TUMBLER_CLEANER {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    #endregion

    #region Тумблеры

    public EnumTumbler<TUMBLER_GLASS_HEATING> tumbler_glass_heating =
        new EnumTumbler<TUMBLER_GLASS_HEATING>(TUMBLER_GLASS_HEATING.OFF);

    public EnumTumbler<TUMBLER_LIGHT> tumbler_light = new EnumTumbler<TUMBLER_LIGHT>(TUMBLER_LIGHT.OFF);
    public EnumTumbler<TUMBLER_FAN> tumbler_fan = new EnumTumbler<TUMBLER_FAN>(TUMBLER_FAN.OFF);
    public EnumTumbler<TUMBLER_CLEANER> tumbler_cleaner = new EnumTumbler<TUMBLER_CLEANER>(TUMBLER_CLEANER.OFF);


    public EnumTumbler<TUMBLER_POSITION> tumbler_position = new EnumTumbler<TUMBLER_POSITION>(TUMBLER_POSITION.BATTLE);
    public EnumTumbler<TUMBLER_TRACKING> tumbler_tracking = new EnumTumbler<TUMBLER_TRACKING>(TUMBLER_TRACKING.AUTO);

    #endregion

    #region Индикаторы

    public EnumIndicator<TOGGLE_STANDART> indicator_heating =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_GLASS_HEATING", "Обогрев стекла");

    #endregion
}

[Serializable]
public class Strela10_GuidancePanel : PanelLibrary {
    [Description("Тумблер 'Борт'")]
    public enum TUMBLER_BOARD {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Тумблер 'Охлаждение'")]
    public enum TUMBLER_COOL {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Тумблер 'Слежение-Пуск'")]
    public enum TUMBLER_TRACK_LAUNCH {
        [Description("Выкл")]
        OFF,

        [Description("Слежение")]
        TRACKING,

        [Description("Пуск")]
        LAUNCH,
    }

    [Description("Гашетка включения приводов наведения")]
    public enum TUMBLER_TRIGGER_DRIVE {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    public EnumTumbler<TUMBLER_COOL> tumbler_cool = new EnumTumbler<TUMBLER_COOL>(TUMBLER_COOL.OFF);
    public EnumTumbler<TUMBLER_BOARD> tumbler_board = new EnumTumbler<TUMBLER_BOARD>(TUMBLER_BOARD.OFF);

    public EnumTumbler<TUMBLER_TRACK_LAUNCH> tumbler_tl =
        new EnumTumbler<TUMBLER_TRACK_LAUNCH>(TUMBLER_TRACK_LAUNCH.OFF);

    public EnumTumbler<TUMBLER_TRIGGER_DRIVE> tumbler_trigger =
        new EnumTumbler<TUMBLER_TRIGGER_DRIVE>(TUMBLER_TRIGGER_DRIVE.OFF);

    public Joystick Operator_Joystick_Tower_Horizontal =
        new Joystick("OperatorJoystickHorizontal", "Джойстик горизонтального поворота башни");

    public Joystick Operator_Joystick_Tower_Vertical =
        new Joystick("OperatorJoystickVertical", "Джойстик вертикального поворота башни");
}

[Serializable]
public class Strela10_VizorPanel : PanelLibrary {
    [Description("Тумблер 'Подсветка'")]
    public enum TUMBLER_ILLUM {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Переключатель режимов'")]
    public enum TUMBLER_POSITION_WORK_TYPE {
        [Description("-50 -10")]
        POS_1,

        [Description("-10 +30")]
        POS_2,

        [Description("+30 +50")]
        POS_3,

        [Description("КОНТР. ВЕНТ")]
        POS_4,

        [Description("МАНЕВР")]
        POS_5,

        [Description("КОНТР. ПОМЕХ")]
        POS_6,

        [Description("КОНТР. ВМП")]
        POS_7,

        [Description("КОНТР. НС")]
        POS_8,
    }

    [Description("Переключатель включение питания")]
    public enum TUMBLER_POWER_NRZ {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Переключатель работы нрз")]
    public enum TUMBLER_WORK_NRZ {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Переключатель ВГ")]
    public enum TUMBLER_VV {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Переключатель ВВ")]
    public enum TUMBLER_СС {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Переключатель ВП")]
    public enum TUMBLER_СС2 {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Ложный тумблер")]
    public enum TUMBLER_LOSE {
        [Description("Вкл")]
        ON,
    }

    [Description("Переключатель кодов")]
    public enum TUMBLER_CODE_POSITION {
        [Description("0")]
        P_0,

        [Description("1")]
        P_1,

        [Description("2")]
        P_2,

        [Description("3")]
        P_3,

        [Description("4")]
        P_4,

        [Description("5")]
        P_5,

        [Description("6")]
        P_6,
    }

    [Description("Светофильтры верхний тумблер")]
    public enum TUMBLER_FILTER_UP {
        [Description("Первое положение")]
        LEFT,

        [Description("Среднее положение")]
        CENTER,

        [Description("Крайнее положение")]
        RIGTH
    }

    [Description("Светофильтры ZOOM")]
    public enum TUMBLER_FILTER_DOWN {
        [Description("X2")]
        X_2,

        [Description("X4")]
        X_4,
    }

    [Description("КОНТРОЛЬ ВЗ")]
    public enum TUMBLER_VZ {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("КОНТРОЛЬ СД")]
    public enum TUMBLER_SD {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("КОНТРОЛЬ УПЧ")]
    public enum TUMBLER_YPCH {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }


    public EnumTumbler<TUMBLER_ILLUM> tumbler_illum = new EnumTumbler<TUMBLER_ILLUM>(TUMBLER_ILLUM.OFF);

    public EnumTumbler<TUMBLER_POSITION_WORK_TYPE> tumbler_positon_work =
        new EnumTumbler<TUMBLER_POSITION_WORK_TYPE>(TUMBLER_POSITION_WORK_TYPE.POS_1);

    public EnumTumbler<TUMBLER_POWER_NRZ> tumbler_nrz = new EnumTumbler<TUMBLER_POWER_NRZ>(TUMBLER_POWER_NRZ.OFF);
    public EnumTumbler<TUMBLER_WORK_NRZ> tumbler_work_nrz = new EnumTumbler<TUMBLER_WORK_NRZ>(TUMBLER_WORK_NRZ.OFF);
    public EnumTumbler<TUMBLER_VV> tumbler_vv = new EnumTumbler<TUMBLER_VV>(TUMBLER_VV.OFF);
    public EnumTumbler<TUMBLER_СС> tumbler_cc = new EnumTumbler<TUMBLER_СС>(TUMBLER_СС.OFF);
    public EnumTumbler<TUMBLER_СС2> tumbler_cc2 = new EnumTumbler<TUMBLER_СС2>(TUMBLER_СС2.OFF);
    public EnumTumbler<TUMBLER_LOSE> tumbler_lose = new EnumTumbler<TUMBLER_LOSE>(TUMBLER_LOSE.ON);

    public EnumTumbler<TUMBLER_CODE_POSITION> tumbler_pos =
        new EnumTumbler<TUMBLER_CODE_POSITION>(TUMBLER_CODE_POSITION.P_1);

    public EnumTumbler<TUMBLER_FILTER_UP> tumbler_filter = new EnumTumbler<TUMBLER_FILTER_UP>(TUMBLER_FILTER_UP.LEFT);
    public EnumTumbler<TUMBLER_FILTER_DOWN> tumbler_x2 = new EnumTumbler<TUMBLER_FILTER_DOWN>(TUMBLER_FILTER_DOWN.X_2);
    public EnumTumbler<TUMBLER_VZ> tumbler_vz = new EnumTumbler<TUMBLER_VZ>(TUMBLER_VZ.OFF);
    public EnumTumbler<TUMBLER_SD> tumbler_sd = new EnumTumbler<TUMBLER_SD>(TUMBLER_SD.OFF);
    public EnumTumbler<TUMBLER_YPCH> tumbler_ypch = new EnumTumbler<TUMBLER_YPCH>(TUMBLER_YPCH.OFF);

    public EnumIndicator<TOGGLE_STANDART> indicator_heating =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_U_AMPER", "Режим измерения");

    public EnumIndicator<TOGGLE_STANDART> indicator_center =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_U_CENTER", "ЗОНА");

    public EnumIndicator<TOGGLE_STANDART> indicator_back =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_U_BACK", "НАЗАД");

    public EnumIndicator<TOGGLE_STANDART> indicator_pv =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_U_PV", "ПВ");

    public EnumIndicator<TOGGLE_STANDART> indicator_vent =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_U_VENT", "КОНТРОЛЬ ВЕНТ");

    public EnumIndicator<TOGGLE_STANDART> indicator_nc =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_U_NC", "КОНТРОЛЬ НС");

    public EnumIndicator<TOGGLE_STANDART> indicator_115 =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_U_115", "115В");

    public EnumIndicator<TOGGLE_STANDART> indicator_nrz_power =
        new EnumIndicator<TOGGLE_STANDART>(TOGGLE_STANDART.OFF, "INDICATOR_U_NRZ_POWER", "НРЗ готов");
}

[Serializable]
public class Strela10_SoundPanel : PanelLibrary {
    public Spinner spinner_soundCo = new Spinner("SPINNER_SOUNDCO", "Звук ЦО", 0, 360);
    public Spinner spinner_speech = new Spinner("SPINNER_SPEECH", "Речь", 0, 360);
    public Spinner spinner_amplifier = new Spinner("SPINNER_AMPLIFIER", "Усиление", 0, 360);
}

[Serializable]
public class Strela10_CommonPanel : PanelLibrary {
    [Description("Ножная педаль")]
    public enum TUMBLER_PEDAL {
        [Description("Выкл")]
        OFF,

        [Description("Вкл")]
        ON,
    }

    [Description("Ручной ......")]
    public enum HANDL {
        [Description("Закрыто")]
        OFF,

        [Description("Открыто")]
        ON,
    }

    [Description("Стопор по Азимуму")]
    public enum PEDAL_AZIM {
        [Description("Застопорено")]
        OFF,

        [Description("Снять стопор")]
        ON,
    }

    [Description("Стопор по азимуту ")]
    public enum TUMBLER_STOP {
        [Description("Растопорено")]
        OFF,

        [Description("Застопорено")]
        ON,
    }

    public EnumTumbler<TUMBLER_PEDAL> tumbler_illum = new EnumTumbler<TUMBLER_PEDAL>(TUMBLER_PEDAL.OFF);
    public EnumTumbler<HANDL> tumbler_handl = new EnumTumbler<HANDL>(HANDL.OFF);
    public EnumTumbler<PEDAL_AZIM> tumbler_Azim = new EnumTumbler<PEDAL_AZIM>(PEDAL_AZIM.OFF);
    public EnumTumbler<TUMBLER_STOP> tumbler_stop = new EnumTumbler<TUMBLER_STOP>(TUMBLER_STOP.OFF);
}