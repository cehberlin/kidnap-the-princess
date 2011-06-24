﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.HelperClasses;
using MagicWorld.DynamicLevelContent;
using System.Collections.Generic;
using MagicWorld.Constants;
using System.Diagnostics;
using MagicWorld.Services;
using MagicWorld.Gleed2dLevelContent;

namespace MagicWorld
{
    /// <summary>
    /// A monster who is impeding the progress of our fearless adventurer.
    /// </summary>
    public class Bat : Enemy
    {
        /// <summary>
        /// The path the Enemy should follow
        /// </summary>
        private PathItem path = null;

        private int pathPosition = 0;
        private int nextPosition = 1;

        Vector2 currentPathPosition;
        Vector2 nextPathPosition;

        private float deltaX;
        private float deltaY;

        private float steps = 0;

        private float movementSpeedX = 0;
        private float movementSpeedY = 0;

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
        private const float MoveSpeed = 128f;//64.0f;


        public override Bounds Bounds
        {
            get
            {
                float width = (sprite.Animation.FrameWidth * 0.5f);
                float height = (sprite.Animation.FrameHeight * 0.5f);
                float left = (float)Math.Round(Position.X - width / 2);
                float top = (float)Math.Round(Position.Y - height / 2);
                return new Bounds(left, top, width, height);
            }
        }

        Bounds oldBounds;


        /// <summary>
        /// callback delegate for collision with specific objects
        /// </summary>
        //protected CollisionManager.OnCollisionWithCallback collisionCallback;

        #region loading

        /// <summary>
        /// Constructs a new Enemy.
        /// </summary>
        public Bat(Level level, Vector2 position, string spriteSet, PathItem path)
            : base(level, position, spriteSet)
        {
            this.path = path;
            currentPathPosition = path.WorldPoints[pathPosition];
            nextPathPosition = path.WorldPoints[nextPosition];
            base.position = currentPathPosition + new Vector2(sprite.Animation.FrameWidth * 0.25f, sprite.Animation.FrameHeight * 0.25f);
        }

