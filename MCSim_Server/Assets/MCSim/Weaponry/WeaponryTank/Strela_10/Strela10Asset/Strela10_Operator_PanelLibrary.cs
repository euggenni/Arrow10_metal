using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.ComponentModel;

[Serializable]
public class Strela10_Operator_PanelLibrary : Library
{
    // ������ ���� �������
    List<PanelLibrary> list_panels = new List<PanelLibrary>();

    [EnumDescription("������ ���������")]
    public Strela10_OperatorPanel operatorpanel = new Strela10_OperatorPanel();

    [EnumDescription("������ ������������ ����������")]
    public Strela10_OperationalPanel operationalpanel = new Strela10_OperationalPanel();

    [EnumDescription("���� ����������")]
    public Strela10_ControlBlock controlblockpanel = new Strela10_ControlBlock();

    [EnumDescription("��������� �������")]
    public Strela10_AzimuthIndicator azimuth_indicator = new Strela10_AzimuthIndicator();

    [EnumDescription("������ ���������")]
    public Strela10_SupportPanel support_panel = new Strela10_SupportPanel();

    [EnumDescription("������ ����������")]
    public Strela10_GuidancePanel joystick_panel = new Strela10_GuidancePanel();
    
    [EnumDescription("������ ������")]
    public Strela10_VizorPanel vizor_panel = new Strela10_VizorPanel();

    [EnumDescription("������ ���")]
    public Strela10_ARC arc_panel = new Strela10_ARC();
    
    public override string GetRole()
    {
        return "��������";
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

    #region ���������� ���� ������� � ������ � ������������

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

    #region �������� ENUM

    // ������� ��� - ���������� ������ ���� (Equipment of Area Assessment)
    [Description("���������� ������ ����")]
    public enum TUMBLER_AOZ
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON
    }

    [Description("������ - ����. - ������")]
    public enum TUMBLER_DRIVE_HANDLE_OFF
    {
        [Description("������")]
        MANUAL,
        [Description("������")]
        DRIVE,
        [Description("����")]
        OFF
    }

    [Description("���")]
    public enum TUMBLER_FON
    {
        [Description("I")]
        STATE_1,
        [Description("II")]
        STATE_2
    }

    [Description("��� ������")]
    public enum TUMBLER_WORK_TYPE
    {
        [Description("���.")]
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

    [Description("������� 24�")]
    public enum TUMBLER_POWER_24B
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    [Description("������� 28�")]
    public enum TUMBLER_POWER_28B
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    [Description("������� 30�")]
    public enum TUMBLER_POWER_30B
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    [Description("����� ������")]
    public enum TUMBLER_MODE
    {
        [Description("������")]
        COMBAT,
        [Description("�������")]
        TRAINING,
    }

    [Description("���")]
    public enum TUMBLER_PSP
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    [Description("����� ���")]
    public enum TUMBLER_PSP_MODE
    {
        [Description("��")]
        NP,
        [Description("��")]
        DP,
    }

    [Description("������ �����")]
    public enum TUMBLER_DROP
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    [Description("������ ���")]
    public enum TUMBLER_ACU
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    [Description("�� �� 1��246")]
    public enum TUMBLER_BL
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

      #endregion
    
    public enum INDICATOR_STANDART
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    #region ��������

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

    #region ����������
    public Spinner spinner_power_28B    = new Spinner("SPINNER_POWER_28B", "���.28�", 0, 100);
    public Spinner spinner_brightness   = new Spinner("SPINNER_BRIGHTNESS", "�������", 0, 100);
    public Spinner spinner_nvu_mode     = new Spinner("SPINNER_MVU_MODE", "����� ���", 0, 360);
    public Spinner spinner_voltmeter    = new Spinner("SPINNER_VOLTMETER", "���������", 0, 50);
    #endregion

    #region ����������
    public EnumIndicator<INDICATOR_STANDART> indicator_voltmeter_backlight = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "VOLTMETER_BACKLIGHT", "��������� ����������");

    public EnumIndicator<INDICATOR_STANDART> indicator_i = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_I", "������������ ����� (I)");

    public EnumIndicator<INDICATOR_STANDART> indicator_combat = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_COMBAT", "��������� '������'");
    public EnumIndicator<INDICATOR_STANDART> indicator_training = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_TRAINING", "��������� '�������'");

    public EnumIndicator<INDICATOR_STANDART> indicator_board = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_BOARD", "��������� '����'");

