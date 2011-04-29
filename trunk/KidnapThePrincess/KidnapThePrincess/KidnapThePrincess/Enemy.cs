using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace KidnapThePrincess
{
    class Enemy : Person
    {
        List<Hero> heroes;

        //enemy is asleep
        public Boolean Asleep = false;

        public Enemy(Texture2D tex, List<Hero> heroes)
            : base(tex)
        {
            this.heroes = heroes;
        }

        public override void Update(GameTime time)
        {
            if (!Asleep)
            {
                Direction = heroes[0].Position - Position;
                Direction = Vector2.Normalize(Direction);
                base.Update(time);
            }
        }


        public Boolean IsDangerous()
        {
            return !Asleep; //add more properties here if you add some more above
        }

    }
}
