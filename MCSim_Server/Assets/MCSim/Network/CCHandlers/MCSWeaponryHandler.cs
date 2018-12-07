using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using MilitaryCombatSimulator;

/// <summary>
/// Обработчик событий Weaponry
/// </summary>
public class MCSWeaponryHandler {


    public MCSWeaponryHandler(MCSCommandCenter cc)    {
    }

    /// <summary>
    /// Производится синхронизация контролов, клиент присваивает значения контролов серверной стороны
    /// </summary>
    /// <param name="weaponry"></param>
    public void SynchronizeWeaponryControls(Weaponry weaponry)
    {
        List<PanelXMLData> panelList;

        foreach (MCSPlayer player in weaponry.GetOwners())
        {
            //try
            //{
            // Список данных панели
            panelList = new List<PanelXMLData>();

            CoreToolkit ct = weaponry.Core[player.Role] as CoreToolkit;

            foreach (ControlPanelToolkit panel in ct.Panels)
            {
                PanelXMLData panelData; // Данные о панели
                panelList.Add(panelData = new PanelXMLData(panel.GetName()));      // Добавляем в лист

                foreach (SwitcherToolkit switcher in panel.SwitcherScripts)
                {
                    panelData.Data.Add(new PanelControlXMLData(switcher.GetName(),
                                                               switcher.TumblerStateID));
                }
            }

            string res = MCSSerializer.SerializeToString(panelList);
            Debug.Log(res);

            // Отослать игроку. Он сам определяет свою роль на своей стороне, и присваивает себе значения
            //weaponry.Crew[player] = role;


            //}
            //catch (Exception e)
            //{
            //    Debug.LogError("Не удалось синхронизировать контролы [" + weaponry.name + "] : " + e.Message +
            //                   " ojbect: " + e.Source);
            //}
        }
    }

    public string  MapperControl(string str){
        switch (str) {
            case "TUMBLER_POWER_24B":
                return "ПИТАНИЕ 24B";
            case "TUMBLER_POWER_28B": return "ПИТАНИЕ 28В";
            case "TUMBLER_DRIVE_HANDLE_OFF": return "ПРИВОД - ВЫКЛ. - РУЧНОЕ";
            case "TUMBLER_MODE": return "РЕЖИМ";
            case "PEDAL_AZIM": return "СТОПОР по АЗИМУТУ";
            case "TUMBLER_BOARD": return "ТУМБЛЕР БОРТ";
            case "TUMBLER_POSITION": return "ПЕРЕВОД В ПОЛОЖЕНИЕ";
            case "TUMBLER_WORK_TYPE": return "РЕЖИМ РАБОТЫ";

        }
        return str;
    }
    public string MapperState(string str) {
        switch (str) {
            case "ON": return "ВКЛ";
            case "OFF": return "ВЫКЛ";
            case "MANUAL": return "РУЧНОЕ";
            case "DRIVE": return "ПРИВОД";
            case "BATTLE": return "БОЕВОЕ";
            case "STOWED": return "ПОХОДНОЕ";
            case "AUTO": return "АВТО";
            case "MODE_1": return "РАКЕТА 1";
            case "MODE_2": return "РАКЕТА 2";
            case "MODE_3": return "РАКЕТА 3";
            case "MODE_4": return "РАКЕТА 4";
            
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

            // Если клиент принимает команду с сервера, то первым аргументом идет ID Weaponry
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
                //catch (Exception e) { Debug.LogError("Не удалось установить роль " + cmd.Args[0] + ". Ошибка: " + e.Message); }
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

            Debug.Log("[" + weaponryID + "]: Контрол [" + OutputFileTextMapper.mapControlName(panelName, controlName) + "] на панели [" + OutputFileTextMapper.mapPanelName(panelName) + "] принял состояние [" + controlState + "] курсант[" + Network.player.externalIP + "]");
            //MCSimActionListInfo.text += (Environment.UserDomainName + " - " + MapperControl(controlName) + " состояние " + MapperState(controlState.ToString())+"\n");
            MCSimActionListInfo.text += "Курсант [" + Network.player.externalIP + "]: " + " Контрол [" + OutputFileTextMapper.mapControlName(panelName, controlName) + "] на панели [" + OutputFileTextMapper.mapPanelName(panelName) + "] принял состояние [" + OutputFileTextMapper.mapStateName(controlState.ToString()) + "]" + "\n";
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
                        Debug.LogError("Не удалось определить контрол [" + controlName + "] типа [" + controlType + "] на панели [" + panelName + "]");
                    }
                }
                else
                {
                    Debug.LogWarning("Не найден Weaponry с ID [" + weaponryID + "]");
                }
            }
            catch
            {
                Debug.Log("Не удалось определить Weaponry с ID [" + weaponryID + "]");
            }
        }
    }
}

