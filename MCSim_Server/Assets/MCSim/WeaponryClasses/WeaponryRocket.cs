using UnityEngine;
using System.Collections;

namespace MilitaryCombatSimulator
{
    public abstract class WeaponryRocket : WeaponryProjectile
    {

        // ����� �������������, ��������, ����� ������ ������-������ �� ����� ��������� �� �������� ����
        /// <summary>
        /// �������� ��� ������ ������� ���� ������
        /// </summary>
        public abstract GameObject Target { get; set; }

        /// <summary>
        /// ������ ������
        /// </summary>
        public abstract void Launch();

        public abstract void LaunchWithNotLie();

        public abstract void GetDistanceTargetWar(float a, float b);

    }
}

