#region File Description
//-----------------------------------------------------------------------------
// Gem.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using MagicWorld.DynamicLevelContent;
using MagicWorld.HelperClasses;

namespace MagicWorld
{
    /// <summary>
    /// A valuable item the player can collect.
    /// </summary>
    class Gem:BasicGameElement
    {
        private Texture2D texture;
        private Vector2 origin;
        private SoundEffect collectedSound;

        public const int PointValue = 30;
        public readonly Color Color = Color.Yellow;

        // The gem is animated from a base position along the Y axis.
        private Vector2 basePosition;
        private float bounce;

        /// <summary>
        /// Gets the current position of this gem in world space.
        /// </summary>
        public override  Vector2 Position
        {
            get
            {
                return basePosition + new Vector2(0.0f, bounce);
            }
        }


        /// <summary>
        /// Constructs a new gem.
        /// </summary>
        public Gem(Level level, Vector2 position):base(level)
        {
            this.level = level;
            this.basePosition = position;
            bounds = new Bounds(position, 15f);

            LoadContent("Sprites/Gem");
        }

        /// <summary>
        /// Loads the gem texture and collected sound.
        /// </summary>
        public override void LoadContent(string spriteSet)
        {
            texture = level.Content.Load<Texture2D>(spriteSet);
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            collectedSound = level.Content.Load<SoundEffect>("Sounds/GemCollected");
        }

        /// <summary>
        /// Bounces up and down in the air to entice players to collect them.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Bounce control constants
            const float BounceHeight = 0.18f;
            const float BounceRate = 3.0f;
            const float BounceSync = -0.75f;

            // Bounce along a sine curve over time.
            // Include the X coordinate so that neighboring gems bounce in a nice wave pattern.            
            double t = gameTime.TotalGameTime.TotalSeconds * BounceRate + Position.X * BounceSync;
            bounce = (float)Math.Sin(t) * BounceHeight * texture.Height;
        }

        /// <summary>
        /// Called when this gem has been collected by a player and removed from the level.
        /// </summary>
        /// <param name="collectedBy">
        /// The player who collected this gem. Although currently not used, this parameter would be
        /// useful for creating special powerup gems. For example, a gem could make the player invincible.
        /// </param>
        public void OnCollected(Player collectedBy)
        {
            collectedSound.Play();
        }

        /// <summary>
        /// Draws a gem in the appropriate color.
        /// </summary>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
        }

    }
}
