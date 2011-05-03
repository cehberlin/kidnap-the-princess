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
            Speed = 2f;
            attackDelay = new TimeSpan(0,0,0,0,800);
            Strength = 3;
        }

        /// <summary>
        /// she let the enemy fall asleep
        /// </summary>
        public override void attack(Enemy e,Attack attack)
        {
            e.Asleep = true;

            base.attack(e,attack);
        }


        /// <summary>
        /// she let the enemy fall asleep
        /// </summary>
        public override void attack(GameObject go, Attack attack)
        {
            go.Hitpoints -= Strength * 10; //witch could burn houses

            base.attack(go, attack);
        }
    }
}
