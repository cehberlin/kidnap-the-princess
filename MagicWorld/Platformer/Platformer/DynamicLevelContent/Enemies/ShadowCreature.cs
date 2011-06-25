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
using MagicWorld.Services;
using MagicWorld.StaticLevelContent;

namespace MagicWorld
{
    /// <summary>
    /// A monster who is impeding the progress of our fearless adventurer.
    /// </summary>
    public class ShadowCreature : Enemy
    {

        /// <summary>
        /// How long this enemy has been waiting before turning around.
        /// </summary>
        private TimeSpan waitTime = new TimeSpan(0, 0, 0, 0, 0);

        /// <summary>
        /// How long to wait before turning around.
        /// </summary>
        private TimeSpan MaxWaitTime = new TimeSpan(0, 0, 0, 2, 0);

        /// <summary>
        /// The speed at which this enemy moves along the X axis.
        /// </summary>
        private const float MoveSpeed = 64.0f;

        private float acceleration = 1;

        private float enemyDelta = 100;

        Bounds oldBounds;
        bool isOnGround = false;

        bool isConfused = false;

        public override Bounds Bounds
        {
            get
            {
                float width = (sprite.Animation.FrameWidth * 0.5f);
                float height = (sprite.Animation.FrameHeight * 0.5f);
                float left = (float)Math.Round(Position.X - width / 2);
                float top = (float)Math.Round(Position.Y - height / 2);
                return new Bounds(left, top + 30, width, height);
            }
        }

        #region loading

        /// <summary>
        /// Constructs a new Enemy.
        /// </summary>
        public ShadowCreature(Level level, Vector2 position, string spriteSet)
            : base(level, position, spriteSet)
        {
            collisionCallback = HandleCollisionWithObject;
        }

        /// <summary>
        /// Loads a particular enemy sprite sheet and sounds.
        /// </summary>
        public override void LoadContent(string spriteSet)
        {
            // Load animations.
            base.runAnimation = new Animation("Content/Sprites/ShadowCreatures/ShadowCreatureSpriteSheet", 0.04f, 24, level.Content.Load<Texture2D>("Sprites/ShadowCreatures/ShadowCreatureSpriteSheet"), 0);
            base.idleAnimation = new Animation("Content/Sprites/ShadowCreatures/ShadowCreatureSpriteSheet", 0.04f, 24, level.Content.Load<Texture2D>("Sprites/ShadowCreatures/ShadowCreatureSpriteSheet"), 0);
            sprite.PlayAnimation(idleAnimation);

            oldBounds = this.Bounds;
            base.LoadContent("");
        }

        #endregion


        #region updating

        /// <summary>
        /// modulo count for adding particles
        /// </summary>
        int currentParticles = 0;
        /// <summary>
        /// Paces back and forth along a platform, waiting at either end.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            level.PhysicsManager.ApplyGravity(this, PhysicValues.DEFAULT_GRAVITY, gameTime);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (isFroozen) // ****** isFrozen ******
            {
                CurrentVelocity = new Vector2(CurrentVelocity.X, 0);
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
            else if (isElectrified)
            {
                if (CurrentVelocity.X < 0 && !isConfused)
                {
                    CurrentVelocity = new Vector2(MoveSpeed, 0);
                    isConfused = true;
                }
                else if (CurrentVelocity.X > 0 && !isConfused)
                {
                    CurrentVelocity = new Vector2(-MoveSpeed, 0);
                    isConfused = true;
                }

                currentElectrifiedTime = currentElectrifiedTime.Add(gameTime.ElapsedGameTime);
                if (currentElectrifiedTime >= SpellInfluenceValues.maxElectrifiedTime)
                {
                    CurrentVelocity = new Vector2(-CurrentVelocity.X, 0);
                    isElectrified = false;
                    isConfused = false;
                }

                currentParticles++;

                if (currentParticles % 4 == 0) //only every 4 update cycle
                {
                    level.Game.LightningCreationParticleSystem.AddParticles(GeometryCalculationHelper.getRandomPositionOnCycleBow(position,40));
                }
            }

            if (!isFroozen) // ****** can move ******
            {
                float currentDeltaX = level.Player.Position.X - position.X;
                float currentDeltaY = level.Player.Position.Y - position.Y;
                currentDeltaX = Math.Abs(currentDeltaX);
                currentDeltaY = Math.Abs(currentDeltaY);
                //If player is nearby
                if (level.Player.Position.X > this.position.X && currentDeltaX < enemyDelta && currentDeltaY < enemyDelta)
                {
                    CurrentVelocity = new Vector2(MoveSpeed, CurrentVelocity.Y);
                }
                else if (level.Player.Position.X < this.position.X && currentDeltaX < enemyDelta && currentDeltaY < enemyDelta)
                {
                    CurrentVelocity = new Vector2(-MoveSpeed, CurrentVelocity.Y);
                }

                HandleCollision();
                // Move in the current direction.
                if (isBurning)
                {
                    acceleration = SpellInfluenceValues.burningMovingSpeedFactor;
                }
                else
                {
                    acceleration = 1;
                }
                waitTime = waitTime.Add(gameTime.ElapsedGameTime);
                oldPosition = Position;
                Position = Position + velocity * elapsed * acceleration;

                //if enemy can not move forward
                if (isOnGround)
                {
                    float enemyMovementDelta = oldPosition.X - Position.X;
                    enemyMovementDelta = Math.Abs(enemyMovementDelta);
                    if (enemyMovementDelta < 1)
                    {
                        CurrentVelocity = CurrentVelocity * -1;
                    }
                }
            }
            //only handles physics collision
            level.CollisionManager.HandleGeneralCollisions(this, ref oldBounds, ref isOnGround, collisionCallback);

            if (isOnGround)
            {
                CurrentVelocity = new Vector2(CurrentVelocity.X, 0);
            }
        }

        #endregion

        #region ISpellInfluenceable Member

        private TimeSpan currentElectrifiedTime = new TimeSpan(0, 0, 0);

        private TimeSpan currentFreezeTime = new TimeSpan(0, 0, 0);

        private TimeSpan currentBurningTime = new TimeSpan(0, 0, 0);

        double spellDurationOfActionMs = 0;

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
                spellDurationOfActionMs = spell.DurationOfActionMs;
                return true;
            }

            return false;
        }

        #endregion

        protected void HandleCollisionWithObject(BasicGameElement element, bool xAxisCollision, bool yAxisCollision)
        {
            //TODO SET WAITTIME OR SOMETHING LIKE THIS
            if (element.GetType() == typeof(BlockElement))
            {
                BlockElement e = (BlockElement)element;
                if (!e.Texture.Name.Contains("ground"))
                {
                    CurrentVelocity = CurrentVelocity * -1;
                    //waitTime = MaxWaitTime;
                }
            }
            else if (element.GetType() == typeof(ShadowCreature))
            {
                ShadowCreature e = (ShadowCreature)element;
                CurrentVelocity = CurrentVelocity * -1;
            }
        }
    }
}
