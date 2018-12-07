using UnityEngine;
using System.Collections;
using System;

namespace MilitaryCombatSimulator
{
    /// <summary>
    /// Перечисление типов оружия
    /// </summary>
    public enum ArmsType
    {
        [EnumDescription("Пулемет")]
        Machinegun,
        [EnumDescription("Ракетная установка")]
        RocketLauncher
    }
    /// <summary>
    /// Класс вооружения, установленного на технику
    /// </summary>
    public abstract class WeaponryArms : Weaponry
    {
        /// <summary>
        /// Место появления снаряда
        /// </summary>
        public GameObject ProjectileResp;

        /// <summary>
        /// Тип данного орудия (пулемет, ракетная установка)
        /// </summary>
        public abstract ArmsType ArmsClass { get; }

        private float _fireRate = 0.1f;
        /// <summary>
        /// Возвращает или задает время перезарядки
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
        /// Возвращает или задает возможность автоматической перезарядки
        /// </summary>
        public bool AutoReload { get; set; }

        /// <summary>
        /// Тип WeaponryProjectile, который будет инстанцировать при перезарядке
        /// </summary>
        public System.Type ProjectileType;

        /// <summary>
        /// Возвращает или задает текущий WeaponryProjectile
        /// </summary>
        public abstract WeaponryProjectile Projectile { get; set; }

        /// <summary>
        /// Выстрел установленным WeaponryProjectile
        /// </summary>
        public abstract void Shoot();

        /// <summary>
        /// Перезарядка установленного WeaponryProjectile
        /// </summary>
        public abstract void Reload();

        /// <summary>
        /// Задает стандартный снаряд
        /// </summary>
        public abstract void ChargeProjectile<T>() where T: WeaponryProjectile;


        /// <summary>
        /// Задает стандартный снаряд
        /// </summary>
        public abstract void ChargeProjectile(string projectileType);
    }
}
