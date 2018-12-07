using System;
using System.Collections.Generic;
using System.IO;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

/// <summary>
/// ��������� �����. ������ ������ �������, ����������� �������, ��������� �� ��� � �� ���������.
/// </summary>
public class MCSCommandCenter : MonoBehaviour {
    
    #region Handlers

    /// <summary>
    /// ���������� ������� �������
    /// </summary>
    public MCSWeaponryHandler  WeaponryHandler;

    /// <summary>
    /// ���������� ������� ���������
    /// </summary>
    public MCSSimulationHandler SimulationHandler;

    /// <summary>
    /// ���������� ��������� �������
    /// </summary>
    public MCSServerHandler ServerHandler;

    /// <summary>
    /// ���������� ��������� �������
    /// </summary>
	public MCSUIHandler UIHandler;

	/// <summary>
	/// ���������� ������� ������
	/// </summary>
	public MCSPlayerHandler PlayerHandler;


    #endregion

    #region Execute method's

    /// <summary>
    /// ���������� ��������� ���������� ��������� ������ �������
    /// </summary>
    /// <param name="cmd">������� ��� ����������</param>
    public void Execute(MCSCommand cmd)
    {
        ExecuteSend(Network.player, RPCMode.Others, cmd);
    }

    /// <summary>
    /// ���������� ��������� ��������� ��������� ������ �������
    /// </summary>
    /// <param name="rpcMode">�������� ���������</param>
    /// <param name="cmd">������� ��� ����������</param>
    public void Execute(RPCMode rpcMode, MCSCommand cmd)
    {
        ExecuteSend(Network.player, rpcMode, cmd);
    }

    /// <summary>
    /// ���������� ���������� ��������� ��������� �������.
    /// </summary>
    /// <param name="to">������� ��������</param>
    /// <param name="cmd">������� ��� ����������</param>
    public void Execute(NetworkPlayer to, MCSCommand cmd)
    {
        ExecuteSend(Network.player, to, cmd);
    }


    /// <summary>
    /// ���������� ���������� ��������� ��������� �������� �������.
    /// </summary>
    /// <param name="sender">�����-�����������</param>
    /// <param name="target">�����-����������</param>
    /// <param name="cmd">������� ��� ����������</param>
    private void ExecuteSend(NetworkPlayer sender, NetworkPlayer target, MCSCommand cmd)
    {
        // ���� �� � ���� �����������, �� ��� ����� ������ ��������� � ��������� CommandCenter
        if (Network.player == sender)
        {
            networkView.RPC("RPC_Execute", target, sender, MCSSerializer.SerializeToString(cmd));

            // ��������� ������� � ���
            if (Network.isServer && cmd.isForRecord)
                MCSGlobalSimulation.Log.Record(cmd);
        }
    }

    /// <summary>
    /// ���������� ��������� ��������� ��������� ������ ������� �� ���������� ���������. 
    /// </summary>
    /// <param name="sender">�����-�����������</param>
    /// <param name="cmd">������� ��� ����������</param>
    private void ExecuteSend(NetworkPlayer sender, RPCMode rpcMode, MCSCommand cmd)
    {
        // ���� �� � ���� �����������, �� ��� ����� ������ ��������� � ��������� CommandCenter
        if (Network.player == sender)
        {
            networkView.RPC("RPC_Execute", rpcMode, sender, MCSSerializer.SerializeToString(cmd));

            // ��������� ������� � ���
            if (Network.isServer && cmd.isForRecord)
                MCSGlobalSimulation.Log.Record(cmd);
        }
    }

    [RPC]   // �����������, ����� �� ������ CC �������� �������
    private void RPC_Execute(NetworkPlayer sender, string serializerdCommand)
    {
        MCSCommand cmd = (MCSCommand)MCSSerializer.Deserialize(new StringReader(serializerdCommand), typeof(MCSCommand));

        if (cmd != null)
        {
            // �������� ������ � ����������� ������
            ExecuteCommand(sender, cmd); // ���������� �� ���������
        }
        else
        {
            Debug.LogWarning("Unspecified MCSCommand. " + serializerdCommand);
        }
    }


    /// <summary>
    /// ��������� �������, ��������� �� ������� ���������
    /// </summary>
    /// <param name="sender">�����-�����������</param>
    /// <param name="cmd">������� ��� ����������</param>
    private void ExecuteCommand(NetworkPlayer sender, MCSCommand cmd)
    {
        if (Network.isServer && cmd.isForRecord)
            MCSGlobalSimulation.Log.Record(cmd);

        Debug.Log(">>> Command: " + cmd.Command);

        if (cmd.CommandType == MCSCommandType.Weaponry)
        {
            WeaponryHandler.Execute(sender, cmd);
        }

        if (cmd.CommandType == MCSCommandType.Simulation)
        {
            SimulationHandler.Execute(sender, cmd);
		}

		if (cmd.CommandType == MCSCommandType.Player)
		{
			PlayerHandler.Execute(sender, cmd);
		}
    }

    #endregion

    void Awake()
    {
        // ���������� ������� ������
        WeaponryHandler = new MCSWeaponryHandler(this);
        SimulationHandler = this.gameObject.AddComponent<MCSSimulationHandler>();
            //new MCSSimulationHandler(this);

        // ���� ������ ������ ������� �� �������, ������� ��������� ���������� �������
        if (Network.isServer) {
            ServerHandler = this.gameObject.AddComponent<MCSServerHandler>();
            UIHandler = this.gameObject.AddComponent<MCSUIHandler>();
	        PlayerHandler = this.gameObject.AddComponent<MCSPlayerHandler>();
        }

        MCSGlobalSimulation.CommandCenter = this;

        InitializeGUI();
    }

