using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MilitaryCombatSimulator;
using System;

/// <summary>
/// ������� ������ ������ �� ������� ���������� ��������� ����������
/// </summary>
/// <param name="success">��������� �� ����������</param>
/// <param name="required">��������� ����������</param>
public delegate void OnTaskAttempt(bool success, OrderSubtask required, string dis);

public delegate void OnCurrentControlChanged(PanelControl currentControl);
/// <summary>
/// �����������, ������ �� ���������� ��������� �� ����������
/// </summary>
public  class MCSTrainingChecker : MonoBehaviour
{
    
    /// <summary>
    /// ������� �� ������� ���������� ��������� ����������
    /// </summary>
    public event OnTaskAttempt OnTaskAttempt;
    /// <summary>
    /// ������� �� �������� �� ������ ���������
    /// </summary>
    public event OnCurrentControlChanged OnCurrentControlChanged;
    public  MCSTrainingOrder Order;
    public  Weaponry Weaponry;
    public MCSPlayer Player;
    /// <summary>
    /// ���� ����� �������, ������� ��������� ������
    /// </summary>
    private CoreLibrary.Core _core;

    
    public void SubscribeOnTaskAttempt(OnTaskAttempt callback, OnCurrentControlChanged callbackPosition)
    {
        OnTaskAttempt += callback;
        OnCurrentControlChanged += callbackPosition;

    }
    /// <summary>
    /// ���������� � ������ �� ������� OntaskAttempt
    /// </summary>
    /// <param name="callback"></param>
    public void UnsubscribeOnTaskAttempt(OnTaskAttempt callback)
    {
        OnTaskAttempt -= callback;
    }
    public virtual void CallOnCurrentControlChanged(PanelControl currentControl)
    {
        if (OnCurrentControlChanged != null)
        {
             OnCurrentControlChanged(currentControl);
        }
    }
    public virtual void CallOnTaskAttempt(bool succes, OrderSubtask required, string discription)
    {
        if (OnTaskAttempt != null)
        {
            OnTaskAttempt(succes, required, discription);
        }
    }

    /// <summary>
    /// ������ �������� ������
    /// </summary>
    public List<OrderSubtask> taskCommands = null;

    /// <summary>
    /// ������ ����������� ������
    /// </summary>
    public List<OrderSubtask> completedCommands = new List<OrderSubtask>();

    /// <summary>
    /// ������� ������������ ����������
    /// </summary>
    public OrderSubtask CurrentCommand
    {
        get
        {
            try
            {
                return taskCommands[0];
            }
            catch { return null; }
        }
    }
  
    public void MonitorOrderExecution(Weaponry weaponry, MCSPlayer player, MCSTrainingOrder order)
    {
        try
        {
            // ��������� ������ ����������� �����
            taskCommands = order.Commands;
            Weaponry = weaponry;
            Order = order;
            Player = player;

            
            // ����� ���� � ������� ���������� �������
            
            //foreach (var core in weaponry.Core.Keys) print(core);
           
            weaponry.Core[order.PerformerName].SubscribeOnControlChanged(OnControlChanged);

            _core = weaponry.Core[order.PerformerName];
           
        }
        catch
        {
            Debug.LogError("������ ��� ������� �� ������� ��������� ��������");
            throw;
        }
    }

