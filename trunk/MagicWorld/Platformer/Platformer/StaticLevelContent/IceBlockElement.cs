#region File Description
//-----------------------------------------------------------------------------
// Tile.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.DynamicLevelContent;
using MagicWorld.HelperClasses;

namespace MagicWorld
{
    /// <summary>
    /// A Iceblock --> Could be destroyed by warmspell
    /// </summary>
    class IceBlockElement:BlockElement
    {

        /// <summary>
        /// Constructs a new tile.
        /// </summary>
        public IceBlockElement(Level level, Vector2 position)
            : base("Tiles/Ice_Tile",BlockCollision.Impassable,level, position)
        {
        }

        public override Boolean SpellInfluenceAction(Spell spell)
        {
            if (Texture != null)
            {
                if (spell.GetType() == typeof(WarmSpell))
                {

                    spellState = SpellState.DESTROYED;
                    spellDurationOfActionMs = spell.DurationOfActionMs;
                    
                    return true;
                }
                if (spell.GetType() == typeof(ColdSpell))
                {
                    spellState = SpellState.FROZEN;
                    spellDurationOfActionMs = spell.DurationOfActionMs;
                    return true;
                }
            }
            return false;
        }      

    }
}
