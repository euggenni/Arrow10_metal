using UnityEngine;
using System.Collections;

namespace MilitaryCombatSimulator
{
    public abstract class WeaponryRocket : WeaponryProjectile
    {

        // «десь прописываетс€, например, чтобы ракета воздух-воздух не могла навестись на наземную цель
        /// <summary>
        /// ѕолучить или задать текущую цель ракеты
        /// </summary>
        public abstract GameObject Target { get; set; }

        /// <summary>
        /// «апуск ракеты
        /// </summary>
        public abstract void Launch();

        public abstract void LaunchWithNotLie();

        public abstract void GetDistanceTargetWar(float a, float b);

    }
}

