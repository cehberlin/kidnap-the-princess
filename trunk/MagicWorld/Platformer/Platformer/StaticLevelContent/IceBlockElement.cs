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
        /// some variables for spell reaction
        /// </summary>
        public enum SpellState { NORMAL, BURNED, FROZEN, DESTROYED };
        protected SpellState spellState = SpellState.NORMAL;

        public SpellState State
        {
            get { return spellState; }
        }
        protected double spellDurationOfActionMs = 0;

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

        public override void Update(GameTime gameTime)
        {
            if (spellState != SpellState.NORMAL)
            {
                if (spellDurationOfActionMs > 0)
                {
                    spellDurationOfActionMs -= gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                else
                {
                    spellDurationOfActionMs = 0;
                    spellState = SpellState.NORMAL;
                }
            }
        }

        public override Boolean SpellInfluenceAction(Spell spell)
        {

            if (spell.GetType() == typeof(WarmSpell))
            {
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
