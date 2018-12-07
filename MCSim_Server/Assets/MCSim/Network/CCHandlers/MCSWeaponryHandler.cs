using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using MilitaryCombatSimulator;

/// <summary>
/// ���������� ������� Weaponry
/// </summary>
public class MCSWeaponryHandler {


    public MCSWeaponryHandler(MCSCommandCenter cc)    {
    }

    /// <summary>
    /// ������������ ������������� ���������, ������ ����������� �������� ��������� ��������� �������
    /// </summary>
    /// <param name="weaponry"></param>
    public void SynchronizeWeaponryControls(Weaponry weaponry)
    {
        List<PanelXMLData> panelList;

        foreach (MCSPlayer player in weaponry.GetOwners())
        {
            //try
            //{
            // ������ ������ ������
            panelList = new List<PanelXMLData>();

            CoreToolkit ct = weaponry.Core[player.Role] as CoreToolkit;

            foreach (ControlPanelToolkit panel in ct.Panels)
            {
                PanelXMLData panelData; // ������ � ������
                panelList.Add(panelData = new PanelXMLData(panel.GetName()));      // ��������� � ����

                foreach (SwitcherToolkit switcher in panel.SwitcherScripts)
                {
                    panelData.Data.Add(new PanelControlXMLData(switcher.GetName(),
                                                               switcher.TumblerStateID));
                }
            }

            string res = MCSSerializer.SerializeToString(panelList);
            Debug.Log(res);

            // �������� ������. �� ��� ���������� ���� ���� �� ����� �������, � ����������� ���� ��������
            //weaponry.Crew[player] = role;


            //}
            //catch (Exception e)
            //{
            //    Debug.LogError("�� ������� ���������������� �������� [" + weaponry.name + "] : " + e.Message +
            //                   " ojbect: " + e.Source);
            //}
        }
    }

    public string  MapperControl(string str){
        switch (str) {
            case "TUMBLER_POWER_24B":
                return "������� 24B";
            case "TUMBLER_POWER_28B": return "������� 28�";
            case "TUMBLER_DRIVE_HANDLE_OFF": return "������ - ����. - ������";
            case "TUMBLER_MODE": return "�����";
            case "PEDAL_AZIM": return "������ �� �������";
            case "TUMBLER_BOARD": return "������� ����";
            case "TUMBLER_POSITION": return "������� � ���������";
            case "TUMBLER_WORK_TYPE": return "����� ������";

        }
        return str;
    }
    public string MapperState(string str) {
        switch (str) {
            case "ON": return "���";
            case "OFF": return "����";
            case "MANUAL": return "������";
            case "DRIVE": return "������";
            case "BATTLE": return "������";
            case "STOWED": return "��������";
            case "AUTO": return "����";
            case "MODE_1": return "������ 1";
            case "MODE_2": return "������ 2";
            case "MODE_3": return "������ 3";
            case "MODE_4": return "������ 4";
            
        }
        return str;
    }
    private bool checkEnableLTCEnviroment()
    {

        return false;
    }

    public void Execute(NetworkPlayer sender, MCSCommand cmd) {
        
        if(cmd.Command.Equals("Execute"))
        {
            if (Network.isServer)
                MCSGlobalSimulation.Players.List[sender].Weaponry.Execute(cmd);

            // ���� ������ ��������� ������� � �������, �� ������ ���������� ���� ID Weaponry
            if (Network.isClient)
            {
                int weaponryID = (int)cmd.Args[0];

                if (MCSGlobalSimulation.Weapons.List.ContainsKey(weaponryID))
                {
                    MCSGlobalSimulation.Weapons.List[weaponryID].Execute(cmd);
                }
            }
        }
        
        if (cmd.Command.Equals("SetRole"))
        {
            if (Network.isClient)
            {
                //try {
                (MCSGlobalSimulation.Weapons.List[(int)cmd.Args[0]] as IWeaponryControl).SetRole(Network.player, cmd.Args[1].ToString());


                //}
                //catch (Exception e) { Debug.LogError("�� ������� ���������� ���� " + cmd.Args[0] + ". ������: " + e.Message); }
            }
        }

        if (cmd.Command.Equals("ControlChanged"))
        {
            int weaponryID = (int) cmd.Args[0];
            ControlType controlType = (ControlType) cmd.Args[1];
            string coreName = (string) cmd.Args[2];
            string panelName = (string) cmd.Args[3];
            string controlName = (string) cmd.Args[4];
            object controlState = cmd.Args[5];

            Debug.Log("[" + weaponryID + "]: ������� [" + OutputFileTextMapper.mapControlName(panelName, controlName) + "] �� ������ [" + OutputFileTextMapper.mapPanelName(panelName) + "] ������ ��������� [" + controlState + "] �������[" + Network.player.externalIP + "]");
            //MCSimActionListInfo.text += (Environment.UserDomainName + " - " + MapperControl(controlName) + " ��������� " + MapperState(controlState.ToString())+"\n");
            MCSimActionListInfo.text += "������� [" + Network.player.externalIP + "]: " + " ������� [" + OutputFileTextMapper.mapControlName(panelName, controlName) + "] �� ������ [" + OutputFileTextMapper.mapPanelName(panelName) + "] ������ ��������� [" + OutputFileTextMapper.mapStateName(controlState.ToString()) + "]" + "\n";
            if ("TUMBLER_ACU".Equals(controlName) && "OFF".Equals(controlState))
            {
                WeaponryTank_Strela10.towerBlocker = false;
            }
            else {
                if ("TUMBLER_ACU".Equals(controlName) && "ON".Equals(controlState)) {
                    WeaponryTank_Strela10.towerBlocker = true;
                }
            }
            try
            {
                Weaponry _weaponry;

                _weaponry = MCSGlobalSimulation.Weapons.List[weaponryID];

                if (_weaponry)
                {
                    try
                    {
                        PanelControl control = _weaponry.Core[coreName].GetPanel(panelName).GetControl(controlType,
                                                                                                       controlName);

                        control.State = controlState;

                        CoreToolkit ct = _weaponry.Core[coreName] as CoreToolkit;
                        ct.ControlChanged(control);
                    }
                    catch
                    {
                        Debug.LogError("�� ������� ���������� ������� [" + controlName + "] ���� [" + controlType + "] �� ������ [" + panelName + "]");
                    }
                }
                else
                {
                    Debug.LogWarning("�� ������ Weaponry � ID [" + weaponryID + "]");
                }
            }
            catch
            {
                Debug.Log("�� ������� ���������� Weaponry � ID [" + weaponryID + "]");
            }
        }
    }
}

