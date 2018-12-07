using System;
using System.Collections.Generic;
using UnityEngine;

namespace MilitaryCombatSimulator {
    [Serializable]
    public class MCSCommandLog {
        public MCSCommandLog() {
        }

        //private int _index = 0;

        /// <summary>
        /// Список команд, ожидающих завершения
        /// </summary>
        //public Dictionary<int, MCSNote> Pending = new Dictionary<int, MCSNote>(); 
        /// <summary>
        /// Список завершенных команд
        /// </summary>
        public List<MCSNote> Completed = new List<MCSNote>();

        /// <summary>
        /// Добавляет команду для хранения в списке команд, ожидающих подтверждения выполнения
        /// </summary>
        /// <param name="cmd">Команда для подтверждения</param>
        //public int AddUntilComplete(MCSCommand cmd)
        //{
        //    Pending.Add(_index++, new MCSNote(cmd));
        //    return _index - 1;
        //}
        /// <summary>
        /// Подтверждает выполнение команды с указанным индексом
        /// </summary>
        /// <param name="index">Индекс выполненной команды</param>
        //public void ConfirmCompletion(int index) {
        //    Completed.Add(Pending[index]);
        //    Completed[index].Complete();

        //    Pending.Remove(index);
        //}
        /// <summary>
        /// Добавляет команду для хранения в списке выполненных команд
        /// </summary>
        /// <param name="cmd">Команда для записи</param>
        public void Record(MCSCommand cmd) {
            MCSNote note = new MCSNote(cmd);
            note.Complete();
            Completed.Add(note);
        }
    }

    [Serializable]
    public class MCSNote {
        public MCSNote() {
            // Время в секундах от момента запуска симуляции
            _created = -0.5f;
        }

        public MCSNote(MCSCommand cmd) {
            // Время в секундах от момента запуска симуляции
            _created = MCSGlobalSimulation.SimulationLifeTime;
            Debug.Log("Note time start: " + MCSGlobalSimulation.SimulationLifeTime);
            // Запоминаем команду
            Command = cmd;
        }


        /// <summary>
        /// Время вызова команды в секундах с начала старта симуляции
        /// </summary>
        public float StartTime {
            get { return _created; }
            private set { }
        }

        private float _created = -1f;

        /// <summary>
        /// Время завершения команды в секундах с начала старта симуляции
        /// </summary>
        public float EndTime {
            get { return _ended; }
            private set { _ended = value; }
        }

        private float _ended = -2f;

        public bool isCompleted {
            get {
                if (_ended >= _created) return true;

                return false;
            }

            private set { }
        }

        /// <summary>
        /// Фиксирует завершение выполнения команды.
        /// </summary>
        public void Complete() {
            _ended = MCSGlobalSimulation.SimulationLifeTime;
        }

        /// <summary>
        /// Команда на выполнение
        /// </summary>
        public MCSCommand Command { get; set; }
    }
}