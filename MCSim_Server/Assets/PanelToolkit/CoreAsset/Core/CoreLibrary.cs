using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CoreLibrary : System.Object
{
    /// <summary>
    /// ������� ��� ��������� ������� ��������� �������� ��������
    /// </summary>
    /// <param name="control">�������, ���������� ���� ��������</param>
    public delegate void OnControlChanged(PanelControl control);

    /// <summary>
    /// ��������� ��� �������� ���� ���������� ����� �������
    /// </summary>
    public interface Core
    {
        /// <summary>
        /// ���������� ��� ����
        /// </summary>
        string Name { get; }

        /// <summary>
        /// ����������, �������� �� ���� ��� ������
        /// </summary>
        /// <param name="cpt">������ ControlPanelToolkit ������</param>
        bool ContainsPanelToolkit(ControlPanelToolkit cpt);

        /// <summary>
        /// ���������� ControlPanelToolkit ������
        /// </summary>
        /// <param name="PanelName">��� ������</param>
        ControlPanelToolkit GetPanel(string PanelName);

        /// <summary>
        /// �������� ������ �� �������
        /// </summary>
        /// <param name="index">������ ������ � ������</param>
        void RemovePanel(int index);

        /// <summary>
        /// �������� ������ �� ������� ControlPanelToolkit
        /// </summary>
        /// <param name="cpt">������ ControlPanelToolkit</param>
        void RemovePanel(ControlPanelToolkit cpt);

        /// <summary>
        /// ������� ��������� ��������� �������� ������
        /// </summary>
        /// <param name="control">�������, ���������� ���� ���������</param>
        void ControlChanged(PanelControl control);

        /// <summary>
        /// �������� ��������� ������� � ����
        /// </summary>
        /// <param name="command">��������� �������</param>
        void SendCommandMsg(string command);

        /// <summary>
        /// �������������� ����. ���������� ������������� ���� ������� � �� ���������.
        /// </summary>
        void Virtualize();


        /// <summary>
        /// ���������� true � ������, ���� ���� ���� ����������������.
        /// </summary>
        bool isVirtual { get; }

        /// <summary>
        /// ���������� ��������� �������, �� ������� ����� ������
        /// </summary>
        Transform GetTransform();

        /// <summary>
        /// ����������� �� ������� ��������� �������� ����� �� ������� ������� ����
        /// </summary>
        void SubscribeOnControlChanged(OnControlChanged callback);

        /// <summary>
        /// ���������� �� ������� ��������� �������� ����� �� ������� ������� ����
        /// </summary>
        void UnsubscribeFromOnControlChanged(OnControlChanged callback);
    }

    /// <summary>
    /// ��������� ��� ������ � ��������� ������, ����������� � Core
    /// </summary>
    public abstract class CoreHandler : MonoBehaviour
    {
        /// <summary>
        /// ������� �������� ��������� ��������
        /// </summary>
        public event OnControlChanged ControlChangeCallEvent;

        /// <summary>
        /// ���������� Core, � ������� ������ ������ ����������
        /// </summary>
        public abstract Core Core { get; set; }

        /// <summary>
        /// ������� ��������� ��������� �������� ������
        /// </summary>
        /// <param name="control">�������, ���������� ���� ���������</param>
        public virtual void ControlChanged(PanelControl control) {

            try
            {
                // �������� ������� � ����������� �� ��� �������
                if(ControlChangeCallEvent.GetInvocationList().Length > 0)
                    ControlChangeCallEvent(control);
            } 
            catch {}

            // ���� �� ������ - �������� �� ������ ������� � ��������� ��������
            if (Network.isClient)
            {
                MCSCommand command = new MCSCommand(MCSCommandType.Weaponry, "ControlChanged", false,
                                                    Weaponry.ID,
                                                    (int) control.ControlType,
                                                    control.Core.Name,
                                                    control.GetPanelName(),
                                                    control.GetName(),
                                                    control.State);

                MCSGlobalSimulation.CommandCenter.Execute(command);
            }
            //MCSGlobalSimulation.CommandCenter.ControlChanged(Weaponry.ID, control.Core.Name, control.GetPanelName(), control.GetName(), control.State.ToString());
        }

        public abstract Weaponry Weaponry { get; set; }
    }
}