public class OutputFileTextMapper {
    public static string mapPanelName(string name) {
        switch (name) {
            case "Strela10_OperatorPanel":
                return "Пульт оператора ПО-1";
            case "Strela10_OperationalPanel":
                return "Пульт оперативного управления";
            case "Strela10_ControlBlock":
                return "Блок контроль";
            case "Strela10_AzimuthIndicator":
                return "Указатель азимута";
            case "Strela10_SupportPanel":
                return "Пульт оператора ПО-2";
            case "Strela10_GuidancePanel":
                return "Пульт наведения";
            case "Strela10_VizorPanel":
                return "Оптический визир";
            case "Strela10_ARC":
                return "Аппаратура реализации целеуказаний";
            case "Strela10_SoundPanel":
                return "Панель звука";
            case "Strela10_CommonPanel":
                return "Дополнительная панель";
        }

        return name;
    }

    public static string mapControlName(string panel, string control) {
        switch (panel) {
            case "Strela10_OperatorPanel":
                switch (control) {
                    case "TUMBLER_AOZ":
                        return "Тумблер АОЗ";
                    case "TUMBLER_FON":
                        return "Переключатель фон";
                    case "TUMBLER_DRIVE_HANDLE_OFF":
                        return "Переключатель привод-выкл-ручное";
                    case "TUMBLER_WORK_TYPE":
                        return "Переключатель род работы";
                    case "TUMBLER_POWER_24B":
                        return "Тумблер питание 24 В";
                    case "TUMBLER_POWER_28B":
                        return "Тумблер питание 28 В";
                    case "TUMBLER_POWER_30B":
                        return "Кнопка контроль 30 В";
                    case "TUMBLER_MODE":
                        return "тумблер режим";
                    case "TUMBLER_PSP":
                        return "Тумблер ПСП";
                    case "TUMBLER_PSP_MODE":
                        return "Переключатель режим ПСП";
                    case "TUMBLER_DROP":
                        return "Устройство аварийного сброса ракет";
                    case "TUMBLER_ACU":
                        return "Кнопка АЦУ";
                    case "TUMBLER_BL":
                        return "Тумблер БЛ по 1РЛ246";
                    case "TUMBLER_SS":
                        return "Тумблер СС";
                    case "TUMBLER_TEST":
                        return "Тумблер тест";
                }
                break;
            //case "Strela10_OperationalPanel":
            //    return "Пульт оперативного управления";
            //case "Strela10_ControlBlock":
            //    return "Блок контроль";
            //case "Strela10_AzimuthIndicator":
            //    return "Указатель азимута";
            case "Strela10_SupportPanel":
                switch (control) {
                    case "TUMBLER_GLASS_HEATING":
                        return "Тумблер обогрева стекла";
                    case "TUMBLER_LIGHT":
                        return "Тумблер освещение";
                    case "TUMBLER_FAN":
                        return "Тумблер вентилятора";
                    case "TUMBLER_CLEANER":
                        return "Кнопка омывателя";
                    case "TUMBLER_POSITION":
                        return "Тумблер боевой-походый";
                    case "TUMBLER_TRACKING":
                        return "Тумблер слежение ручное-автомат";
                }
                break;
            case "Strela10_GuidancePanel":
                switch (control) {
                    case "TUMBLER_COOL":
                        return "Тумблер охлаждение";
                    case "TUMBLER_BOARD":
                        return "Кнопка борт";
                    case "TUMBLER_TRACK_LAUNCH":
                        return "Кнопка слежение-пуск";
                    case "TUMBLER_TRIGGER_DRIVE":
                        return "Гашетка";
                }
                break;
            //case "Strela10_VizorPanel":
            //    return "Оптический визир";
            //case "Strela10_ARC":
            //    return "Аппаратура реализации целеуказаний";
            //case "Strela10_SoundPanel":
            //    return "Панель звука";
            case "Strela10_CommonPanel":
                switch (control) {
                    case "PEDAL_AZIM":
                        return "Педаль стопора станка";
                }
                break;
        }

        return control;
    }

    public static string mapStateName(string state) {
        switch (state) {
            case "ON":
                return "ВКЛ";
            case "OFF":
                return "ВЫКЛ";
            case "TRACKING":
                return "Слежение";
            case "LAUNCH":
                return "Пуск";
            case "BATTLE":
                return "Боевой";
            case "STOWED":
                return "Походный";
            case "AUTO":
                return "Авто";
            case "MANUAL":
                return "Ручное";
            case "STATE_1":
                return "1";
            case "STATE_2":
                return "2";
            case "DRIVE":
                return "Привод";
            case "MODE_1":
                return "Пост 1";
            case "MODE_2":
                return "Пост 2";
            case "MODE_3":
                return "Пост 3";
            case "MODE_4":
                return "Пост 4";
            case "COMBAT":
                return "Боевой";
            case "TRAINING":
                return "Походный";
            case "NP":
                return "НП";
            case "DP":
                return "ДП";
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
