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
            Speed = 0.7f;
        }

        /// <summary>
        /// he could kill the enemy
        /// </summary>
        public override void attack()
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
