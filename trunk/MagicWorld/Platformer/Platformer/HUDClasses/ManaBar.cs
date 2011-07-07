using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MagicWorld.HUDClasses
{
    class ManaBar : HUDElement
    {
        /// <summary>
        /// milliseconds blink interval
        /// </summary>
        const double COLOR_UPDATE_CYCLE = 400;
        /// <summary>
        /// default color for liquid
        /// </summary>
        static readonly Color DefaultColor = Color.OrangeRed;
        /// <summary>
        /// toggling blink color for liquid
        /// </summary>
        static readonly Color BlinkColor = Color.Purple;

        /// <summary>
        /// mana percentage when blinking starts
        /// </summary>
        const float BlinkPercentage = 0.35f;

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
        /// <summary>
        /// temporary store of last mana values
        /// </summary>
        private int currentMana;
        private int  maxMana;
        public void Update(int currentMana, int maxMana)
        {
            this.currentMana = currentMana;
            this.maxMana = maxMana;
            this.status = currentMana * 100 / maxMana;
            filling.Y = (int)(fullY + (fullHeight - (fullHeight * (status / 100))));
        }

        Color currentColor = DefaultColor;

        double colorUpdateCycle;

        public Color getTextureOverlay(GameTime gameTime)
        {
            if (currentMana < maxMana * BlinkPercentage)
            {
                if(colorUpdateCycle<=0){
                    if (currentColor == DefaultColor)
                    {
                        currentColor = BlinkColor;
                    }
                    else
                    {
                        currentColor = DefaultColor;
                    }
                    colorUpdateCycle=COLOR_UPDATE_CYCLE;
                }
                colorUpdateCycle-=gameTime.ElapsedGameTime.TotalMilliseconds;
                return currentColor;
            }
            return DefaultColor;
        }
    }
}
