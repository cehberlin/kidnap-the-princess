using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.HelperClasses;
using MagicWorld.DynamicLevelContent;
using System.Collections.Generic;
using ParticleEffects;
using MagicWorld.Constants;

namespace MagicWorld.Ingredients
{
    /// <summary>
    /// Base class that determines a ingredient
    /// </summary>
    class Ingredient : BasicGameElement
    {
        public Texture2D Texture;

        public int Width = 40;
        public int Height = 32;
        public int particelCounter = 0;

        public override Vector2 Position
        {
            get { return Position; }
            set { position = value; }
        }

        /// <summary>
        /// Constructs a new Igredient.
        /// </summary>
        public Ingredient(String texture, CollisionType collision, Level level, Vector2 position)
            : base(level)
        {
            this.level = level;
            Collision = collision;
            this.position = position;
            level.Ingredients.Add(this);
            if (texture != null)
            {
                LoadContent(texture);
            }
        }

        /// <summary>
        /// Constructs a new Ingredient.
        /// </summary>
        public Ingredient(String texture, CollisionType collision, Level level, Vector2 position, int width, int height)
            : this(texture, collision, level, position)
        {
            this.Width = width;
            this.Height = height;
            level.Ingredients.Add(this);
        }

        public override Bounds Bounds
        {
            get
            {
                int left = (int)Math.Round(position.X);
                int top = (int)Math.Round(position.Y);

                return new Bounds(left, top, Width, Height);
            }
        }

        #region IAutonomusGameObject Member

        public override void LoadContent(string spriteSet)
        {
            Texture = level.Content.Load<Texture2D>(spriteSet);
            Texture.Name = spriteSet;
            base.LoadContent("");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, Bounds.getRectangle(), Color.White);
                base.Draw(gameTime, spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (particelCounter % 2 == 0)
            {
                Bounds bounds = Bounds;
                level.Game.MagicItemParticleSystem.AddParticles(new ParticleSetting(position + new Vector2(bounds.Width / 2, bounds.Height / 2), Color.LightCyan));
            }
            particelCounter++;
            if (level.CollisionManager.CollidateWithPlayer(this))
            {
                this.isRemovable = true;
                level.CollectedIngredients.Add(this);
                audioService.playSound(Audio.SoundType.collect);
            }
        }
        #endregion

    }
}
