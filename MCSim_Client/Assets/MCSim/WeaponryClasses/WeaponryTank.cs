namespace MilitaryCombatSimulator {
    public abstract class WeaponryTank : Weaponry {
        public struct structMinMax {
            public float Min, Max;
        }

        public struct StructXY {
            public float X, Y;
        }

        public struct MaxAngles {
            public structMinMax X, Y;
        }

        /// <summary>
        /// Скорость поворота башни
        /// </summary>
        public StructXY TowerRotationSpeed;

        /// <summary>
        /// Максимальные углы поворота башни
        /// </summary>
        public MaxAngles TowerLimitAngle;

        /// <summary>
        /// Скорость поворота
        /// </summary>
        public float RotationSpeed;

        /// <summary>
        /// Тяга
        /// </summary>
        public float BoostForce;
    }
}