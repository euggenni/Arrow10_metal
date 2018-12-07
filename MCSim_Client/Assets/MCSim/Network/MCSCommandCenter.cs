using System.IO;
using MilitaryCombatSimulator;
using UnityEngine;

/// <summary>
/// Командный центр. Хранит списки игроков, обрабаывает события, пришедшие от них и их контролов.
/// </summary>
public class MCSCommandCenter : MonoBehaviour {
    void Awake() {
        // Обработчик событий оружия
        WeaponryHandler = new MCSWeaponryHandler(this);
        SimulationHandler = this.gameObject.AddComponent<MCSSimulationHandler>();
        //new MCSSimulationHandler(this);

        PlayerHandler = this.gameObject.AddComponent<MCSPlayerHandler>();

        MCSGlobalSimulation.CommandCenter = this;
    }

    #region Handlers

    /// <summary>
    /// Обработчик событий техники
    /// </summary>
    public MCSWeaponryHandler WeaponryHandler;

    /// <summary>
    /// Обработчик событий симуляции
    /// </summary>
    public MCSSimulationHandler SimulationHandler;

    /// <summary>
    /// Обработчик серверных событий
    /// </summary>
    public MCSServerHandler ServerHandler;


    /// <summary>
    /// Обработчик событий игрока
    /// </summary>
    public MCSPlayerHandler PlayerHandler;

    #endregion

    #region Execute method's

    /// <summary>
    /// Отправляет остальным участникам симуляции данную команду
    /// </summary>
    /// <param name="cmd">Команда для выполнения</param>
    public void Execute(MCSCommand cmd) {
        ExecuteSend(Network.player, RPCMode.Others, cmd);
    }

    /// <summary>
    /// Отправляет указанной аудитории симуляции данную команду
    /// </summary>
    /// <param name="rpcMode">Конечная аудитория</param>
    /// <param name="cmd">Команда для выполнения</param>
    public void Execute(RPCMode rpcMode, MCSCommand cmd) {
        ExecuteSend(Network.player, rpcMode, cmd);
    }

    /// <summary>
    /// Отправляет указанному участнику симуляции команду.
    /// </summary>
    /// <param name="to">Целевой участник</param>
    /// <param name="cmd">Команда для выполнения</param>
    public void Execute(NetworkPlayer to, MCSCommand cmd) {
        ExecuteSend(Network.player, to, cmd);
    }


    /// <summary>
    /// Отправляет указанному участнику симуляции заданную команду.
    /// </summary>
    /// <param name="sender">Игрок-отправитель</param>
    /// <param name="target">Игрок-получатель</param>
    /// <param name="cmd">Команда для выполнения</param>
    private void ExecuteSend(NetworkPlayer sender, NetworkPlayer target, MCSCommand cmd) {
        // Если мы и есть отправитель, то нам нужно только отправить в остальные CommandCenter
        if (Network.player == sender) {
            networkView.RPC("RPC_Execute", target, sender, MCSSerializer.SerializeToString(cmd));

            // Сохраняем команду в лог
            if (Network.isServer && cmd.isForRecord)
                MCSGlobalSimulation.Log.Record(cmd);
        }
    }

    /// <summary>
    /// Отправляет указанной аудитории симуляции данную команду от указанного участника. 
    /// </summary>
    /// <param name="sender">Игрок-отправитель</param>
    /// <param name="cmd">Команда для выполнения</param>
    private void ExecuteSend(NetworkPlayer sender, RPCMode rpcMode, MCSCommand cmd) {
        // Если мы и есть отправитель, то нам нужно только отправить в остальные CommandCenter
        if (Network.player == sender) {
            var serialized = MCSSerializer.SerializeToString(cmd);
            networkView.RPC("RPC_Execute", rpcMode, sender, serialized);

            // Сохраняем команду в лог
            if (Network.isServer && cmd.isForRecord)
                MCSGlobalSimulation.Log.Record(cmd);
        }
    }

    [RPC] // Исполняется, когда из других CC приходит команда
    public void RPC_Execute(NetworkPlayer sender, string serializerdCommand) {
        MCSCommand cmd =
            (MCSCommand) MCSSerializer.Deserialize(new StringReader(serializerdCommand), typeof(MCSCommand));

        if (cmd != null) {
            // Отправка данных в обработчики команд
            ExecuteCommand(sender, cmd); // Отправляем на обработку
        } else {
            Debug.LogWarning("Unspecified MCSCommand. " + serializerdCommand);
        }
    }


    /// <summary>
    /// Обработка команды, пришедшей от другого участника
    /// </summary>
    /// <param name="sender">Игрок-отправитель</param>
    /// <param name="cmd">Команда для выполнения</param>
    private void ExecuteCommand(NetworkPlayer sender, MCSCommand cmd) {
        if (Network.isServer && cmd.isForRecord)
            MCSGlobalSimulation.Log.Record(cmd);

        if (cmd.CommandType == MCSCommandType.Weaponry) {
            WeaponryHandler.Execute(sender, cmd);
        }

        if (cmd.CommandType == MCSCommandType.Simulation) {
            SimulationHandler.Execute(sender, cmd);
        }

        if (cmd.CommandType == MCSCommandType.Player) {
            PlayerHandler.Execute(sender, cmd);
        }
    }

    #endregion