        /// <summary>
        /// Loads a particular enemy sprite sheet and sounds.
        /// </summary>
        public override void LoadContent(string spriteSet)
        {
            // Load animations.
            base.runAnimation = new Animation("Content/Sprites/Bat/BatSpritesheet", 0.02f, 12, level.Content.Load<Texture2D>("Sprites/Bat/BatSpritesheet"), 0);
            base.idleAnimation = new Animation("Content/Sprites/Bat/BatSpritesheet", 0.02f, 12, level.Content.Load<Texture2D>("Sprites/Bat/BatSpritesheet"), 0);
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
            //level.PhysicsManager.ApplyGravity(this, PhysicValues.DEFAULT_GRAVITY, gameTime);

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
            else if (isElectrified)
            {
                //if (isOnGround)
                //    velocity = new Vector2(MoveSpeed, 0);
                //currentElectrifiedTime = currentElectrifiedTime.Add(gameTime.ElapsedGameTime);
                //if (currentElectrifiedTime >= SpellInfluenceValues.maxElectrifiedTime)
                //{
                //    isElectrified = false;
                //}
                currentParticles++;

                if (currentParticles % 4 == 0) //only every 4 update cycle
                {
                    level.Game.LightningCreationParticleSystem.AddParticles(GeometryCalculationHelper.getRandomPositionOnCycleBow(position, 20));
                }
            }

            float acceleration = 1;

            if (!isFroozen && !isElectrified) // ****** can move ******
            {
                deltaX = 0;
                deltaY = 0;
                if (nextPosition == 2)
                    nextPosition = 2;
                deltaX = Math.Abs(currentPathPosition.X) - Math.Abs(nextPathPosition.X);
                deltaX = Math.Abs(deltaX);
                deltaY = Math.Abs(currentPathPosition.Y) - Math.Abs(nextPathPosition.Y);
                deltaY = Math.Abs(deltaY);

                if (deltaX > deltaY)
                {
                    steps = deltaX / MoveSpeed;
                    if (currentPathPosition.X > nextPathPosition.X)
                        movementSpeedX = -MoveSpeed;
                    else
                        movementSpeedX = MoveSpeed;
                    if (currentPathPosition.Y > nextPathPosition.Y)
                        movementSpeedY = deltaY / steps * -1;
                    else
                        movementSpeedY = deltaY / steps;
                }
                else if (deltaY > deltaX)
                {
                    steps = deltaY / MoveSpeed;
                    if (currentPathPosition.Y > nextPathPosition.Y)
                        movementSpeedY = -MoveSpeed;
                    else
                        movementSpeedY = MoveSpeed;
                    movementSpeedX = deltaX / steps;
                }
                else if (deltaY == deltaX)
                {
                    steps = deltaX / MoveSpeed;
                    if (currentPathPosition.X > nextPathPosition.X)
                        movementSpeedX = -MoveSpeed;
                    else
                        movementSpeedX = MoveSpeed;

                    if (currentPathPosition.Y > nextPathPosition.Y)
                        movementSpeedY = -MoveSpeed;
                    else
                        movementSpeedY = MoveSpeed;
                }

                velocity = new Vector2(movementSpeedX, movementSpeedY);

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

                #region Enemy Movement
                //from left to right
                if (currentPathPosition.X > nextPathPosition.X && currentPathPosition.Y == nextPathPosition.Y)
                {
                    if (pathPosition <= path.WorldPoints.Length - 1 && Position.X < nextPathPosition.X - oldBounds.Width / 2)
                    {
                        if (pathPosition >= path.WorldPoints.Length - 1)
                            pathPosition = 0;
                        else
                            pathPosition++;
                        if (nextPosition >= path.WorldPoints.Length - 1)
                            nextPosition = 0;
                        else
                            nextPosition++;
                    }
                //from bottom to top
                }
                else if (currentPathPosition.X == nextPathPosition.X && currentPathPosition.Y > nextPathPosition.Y)
                {
                    if (pathPosition <= path.WorldPoints.Length - 1 && Position.Y < nextPathPosition.Y - oldBounds.Height / 2)
                    {
                        if (pathPosition >= path.WorldPoints.Length - 1)
                            pathPosition = 0;
                        else
                            pathPosition++;
                        if (nextPosition >= path.WorldPoints.Length - 1)
                            nextPosition = 0;
                        else
                            nextPosition++;
                    }

                }
                //from right to left
                else if (currentPathPosition.X < nextPathPosition.X && currentPathPosition.Y == nextPathPosition.Y)
                {
                    if (pathPosition <= path.WorldPoints.Length - 1 && Position.X > nextPathPosition.X - oldBounds.Width / 2)
                    {
                        if (pathPosition >= path.WorldPoints.Length - 1)
                            pathPosition = 0;
                        else
                            pathPosition++;
                        if (nextPosition >= path.WorldPoints.Length - 1)
                            nextPosition = 0;
                        else
                            nextPosition++;
                    }
                }
                //from top to bottom
                else if (currentPathPosition.X == nextPathPosition.X && currentPathPosition.Y < nextPathPosition.Y)
                {
                    if (pathPosition <= path.WorldPoints.Length - 1 && Position.Y > nextPathPosition.Y - oldBounds.Height / 2)
                    {
                        if (pathPosition >= path.WorldPoints.Length - 1)
                            pathPosition = 0;
                        else
                            pathPosition++;
                        if (nextPosition >= path.WorldPoints.Length - 1)
                            nextPosition = 0;
                        else
                            nextPosition++;
                    }
                }
                //from lower left to upper right
                else if (currentPathPosition.X < nextPathPosition.X && currentPathPosition.Y > nextPathPosition.Y)
                {
                    if (pathPosition <= path.WorldPoints.Length - 1 && Position.Y < nextPathPosition.Y - oldBounds.Height / 2
                        && Position.X > nextPathPosition.X - oldBounds.Width / 2)
                    {
                        if (pathPosition >= path.WorldPoints.Length - 1)
                            pathPosition = 0;
                        else
                            pathPosition++;
                        if (nextPosition >= path.WorldPoints.Length - 1)
                            nextPosition = 0;
                        else
                            nextPosition++;
                    }
                }
                //from top left to lower right
                else if (currentPathPosition.X < nextPathPosition.X && currentPathPosition.Y < nextPathPosition.Y)
                {
                    if (pathPosition <= path.WorldPoints.Length - 1 && Position.Y > nextPathPosition.Y - oldBounds.Height / 2
                        && Position.X > nextPathPosition.X - oldBounds.Width / 2)
                    {
                        if (pathPosition >= path.WorldPoints.Length - 1)
                            pathPosition = 0;
                        else
                            pathPosition++;
                        if (nextPosition >= path.WorldPoints.Length - 1)
                            nextPosition = 0;
                        else
                            nextPosition++;
                    }
                }
                //from top right to lower left
                else if (currentPathPosition.X > nextPathPosition.X && currentPathPosition.Y < nextPathPosition.Y)
                {
                    if (pathPosition <= path.WorldPoints.Length - 1 && Position.Y > nextPathPosition.Y - oldBounds.Height / 2
                        && Position.X < nextPathPosition.X - oldBounds.Width / 2)
                    {
                        if (pathPosition >= path.WorldPoints.Length - 1)
                            pathPosition = 0;
                        else
                            pathPosition++;
                        if (nextPosition >= path.WorldPoints.Length - 1)
                            nextPosition = 0;
                        else
                            nextPosition++;
                    }
                }
                //from lower right to top right
                else if (currentPathPosition.X > nextPathPosition.X && currentPathPosition.Y > nextPathPosition.Y)
                {
                    if (pathPosition <= path.WorldPoints.Length - 1 && Position.Y < nextPathPosition.Y - oldBounds.Height / 2
                        && Position.X < nextPathPosition.X - oldBounds.Width / 2)
                    {
                        if (pathPosition >= path.WorldPoints.Length - 1)
                            pathPosition = 0;
                        else
                            pathPosition++;
                        if (nextPosition >= path.WorldPoints.Length - 1)
                            nextPosition = 0;
                        else
                            nextPosition++;
                    }
                }
                #endregion Enemy Movement

                currentPathPosition = path.WorldPoints[pathPosition];
                nextPathPosition = path.WorldPoints[nextPosition];
                Position = Position + velocity * elapsed * acceleration;
            }
            //only handles physics collision
            //level.CollisionManager.HandleGeneralCollisions(this, velocity, ref oldBounds, ref isOnGround, collisionCallback);
        }

        #endregion

        #region drawing

        /// <summary>
        /// Draws the animated enemy.
        /// </summary>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
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
            }

            return false;
        }

        #endregion
    }
}
