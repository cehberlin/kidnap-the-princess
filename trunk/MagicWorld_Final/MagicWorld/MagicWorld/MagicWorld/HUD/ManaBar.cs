using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicWorld.HUD
{
    /// <summary>
    /// Shows the amount of mana the player has. The maximum possible amount is shown grayed out if mana is not at the maximum value.
    /// </summary>
    public class ManaBar : HUDElement
    {
        //TODO: Update currentMana
        /// <summary>
        /// Percentage of mana currently available.
        /// </summary>
        int currentMana;

        Texture2D manaTex;
        /// <summary>
        /// The texture representing the mana bar.
        /// </summary>
        public Texture2D Texture { get { return manaTex; } }

        private Rectangle emptyManaRectangle;
        /// <summary>
        /// The rectangle used to draw the complete but empty mana bar.
        /// </summary>
        public Rectangle EmptyManaRectangle
        {
            get { return emptyManaRectangle; }
            set { emptyManaRectangle = value; }
        }

        private Rectangle currentManaRectangle;
        /// <summary>
        /// The rectangle used to draw the current amount of mana over the empty rectangle.
        /// </summary>
        public Rectangle CurrentManaRectangle
        {
            get { return new Rectangle(emptyManaRectangle.X, emptyManaRectangle.Y, emptyManaRectangle.Width * currentMana / 100, emptyManaRectangle.Height); }
        }

        public ManaBar(Texture2D tex, Vector2 pos)
        {
            currentMana = 100;
            Position = pos;
            Height = 20;
            Width = 300;
            manaTex = tex;

            emptyManaRectangle = new Rectangle((int)pos.X, (int)pos.Y, Width, Height);
            currentManaRectangle = new Rectangle((int)pos.X, (int)pos.Y, Width, Height);
        }
    }
}
