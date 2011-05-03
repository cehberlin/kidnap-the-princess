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
        //used for a more detailed collision detection
        private Vector2 colOffset;
        //AI options
        private int aiNumber;

        public int AiNumber
        {
            get { return aiNumber; }
            set { aiNumber = value; }
        }
        
        private bool isCarrying;

        public bool IsCarrying
        {
            get { return isCarrying; }
            set { isCarrying = value; }
        }
        private bool isEscorting;

        public bool IsEscorting
        {
            get { return isEscorting; }
            set { isEscorting = value; }
        }
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
            colOffset=new Vector2(3,0);
        }

        public override void Update(GameTime time)
        {
            if (!Asleep)
            {
                Direction = Destination - Position;
                Direction = Vector2.Normalize(Direction);
                base.Update(time);
                CollisionArea = new Rectangle((int)colOffset.X+CollisionArea.X,(int) colOffset.Y+CollisionArea.Y, CollisionArea.Width, CollisionArea.Height);
            }
        }


        public Boolean IsDangerous()
        {
            return !Asleep; //add more properties here if you add some more above
        }

    }
}
