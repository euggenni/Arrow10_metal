namespace MilitaryCombatSimulator {
    public abstract class WeaponryPlane : Weaponry {
        public int distance;

        public struct StructFloor {
            public int Min, Max;
        }

        /// <summary>
        /// Ограничения высоты полета
        /// </summary>
        public StructFloor Floor;
    }
}