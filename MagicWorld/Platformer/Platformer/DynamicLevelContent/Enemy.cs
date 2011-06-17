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

namespace MagicWorld
{
    /// <summary>
    /// A monster who is impeding the progress of our fearless adventurer.
    /// </summary>
    public class Enemy : BasicGameElement, IEnemyService
    {

        // Animations
        protected Animation runAnimation;
        protected Animation idleAnimation;
        protected AnimationPlayer sprite;

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
                float top = (float)Math.Round(Position.Y - height / 2);
                return new Bounds(left, top + 30, width, height);
            }
        }

        /// <summary>
        /// callback delegate for collision with specific objects
        /// </summary>
        protected CollisionManager.OnCollisionWithCallback collisionCallback;

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
            //collisionCallback = HandleCollisionWithObject;
            ResetVelocity();
        }

        /// <summary>
        /// Loads a particular enemy sprite sheet and sounds.
        /// </summary>
        public override void LoadContent(string spriteSet)
        {
            base.LoadContent(spriteSet);
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

        }

        /// <summary>
        /// Checks for collision with level elements like bounds...
        /// </summary>
        /// <returns>returns true if collision occured</returns>
        private void HandleCollision()
        {
            if (level.CollisionManager.CollidateWithLevelBounds(this))
            {
                this.isRemovable = true;
            }

            if (level.CollisionManager.CollidateWithPlayer(this))
            {
                level.Player.OnKilled(this);
            }
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
                level.Player.Position.X.Equals(this.Position.X)
                || isFroozen
                )
            {
                sprite.PlayAnimation(idleAnimation);
            }
            else
            {
                sprite.PlayAnimation(runAnimation);
            }

            //Draw facing the way the enemy is moving.
            SpriteEffects flip = velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            sprite.Draw(gameTime, spriteBatch, Position, flip);

            base.Draw(gameTime, spriteBatch);
        }

        #endregion


        public Boolean isBurning { get; set; }
        public Boolean isFroozen { get; set; }
        public Boolean isElectrified { get; set; }

        public Boolean IsBurning
        {
            get { return isBurning; }
            set { isBurning = value; }
        }

        public Boolean IsFroozen
        {
            get { return isFroozen; }
            set { isFroozen = value; }
        }

        public Boolean IsElectrified
        {
            get { return isElectrified; }
            set { isElectrified = value; }
        }

        public object GetService(Type serviceType)
        {
            return this;
        }
    }
}
