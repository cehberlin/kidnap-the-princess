using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ParticleEffects;

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

        public TorchSwitch(String texture, Level level, Vector2 position, String id, bool isLit = false)
            : base(texture, CollisionType.Platform, level, position, id)
        {
            this.IsLit = isLit;
        }

        public override void Update(GameTime gameTime)
        {
            ////only used for first activation if object is created lit
            //if (!Activated && IsLit)
            //{
            //    Activate();
            //}
            if (IsLit)
            {
                if ((level.Player.Position - Position).Length() < 300)
                {
                    audioService.playSoundLoop(Audio.SoundType.torchBurning, 0.6f);
                }
                else
                {
                    audioService.stopSoundLoop(Audio.SoundType.torchBurning, false);
                }
            }
            else
            {
                audioService.stopSoundLoop(Audio.SoundType.torchBurning, true);
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

        int currentParticles = 0;
        public override void Draw(GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (IsLit)
            {
                if (currentParticles % 12 == 0) //only every .. update cycle
                {
                    level.Game.SmokeParticleSystem.AddParticles(new ParticleSetting(Position + new Vector2(Width / 2, -20)));
                    level.Game.FireParticleSystem.AddParticles(new ParticleSetting(Position + new Vector2(Width / 2, -20)));
                }
                currentParticles++;
            }
            base.Draw(gameTime, spriteBatch);
        }
    }
}
