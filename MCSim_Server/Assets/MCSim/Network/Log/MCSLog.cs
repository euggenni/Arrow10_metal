using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

namespace MilitaryCombatSimulator
{
    [Serializable]
    public class MCSCommandLog
    {
        public MCSCommandLog() {
        }

        //private int _index = 0;

        /// <summary>
        /// —писок команд, ожидающих завершени€
        /// </summary>
        //public Dictionary<int, MCSNote> Pending = new Dictionary<int, MCSNote>(); 
        
        /// <summary>
        /// —писок завершенных команд
        /// </summary>
        public List<MCSNote> Completed = new List<MCSNote>(); 

        /// <summary>
        /// ƒобавл€ет команду дл€ хранени€ в списке команд, ожидающих подтверждени€ выполнени€
        /// </summary>
        /// <param name="cmd"> оманда дл€ подтверждени€</param>
        //public int AddUntilComplete(MCSCommand cmd)
        //{
        //    Pending.Add(_index++, new MCSNote(cmd));
        //    return _index - 1;
        //}

        /// <summary>
        /// ѕодтверждает выполнение команды с указанным индексом
        /// </summary>
        /// <param name="index">»ндекс выполненной команды</param>
        //public void ConfirmCompletion(int index) {
        //    Completed.Add(Pending[index]);
        //    Completed[index].Complete();

        //    Pending.Remove(index);
        //}

        /// <summary>
        /// ƒобавл€ет команду дл€ хранени€ в списке выполненных команд
        /// </summary>
        /// <param name="cmd"> оманда дл€ записи</param>
        public void Record(MCSCommand cmd)
        {
            MCSNote note = new MCSNote(cmd);
            note.Complete();
            Completed.Add(note);
        }
    }

    [Serializable]
    public class MCSNote
    {
        public MCSNote()
        {
            // ¬рем€ в секундах от момента запуска симул€ции
            _created = -0.5f;
        }

        public MCSNote(MCSCommand cmd) {
            // ¬рем€ в секундах от момента запуска симул€ции
            _created = MCSGlobalSimulation.SimulationLifeTime;
            // «апоминаем команду
            Command = cmd;
        }


        /// <summary>
        /// ¬рем€ вызова команды в секундах с начала старта симул€ции
        /// </summary>
        public float StartTime
        {
            get { return _created; }
            private set { }
        }
        private float _created = -1f;

        /// <summary>
        /// ¬рем€ завершени€ команды в секундах с начала старта симул€ции
        /// </summary>
        public float EndTime
        {
            get { return _ended; }
            private set { _ended = value; }
        }
        private float _ended = -2f;
        
        public bool isCompleted
        {
            get
            {
                if (_ended >= _created) return true;

                return false;
            }
            
            private set { }
        }

        /// <summary>
        /// ‘иксирует завершение выполнени€ команды.
        /// </summary>
        public void Complete() {
            _ended = MCSGlobalSimulation.SimulationLifeTime;
        }

        /// <summary>
        ///  оманда на выполнение
        /// </summary>
        public MCSCommand Command { get; set; }
    }

}

