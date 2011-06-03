using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MagicWorld.DynamicLevelContent
{

    class Matter : DynamicBlockElement
    {
        public const double DEFAULT_LIFE_TIME_MS = 200;

        double lifetimeMs;

        public Matter(String texture, Level level, Vector2 position,double lifeTimeMs) :
            base(texture, BlockCollision.Impassable, level, position)
        {
            this.lifetimeMs = lifeTimeMs;
        }

        public override void Update(GameTime gameTime)
        {
            if (lifetimeMs <= 0)
            {
                level.GeneralColliadableGameElements.Remove(this);
                return;
            }
            else
            {
                lifetimeMs -= gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// cold spell increases lifetime warm spell shortens on 10%
        /// </summary>
        /// <param name="spell"></param>
        /// <returns></returns>
        public override bool SpellInfluenceAction(Spell spell)
        {
            if (spell.GetType() == typeof(WarmSpell))
            {
                lifetimeMs *= 0.9;
            }
            if (spell.GetType() == typeof(ColdSpell))
            {
                lifetimeMs *= 1.1;
            }
            return base.SpellInfluenceAction(spell);
        }

    }
}
