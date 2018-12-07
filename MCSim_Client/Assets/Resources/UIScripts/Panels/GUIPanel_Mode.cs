using UnityEngine;
using System.Collections;
using MilitaryCombatSimulator;
using System;

public class GUIPanel_Mode : GUIPanel {
    MCSTrainingOrder _order;
    MCSTrainingChecker _checker;
    Weaponry weaponrys;
    int i = 0;
    private static GameObject _gameObject;

    Strela10_TrainingInfo info;

	private UIPopupList _list;

    void Start()
    {
        Show();
        info = new Strela10_TrainingInfo();

        _list = GetControl<UIPopupList>("Popup_List");

        foreach (var order in info.GetAllOrders())
        {
            _list.items.Add(order.OrderName);
        }
    }

	void FixedUpdate()
	{
        //try
        //{
        //    if (MCSPlayer.Me.Weaponry != null)
        //    {
        //        Close();
        //    }
        //}
        //catch { }
	}
    
    public void SetChecker(MCSTrainingChecker chek) {
        _checker = chek;
        
        /*list.items.Add(_checker.trOrder.OrderList[0]);
        list.items.Add(_checker.trOrder.OrderList[1]);*/
    }

    void StartTrainingExit() {
        Close();
    }
    void StartTrainingTopography() {
        Application.LoadLevel("topographyTools");
    }

    void StartTraining()
    {
        var list = GetControl<UIPopupList>("Popup_List");

        foreach (var order in info.GetAllOrders())
        {
            if (list.selection.Equals(order.OrderName))
            {
                Close();

                int wid = -1;

                try
                {
                    wid = MCSGlobalSimulation.Players.List[Network.player].Weaponry.ID;
                }
                catch { }
               
                    MCSGlobalSimulation.CommandCenter.Execute(RPCMode.Server, new MCSCommand(MCSCommandType.Simulation, "ClientInstantiateRequest", false, "WeaponryTank_Strela10", wid, list.selection));
               
                
            }
        }
    }
    
}
