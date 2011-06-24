using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MagicWorld.HelperClasses
{
    class GeometryCalculationHelper
    {
        public static Random random = new Random(34353463);
        public static Vector2 getRandomPositionOnCycleBow(Vector2 pos_center,float radius)
        {
            //random points on cycle bow
            float angle = (float)(random.NextDouble() * 2 * Math.PI);

            return pos_center + new Vector2((float)Math.Sin(angle) * radius, (float)Math.Cos(angle) * radius);
        }
    }
}
