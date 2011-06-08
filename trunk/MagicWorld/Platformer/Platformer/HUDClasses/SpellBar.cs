using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MagicWorld.HUDClasses
{
    class SpellBar : HUDElement
    {
        //TODO: Display more than the current spell (Prev and next). 
        List<Spells.SpellType> spells;
        /// <summary>
        /// The currently selected spell.
        /// </summary>
        public Spells.SpellType CurrentSpell;

        public SpellBar(Vector2 pos)
            : base(pos)
        {
        }

        public void Update(Spells.SpellType type)
        {
            CurrentSpell = type;
        }
    }
}
