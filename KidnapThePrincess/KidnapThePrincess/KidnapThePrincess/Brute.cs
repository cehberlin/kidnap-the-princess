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
        public Brute(Texture2D tex, Rectangle area) :
            base(tex, area)
        {
            Speed = 1.5f;
            attackDelay = new TimeSpan(0,0,0,1);
            Strength = 20;
            Type = 1;
        }


        /// <summary>
        /// he throws the enemy to the top but gives less damage
        /// </summary>
        public override void attack(Enemy e,Attack attack)
        {

            e.Position += new Vector2(0, -140);

            e.Hitpoints -= Strength / 20;

            base.attack(e,attack);
        }


        /// <summary>
        /// on objects he did big damage
        /// </summary>
        /// <param name="go"></param>
        /// <param name="attack"></param>
        public override void attack(GameObject go,Attack attack)
        {

            go.Position += new Vector2(0, -50);

            go.Hitpoints -= this.Strength;

            base.attack(go,attack);
        }

    }
}
