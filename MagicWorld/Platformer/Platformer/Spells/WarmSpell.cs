using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MagicWorld.Spells;
using MagicWorld.Constants;

namespace MagicWorld
{
    class WarmSpell:Spell 
    {

        public WarmSpell(string spriteSet, Vector2 _origin, Level level)
            : base(spriteSet, _origin, level, SpellConstantsValues.WarmSpellConstants.BasicCastingCost, SpellConstantsValues.WarmSpellConstants.CastingCostPerSecond, SpellType.WarmingSpell)
        {
            Force = SpellConstantsValues.WarmSpell_Force;
            survivalTimeMs = SpellConstantsValues.WarmSpell_survivalTimeMs;
            MoveSpeed = SpellConstantsValues.WarmSpell_MoveSpeed;
            LoadContent(spriteSet);
            sprite.PlayAnimation(idleAnimation);
            durationOfActionMs = SpellConstantsValues.WarmSpell_durationOfActionMs;
        }

        public override void LoadContent(string spriteSet)
        {
            // Load animations.
            spriteSet = "Sprites/WarmSpell/";
            //runAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Run"), 0.1f, true,3);
            runAnimation = new Animation("Content/Sprites/WarmSpell/Run", 0.1f, 3, level.Content.Load<Texture2D>(spriteSet + "Run"), 0);
            //idleAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Idle"), 0.5f, true,3);
            idleAnimation = runAnimation;

            base.LoadContent(spriteSet);
        }

        protected override void HandleMovement(GameTime gameTime)
        {
            level.PhysicsManager.ApplyGravity(this, SpellConstantsValues.WarmSpellGravity, gameTime);
            base.HandleMovement(gameTime);
        }

        protected override void OnRemove()
        {
            level.Game.ExplosionParticleSystem.AddParticles(position);
            level.Game.ExplosionSmokeParticleSystem.AddParticles(position);
            base.OnRemove();
        }


        /// <summary>
        /// adds creation particles
        /// </summary>
        public override void AddOnCreationParticles()
        {
            if (level.Game.FireMagicParticleSystem.CurrentParticles() < 10)
            {
                level.Game.FireMagicParticleSystem.AddParticles(position);
            }
        }
    }
}
