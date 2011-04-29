using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace KidnapThePrincess
{
    class Widow:Hero
    {
        public Widow(Texture2D tex, Rectangle area,List<Enemy> enemies) :
            base(tex, area, enemies)
        {
            Speed = 0.8f;
            attackDelay = 750;
        }

        /// <summary>
        /// she let the enemy fall asleep
        /// </summary>
        protected override void attack()
        {
            foreach (Enemy e in enemies)
            {
                if(GeometryHelper.Intersects(this.Bounds,e.Bounds)){
                    e.Asleep = true;
                }
            }


            base.attack();
        }
    }
}
