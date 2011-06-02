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
using Platformer.HelperClasses;
using Platformer.DynamicLevelContent;

namespace Platformer
{
    /// <summary>
    /// Facing direction along the X axis.
    /// </summary>
    enum FaceDirection
    {
        Left = -1,
        Right = 1,
    }

    /// <summary>
    /// A monster who is impeding the progress of our fearless adventurer.
    /// </summary>
    class Enemy:BasicGameElement
    {

        // Animations
        private Animation runAnimation;
        private Animation idleAnimation;
        private AnimationPlayer sprite;

        /// <summary>
        /// The direction this enemy is facing and moving along the X axis.
        /// </summary>
        private FaceDirection direction = FaceDirection.Left;

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

        /// <summary>
        /// Constructs a new Enemy.
        /// </summary>
        public Enemy(Level level, Vector2 position, string spriteSet)
            : base(level)
        {
            this.Position = position;

            LoadContent(spriteSet);

            //init spell states
            isBurning = false;
            isFroozen = false;
        }

        /// <summary>
        /// Loads a particular enemy sprite sheet and sounds.
        /// </summary>
        public override void LoadContent(string spriteSet)
        {
            // Load animations.
            spriteSet = "Sprites/" + spriteSet + "/";
            runAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Run"), 0.1f, true,10);
            idleAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Idle"), 0.15f, true, 11);
            sprite.PlayAnimation(idleAnimation);

            // Calculate bounds within texture size.
            int width = (int)(idleAnimation.FrameWidth * 0.35);
            int left = (idleAnimation.FrameWidth - width) / 2;
            int height = (int)(idleAnimation.FrameWidth * 0.7);
            int top = idleAnimation.FrameHeight - height;
            bounds = new Bounds(left, top, width, height);
            base.LoadContent("");
        }


        /// <summary>
        /// Paces back and forth along a platform, waiting at either end.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (isFroozen)
            {
                currentFreezeTime = currentFreezeTime.Add(gameTime.ElapsedGameTime);
                if (currentFreezeTime >= maxFreezeTime)
                {
                    isFroozen = false;
                    idleAnimation.TextureColor = Color.White;
                    runAnimation.TextureColor = Color.White;
                }
            }
            else if (isBurning)
            {
                currentBurningTime = currentBurningTime.Add(gameTime.ElapsedGameTime);
                if (currentBurningTime >= maxBurningTime)
                {
                    isBurning = false;
                    idleAnimation.TextureColor = Color.White;
                    runAnimation.TextureColor = Color.White;
                }
            }

            if (!isFroozen)
            {
                //if (level.Player.Position.X > this.position.X +10 && this.direction.Equals(FaceDirection.Left) ||
                //    level.Player.Position.X < this.position.X -10 && this.direction.Equals(FaceDirection.Right))
                //{
                //    direction = (FaceDirection)(-(int)direction);
                //}

                // Calculate tile position based on the side we are walking towards.

                //replace collision detection
            //    float posX = Position.X + localBounds.Width / 2 * (int)direction;
            //    int tileX = (int)Math.Floor(posX / Tile.Width) - (int)direction;
            //    int tileY = (int)Math.Floor(Position.Y / Tile.Height);

            //    if (waitTime > 0)
            //    {
            //        // Wait for some amount of time.
            //        waitTime = Math.Max(0.0f, waitTime - (float)gameTime.ElapsedGameTime.TotalSeconds);
            //        if (waitTime <= 0.0f)
            //        {
            //            // Then turn around.
            //            direction = (FaceDirection)(-(int)direction);
            //        }
            //    }
            //    else
            //    {

            //        TileCollision collisonOne = Level.GetCollision(tileX + (int)direction, tileY);
            //        TileCollision collisonTwo = Level.GetCollision(tileX + (int)direction, tileY - 1);
            //        // If we are about to run into a wall or off a cliff, start waiting.
            //        if (collisonTwo == TileCollision.Impassable ||
            //            collisonOne == TileCollision.Passable )
            //        {
            //            waitTime = MaxWaitTime;
            //        }
            //        if (this.position.X < level.Player.Position.X &&
            //            (this.position.X + 6 > level.Player.Position.X))
            //        {
            //            waitTime = MaxWaitTime;
            //        }
            //        else
            //        {
            //            // Move in the current direction.
            //            Vector2 velocity;
            //            if (isBurning)
            //            {
            //                velocity = new Vector2((int)direction * MoveSpeed * elapsed * burningMovingSpeedFactor, 0.0f);
            //            }
            //            else
            //            {
            //                velocity = new Vector2((int)direction * MoveSpeed * elapsed, 0.0f);
            //            }
            //            position = position + velocity;
            //        }
            //    }
            }
        }

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
            SpriteEffects flip = direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            sprite.Draw(gameTime, spriteBatch, Position, flip);

            base.Draw(gameTime, spriteBatch);
        }

        #region ISpellInfluenceable Member


        private TimeSpan currentFreezeTime = new TimeSpan(0, 0, 0);
        private TimeSpan maxFreezeTime = new TimeSpan(0,0,5);

        private TimeSpan currentBurningTime = new TimeSpan(0, 0, 0);
        private TimeSpan maxBurningTime = new TimeSpan(0, 0, 3);
        private float burningMovingSpeedFactor = 2f; // Factor the enemy is moving faster while under influence of warm spell

        /// <summary>
        /// some variables for spell reaction
        /// </summary>
        //enum SpellState { NORMAL, BURNED, FROZEN };
        //SpellState spellState = SpellState.NORMAL;
        double spellDurationOfActionMs = 0;

        public Boolean isBurning { get; set; }
        public Boolean isFroozen { get; set; }

        public override bool SpellInfluenceAction(Spell spell)
        {
            if (spell.GetType() == typeof(WarmSpell))
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
                    currentBurningTime = new TimeSpan(0,0,0);
                    spellDurationOfActionMs = spell.DurationOfActionMs;
                    idleAnimation.TextureColor = Color.Red;
                    runAnimation.TextureColor = Color.Red;
                }
                return true;
            }
            if (spell.GetType() == typeof(ColdSpell))
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
            return false;
        }

        #endregion
    }
}
