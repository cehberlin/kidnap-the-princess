using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MagicWorld.HUDClasses
{
    class IngredientBar:HUDElement
    {
        private int max;
        /// <summary>
        /// Amount of ingredients that can be collected in the level.
        /// </summary>
        public int Maximum
        {
            get { return max; }
            set { max = value; }
        }

        private int current;
        /// <summary>
        /// Amount of ingredients the player has right now.
        /// </summary>
        public int Current
        {
            get { return current; }
            set { current = value; }
        }

        private int needed;
        /// <summary>
        /// Amount of ingredients needed to finish the level.
        /// </summary>
        public int Needed
        {
            get { return needed; }
            set { needed = value; }
        }
        

        public IngredientBar(Vector2 pos)
            :base(pos)
        {
            current = 0;
            max = 0;
            needed = 0;
        }
    }
}
