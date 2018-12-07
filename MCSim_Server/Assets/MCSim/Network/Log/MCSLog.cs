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
        /// ������ ������, ��������� ����������
        /// </summary>
        //public Dictionary<int, MCSNote> Pending = new Dictionary<int, MCSNote>(); 
        
        /// <summary>
        /// ������ ����������� ������
        /// </summary>
        public List<MCSNote> Completed = new List<MCSNote>(); 

        /// <summary>
        /// ��������� ������� ��� �������� � ������ ������, ��������� ������������� ����������
        /// </summary>
        /// <param name="cmd">������� ��� �������������</param>
        //public int AddUntilComplete(MCSCommand cmd)
        //{
        //    Pending.Add(_index++, new MCSNote(cmd));
        //    return _index - 1;
        //}

        /// <summary>
        /// ������������ ���������� ������� � ��������� ��������
        /// </summary>
        /// <param name="index">������ ����������� �������</param>
        //public void ConfirmCompletion(int index) {
        //    Completed.Add(Pending[index]);
        //    Completed[index].Complete();

        //    Pending.Remove(index);
        //}

        /// <summary>
        /// ��������� ������� ��� �������� � ������ ����������� ������
        /// </summary>
        /// <param name="cmd">������� ��� ������</param>
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
            // ����� � �������� �� ������� ������� ���������
            _created = -0.5f;
        }

        public MCSNote(MCSCommand cmd) {
            // ����� � �������� �� ������� ������� ���������
            _created = MCSGlobalSimulation.SimulationLifeTime;
            // ���������� �������
            Command = cmd;
        }


        /// <summary>
        /// ����� ������ ������� � �������� � ������ ������ ���������
        /// </summary>
        public float StartTime
        {
            get { return _created; }
            private set { }
        }
        private float _created = -1f;

        /// <summary>
        /// ����� ���������� ������� � �������� � ������ ������ ���������
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
        /// ��������� ���������� ���������� �������.
        /// </summary>
        public void Complete() {
            _ended = MCSGlobalSimulation.SimulationLifeTime;
        }

        /// <summary>
        /// ������� �� ����������
        /// </summary>
        public MCSCommand Command { get; set; }
    }

}

