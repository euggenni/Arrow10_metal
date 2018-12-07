using UnityEngine;
using System.Collections;
using System;

namespace MilitaryCombatSimulator
{
    /// <summary>
    /// ������������ ����� ������
    /// </summary>
    public enum ArmsType
    {
        [EnumDescription("�������")]
        Machinegun,
        [EnumDescription("�������� ���������")]
        RocketLauncher
    }
    /// <summary>
    /// ����� ����������, �������������� �� �������
    /// </summary>
    public abstract class WeaponryArms : Weaponry
    {
        /// <summary>
        /// ����� ��������� �������
        /// </summary>
        public GameObject ProjectileResp;

        /// <summary>
        /// ��� ������� ������ (�������, �������� ���������)
        /// </summary>
        public abstract ArmsType ArmsClass { get; }

        private float _fireRate = 0.1f;
        /// <summary>
        /// ���������� ��� ������ ����� �����������
        /// </summary>
        protected float FireRate
        {
            get { return _fireRate; }
            set
            {
                if (value > 0.01f)
                    _fireRate = value;
                else 
                    _fireRate = 0.01f;
            }
        }

        /// <summary>
        /// ���������� ��� ������ ����������� �������������� �����������
        /// </summary>
        public bool AutoReload { get; set; }

        /// <summary>
        /// ��� WeaponryProjectile, ������� ����� �������������� ��� �����������
        /// </summary>
        public System.Type ProjectileType;

        /// <summary>
        /// ���������� ��� ������ ������� WeaponryProjectile
        /// </summary>
        public abstract WeaponryProjectile Projectile { get; set; }

        /// <summary>
        /// ������� ������������� WeaponryProjectile
        /// </summary>
        public abstract void Shoot();

        /// <summary>
        /// ����������� �������������� WeaponryProjectile
        /// </summary>
        public abstract void Reload();

        /// <summary>
        /// ������ ����������� ������
        /// </summary>
        public abstract void ChargeProjectile<T>() where T: WeaponryProjectile;


        /// <summary>
        /// ������ ����������� ������
        /// </summary>
        public abstract void ChargeProjectile(string projectileType);
    }
}
