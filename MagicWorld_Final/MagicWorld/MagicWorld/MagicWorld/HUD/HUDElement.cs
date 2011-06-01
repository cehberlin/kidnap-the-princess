using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MagicWorld.HUD
{
    /// <summary>
    /// A class holding data that every HUD element contains.
    /// </summary>
    public abstract class HUDElement
    {
        private int height;
        /// <summary>
        /// The height of the element.
        /// </summary>
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        private int width;
        /// <summary>
        /// The width of the element.
        /// </summary>
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        private Vector2 pos;
        /// <summary>
        /// The position of the element.
        /// </summary>
        public Vector2 Position
        {
            get { return pos; }
            set { pos = value; }
        }
        
    }
}
