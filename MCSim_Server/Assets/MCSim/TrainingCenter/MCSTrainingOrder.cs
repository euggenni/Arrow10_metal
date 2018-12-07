using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MilitaryCombatSimulator
{
    [Serializable]
    [XmlRoot("MCSTrainingOrder")]
    [XmlInclude(typeof(OrderSubtask))] // include type class Person
    public class MCSTrainingOrder
    {
        /// <summary>
        /// ����������� �������
        /// </summary>
        [XmlElement("PerformerName")]
        public string PerformerName;

        /// <summary>
        /// ��� �������
        /// </summary>
        [XmlElement("OrderName")]
        public string OrderName;



        /// <summary>
        /// ������ ������ �� ����������
        /// </summary>
        [XmlArray("CommandsArray")]
        [XmlArrayItem("OrderObject")]
        public List<OrderSubtask> Commands;

        
        public MCSTrainingOrder(string performerName, string orderName, List<OrderSubtask> commands)
        {
            PerformerName = performerName;
            OrderName = orderName;
            Commands = commands;
            
        }

        public MCSTrainingOrder()
        {
            
        }
       
    }
}