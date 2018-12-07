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
        /// Ограничения высоты полета
        /// </summary>
        public StructFloor Floor;  
    }
}

