using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MagicWorld.HUDClasses
{
    class IngredientBar : HUDElement
    {
        /// <summary>
        /// Amount of ingredients that can be collected in the level.
        /// </summary>
        private int max;
        /// <summary>
        /// Amount of ingredients the player has right now.
        /// </summary>
        private int current;
        /// <summary>
        /// Amount of ingredients needed to finish the level.
        /// </summary>
        private int needed;

        public IngredientBar(Vector2 pos)
            : base(pos)
        {
            current = 0;
            max = 0;
            needed = 0;
        }

        public String IngredientString
        {
            get { return "Ingredients: "+ current.ToString() + "\nNeeded: "+ needed.ToString() + " of " + max.ToString(); }
        }

        public void SetState(int cur, int need, int total)
        {
            current = cur;
            needed = need;
            max = total;
        }
    }
}
