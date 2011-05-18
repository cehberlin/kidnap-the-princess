using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Platformer.DynamicLevelContent
{

    class MatterTile : DynamicTile
    {
        public const double DEFAULT_LIFE_TIME_MS = 200;

        double lifetimeMs;

        public MatterTile(String texture, Level level, Vector2 position,double lifeTimeMs) :
            base(texture, TileCollision.Impassable, level, position)
        {
            this.lifetimeMs = lifeTimeMs;
        }

        public MatterTile(String texture, Level level, int x, int y, double lifeTimeMs) :
            base(texture, TileCollision.Platform, level, x,y)
        {
            this.lifetimeMs = lifeTimeMs;
        }

        public override void Update(GameTime gameTime)
        {
            if (lifetimeMs <= 0)
            {
                level.ClearTile(X, Y);
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
