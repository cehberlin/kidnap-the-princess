using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld.Constants
{
    public class SpellInfluenceValues
    {
        private SpellInfluenceValues(){}


#region "Enemy influence"

        public static TimeSpan maxElectrifiedTime = new TimeSpan(0, 0, 5);

        public static TimeSpan maxFreezeTime = new TimeSpan(0, 0, 5);

        public static TimeSpan maxBurningTime = new TimeSpan(0, 0, 3);
        public static float burningMovingSpeedFactor = 2f; // Factor the enemy is moving faster while under influence of warm spell

#endregion

        public static float PullAcceleration = 1.5f;
        public static float PushAcceleration = PullAcceleration;
    }
}
