using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MagicWorld.Constants;
using MagicWorld.HelperClasses;
using Microsoft.Xna.Framework.Graphics;

namespace MagicWorld.DynamicLevelContent.Enemies
{
    /// <summary>
    /// dangerous flying objects are described as own enemy
    /// </summary>
    class Bullet:Enemy{

        protected float rotation = 0.0f;

        protected bool enable_gravity;

        public override Bounds Bounds
        {
            get
            {
                float radius = 0;
                if (sprite.Animation != null)
                {
                    // Calculate bounds within texture size.
                    radius = (sprite.Animation.FrameWidth + sprite.Animation.FrameHeight) / 2 * 0.3f;
                }
                return new Bounds(position, radius);
            }
        }
    
        public Bullet(Level level, Vector2 position, Vector2 velocity, string spriteSet,bool enable_gravity=true):base(level,position,spriteSet){
            collisionCallback = HandleCollisionWithObject;
            this.enable_gravity = enable_gravity;
        }

        public override void LoadContent(string spriteSet)
        {
            // Load animations.
            spriteSet = "Sprites/WarmSpell/";
            //runAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Run"), 0.1f, true,3);
            runAnimation = new Animation("Content/Sprites/WarmSpell/Run", 0.1f, 3, level.Content.Load<Texture2D>(spriteSet + "Run"), 0);
            runAnimation.Opacity = 0.8f;
            //idleAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Idle"), 0.5f, true,3);
            idleAnimation = runAnimation;

            base.LoadContent(spriteSet);
        }

        protected void HandleCollisionWithObject(BasicGameElement element, bool xAxisCollision, bool yAxisCollision)
        {
            if (element.GetType() != typeof(BulletLauncher))//no collision handling with launcher
            {
                isRemovable = true;
                level.Game.ExplosionParticleSystem.AddParticles(position);
                level.Game.ExplosionSmokeParticleSystem.AddParticles(position);
            }
        }

        public override void Update(GameTime gameTime)
        {
            HandleMovement(gameTime);
            level.CollisionManager.HandleCollisionWithoutRestrictions(this, collisionCallback);
            if (level.CollisionManager.CollidateWithLevelBounds(this))
            {
                isRemovable = true;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 paintPosition = Bounds.Position;

            // Draw that sprite.
            if (sprite.Animation != null)
                sprite.Draw(gameTime, spriteBatch, Position, SpriteEffects.None, rotation);
            base.Draw(gameTime, spriteBatch);
        }

        protected  void HandleMovement(GameTime gameTime)
        {
            if (enable_gravity)
            {
                level.PhysicsManager.ApplyGravity(this, SpellConstantsValues.WarmSpellGravity, gameTime);
            }
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position = Position + velocity * elapsed;

            GeometryCalculationHelper.RotateToDirection(oldPosition, position);
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));
            base.Update(gameTime);
        }
    }
}
