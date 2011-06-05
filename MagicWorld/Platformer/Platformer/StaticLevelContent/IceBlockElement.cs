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
    class IceBlockElement : BlockElement
    {

        /// <summary>
        /// Constructs a new tile.
        /// </summary>
        public IceBlockElement(Level level, Vector2 position)
            : base("Tiles/Ice_Tile", CollisionType.Impassable, level, position)
        {
        }

        public IceBlockElement(Level level, Vector2 position, int width, int height) :
            base("Tiles/Ice_Tile", CollisionType.Impassable, level, position, width, height)
        {
        }

        public override Boolean SpellInfluenceAction(Spell spell)
        {

            if (spell.GetType() == typeof(WarmSpell))
            {
                spellDurationOfActionMs = spell.DurationOfActionMs;
                this.IsRemovable = true;

                return true;
            }
            if (spell.GetType() == typeof(ColdSpell))
            {
                spellState = SpellState.FROZEN;
                spellDurationOfActionMs = spell.DurationOfActionMs;
                return true;
            }
            return false;
        }

    }
}
