using System;
using UnityEngine;

namespace MilitaryCombatSimulator {
    /// <summary>
    /// Задача на взаимодействие с контролом
    /// </summary>
    [Serializable]
    public class OrderSubtask {
        public OrderSubtask() {
            Description = "";
            PanelName = "";
            ControlName = "";
            State = "";
        }

        public OrderSubtask(string dicription, string panel, string control, string state) {
            Description = dicription;
            PanelName = panel;
            ControlName = control;
            State = state;
        }

        public OrderSubtask(string dicription, bool correct) {
            Description = dicription;
            Correct = correct;
        }

        /// <summary>
        /// Имя панели
        /// </summary>
        [SerializeField]
        public string PanelName { get; set; }

        /// <summary>
        /// Имя контрола
        /// </summary>
        [SerializeField]
        public string ControlName { get; set; }

        /// <summary>
        /// Подсказка к задаче
        /// </summary>
        [SerializeField]
        public string Description { get; set; }

        /// <summary>
        /// Неободимое состояние контрола
        /// </summary>
        [SerializeField]
        public string State { get; set; }

        public bool Correct { get; set; }
    }
}