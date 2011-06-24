using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld.Constants
{
    class SpellInfluenceValues
    {
        /// <summary>
        /// Please add constants from the bottom if you want to change them online
        /// </summary>
        /// <returns></returns>
        public static List<ConstantGroup> getChangeableConstants()
        {
            List<ConstantGroup> Constants = new List<ConstantGroup>();

            return Constants;
        }

        private SpellInfluenceValues(){}


#region "Enemy influence"

        public static TimeSpan maxElectrifiedTime = new TimeSpan(0, 0, 4);

        public static TimeSpan maxFreezeTime = new TimeSpan(0, 0, 5);

        public static TimeSpan maxBurningTime = new TimeSpan(0, 0, 3);

        public static TimeSpan maxPushingTime = new TimeSpan(0, 0, 1);

        public static TimeSpan maxPullingTime = new TimeSpan(0, 0, 1);

        public static float burningMovingSpeedFactor = 2f; // Factor the enemy is moving faster while under influence of warm spell

#endregion

    }
}
