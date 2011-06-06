using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MagicWorld.HUDClasses
{
    class ManaBar : HUDElement
    {
        /// <summary>
        /// Percentage of current mana the player has.
        /// </summary>
        int status;
        private Rectangle filling;
        /// <summary>
        /// Used to draw the filling of the bar.
        /// </summary>
        public Rectangle Filling
        {
            get { return filling; }
            set { filling = value; }
        }
        
        public ManaBar(Vector2 pos)
            :base(pos)
        {
            status = 100;
        }
    }
}
