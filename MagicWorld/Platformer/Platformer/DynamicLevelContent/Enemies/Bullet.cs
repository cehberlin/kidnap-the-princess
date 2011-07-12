using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MagicWorld.Constants;
using MagicWorld.HelperClasses;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.Spells;
using ParticleEffects;

namespace MagicWorld.DynamicLevelContent.Enemies
{
    /// <summary>
    /// dangerous flying objects are described as own enemy
    /// </summary>
    class Bullet : Enemy
    {
        protected float rotation = 0.0f;

        /// <summary>
        /// general flag if gravity is enabled
        /// </summary>
        protected bool enable_gravity;

        protected bool disableGravityBySpell = false;

        /// <summary>
        /// scale for drawing and bounds
        /// </summary>
        protected float scale = 1.5f;

        /// <summary>
        /// timing counters for spell influence
        /// </summary>
        protected double nogravityInfluenceTime=0;
        protected double frozenInfluenceTime = 0;

        /// <summary>
        /// velocity used when element is frozen
        /// </summary>
        Vector2 frozenVelocity = Vector2.Zero;

        public override Bounds Bounds
        {
            get
            {
                float radius = 0;
                if (sprite.Animation != null)
                {
                    // Calculate bounds within texture size.
                    radius = (sprite.Animation.FrameWidth + sprite.Animation.FrameHeight) / 2 * 0.3f * scale;
                }
                return new Bounds(Position, radius);
            }
        }

        public Bullet(Level level, Vector2 position, Vector2 velocity, string spriteSet, bool enable_gravity = true)
            : base(level, position, spriteSet)
        {
            collisionCallback = HandleCollisionWithObject;
            this.enable_gravity = enable_gravity;
            this.velocity = velocity;
        }

        public override void LoadContent(string spriteSet)
        {
            // Load animations.
            spriteSet = "Sprites/WarmSpell/";
            //runAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Run"), 0.1f, true,3);
            runAnimation = new Animation("Content/Sprites/WarmSpell/Run", 0.1f, 3, level.Content.Load<Texture2D>(spriteSet + "Run"), 0);
            runAnimation.Opacity = 0.8f;
            runAnimation.Scale = scale;
            //idleAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Idle"), 0.5f, true,3);
            idleAnimation = runAnimation;

            base.LoadContent(spriteSet);
        }

        protected void HandleCollisionWithObject(BasicGameElement element, bool xAxisCollision, bool yAxisCollision)
        {
            if (element.GetType() != typeof(BulletLauncher))//no collision handling with launcher
            {
                isRemovable = true;
                level.Game.ExplosionParticleSystem.AddParticles(new ParticleSetting(position));
                level.Game.ExplosionSmokeParticleSystem.AddParticles(new ParticleSetting(position));
                if (isFroozen)
                {
                    level.visibilityService.Remove(this);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            handleSpellInfluence(gameTime);

            HandleMovement(gameTime);

            HandleCollision();
          
            base.Update(gameTime);
        }

        protected override void HandleCollision()
        {
            level.CollisionManager.HandleCollisionWithoutRestrictions(this, collisionCallback);
            if (level.CollisionManager.CollidateWithLevelBounds(this))
            {
                isRemovable = true;
                if (isFroozen)
                {
                    level.visibilityService.Remove(this);
                }
            }
        }

        /// <summary>
        /// handles duration of spell influence...
        /// </summary>
        /// <param name="gameTime"></param>
        protected void handleSpellInfluence(GameTime gameTime)
        {
            if (disableGravityBySpell)
            {
                if (nogravityInfluenceTime <= 0)
                {
                    disableGravityBySpell = false;
                }
                else
                {
                    nogravityInfluenceTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }

            if (isFroozen)
            {
                if (frozenInfluenceTime <= 0)
                {
                    isFroozen = false;
                    frozenVelocity = Vector2.Zero;
                    level.visibilityService.Remove(this);
                }
                else
                {
                    frozenInfluenceTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
                    level.visibilityService.Add(this);
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 paintPosition = Bounds.Position;

            // Draw that sprite.
            if (sprite.Animation != null)
                sprite.Draw(gameTime, spriteBatch, Position, SpriteEffects.None, rotation);
            //ATTENTION I OVERRIDE EVERY DRAWING FROM BASE CLASS TO AVOID DRAWING TWICE,BUT THEREFORE NO BOUNDS ARE DRAWN
        }
                

        protected void HandleMovement(GameTime gameTime)
        {
            if (enable_gravity && !disableGravityBySpell)
            {
                level.PhysicsManager.ApplyGravity(this, SpellConstantsValues.WarmSpellGravity, gameTime);
            }

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!isFroozen)
            {
                Position = Position + velocity * elapsed;
            }
            else if (enable_gravity && !disableGravityBySpell) //only apply gravity
            {
                frozenVelocity += level.PhysicsManager.getGravity(SpellConstantsValues.WarmSpellGravity, gameTime);
                Position = Position + frozenVelocity * elapsed;
            }

            if ((position - OldPosition).Length() > 0.5f)
            {
                rotation = GeometryCalculationHelper.RotateToDirection(OldPosition, position);
            }
            
            base.Update(gameTime);
        }

        public override bool SpellInfluenceAction(Spell spell)
        {
            if (spell.GetType() == typeof(ColdSpell))
            {
                isFroozen = true;
                frozenInfluenceTime = spell.DurationOfActionMs;
            }
            else if (spell.GetType() == typeof(NoGravitySpell))
            {
                disableGravityBySpell = true;
                nogravityInfluenceTime = spell.DurationOfActionMs;
            }
            return base.SpellInfluenceAction(spell);
        }

        public override Rectangle getDrawingArea()
        {
            float width = (sprite.Animation.FrameWidth);
            float height = (sprite.Animation.FrameHeight);
            float left = (float)Math.Round(Position.X - width / 2);
            float top = (float)Math.Round(Position.Y - height / 2);

            return new Rectangle((int)left, (int)top-40, (int)width, (int)height +80);
        }
    }
}
