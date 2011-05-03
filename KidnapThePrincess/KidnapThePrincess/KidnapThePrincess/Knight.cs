using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace KidnapThePrincess
{
    class Knight:Hero
    {
        public Knight(Texture2D tex, Rectangle area) :
            base(tex, area)
        {
            Speed = 2f;
            attackDelay = new TimeSpan(0,0,0,0,600);
            Strength = 5;
        }

        /// <summary>
        /// he could kill the enemy
        /// </summary>
        protected override void attack()
        {
            /*
            foreach (Enemy e in enemies)
            {
                if (GeometryHelper.Intersects(this.Bounds, e.Bounds))
                {
                    enemies.Remove(e);
                    break;
                }
            }*/

            base.attack();
        }
    }
}
