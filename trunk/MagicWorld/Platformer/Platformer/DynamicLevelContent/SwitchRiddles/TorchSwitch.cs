using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MagicWorld.DynamicLevelContent.SwitchRiddles
{
    /// <summary>
    /// switch which is like on and off, but on is, if its burning and off if not
    /// could be enabled disabled by cold and warm spell
    /// </summary>
    class TorchSwitch : AbstractSwitch
    {

        /// <summary>
        /// true if the torch is lit
        /// </summary>
        public Boolean IsLit { get; set; }

        public TorchSwitch(String texture, Level level, Vector2 position, String id, bool isLit=false)
            : base(texture, CollisionType.Platform, level, position, id)
        {
            this.IsLit = isLit;
        }

        public override void Update(GameTime gameTime)
        {
            //only used for first activation if object is created lit
            if (!Activated && IsLit)
            {
                Activate();
            }
            base.Update(gameTime);
        }


        public override bool SpellInfluenceAction(Spell spell)
        {
            if (spell.GetType() == typeof(WarmSpell) && !IsLit)
            {
                Activate();
                IsLit = true;
                return true;
            }
            else if (spell.GetType() == typeof(ColdSpell) && IsLit)
            {
                Deactivate();
                IsLit = false;
                return true;
            }

            return base.SpellInfluenceAction(spell);
        }


        public override void Draw(GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (IsLit)
            {
                if (level.Game.SmokeParticleSystem.CurrentParticles() < 20)
                {
                    level.Game.SmokeParticleSystem.AddParticles(Position + new Vector2(Width / 2, -20));
                }
                if (level.Game.FireParticleSystem.CurrentParticles() < 20)
                {
                    level.Game.FireParticleSystem.AddParticles(Position + new Vector2(Width / 2, -20));
                }
            }
            base.Draw(gameTime, spriteBatch);
        }
    }
}
