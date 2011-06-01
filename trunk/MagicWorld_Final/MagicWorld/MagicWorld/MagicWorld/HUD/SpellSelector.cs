using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicWorld.HUD
{
    /// <summary>
    /// shows a selected spell
    /// </summary>
    public class SpellSelector : HUDElement
    {
        //TODO: Use the actual available spells. Update current spell.
        public Texture2D[] runes;
        int spellCount = 9;
        public int Index { get { return index; } }
        int index = 0;

        public SpellSelector(Vector2 pos)
        {
            Position = pos;
            runes = new Texture2D[spellCount];
        }
    }
}
