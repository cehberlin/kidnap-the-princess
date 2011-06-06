using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MagicWorld.HUDClasses
{
    class SpellBar : HUDElement
    {
        List<Spells.SpellType> spells;

        public SpellBar(Vector2 pos)
            : base(pos)
        {
        }
    }
}
