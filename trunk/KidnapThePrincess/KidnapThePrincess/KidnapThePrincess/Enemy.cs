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

        private Vector2 dest;
        public Vector2 Destination
        {
            get { return dest; }
            set { dest = value; }
        }

        public Enemy(Texture2D tex, List<Hero> heroes)
            : base(tex)
        {
            this.heroes = heroes;
        }

        public override void Update()
        {

            Direction = Destination - Position;
            Direction = Vector2.Normalize(Direction);
            base.Update();
        }

    }
}
