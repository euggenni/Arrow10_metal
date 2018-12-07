using UnityEngine;
using System.Collections;
using System;
using System.Xml.Serialization;

namespace MilitaryCombatSimulator
{
    /// <summary>
    /// ������ �� �������������� � ���������
    /// </summary>
    [Serializable]
    public class OrderSubtask 
    {
        public OrderSubtask()
        {
            Description = "";
            PanelName = "";
            ControlName = "";
            State = "";
        }

        public OrderSubtask(string dicription, string panel, string control, string state)
        {
            Description = dicription;
            PanelName = panel;
            ControlName = control;
            State = state;
        }

        /// <summary>
        /// ��� ������
        /// </summary>
        [SerializeField]
        public string PanelName { get; set; }

        /// <summary>
        /// ��� ��������
        /// </summary>
        [SerializeField]
        public string ControlName { get; set; }

        /// <summary>
        /// ��������� � ������
        /// </summary>
        [SerializeField]
        public string Description { get; set; }

        /// <summary>
        /// ���������� ��������� ��������
        /// </summary>
        [SerializeField]
        public string State { get; set; }
    }
}