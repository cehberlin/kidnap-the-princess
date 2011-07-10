using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.HelperClasses;
using ParticleEffects;

namespace MagicWorld.Spells
{
    public class PushSpell : Spell
    {

        public override Bounds Bounds
        {
            get
            {
                return new Bounds(Position, getRadius());
            }
        }

        protected float getRadius()
        {
            float radius = 0;
            if (sprite.Animation != null)
            {
                // Calculate bounds within texture size.
                radius = (sprite.Animation.FrameWidth) / 2 * 1f * currentScale;
            }
            return radius;
        }

        public PushSpell(string spriteSet, Vector2 _origin, Level level)
            : base(spriteSet, _origin, level, SpellConstantsValues.PushSpellConstants.BasicCastingCost, SpellConstantsValues.PushSpellConstants.CastingCostPerSecond, SpellType.PushSpell)
        {
            survivalTimeMs = SpellConstantsValues.PushSpell_survivalTimeMs;
            MoveSpeed = 0;
            sprite.PlayAnimation(idleAnimation);
            durationOfActionMs = SpellConstantsValues.PullSpell_durationOfActionMs;
            base.Position = level.Player.Position;
            MaxScale = SpellConstantsValues.PushSpell_MaxSize;
            growFactor = SpellConstantsValues.PushSpell_GrowRate;
            currentScale = ManaBasicCost * growFactor;
        }

        public override void LoadContent(string spriteSet)
        {
            // Load animations.
            spriteSet = "Sprites/PushSpell/";
            runAnimation = new Animation("Content/Sprites/PushSpell/PushSpriteSheet", 0.1f, 10, level.Content.Load<Texture2D>(spriteSet + "PushSpriteSheet"), 0);
            runAnimation.Opacity = 1f;
            idleAnimation = runAnimation;

            base.LoadContent(spriteSet);
        }

        public override void Update(GameTime gameTime)
        {
            if (SpellState == State.CREATING)
            {
                base.Position = level.Player.Position;

                //only start playing if animation changes because frame position is reseted
                if (sprite.Animation != idleAnimation)
                {
                    sprite.PlayAnimation(idleAnimation);
                }
            }
            level.CollisionManager.HandleCollisionWithoutRestrictions(this, collisionCallback);
        }

        /// <summary>
        /// only check collision after casting is over then remove spell
        /// </summary>
        protected override void OnWorkingStart()
        {            
            SpellState = State.REMOVE;
        }

        public override void AddOnCreationParticles()
        {
            if (level.Game.PushCreationParticleSystem.CurrentParticles() < 10)
            {
                level.Game.PushCreationParticleSystem.AddParticles(new ParticleSetting(position, getRadius()));
            }
        }

    }
}
