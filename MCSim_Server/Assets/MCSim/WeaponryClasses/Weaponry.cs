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
    /// ������������ ����������� ����� ��� ���� ����� �������.
    /// </summary>
    public abstract class Weaponry : MonoBehaviour
	{
		/// <summary>
		/// �������, ��������� ����� ������ ������������� Weaponry ����� �������� � ��������
		/// </summary>
		public event onWeaponryInstantiate OnWeaponryInstantiated;

        /// <summary>
        /// ������� �������
        /// </summary>
        public abstract Hashtable Resources { get; }

        /// <summary>
        /// ���������� ��� ������ ��� ������� Weaponry
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// ���������� ��� ������ ��������� ������� Weaponry (None, Ground, Air, Water)
        /// </summary>
        public abstract WeaponryCategory Category { get; }


        /// <summary>
        /// ���������� ���� � ������� Weaponry
        /// </summary>
        public abstract string PrefabPath { get; }

        /// <summary>
        /// ���������� ��� ������ ������������� ������� Weaponry (���, ����, �����������)
        /// </summary>
        //public abstract string Classification { get; }

        /// <summary>
        /// �������� ������� ������� Weaponry ��� ����������
        /// </summary>
        /// <param name="cmd">�������</param>
        [RPC] 
        public abstract void Execute(MCSCommand cmd);

        private int _id = -1;
        /// <summary>
        /// ���������� ����� Weaponry � �����
        /// </summary>
        public int ID { get { return _id; } set { _id = value; } }

        /// <summary>
        /// ��������� Core ��� ����� Weaponry (���� "��� ���� - ����")
        /// </summary>
        public Dictionary<string, CoreLibrary.Core> Core = new Dictionary<string, CoreLibrary.Core>();

		/// <summary>
		/// ������� ���������� ��� ��������������� ������� � ����, ����� ������������� NetworkView
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
					Debug.LogError("������ ��� ������ ������� ������������� �������");
					throw e;
				}
			}
		}

        /// <summary>
        /// ���������� �������
        /// </summary>
        public abstract void Destroy();


        /// <summary>
        /// ���������� ������ ���������� ������� Weaponry
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
                    // ����������� Weaponry ��� ����� ������
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