    /// <summary>
    /// ������������� GUI
    /// </summary>
    void InitializeGUI()
    {
        MCSUICenter.LightHouse = GameObject.Find("LightHouse");
        MCSUICenter.Store = MCSUICenter.LightHouse.GetComponent<GUILibrary>();
    }

    /// <summary>
    /// ���������� ViewID ���������� Weaponry. ���������� �� ������� �������
    /// </summary>
    /// <param name="weaponryID">ID ������� Weaponry, �������� ����� ����������� viewID</param>
    /// <param name="viewID">viewID ������� NetworkView</param>
    [RPC]
    public void RPC_AssignViewID(int weaponryID, NetworkViewID viewID)
    {
        try
        {
            NetworkView[] nwlist = MCSGlobalSimulation.Weapons.List[weaponryID].gameObject.GetComponentsInChildren<NetworkView>();

            int i = 0;
            foreach (NetworkView nview in nwlist)
            {
                if (!nview.enabled)
                {
                    nview.viewID = viewID;
                    nview.enabled = true;

                    // ���� ������� ��������� - ������� ������������� �������������
                    if(i == nwlist.Length - 1)
                    {
                        // ��������� � ���������� � ������ ViewID'���
                        MCSCommand newCmd = new MCSCommand();
                        newCmd.CommandType = MCSCommandType.Simulation;
                        newCmd.Command = "WeaponrySynchronized";

                        newCmd.Args = new object[] { weaponryID }; // ID Weaponry

                        MCSGlobalSimulation.CommandCenter.Execute(RPCMode.Server, newCmd);
                    }

                    return;
                }
                i++;
            }
        }
        catch (Exception e) { Debug.LogError("������ ��� ���������� viewID: " + e.Message); }
    }


    #region ������������� ���������
    /// <summary>
    /// ����� ������������ ���������� �� ��������� ��������� ��� �������������
    /// </summary>
    private void ControlMonitor(int weaponryID, PanelControl control)
    {
        if (Network.isServer)
        {
            Debug.LogWarning("������ �� ����� ����� ��������� �������� ��������� ��������.");
        }
        else // ������
        {
            GameObject go = new GameObject();
            go.name = "Monitor(" + control.GetName() + ")";
            go.transform.parent = control.gameObject.transform;
			  
            ControlMonitor monitor = go.AddComponent<ControlMonitor>();
            monitor.Control = control;

            monitor.networkView.viewID = Network.AllocateViewID(); // �������� ���� ��� ��������
            Debug.Log("�������� viewID [" + networkView.viewID + "] ��� �������� " + control.GetName());
            networkView.RPC("RPC_ControlMonitor", RPCMode.Server, weaponryID, (int)control.ControlType, control.GetPanelName(), control.GetName(), monitor.networkView.viewID);
        }
    }

    // ����������� ������ �� �������, ����� ���� ��� ��������� ������� �� �����
    private void ControlMonitor(int weaponryID, PanelControl control, NetworkViewID viewID)
    {
        if (Network.isServer)
        {
            GameObject go = new GameObject();
            go.name = "Monitor(" + control.GetName() + ")";
            go.transform.parent = control.gameObject.transform;

            ControlMonitor monitor = go.AddComponent<ControlMonitor>();
            monitor.Control = control;
            monitor.networkView.viewID = viewID;
        }
    }

    /// <summary>
    /// �������� ��������. ����������� �� ������� �������.
    /// </summary>
    /// <param name="weaponryID">ID ������� Weaponry, ���������� � ���������</param>
    /// <param name="controlType">��� ��������</param>
    /// <param name="controlPanel">��� ������</param>
    /// <param name="controlName">��� ��������</param>
    /// <param name="viewID">ViewID NetworkView, ��������������� �������������</param>
    [RPC]
    private void RPC_ControlMonitor(int weaponryID, int controlType, string controlPanel, string controlName, NetworkViewID viewID)
    {
        Debug.Log("�������� � ������� wID [" + weaponryID + "]");

        Weaponry weaponry = null;
        try
        {
            weaponry = MCSGlobalSimulation.Weapons.List[weaponryID];
        }
        catch
        {
            Debug.LogError("�� ������� ���������� Weaponry � [ID:" + weaponryID + "]. ������������� ��������� ����������.");
            return;
        }

        if (weaponry == null) return;

        Debug.Log("���������� Weaponry � ID [" + weaponryID + "]");

        //IWeaponryControl iweaponry = weaponry as IWeaponryControl;
        ControlPanelToolkit panel;
        PanelControl panel_control;

        foreach (CoreToolkit core in weaponry.Core.Values)
        {
            panel = core.GetPanel(controlPanel);

            if (panel != null)
            {

                panel_control = panel.GetControl((ControlType)controlType, controlName);

                if (panel_control != null) // ����� ������ �������
                {
                    //Debug.Log("������ ������� [" + controlName + "] �� ������ [" + controlPanel + "] wID [" + weaponryID + "]");
                    ControlMonitor(weaponryID, panel_control, viewID);
                    return;
                }
            }
        }

        Debug.Log("�� ������ ������� " + controlName + " � ViewID " + viewID);
    }
    #endregion

    //[RPC]
    //public void ControlChanged(int weaponryID, string coreName, string panelName, string controlName, string controlState)
    //{
    //    if (Network.isClient)
    //    {
    //        networkView.RPC("ControlChanged", RPCMode.Server, weaponryID, panelName, controlName, controlState);
    //    }
    //    else if(Network.isServer)
    //    {
           
    //    }
    //}
}
