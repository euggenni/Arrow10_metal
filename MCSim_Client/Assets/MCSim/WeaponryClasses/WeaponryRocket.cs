using System;
using UnityEngine;
using System.Collections;

namespace MilitaryCombatSimulator {
    public abstract class WeaponryRocket : WeaponryProjectile {
        // Здесь прописывается, например, чтобы ракета воздух-воздух не могла навестись на наземную цель
        /// <summary>
        /// Получить или задать текущую цель ракеты
        /// </summary>
        public abstract GameObject Target { get; set; }

        /// <summary>
        /// Запуск ракеты
        /// </summary>
        public abstract void Launch();

        /// <summary>
        /// Возвращает найденную ракетой цель
        /// </summary>
        public abstract GameObject FindTarget();

        public abstract void GetDistanceTargetWar(float upDistanceTarget, float downDistantanceTarget);

        public abstract Double[] GetParameterForShootTarget();
    }

    public class Cooler {
        public WeaponryTank_Strela10 Host { get; private set; }
        public WeaponryRocket Rocket { get; private set; }
        public float WorkingTimeSecs { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsUsed { get; private set; }
        public bool IsFull { get { return !IsUsed && !IsActive; } }

        public Cooler(WeaponryTank_Strela10 host, WeaponryRocket rocket, float workingTimeSecs) {
            Host = host;
            Rocket = rocket;
            WorkingTimeSecs = workingTimeSecs;
        }

        public void Activate() {
            if (IsUsed || IsActive) {
                return;
            }
            IsActive = true;
            Rocket.StartCoroutine(DeactivateAfterTimeOut());
        }

        private IEnumerator DeactivateAfterTimeOut() {
            yield return new WaitForSeconds(WorkingTimeSecs);
            IsActive = false;
            IsUsed = true;
            if (Host.Arms.Projectile == Rocket) {
                Host.Core["Strela-10_Operator"].GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Indicator, "INDICATOR_I").State = "OFF";
            }
        }

    }
}