using UnityEngine;
using System.Collections;

namespace MilitaryCombatSimulator
{
    public abstract class WeaponryTank : Weaponry
    {
        public struct structMinMax
        {
            public float Min, Max;
        }

        public struct StructXY
        {
            public float X, Y;
        }

        public struct MaxAngles
        {
            public structMinMax X, Y;
        }

        /// <summary>
        /// �������� �������� �����
        /// </summary>
        public StructXY TowerRotationSpeed;

        /// <summary>
        /// ������������ ���� �������� �����
        /// </summary>
        public MaxAngles TowerLimitAngle;

        /// <summary>
        /// �������� ��������
        /// </summary>
        public float RotationSpeed;

        /// <summary>
        /// ����
        /// </summary>
        public float BoostForce;
    }
}
