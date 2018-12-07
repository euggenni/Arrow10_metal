using UnityEngine;
using System.Collections;

namespace MilitaryCombatSimulator
{
    public abstract class WeaponryPlane : Weaponry
    {

        public struct StructFloor
        {
            public int Min, Max;
        }

        /// <summary>
        /// ����������� ������ ������
        /// </summary>
        public StructFloor Floor;  
    }
}

