using System.Collections.Generic;
using MilitaryCombatSimulator;
using UnityEngine;

public delegate void onCheckerChanged(MCSTrainingChecker checker);

/// <summary>
/// Центр для иниацилазиации проверок
/// </summary>
public class MCSTrainingCenter : MonoBehaviour {
    /// <summary>
    /// Событие на появление нового проверщика тренировочной активности
    /// </summary>
    /// 
    public static bool initTrain = false;

    public static event onCheckerChanged OnCheckerInitialized;

    private static void CallOnCheckerInitialized(MCSTrainingChecker checker) {
        onCheckerChanged handler = OnCheckerInitialized;
        if (handler != null) handler(checker);
    }

    /// <summary>
    /// Событие на завершение работы проверщика тренировочной активности
    /// </summary>
    public static event onCheckerChanged OnCheckerDisposed;

    private static void CallOnCheckerDisposed(MCSTrainingChecker checker) {
        onCheckerChanged handler = OnCheckerDisposed;
        if (handler != null) handler(checker);
    }

    /// <summary>
    /// Список следящих за испытуемыми
    /// </summary>
    public static Dictionary<MCSPlayer, MCSTrainingChecker> CheckersList =
        new Dictionary<MCSPlayer, MCSTrainingChecker>();

    private static GameObject _gameObject;


    private void Awake() {
        _gameObject = gameObject;
    }

    public static void InitializeTrainingMode(Weaponry weaponry, MCSPlayer player, MCSTrainingOrder order) {
        var checker = _gameObject.AddComponent<MCSTrainingChecker>();
        checker.MonitorOrderExecution(weaponry, player, order);

        var panel = UICenter.Panels.Instantiate("PanelTrainingMode") as GUIPanel_Mode;
        panel.SendMessage("SetChecker", checker);
        panel.Show();

        CheckersList.Add(player, checker);

        CallOnCheckerInitialized(checker);
    }

    public static void InitializeTraining(Weaponry weaponry, MCSPlayer player, MCSTrainingOrder order) {
        var panelTraining = UICenter.Panels.Instantiate("PanelTrainingMode");
        panelTraining.Close();

        var checker = _gameObject.AddComponent<MCSTrainingChecker>();
        checker.MonitorOrderExecution(weaponry, player, order);
        Debug.Log("Инициализируем тренировку");
        // Создаем панель
        var panel = UICenter.Panels.Instantiate("Panel_Training") as GUIPanel_Training;
        panel.SendMessage("SetChecker", checker);
        panel.Show();

        CheckersList.Add(player, checker);

        CallOnCheckerInitialized(checker);
        if (order.OrderName.Equals("Исходные настройки") || order.OrderName.Equals("К БОЮ")) {
            var panelStart = UICenter.Panels.Instantiate("MessageWindowPart1");
            panelStart.Show();
        }
        if (order.OrderName.Equals("ФУНКЦИОН. КОНТРОЛЬ")) {
            Debug.LogError("Инициализируем фук контроль");
            initTrain = true;
            Strela10_Operator_CoreHandler.CoreStatic.GetPanel("Strela10_OperatorPanel").GetControl(
                ControlType.Tumbler, "TUMBLER_POWER_24B").GetComponent<SwitcherToolkit>().TumblerStateID = 1;
            Strela10_Operator_CoreHandler.CoreStatic.GetPanel("Strela10_OperatorPanel").GetControl(
                ControlType.Tumbler, "TUMBLER_POWER_24B").GetComponent<SwitcherToolkit>().ControlChanged();
            Strela10_Operator_CoreHandler.CoreStatic.GetPanel("Strela10_OperatorPanel").GetControl(
                ControlType.Tumbler, "TUMBLER_POWER_28B").GetComponent<SwitcherToolkit>().TumblerStateID = 1;
            Strela10_Operator_CoreHandler.CoreStatic.GetPanel("Strela10_OperatorPanel").GetControl(
                ControlType.Tumbler, "TUMBLER_POWER_28B").GetComponent<SwitcherToolkit>().ControlChanged();
            Strela10_Operator_CoreHandler.CoreStatic.GetPanel("Strela10_OperatorPanel").GetControl(
                ControlType.Tumbler, "TUMBLER_DRIVE_HANDLE_OFF").GetComponent<SwitcherToolkit>().TumblerStateID = 1;
            Strela10_Operator_CoreHandler.CoreStatic.GetPanel("Strela10_OperatorPanel").GetControl(
                ControlType.Tumbler, "TUMBLER_DRIVE_HANDLE_OFF").GetComponent<SwitcherToolkit>().ControlChanged();

            Strela10_Operator_CoreHandler.CoreStatic.GetPanel("Strela10_SupportPanel").GetControl(
                ControlType.Tumbler, "TUMBLER_POSITION").GetComponent<SwitcherToolkit>().TumblerStateID = 0;
            Strela10_Operator_CoreHandler.CoreStatic.GetPanel("Strela10_SupportPanel").GetControl(
                ControlType.Tumbler, "TUMBLER_POSITION").GetComponent<SwitcherToolkit>().ControlChanged();

            Strela10_Operator_CoreHandler.CoreStatic.GetPanel("Strela10_CommonPanel").GetControl(
                ControlType.Tumbler, "PEDAL_AZIM").GetComponent<SwitcherToolkit>().TumblerStateID = 1;
            Strela10_Operator_CoreHandler.CoreStatic.GetPanel("Strela10_CommonPanel").GetControl(
                ControlType.Tumbler, "PEDAL_AZIM").GetComponent<SwitcherToolkit>().ControlChanged();
        }
    }

    public static void InitializeExam(Weaponry weaponry, MCSPlayer player, MCSTrainingOrder order) {
        var panelTraining = UICenter.Panels.Instantiate("PanelTrainingMode");
        panelTraining.Close();

        var checkerExam = _gameObject.AddComponent<MCSExamChecker>();

        checkerExam.MonitorOrderExecution(weaponry, player, order);

        // Создаем панель
        var panel = UICenter.Panels.Instantiate("Panel_Exam") as GUIPanel_Exam;
        panel.SendMessage("SetOrderExam", order);
        panel.SendMessage("SetCheckerExam", checkerExam);
        panel.Show();

        CheckersList.Add(player, checkerExam);
    }

    /// <summary>
    /// Прервать обучение или экзамен
    /// </summary>
    public static void InterruptCheckers(MCSPlayer player, bool sendToServer) {
        Debug.LogError("Просят уничтожить чекер");
        MCSTrainingChecker checker;
        //UICenter.Panels.CloseGroup("Training");

        Debug.Log(CheckersList.Count);

        if (player != null) {
            try {
                checker = CheckersList[player];
            } catch {
                return;
            }

            CallOnCheckerDisposed(checker);

            CheckersList.Remove(player);

            print("Уничтожили чекер");

            if (sendToServer) // Отсылаем игроку требование прервать учебную активность
                MCSGlobalSimulation.CommandCenter.Execute(RPCMode.Server,
                    new MCSCommand(MCSCommandType.Player, "InterruptCheckers", false,
                        checker.Weaponry.ID, checker.Order.PerformerName));
            initTrain = false;
            Destroy(checker);

            var panelTraining = UICenter.Panels.Instantiate("PanelTrainingMode");
            panelTraining.Show();
        } else Debug.LogWarning("Не найден Checker для данного игрока " + player.NetworkPlayer);
    }
}