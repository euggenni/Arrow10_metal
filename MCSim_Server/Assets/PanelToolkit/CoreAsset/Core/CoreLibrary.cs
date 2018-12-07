using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CoreLibrary : System.Object
{
    /// <summary>
    /// Делегат для обработки события изменения значения контрола
    /// </summary>
    /// <param name="control">Контрол, изменивший свое значение</param>
    public delegate void OnControlChanged(PanelControl control);

    /// <summary>
    /// Интерфейс для создания ядра управления любой техники
    /// </summary>
    public interface Core
    {
        /// <summary>
        /// Возвращает имя Ядра
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Определяет, содержит ли ядро эту панель
        /// </summary>
        /// <param name="cpt">Скрипт ControlPanelToolkit панели</param>
        bool ContainsPanelToolkit(ControlPanelToolkit cpt);

        /// <summary>
        /// Возвращает ControlPanelToolkit панели
        /// </summary>
        /// <param name="PanelName">Имя панели</param>
        ControlPanelToolkit GetPanel(string PanelName);

        /// <summary>
        /// Удаление панели по индексу
        /// </summary>
        /// <param name="index">Индекс панели в списке</param>
        void RemovePanel(int index);

        /// <summary>
        /// Удаление панели по скрипту ControlPanelToolkit
        /// </summary>
        /// <param name="cpt">Скрипт ControlPanelToolkit</param>
        void RemovePanel(ControlPanelToolkit cpt);

        /// <summary>
        /// Событие изменения состояния контрола панели
        /// </summary>
        /// <param name="control">Контрол, изменивший свое состояние</param>
        void ControlChanged(PanelControl control);

        /// <summary>
        /// Отправка текстовой команды в ядро
        /// </summary>
        /// <param name="command">Текстовая команда</param>
        void SendCommandMsg(string command);

        /// <summary>
        /// Виртуализирует Ядро. Происходит инициализация всех панелей и их контролов.
        /// </summary>
        void Virtualize();


        /// <summary>
        /// Возвращает true в случае, если ядро было виртуализировано.
        /// </summary>
        bool isVirtual { get; }

        /// <summary>
        /// Возвращает трансформ объекта, на котором висит скрипт
        /// </summary>
        Transform GetTransform();

        /// <summary>
        /// Подписаться на событие изменение контрола одной из панелей данного ядра
        /// </summary>
        void SubscribeOnControlChanged(OnControlChanged callback);

        /// <summary>
        /// Отписаться от события изменения контрола одной из панелей данного ядра
        /// </summary>
        void UnsubscribeFromOnControlChanged(OnControlChanged callback);
    }

    /// <summary>
    /// Интерфейс для приема и обработки команд, поступающих в Core
    /// </summary>
    public abstract class CoreHandler : MonoBehaviour
    {
        /// <summary>
        /// Событие изменеия состояния контрола
        /// </summary>
        public event OnControlChanged ControlChangeCallEvent;

        /// <summary>
        /// Возвращает Core, с которым связан данный обработчик
        /// </summary>
        public abstract Core Core { get; set; }

        /// <summary>
        /// Событие изменения состояния контрола панели
        /// </summary>
        /// <param name="control">Контрол, изменивший свое состояние</param>
        public virtual void ControlChanged(PanelControl control) {

            try
            {
                // Вызываем событие у подписчиков на это событие
                if(ControlChangeCallEvent.GetInvocationList().Length > 0)
                    ControlChangeCallEvent(control);
            } 
            catch {}

            // Если мы клиент - отслыаем на сервер событие о изменении контрола
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
