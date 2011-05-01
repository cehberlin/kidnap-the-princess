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
        public Knight(Texture2D tex, Rectangle area, List<Enemy> enemies) :
            base(tex, area,enemies)
        {
            Speed = 1.7f;
            attackDelay = 1500;
        }

        /// <summary>
        /// he could kill the enemy
        /// </summary>
        protected override void attack()
        {
            foreach (Enemy e in enemies)
            {
                if (GeometryHelper.Intersects(this.Bounds, e.Bounds))
                {
                    enemies.Remove(e);
                    break;
                }
            }

            base.attack();
        }
    }
}
