#region File Description
//-----------------------------------------------------------------------------
// Enemy.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.HelperClasses;
using MagicWorld.DynamicLevelContent;
using System.Collections.Generic;
using MagicWorld.Constants;
using System.Diagnostics;

namespace MagicWorld
{
    /// <summary>
    /// A monster who is impeding the progress of our fearless adventurer.
    /// </summary>
    public class Enemy : BasicGameElement
    {

        // Animations
        private Animation runAnimation;
        private Animation idleAnimation;
        private AnimationPlayer sprite;

        /// <summary>
        /// How long this enemy has been waiting before turning around.
        /// </summary>
        private float waitTime;

        /// <summary>
        /// How long to wait before turning around.
        /// </summary>
        private const float MaxWaitTime = 0.5f;

        /// <summary>
        /// The speed at which this enemy moves along the X axis.
        /// </summary>
        private const float MoveSpeed = 64.0f;


        public override Bounds Bounds
        {
            get
            {
                float width = (sprite.Animation.FrameWidth * 0.5f);
                float height = (sprite.Animation.FrameHeight * 0.5f);
                float left = (float)Math.Round(Position.X - width / 2);
                float top = (float)Math.Round(Position.Y - height/2);
                return new Bounds(left, top+30, width, height);
            }
        }

        Bounds oldBounds;
        bool isOnGround = false;

        #region loading

        /// <summary>
        /// Constructs a new Enemy.
        /// </summary>
        public Enemy(Level level, Vector2 position, string spriteSet)
            : base(level)
        {
            this.Position = position;
 
            LoadContent(spriteSet);

            Collision = CollisionType.Impassable;

            //init spell states
            isBurning = false;
            isFroozen = false;
            isElectrified = false;
            debugColor = Color.Red;
            ResetVelocity();            
        }

        /// <summary>
        /// Loads a particular enemy sprite sheet and sounds.
        /// </summary>
        public override void LoadContent(string spriteSet)
        {
            // Load animations.
            runAnimation = new Animation("Content/Sprites/ShadowCreatures/ShadowCreatureSpriteSheet", 0.04f, 24, level.Content.Load<Texture2D>("Sprites/ShadowCreatures/ShadowCreatureSpriteSheet"), 0);
            idleAnimation = new Animation("Content/Sprites/ShadowCreatures/ShadowCreatureSpriteSheet", 0.04f, 24, level.Content.Load<Texture2D>("Sprites/ShadowCreatures/ShadowCreatureSpriteSheet"), 0);
            sprite.PlayAnimation(idleAnimation);

            oldBounds = this.Bounds;

            base.LoadContent("");
        }

        #endregion


        #region updating


        private void ResetVelocity()
        {
            velocity = new Vector2(-MoveSpeed, 0);
        }

        /// <summary>
        /// Paces back and forth along a platform, waiting at either end.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            level.PhysicsManager.ApplyGravity(this, PhysicValues.DEFAULT_GRAVITY, gameTime);
            
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (isFroozen) // ****** isFrozen ******
            {
                currentFreezeTime = currentFreezeTime.Add(gameTime.ElapsedGameTime);
                if (currentFreezeTime >= SpellInfluenceValues.maxFreezeTime)
                {
                    isFroozen = false;
                    idleAnimation.TextureColor = Color.White;
                    runAnimation.TextureColor = Color.White;
                }
            }
            else if (isBurning) // ****** isBurning ******
            {
                currentBurningTime = currentBurningTime.Add(gameTime.ElapsedGameTime);
                if (currentBurningTime >= SpellInfluenceValues.maxBurningTime)
                {
                    isBurning = false;
                    idleAnimation.TextureColor = Color.White;
                    runAnimation.TextureColor = Color.White;
                }
            }

            // ****** isElectrified ******
            if (isElectrified)
            {
                currentElectrifiedTime = currentElectrifiedTime.Add(gameTime.ElapsedGameTime);
                if (currentBurningTime >= SpellInfluenceValues.maxElectrifiedTime)
                {
                    isElectrified = false;
                }
            }

            float acceleration=1;

