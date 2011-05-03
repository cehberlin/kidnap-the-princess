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
        public override void attack(Enemy e, Attack attack)
        {
            e.Hitpoints -= this.Strength;

            base.attack(e,attack);
        }


        /// <summary>
        /// he could kill the enemy
        /// </summary>
        public override void attack(GameObject go, Attack attack)
        {
            go.Hitpoints -= this.Strength;

            base.attack(go, attack);
        }
    }
}
