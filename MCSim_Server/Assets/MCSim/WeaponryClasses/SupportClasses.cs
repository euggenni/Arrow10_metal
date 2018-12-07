using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

namespace MilitaryCombatSimulator
{
    [Serializable]
    public enum MCSCommandType
    {
        None,
        Weaponry,
        Player,
        Simulation,
    }

    /// <summary>
    /// ����� ������������ ��� �������� �������� ������, ������������ ����� �������� � ��������
    /// </summary> 
    [Serializable]
    public class MCSCommand
    {
        public MCSCommand()
        {
            _commandType = MCSCommandType.None;
            _command = "none";
        }

        /// <summary>
        /// ����������� ������ MCSCommand. 
        /// </summary>
        /// <param name="commandType">��� �������</param>
        /// <param name="command">�������</param>
        /// <param name="arg">������ ����������</param>
        public MCSCommand(MCSCommandType commandType, string command, bool isforrecord, params object[] arg)
        {
            _commandType = commandType;
            _command = command;
            _args = arg;
            isForRecord = isforrecord;
        }

        private MCSCommandType _commandType = MCSCommandType.None;
        /// <summary>
        /// ��� �������
        /// </summary>
        public MCSCommandType CommandType
        {
            get { return _commandType; }
            set { _commandType = value; }
        }

        private string _command;
        /// <summary>
        /// ��������� �������
        /// </summary>
        public string Command
        {
            get { return _command; }
            set { _command = value; }
        }

        private object[] _args;
        /// <summary>
        /// ������ ����������
        /// </summary>
        public object[] Args
        {
            get { return _args; }
            set { _args = value; }
        }

        /// <summary>
        /// ���������� ��� ������ ������������� ��������� ������� � ���
        /// </summary>
        public bool isForRecord { get; set; }
    }

    /// <summary>
    /// ��������� ��� ��������� ������ � �������� Weaponry
    /// </summary>
    public interface IWeaponryControl
    {
        /// <summary>
        /// ��������� �������� ��������
        /// </summary>
        /// <param name="panelname">��� ������</param>
        /// <param name="controlname">��� ��������</param>
        /// <param name="value">��������</param>
        void SetControl(string panelname, string controlname, object value);
        
        /// <summary>
        /// ���������� ����������� Core, ��� �������� �������� (������������ �� �������).
        /// </summary>
        void Virtualize();

        /// <summary>
        /// ���������� �������� Core
        /// </summary>
        //CoreLibrary.Core Core { get; }

        /// <summary>
        /// ������� ���� Weaponry (�������� ������ � ������������ � �����).
        /// </summary>
        /// <param name="player">�����, �������� ����� ��������� ����</param>
        /// <param name="roleName">�������� ����</param>
        void SetRole(NetworkPlayer player, string roleName);

        /// <summary>
        /// ���������� ��� ������ ������������ ����� ����������� � ������ ������� Weaponry
        /// </summary>
        Dictionary<NetworkPlayer, string> Crew { get; set; }
            
        /// <summary>
        /// ���������� ����, ���������� �� ������� Weaponry
        /// </summary>
        /// <returns></returns>
        //List<string> GetRoles();

        /// <summary>
        /// ���������� ������ NetworkPlayer ���������� ������� Weaponry
        /// </summary>
        List<NetworkPlayer> Owners { get; }

        /// <summary>
        /// ���������� ��������� �� ������ ����� ����� ���������� ������� Weaponry
        /// </summary>
        bool isOwner { get; }

        /// <summary>
        /// ���������������� networkView's �� ���� Weaponry. ���������� ������ ViewID
        /// </summary>
        NetworkViewID[] InitializeNetworkViews();

        /// <summary>
        /// �������� networkViewID � ����� Weaponry, ��� ������������ ���������� NetworkView
        /// </summary>
        void AddNetworkViewID(NetworkViewID viewId);
    }

    /// <summary>
    /// ��������� ��� �������������� � Weaponry � ������ ������������ ������
    /// </summary>
    public interface ISimEditable
    {
        
    }
}