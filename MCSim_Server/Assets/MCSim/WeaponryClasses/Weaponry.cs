using System;
using System.Collections.Generic;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

namespace MilitaryCombatSimulator
{
	public delegate void onWeaponryInstantiate();

    public enum WeaponryCategory
    {
        None = -1,
        Ground = 0,
        Air = 1,
        Water = 2
    }

    /// <summary>
    /// Родительский абстрактный класс для всех видов техники.
    /// </summary>
    public abstract class Weaponry : MonoBehaviour
	{
		/// <summary>
		/// Событие, вызваемое после полной синхронизации Weaponry между клиентом и сервером
		/// </summary>
		public event onWeaponryInstantiate OnWeaponryInstantiated;

        /// <summary>
        /// Ресурсы объекта
        /// </summary>
        public abstract Hashtable Resources { get; }

        /// <summary>
        /// Возвращает или задает имя объекта Weaponry
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Возвращает или задает категорию объекта Weaponry (None, Ground, Air, Water)
        /// </summary>
        public abstract WeaponryCategory Category { get; }


        /// <summary>
        /// Возвращает путь к префабу Weaponry
        /// </summary>
        public abstract string PrefabPath { get; }

        /// <summary>
        /// Возвращает или задает классификацию объекта Weaponry (ЗРК, Танк, Истребитель)
        /// </summary>
        //public abstract string Classification { get; }

        /// <summary>
        /// Передача команды объекту Weaponry для исполнения
        /// </summary>
        /// <param name="cmd">Команда</param>
        [RPC] 
        public abstract void Execute(MCSCommand cmd);

        private int _id = -1;
        /// <summary>
        /// Уникальный номер Weaponry в сцене
        /// </summary>
        public int ID { get { return _id; } set { _id = value; } }

        /// <summary>
        /// Хранилище Core для этого Weaponry (пара "имя ядра - ядро")
        /// </summary>
        public Dictionary<string, CoreLibrary.Core> Core = new Dictionary<string, CoreLibrary.Core>();

		/// <summary>
		/// Событие вызывается при инстанцировании объекта в сети, после синхронизации NetworkView
		/// </summary>
		public virtual void OnWeaponryInstantiate()
		{
			if (OnWeaponryInstantiated != null)
			{
				try
				{
					OnWeaponryInstantiated();
				}
				catch (Exception e)
				{
					Debug.LogError("Ошибка при вызове события синхронизации объекта");
					throw e;
				}
			}
		}

        /// <summary>
        /// Разрушение техники
        /// </summary>
        public abstract void Destroy();


        /// <summary>
        /// Возвращает список владельцев данного Weaponry
        /// </summary>
        /// <returns></returns>
        public List<MCSPlayer> GetOwners()
        {
            var players = new List<MCSPlayer>();

            foreach (MCSPlayer player in MCSGlobalSimulation.Players.List.Values)
            {
                try
                {
                    if (player.Weaponry.Equals(this))
                    {
                        players.Add(player);
                    }
                }
                catch
                {
                    // Отсутствует Weaponry для этого игрока
                    continue;
                }
            }

            return players;
        }

        public void OnDestroy()
        {
            MCSGlobalSimulation.Weapons.List.Remove(ID);
        }
    }


}

