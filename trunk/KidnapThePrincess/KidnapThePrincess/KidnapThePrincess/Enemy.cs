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
        //enemy is asleep
        public Boolean Asleep = false;
        private Vector2 destination;

        public Vector2 Destination
        {
            get { return destination; }
            set { destination = value; }
        }
        

        public Enemy(Texture2D tex)
            : base(tex)
        {
        }

        public override void Update(GameTime time)
        {
            if (!Asleep)
            {
                Direction = Destination - Position;
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
