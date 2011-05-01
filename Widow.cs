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
        public Widow(Texture2D tex, Rectangle area) :
            base(tex, area)
        {
            Speed = 1.6f;
            attackDelay = new TimeSpan(0,0,0,0,800);
            Strength = 3;
        }

        /// <summary>
        /// she let the enemy fall asleep
        /// </summary>
        protected override void attack()
        {
            /*foreach (Enemy e in enemies)
            {
                if(GeometryHelper.Intersects(this.Bounds,e.Bounds)){
                    e.Asleep = true;
                }
            }


            base.attack();*/
        }
    }
}
