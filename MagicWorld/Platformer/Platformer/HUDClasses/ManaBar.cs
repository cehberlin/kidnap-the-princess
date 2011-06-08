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
        float status;
        private Rectangle filling;
        /// <summary>
        /// Used to draw the filling of the bar.
        /// </summary>
        public Rectangle Filling
        {
            get
            {
                return new Rectangle(filling.X,
                    filling.Y,
                    filling.Width,
                    (int)((float)filling.Height / (float)100 * status));
            }
            set { filling = value; }
        }

        public int fullY;
        public int fullHeight;

        public ManaBar(Vector2 pos)
            : base(pos)
        {
            status = 100;
        }

        public void Update(int currentMana, int maxMana)
        {
            this.status = currentMana * 100 / maxMana;
            filling.Y = (int)(fullY + (fullHeight - (fullHeight * (status / 100))));
        }
    }
}