    /// <summary>
    /// Назначение ViewID указанному Weaponry. Вызывается на стороне клиента
    /// </summary>
    /// <param name="weaponryID">ID объекта Weaponry, которому будет назначаться viewID</param>
    /// <param name="viewID">viewID объекте NetworkView</param>
    [RPC]
    public void RPC_AssignViewID(int weaponryID, NetworkViewID viewID) {
        //try
        //{
        NetworkView[] nwlist = MCSGlobalSimulation.Weapons.List[weaponryID].gameObject
            .GetComponentsInChildren<NetworkView>();

        int i = 0;
        foreach (NetworkView nview in nwlist) {
            if (!nview.enabled) {
                nview.viewID = viewID;
                nview.enabled = true;
                // Если приняли последний - отылаем подтверждение синхронизации
                //Debug.Log("Назначаем ViewID для [" + weaponryID + "] : " + viewID);
                if (i == nwlist.Length - 1) {
                    MCSGlobalSimulation.Weapons.List[weaponryID].OnWeaponryInstantiate();

                    // Сообщение о готовности и запрос ViewID'шек
                    MCSCommand newCmd = new MCSCommand();
                    newCmd.CommandType = MCSCommandType.Simulation;
                    newCmd.Command = "WeaponrySynchronized";
                    Debug.Log("Синхронизирован [" + weaponryID + "] " +
                              MCSGlobalSimulation.Weapons.List[weaponryID].Name);

                    newCmd.Args = new object[] {weaponryID}; // ID Weaponry

                    MCSGlobalSimulation.CommandCenter.Execute(RPCMode.Server, newCmd);
                }

                return;
            }
            i++;
        }
        //}
        //catch (Exception e) { Debug.LogError("Ошибка при назначении viewID: " + e.Message); }
    }


    #region Синхронизация контролов

    /// <summary>
    /// Метод устаналивает наблюдения за указанным контролом для синхронизации
    /// </summary>
    public void ControlMonitor(PanelControl control) {
        if (Network.isServer) {
            Debug.LogWarning("Сервер не имеет права создавать мониторы элементов контрола.");
        } else // КЛИЕНТ
        {
            Weaponry weaponry = control.gameObject.GetComponentInParents<Weaponry>(false);

            if (weaponry) {
                Debug.Log("Найден предок " + weaponry.Name);
                ControlMonitor(weaponry.ID, control);
            } else
                Debug.LogWarning("Не найден родитель Weaponry для контрола [" + control.GetName() + "]");
        }
    }

    /// <summary>
    /// Метод устаналивает наблюдения за указанным контролом для синхронизации
    /// </summary>
    public void ControlMonitor(int weaponryID, PanelControl control) {
        Debug.Log("Просят установить слежение за контролом " + control.GetName());

        if (Network.isServer) {
            Debug.LogWarning("Сервер не имеет права создавать мониторы элементов контрола.");
        } else // КЛИЕНТ
        {
            var go = new GameObject {name = "Monitor(" + control.GetName() + ")"};
            go.transform.parent = control.gameObject.transform;

            Debug.Log(go.name);

            ControlMonitor monitor = go.AddComponent<ControlMonitor>();
            monitor.Control = control;

            monitor.networkView.viewID = Network.AllocateViewID(); // Выделяем айди для контрола
            Debug.Log("Выделили viewID [" + networkView.viewID + "] для контрола " + control.GetName());
            networkView.RPC("RPC_ControlMonitor", RPCMode.Server, weaponryID, (int) control.ControlType,
                control.GetPanelName(), control.GetName(), monitor.networkView.viewID);
        }
    }

    // Выполняется только на сервере, после того как вычислили контрол по имени
    private void ControlMonitor(int weaponryID, PanelControl control, NetworkViewID viewID) {
        if (Network.isServer) {
            GameObject go = new GameObject();
            go.name = "Monitor(" + control.GetName() + ")";
            go.transform.parent = control.gameObject.transform;

            ControlMonitor monitor = go.AddComponent<ControlMonitor>();
            monitor.Control = control;
            monitor.networkView.viewID = viewID;
        }
    }

    /// <summary>
    /// Создание контрола. Выполняется на стороне сервера.
    /// </summary>
    /// <param name="weaponryID">ID объекта Weaponry, связанного с контролом</param>
    /// <param name="controlType">Тип контрола</param>
    /// <param name="controlPanel">Имя панели</param>
    /// <param name="controlName">Имя контрола</param>
    /// <param name="viewID">ViewID NetworkView, осуществляющего синхронизацию</param>
    [RPC]
    private void RPC_ControlMonitor(int weaponryID, int controlType, string controlPanel, string controlName,
        NetworkViewID viewID) {
        Weaponry weaponry;
        try {
            weaponry = MCSGlobalSimulation.Weapons.List[weaponryID];
        } catch {
            Debug.LogError("Не удалось определить Weaponry с [ID:" + weaponryID +
                           "]. Синхронизация контролов невозможна.");
            return;
        }

        //IWeaponryControl iweaponry = weaponry as IWeaponryControl;
        ControlPanelToolkit panel;
        PanelControl panel_control;

        foreach (var core in weaponry.Core.Values) {
            panel = core.GetPanel(controlPanel);
            panel_control = null;

            if (panel != null) {
                panel_control = panel.GetControl((ControlType) controlType, controlName);

                if (panel_control != null) // Нашли нужный контрол
                {
                    Debug.Log("Найден контрол [" + controlName + "] на панели [" + controlPanel + "]");
                    ControlMonitor(weaponryID, panel_control, viewID);
                    return;
                }
            }
        }


        Debug.Log("Не найден контрол " + controlName + " с ViewID " + viewID);
    }

    #endregion

    [RPC]
    public void ControlChanged(int weaponryID, string coreName, string panelName, string controlName,
        string controlState) {
        if (Network.isClient) {
            networkView.RPC("ControlChanged", RPCMode.Server, weaponryID, coreName, panelName, controlName,
                controlState);
        }
    }
}