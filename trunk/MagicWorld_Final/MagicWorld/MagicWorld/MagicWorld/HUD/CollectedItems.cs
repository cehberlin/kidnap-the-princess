using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicWorld.HUD
{
    /// <summary>
    /// Shows the collected ingredients for the level. It's like a score board.
    /// </summary>
    public class CollectedItems : HUDElement
    {
        private int maxItems;
        /// <summary>
        /// Amount of items in the level.
        /// </summary>
        public int MaxItems
        {
            get { return maxItems; }
            set { maxItems = value; }
        }
        private int currentItems;
        /// <summary>
        /// Amount of already collected items.
        /// </summary>
        public int CurrentItems
        {
            get { return currentItems; }
            set { currentItems = value; }
        }
        private int neededItems;
        /// <summary>
        /// Amount of items needed to finish the level.
        /// </summary>
        public int NeededItems
        {
            get { return neededItems; }
            set { neededItems = value; }
        }
        /// <summary>
        /// Information how many items have been collected already, how many have to be collect and how many can be collected.
        /// </summary>
        public String InfoString { get { return currentItems.ToString() + "/" + neededItems.ToString() + "/" + maxItems.ToString(); } }
        /// <summary>
        /// The position where the info text will be drawn.
        /// </summary>
        public Vector2 InfoPosition { get { return Position + new Vector2(Texture.Width, 0); } }
        private Texture2D tex;
        /// <summary>
        /// Texture representing the collectible item of the level.
        /// </summary>
        public Texture2D Texture
        {
            get { return tex; }
            set { tex = value; }
        }

        public CollectedItems(Vector2 pos, Texture2D texture)
        {
            currentItems = 0;
            Position = pos;
            tex = texture;
            Height = texture.Height;
            Width = texture.Width;
        }
    }
}
