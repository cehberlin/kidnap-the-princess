using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace KidnapThePrincess
{
    class Brute:Hero
    {
        public Brute(Texture2D tex, Rectangle area, List<Enemy> enemies) :
            base(tex, area,enemies)
        {
            Speed = 1.5f;
            attackDelay = 400;
        }


        /// <summary>
        /// he throws the enemy to the top
        /// </summary>
        protected override void attack()
        {
            foreach (Enemy e in enemies)
            {
                if (GeometryHelper.Intersects(this.Bounds, e.Bounds))
                {
                    e.Position += new Vector2(0, -140);                    
                }
            }

            base.attack();
        }

    }
}