    public void OnControlChanged(PanelControl control)
    {
        //_core.GetPanel(CurrentCommand.PanelName).gameObject
       
        if (taskCommands.Count != 0)
        {
            if (CurrentCommand == null) { Debug.LogError("NULL CurrentCommand"); return; }

            switch (control.ControlType)
            {
                case ControlType.Tumbler:

                    // �������� �� ������� � �����������

                    if (CurrentCommand.PanelName == control.GetPanelName() && CurrentCommand.ControlName == control.GetName() && CurrentCommand.State.Equals(control.State))
                    {
                        if (taskCommands.Count > 1)
                        {
                           // Debug.Log(_core.GetPanel(CurrentCommand.PanelName).GetControl(GetControl(taskCommands[0]).ControlType, CurrentCommand.ControlName).transform.position.x);
                            CallOnTaskAttempt(true, CurrentCommand, taskCommands[1].Description); // �������� �� �������� ����������
                            completedCommands.Add(CurrentCommand);
                            taskCommands.RemoveAt(0);
                            CallOnCurrentControlChanged(Weaponry.Core[Order.PerformerName].GetPanel(Order.Commands[0].PanelName).GetControl(GetControlType(taskCommands[0]).ControlType, taskCommands[0].ControlName));

                            //BackPosition(Order, Weaponry);
                            // ��������� ������� ������� "�������" ������ ������ ����� ���� �����������. �� ������ ������ ���������� � 0.0.0
                          //  UICenter.UICamera.WorldToScreenPoint(Vector3.zero);
                        }
                        else
                        {
                            CallOnTaskAttempt(true, CurrentCommand, CurrentCommand.Description); // �������� �� �������� ����������
                            completedCommands.Add(CurrentCommand);
                            taskCommands.RemoveAt(0);
                            try
                            {
                                CallOnCurrentControlChanged(Weaponry.Core[Order.PerformerName].GetPanel(Order.Commands[0].PanelName).GetControl(GetControlType(taskCommands[0]).ControlType, taskCommands[0].ControlName));
                            }
                            catch (Exception r) { }        
                            }
                    }

                    else
                    {
                        CallOnTaskAttempt(false, CurrentCommand, CurrentCommand.Description); // �������� � ���������� ����������
                    }



                    break;

                case ControlType.Spinner:

                    double a;
                    try
                    {
                        a = Convert.ToDouble(CurrentCommand.State) * 0.2;
                    }
                    catch
                    {
                        CallOnTaskAttempt(false, CurrentCommand, taskCommands[1].Description); // �������� � ���������� ����������
                        return;
                    }

                    if (CurrentCommand.PanelName == control.GetPanelName() && CurrentCommand.ControlName == control.GetName() && (Convert.ToInt32(CurrentCommand.State) - a < Convert.ToInt32(control.State)) && (Convert.ToInt32(CurrentCommand.State) + a > Convert.ToInt32(control.State)))
                    {
                        if (taskCommands.Count > 1)
                        {
                            CallOnTaskAttempt(true, CurrentCommand, taskCommands[1].Description); // �������� � �������� ����������
                            completedCommands.Add(CurrentCommand);
                            taskCommands.RemoveAt(0);
                            CallOnCurrentControlChanged(Weaponry.Core[Order.PerformerName].GetPanel(Order.Commands[0].PanelName).GetControl(GetControlType(taskCommands[0]).ControlType, taskCommands[0].ControlName));

                        }
                        else
                        {
                            CallOnTaskAttempt(true, CurrentCommand, CurrentCommand.Description); // �������� � �������� ����������
                            completedCommands.Add(CurrentCommand);
                            taskCommands.RemoveAt(0);
                            CallOnCurrentControlChanged(Weaponry.Core[Order.PerformerName].GetPanel(Order.Commands[0].PanelName).GetControl(GetControlType(taskCommands[0]).ControlType, taskCommands[0].ControlName));

                        }
                    }
                    else
                    {
                        CallOnTaskAttempt(false, CurrentCommand, CurrentCommand.Description); // �������� � ���������� ����������
                    }

                    break;
            }
        }

        if (taskCommands.Count == 0)
        {
            CallOnTaskAttempt(true, null, "������ �����!");
            CallOnCurrentControlChanged(null);
            Weaponry.Core[Order.PerformerName].UnsubscribeFromOnControlChanged(OnControlChanged);           
        }
    }

    public PanelControl GetCurrentControl()
    {
        var control = GetControlType(CurrentCommand);
        if (control != null)
            return control;
        else
        {
            Debug.LogError("������ ��� ��������� ��������� �������� ������� �������");
            return null;
        }
    }

    
    public   Vector3 GetCurrentControlPosition(OrderSubtask order){
        
        
        return _core.GetPanel(order.PanelName).GetControl(GetControlType(order).ControlType, order.ControlName).transform.position;
    }

    /// <summary>  /// �������� �������� �� �������    /// </summary>
    public PanelControl GetControlType(OrderSubtask task) 
    {

        if (_core == null)
        {
            Debug.LogError("�� ������ ���� ��� Checker'a");
            return null;
        }
        
        PanelControl control = null;
        
        foreach (ControlType mkey in Enum.GetValues(typeof(ControlType))) {
            control = _core.GetPanel(task.PanelName).GetControl(mkey, task.ControlName);

            if (control != null)
                return control;
        }

        Debug.LogError("������� ������� " + task.ControlName + " �� ������");
        return null;
    }    
}