    public EnumIndicator<INDICATOR_STANDART> indicator_launcher_1 = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "LAUNCHER_1", "��������� '���� 1'");
    public EnumIndicator<INDICATOR_STANDART> indicator_launcher_2 = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "LAUNCHER_2", "��������� '���� 2'");
    public EnumIndicator<INDICATOR_STANDART> indicator_launcher_3 = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "LAUNCHER_3", "��������� '���� 3'");
    public EnumIndicator<INDICATOR_STANDART> indicator_launcher_4 = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "LAUNCHER_4", "��������� '���� 4'");

    public EnumIndicator<INDICATOR_STANDART> indicator_bl = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_BL", "��������� '�� �� 1��246'");

    public EnumIndicator<INDICATOR_STANDART> indicator_pz_ohl_1 = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "CONTR_PZ_OHL_1", "�����.��.���. I");
    public EnumIndicator<INDICATOR_STANDART> indicator_pz_ohl_2 = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "CONTR_PZ_OHL_2", "�����.��.���. II");

    public EnumIndicator<INDICATOR_STANDART> indicator_stowed = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_STOWED", "��������� '�����'");
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

    #region ��������

    [Description("������� ��������� ���")]
    public enum TUMBLER_AOZ
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    [Description("�����")]
    public enum TUMBLER_MODE
    {
        [Description("������")]
        COMBAT,
        [Description("�������")]
        TRAINING,
    }

    #endregion

    public enum INDICATOR_STANDART
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    #region ��������

    public EnumTumbler<TUMBLER_AOZ> tumbler_eaa = new EnumTumbler<TUMBLER_AOZ>(TUMBLER_AOZ.OFF);
    public EnumTumbler<TUMBLER_MODE> tumbler_fon = new EnumTumbler<TUMBLER_MODE>(TUMBLER_MODE.TRAINING);
    #endregion

    #region ����������
    public EnumIndicator<INDICATOR_STANDART> indicator_aoz = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_AOZ", "��������� ��������� ���");
    public EnumIndicator<INDICATOR_STANDART> indicator_radiation = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_RADIATION", "��������� '���������'");
    public EnumIndicator<INDICATOR_STANDART> indicator_battle = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_BATTLE", "��������� '������'");
    public EnumIndicator<INDICATOR_STANDART> indicator_training = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_TRAINING", "��������� '�������'");
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

    #region �������� ENUM

    [Description("������ ���������")]
    public enum TUMBLER_STATUS
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    [Description("������������� '������/��������-I/��������-II'")]
    public enum TUMBLER_MODE
    {
        [Description("������")]
        WORK,
        [Description("��������-I")]
        CHECK_I,
        [Description("��������-II")]
        CHECK_II,
    }

    [Description("������� '�������-I'")]
    public enum TUMBLER_STAGE_1
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    [Description("������� '�������-II'")]
    public enum TUMBLER_STAGE_2
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    [Description("������� '�������-III'")]
    public enum TUMBLER_STAGE_3
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    [Description("������� '�������-IV'")]
    public enum TUMBLER_STAGE_4
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    #endregion

    public enum INDICATOR_STANDART
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    #region ��������

    public EnumTumbler<TUMBLER_STATUS> tumbler_status = new EnumTumbler<TUMBLER_STATUS>(TUMBLER_STATUS.OFF);
    public EnumTumbler<TUMBLER_MODE>   tumbler_mode      = new EnumTumbler<TUMBLER_MODE>(TUMBLER_MODE.WORK);

    public EnumTumbler<TUMBLER_STAGE_1> tumbler_stage_1 = new EnumTumbler<TUMBLER_STAGE_1>(TUMBLER_STAGE_1.OFF);
    public EnumTumbler<TUMBLER_STAGE_2> tumbler_stage_2 = new EnumTumbler<TUMBLER_STAGE_2>(TUMBLER_STAGE_2.OFF);
    public EnumTumbler<TUMBLER_STAGE_3> tumbler_stage_3 = new EnumTumbler<TUMBLER_STAGE_3>(TUMBLER_STAGE_3.OFF);
    public EnumTumbler<TUMBLER_STAGE_4> tumbler_stage_4 = new EnumTumbler<TUMBLER_STAGE_4>(TUMBLER_STAGE_4.OFF);

    #endregion

    #region ����������
    public EnumIndicator<INDICATOR_STANDART> indicator_status = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_STATUS", "��������� ��������� �������");
    #endregion

    #region ����������
    public Spinner spinner_brightess = new Spinner("SPINNER_BRIGHTNESS", "���. ������� �������� ����", 0, 100);    
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

    #region �������� ENUM

    [Description("������ ���������")]
    public enum TUMBLER_A3
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    #endregion

    #region ��������

    public EnumTumbler<TUMBLER_A3> tumbler_a3 = new EnumTumbler<TUMBLER_A3>(TUMBLER_A3.OFF);

    #endregion


    public enum INDICATOR_STANDART
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }


    #region ����������
    public EnumIndicator<INDICATOR_STANDART> indicator_status = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_STATUS", "��������� ��������� ���");
    #endregion

    #region ����������
    public Spinner spinner_brightess = new Spinner("SPINNER_BRIGHTNESS", "���. ���", 0, 100);
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

    #region �������� ENUM

    [Description("������� '��������� �����'")]
    public enum TUMBLER_BACKLIGHT
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    [Description("������� ��������� ����� '�� - ���3'")]
    public enum TUMBLER_C
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }
    #endregion

    public enum INDICATOR_STANDART
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    #region ��������

    public EnumTumbler<TUMBLER_BACKLIGHT> tumbler_backlight = new EnumTumbler<TUMBLER_BACKLIGHT>(TUMBLER_BACKLIGHT.OFF);
    public EnumTumbler<TUMBLER_C> tumbler_c = new EnumTumbler<TUMBLER_C>(TUMBLER_C.OFF);

    #endregion

    #region ����������
    public EnumIndicator<INDICATOR_STANDART> indicator_left = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_LEFT", "��������� '���� �����'");
    public EnumIndicator<INDICATOR_STANDART> indicator_right = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_RIGHT", "��������� '���� ������'");
    public EnumIndicator<INDICATOR_STANDART> indicator_forward = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_FORWARD", "��������� '���� �����'");
    
    public EnumIndicator<INDICATOR_STANDART> indicator_backlight = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_BACKLIGHT", "��������� �����");
    #endregion

    #region ����������
    public Spinner spinner_tower_angle = new Spinner("SPINNER_TOWERANGLE", "�������-��������� �������� ����� (�����)", 0, 360);
    public Spinner spinner_launchers_pointer= new Spinner("SPINNER_ORIENTATION", "�������-��������� �������������� �������� ��������� (������)", 0, 360);

    public Spinner spinner_amplifier = new Spinner("SPINNER_AMPLIFIER", "���������-���������", 0, 360);
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

    #region �������� ENUM

    [Description("������� '����������'")]
    public enum TUMBLER_FAN
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    [Description("������� '������� ������'")]
    public enum TUMBLER_GLASS_HEATING
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    [Description("������� '���������'")]
    public enum TUMBLER_LIGHT
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    [Description("������� '���������'")]
    public enum TUMBLER_POSITION
    {
        [Description("������")]
        BATTLE,
        [Description("��������")]
        STOWED,
    }

    [Description("������� '��������'")]
    public enum TUMBLER_TRACKING
    {
        [Description("�������.")]
        AUTO,
        [Description("������")]
        MANUAL,
    }

    [Description("������� '���������'")]
    public enum TUMBLER_CLEANER
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }
    #endregion

    //-------------------------------------------------------

    public enum INDICATOR_STANDART
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    #region ��������

    public EnumTumbler<TUMBLER_GLASS_HEATING> tumbler_glass_heating = new EnumTumbler<TUMBLER_GLASS_HEATING>(TUMBLER_GLASS_HEATING.OFF);
    public EnumTumbler<TUMBLER_LIGHT> tumbler_light = new EnumTumbler<TUMBLER_LIGHT>(TUMBLER_LIGHT.OFF);
    public EnumTumbler<TUMBLER_FAN> tumbler_fan = new EnumTumbler<TUMBLER_FAN>(TUMBLER_FAN.OFF);
    public EnumTumbler<TUMBLER_CLEANER> tumbler_cleaner = new EnumTumbler<TUMBLER_CLEANER>(TUMBLER_CLEANER.OFF);


    public EnumTumbler<TUMBLER_POSITION> tumbler_position = new EnumTumbler<TUMBLER_POSITION>(TUMBLER_POSITION.BATTLE);
    public EnumTumbler<TUMBLER_TRACKING> tumbler_tracking = new EnumTumbler<TUMBLER_TRACKING>(TUMBLER_TRACKING.AUTO);
    #endregion

    #region ����������
    public EnumIndicator<INDICATOR_STANDART> indicator_heating = new EnumIndicator<INDICATOR_STANDART>(INDICATOR_STANDART.OFF, "INDICATOR_GLASS_HEATING", "������� ������");
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


    [Description("������� '����'")]
    public enum TUMBLER_BOARD
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    [Description("������� '����������'")]
    public enum TUMBLER_COOL
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    [Description("������� '��������-����'")]
    public enum TUMBLER_TRACK_LAUNCH
    {
        [Description("����")]
        OFF,
        [Description("��������")]
        TRACKING,
        [Description("����")]
        LAUNCH,
    }

    [Description("������� ��������� �������� ���������")]
    public enum TUMBLER_TRIGGER_DRIVE
    {
        [Description("����")]
        OFF,
        [Description("���")]
        ON,
    }

    public EnumTumbler<TUMBLER_COOL> tumbler_cool = new EnumTumbler<TUMBLER_COOL>(TUMBLER_COOL.OFF);
    public EnumTumbler<TUMBLER_BOARD> tumbler_board = new EnumTumbler<TUMBLER_BOARD>(TUMBLER_BOARD.OFF);
    public EnumTumbler<TUMBLER_TRACK_LAUNCH> tumbler_tl = new EnumTumbler<TUMBLER_TRACK_LAUNCH>(TUMBLER_TRACK_LAUNCH.OFF);
    public EnumTumbler<TUMBLER_TRIGGER_DRIVE> tumbler_trigger = new EnumTumbler<TUMBLER_TRIGGER_DRIVE>(TUMBLER_TRIGGER_DRIVE.OFF);

    public Joystick Operator_Joystick_Tower_Horizontal = new Joystick("OperatorJoystickHorizontal", "�������� ��������������� �������� �����");
    public Joystick Operator_Joystick_Tower_Vertical = new Joystick("OperatorJoystickVertical", "�������� ������������� �������� �����");
  

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


    [Description("������� '���������'")]
    public enum TUMBLER_ILLUM
    {
        [Description("����")]
        OFF,
        [Description("���")]
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