            if (!isFroozen && !isElectrified) // ****** can move ******
            {
                //TODO let enemies run in player direction, is buggy because enemies are shakeing at obstacles
                //if (level.Player.Position.X > this.position.X + 10 && this.direction.Equals(FaceDirection.Left) ||
                //    level.Player.Position.X < this.position.X - 10 && this.direction.Equals(FaceDirection.Right))
                //{
                //    direction = (FaceDirection)(-(int)direction);
                //}

                if (waitTime > 0)
                {
                    // Wait for some amount of time.
                    waitTime = Math.Max(0.0f, waitTime - (float)gameTime.ElapsedGameTime.TotalSeconds);
                    if (waitTime <= 0.0f)
                    {
                        // Then turn around.                     
                        velocity = velocity * -1;
                    }
                }
                else
                {
                    if (HandleCollision())
                    {
                        //let enemy bounce back after collision, neccessary so you are not kept in the collision
                        Position = oldPosition - Position;
                    }
                    else
                    {                            
                            // Move in the current direction.
                            if (isBurning)
                            {
                                acceleration = SpellInfluenceValues.burningMovingSpeedFactor;
                            }
                            else
                            {                                
                                acceleration = 1;
                            }
                            Position = Position + velocity * elapsed * acceleration;
                    }
                }
            }
            //only handles physics collision
            level.CollisionManager.HandleGeneralCollisions(this, velocity, ref oldBounds, ref isOnGround);
            if (isOnGround)
            {
                ResetVelocity();
            }
        }

        /// <summary>
        /// Checks for collision with level elements
        /// </summary>
        /// <returns>returns true if collision occured</returns>
        private bool HandleCollision()
        {           
            if (level.CollisionManager.CollidateWithLevelBounds(this))
            {
                this.isRemovable = true;
                return true;
            }

            List<BasicGameElement> collisionObjects = new List<BasicGameElement>();
            level.CollisionManager.CollidateWithGeneralLevelElements(this, ref collisionObjects);

            foreach (BasicGameElement t in collisionObjects)
            {
                if (t.Collision == CollisionType.Platform)
                {
                    waitTime = MaxWaitTime;
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region drawing

        /// <summary>
        /// Draws the animated enemy.
        /// </summary>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Stop running when the game is paused or before turning around.
            if (!level.Player.IsAlive ||
                level.ReachedExit ||
                level.TimeRemaining == TimeSpan.Zero ||
                waitTime > 0 || level.Player.Position.X.Equals(this.Position.X)
                || isFroozen
                )
            {
                sprite.PlayAnimation(idleAnimation);
            }
            else
            {
                sprite.PlayAnimation(runAnimation);
            }

            // Draw facing the way the enemy is moving.
            SpriteEffects flip = velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            sprite.Draw(gameTime, spriteBatch, Position, flip);

            base.Draw(gameTime, spriteBatch);
        }

        #endregion

        #region ISpellInfluenceable Member

        private TimeSpan currentElectrifiedTime = new TimeSpan(0, 0, 0);

        private TimeSpan currentFreezeTime = new TimeSpan(0, 0, 0);

        private TimeSpan currentBurningTime = new TimeSpan(0, 0, 0);
        
        double spellDurationOfActionMs = 0;

        public Boolean isBurning { get; set; }
        public Boolean isFroozen { get; set; }
        public Boolean isElectrified { get; set; }

        public override bool SpellInfluenceAction(Spell spell)
        {
            if (spell.SpellType == Spells.SpellType.WarmingSpell)
            {
                if (isFroozen)
                {
                    isFroozen = false;
                    isBurning = false;
                    idleAnimation.TextureColor = Color.White;
                    runAnimation.TextureColor = Color.White;
                }
                else
                {
                    isBurning = true;
                    currentBurningTime = new TimeSpan(0, 0, 0);
                    spellDurationOfActionMs = spell.DurationOfActionMs;
                    idleAnimation.TextureColor = Color.Red;
                    runAnimation.TextureColor = Color.Red;
                }
                return true;
            }
            if (spell.SpellType == Spells.SpellType.ColdSpell)
            {
                if (isBurning)
                {
                    isFroozen = false;
                    isBurning = false;
                    idleAnimation.TextureColor = Color.White;
                    runAnimation.TextureColor = Color.White;
                }
                else
                {
                    isFroozen = true;
                    idleAnimation.TextureColor = Color.Blue;
                    runAnimation.TextureColor = Color.Blue;
                    spellDurationOfActionMs = spell.DurationOfActionMs;
                    currentFreezeTime = new TimeSpan(0, 0, 0);
                }
                return true;
            }
            if (spell.SpellType == Spells.SpellType.ElectricSpell)
            {
                isElectrified = true;
                currentElectrifiedTime = new TimeSpan(0, 0, 0);
            }

            return false;
        }

        #endregion
    }
}
