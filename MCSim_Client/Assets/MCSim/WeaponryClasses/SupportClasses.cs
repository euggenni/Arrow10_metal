using System;
using System.Collections.Generic;
using UnityEngine;

namespace MilitaryCombatSimulator {
    [Serializable]
    public enum MCSCommandType {
        None,
        Weaponry,
        Player,
        Simulation,
    }

    /// <summary>
    /// Класс используемый для хранения значения команд, передаваемых между Сервером и Клиентом
    /// </summary> 
    [Serializable]
    public class MCSCommand {
        public MCSCommand() {
            _commandType = MCSCommandType.None;
            _command = "none";
        }

        /// <summary>
        /// Конструктор класса MCSCommand. 
        /// </summary>
        /// <param name="commandType">Тип команды</param>
        /// <param name="command">Команда</param>
        /// <param name="arg">Список аргументов</param>
        public MCSCommand(MCSCommandType commandType, string command, bool isforrecord, params object[] arg) {
            _commandType = commandType;
            _command = command;
            _args = arg;
            isForRecord = isforrecord;
        }

        private MCSCommandType _commandType = MCSCommandType.None;

        /// <summary>
        /// Тип команды
        /// </summary>
        public MCSCommandType CommandType {
            get { return _commandType; }
            set { _commandType = value; }
        }

        private string _command;

        /// <summary>
        /// Строковая команда
        /// </summary>
        public string Command {
            get { return _command; }
            set { _command = value; }
        }

        private object[] _args;

        /// <summary>
        /// Список аргументов
        /// </summary>
        public object[] Args {
            get { return _args; }
            set { _args = value; }
        }

        /// <summary>
        /// Возвращает или задает необходимость занесения команды в лог
        /// </summary>
        public bool isForRecord { get; set; }
    }

    /// <summary>
    /// Интерфейс для удаленной работы с объектом Weaponry
    /// </summary>
    public interface IWeaponryControl {
        /// <summary>
        /// Изменение значения контрола
        /// </summary>
        /// <param name="panelname">Имя панели</param>
        /// <param name="controlname">Имя контрола</param>
        /// <param name="value">Значение</param>
        void SetControl(string panelname, string controlname, object value);

        /// <summary>
        /// Заполнение виртуальных Core, для хранения значений (используется на Сервере).
        /// </summary>
        void Virtualize();

        /// <summary>
        /// Возвращает значение Core
        /// </summary>
        //CoreLibrary.Core Core { get; }
        /// <summary>
        /// Задание роли Weaponry (создание пульта в соответствии с ролью).
        /// </summary>
        /// <param name="player">Игрок, которому будет назначена роль</param>
        /// <param name="roleName">Название роли</param>
        void SetRole(NetworkPlayer player, string roleName);

        /// <summary>
        /// Возвращает или задает соответствие между владельцами и ролями данного Weaponry
        /// </summary>
        Dictionary<NetworkPlayer, string> Crew { get; set; }

        /// <summary>
        /// Возвращает роли, допустимые на данному Weaponry
        /// </summary>
        /// <returns></returns>
        //List<string> GetRoles();
        /// <summary>
        /// Возвращает список NetworkPlayer владельцев данного Weaponry
        /// </summary>
        List<NetworkPlayer> Owners { get; }

        /// <summary>
        /// Определяет находится ли данный игрок среди владельцев данного Weaponry
        /// </summary>
        bool isOwner { get; }
    }

    /// <summary>
    /// Интерфейс для взаимодействия с Weaponry в режиме Тактического Центра
    /// </summary>
    public interface ISimEditable {
    }
}