using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace KidnapThePrincess
{
    class Goblin : Hero
    {
        private int enemiesAttached;

        public int EnemiesAttached
        {
            get { return enemiesAttached; }
            set { enemiesAttached = value; }
        }
        
        Boolean attacked = false;

        public Boolean Attacked
        {
            get { return attacked; }
            set { attacked = value; }
        }


        public Goblin(Texture2D tex, Rectangle area) :
            base(tex, area)
        {
            Speed = 1.3f;
            canMoveFreezed = true; //necessary for carrieing to castle 
            enemiesAttached = 0;
        }


        public override void Update(GameTime time)
        {
            base.Update(time);
        }
    }
}
