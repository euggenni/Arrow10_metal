using UnityEngine;

public class ModulSequence : MonoBehaviour {
    public static int index;

    void Start() {
        var weaponry = GetComponent<WeaponryTank_Strela10>();
        weaponry.SetRole(Network.player, "Strela-10_Operator");

        var trainingbehaviour = GetComponent<MCSTrainingBehaviour>();
        var order = trainingbehaviour.GetInfoObject().GetAllOrders()[0];
        Debug.Log(order.PerformerName + " " + order.OrderName + " " + order.Commands.Count);
        // MCSTrainingCenter.InitializeExam(weaponry, null, order);
        // MCSTrainingCenter.InitializeTraining(weaponry, null, order);

        MCSTrainingCenter.InitializeTrainingMode(weaponry, null, order);


        //MCSGlobalSimulation.Players.List.
        //MCSGlobalSimulation.Weapons.List[1].Core[
//MCSGlobalSimulation.
    }


    //// в данном варианте команды перебираются последовательно все 3
    //List<OrderSubtask> taskCommands = null;
    //List<OrderSubtask> completedCommands = new List<OrderSubtask>();
    ////счетчик для послдовательного перебора команд
    //int i = 0;

    //public CoreLibrary.CoreHandler corehandler;
    //// Use this for initialization
    //void Start()
    //{
    //    SetCoreHandler(corehandler);
    //}


    //// Получить CoreHandler
    //public void SetCoreHandler(CoreLibrary.CoreHandler handler)
    //{
    //    //if (handler == corehandler)
    //    //{
    //    //    Debug.LogWarning("This handler is already setted");
    //    //    return;
    //    //}

    //    corehandler = handler;
    //    corehandler.ControlChangeCallEvent += OnControlChanged;


    //    Strela10_Operator_Commands operations = new Strela10_Operator_Commands();
    //    List<string> list = operations.GetAllOrders();
    //    taskCommands = operations.LoadCommands(list[0]);
    //    i++;
    //}


    //void OnDestroy()
    //{
    //    corehandler.ControlChangeCallEvent -= OnControlChanged;
    //}
}