public class OutputFileTextMapper {
    public static string mapPanelName(string name) {
        switch (name) {
            case "Strela10_OperatorPanel":
                return "����� ��������� ��-1";
            case "Strela10_OperationalPanel":
                return "����� ������������ ����������";
            case "Strela10_ControlBlock":
                return "���� ��������";
            case "Strela10_AzimuthIndicator":
                return "��������� �������";
            case "Strela10_SupportPanel":
                return "����� ��������� ��-2";
            case "Strela10_GuidancePanel":
                return "����� ���������";
            case "Strela10_VizorPanel":
                return "���������� �����";
            case "Strela10_ARC":
                return "���������� ���������� ������������";
            case "Strela10_SoundPanel":
                return "������ �����";
            case "Strela10_CommonPanel":
                return "�������������� ������";
        }

        return name;
    }

    public static string mapControlName(string panel, string control) {
        switch (panel) {
            case "Strela10_OperatorPanel":
                switch (control) {
                    case "TUMBLER_AOZ":
                        return "������� ���";
                    case "TUMBLER_FON":
                        return "������������� ���";
                    case "TUMBLER_DRIVE_HANDLE_OFF":
                        return "������������� ������-����-������";
                    case "TUMBLER_WORK_TYPE":
                        return "������������� ��� ������";
                    case "TUMBLER_POWER_24B":
                        return "������� ������� 24 �";
                    case "TUMBLER_POWER_28B":
                        return "������� ������� 28 �";
                    case "TUMBLER_POWER_30B":
                        return "������ �������� 30 �";
                    case "TUMBLER_MODE":
                        return "������� �����";
                    case "TUMBLER_PSP":
                        return "������� ���";
                    case "TUMBLER_PSP_MODE":
                        return "������������� ����� ���";
                    case "TUMBLER_DROP":
                        return "���������� ���������� ������ �����";
                    case "TUMBLER_ACU":
                        return "������ ���";
                    case "TUMBLER_BL":
                        return "������� �� �� 1��246";
                    case "TUMBLER_SS":
                        return "������� ��";
                    case "TUMBLER_TEST":
                        return "������� ����";
                }
                break;
            //case "Strela10_OperationalPanel":
            //    return "����� ������������ ����������";
            //case "Strela10_ControlBlock":
            //    return "���� ��������";
            //case "Strela10_AzimuthIndicator":
            //    return "��������� �������";
            case "Strela10_SupportPanel":
                switch (control) {
                    case "TUMBLER_GLASS_HEATING":
                        return "������� �������� ������";
                    case "TUMBLER_LIGHT":
                        return "������� ���������";
                    case "TUMBLER_FAN":
                        return "������� �����������";
                    case "TUMBLER_CLEANER":
                        return "������ ���������";
                    case "TUMBLER_POSITION":
                        return "������� ������-�������";
                    case "TUMBLER_TRACKING":
                        return "������� �������� ������-�������";
                }
                break;
            case "Strela10_GuidancePanel":
                switch (control) {
                    case "TUMBLER_COOL":
                        return "������� ����������";
                    case "TUMBLER_BOARD":
                        return "������ ����";
                    case "TUMBLER_TRACK_LAUNCH":
                        return "������ ��������-����";
                    case "TUMBLER_TRIGGER_DRIVE":
                        return "�������";
                }
                break;
            //case "Strela10_VizorPanel":
            //    return "���������� �����";
            //case "Strela10_ARC":
            //    return "���������� ���������� ������������";
            //case "Strela10_SoundPanel":
            //    return "������ �����";
            case "Strela10_CommonPanel":
                switch (control) {
                    case "PEDAL_AZIM":
                        return "������ ������� ������";
                }
                break;
        }

        return control;
    }

    public static string mapStateName(string state) {
        switch (state) {
            case "ON":
                return "���";
            case "OFF":
                return "����";
            case "TRACKING":
                return "��������";
            case "LAUNCH":
                return "����";
            case "BATTLE":
                return "������";
            case "STOWED":
                return "��������";
            case "AUTO":
                return "����";
            case "MANUAL":
                return "������";
            case "STATE_1":
                return "1";
            case "STATE_2":
                return "2";
            case "DRIVE":
                return "������";
            case "MODE_1":
                return "���� 1";
            case "MODE_2":
                return "���� 2";
            case "MODE_3":
                return "���� 3";
            case "MODE_4":
                return "���� 4";
            case "COMBAT":
                return "������";
            case "TRAINING":
                return "��������";
            case "NP":
                return "��";
            case "DP":
                return "��";
        }

        return state;
    }
}

//public class EnableTLC
//{
//    public string Player;
//    public string WeaponryID;
//    public bool Cool;
//    public bool Psp;
//    public bool PSPState;

//    public static EnableTLC getInstance()
//    {
//       return new EnableTLC();
//    }
//}
