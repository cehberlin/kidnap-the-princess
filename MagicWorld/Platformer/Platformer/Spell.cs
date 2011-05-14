using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    /// <summary>
    /// Base class that determines a spell
    /// </summary>
    abstract class Spell
    {
        #region properties
        /// <summary>
        /// Origin from the object in the screen
        /// </summary>
        public Vector2 Origin;

        public enum State { CREATING, WORKING };
        public State spellState;
        /// <summary>
        /// Actual size of the object
        /// the idea is that the object on the moment of creation grows
        /// </summary>
        public Rectangle Size;
        public Rectangle MaxSize;
        public Rectangle MinSize;
        /// <summary>
        /// Actual Position
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Force of the spell.
        /// It may work as factor that scale distance,velocity or time
        /// </summary>
        private int force;
        public int Force
        {
            set { force = value; }
            get { return force; }
        }

        protected Level level;

        protected SpriteEffects flip = SpriteEffects.None;

        /// <summary>
        /// velocity of the movement of the spell
        /// </summary>
        protected Vector2 velocity;
        public Vector2 Velocity
        {
            set { velocity = value; }
            get { return velocity; }
        }

        protected float direction;

        public float Direction
        {
            set { direction = value; }
            get { return direction; }
        }

        /// <summary>
        /// Texture of object
        /// </summary>
        private Texture2D spellTexture;
        public Texture2D SpellTexture
        {
            set {spellTexture = value;}
        }

        protected float time;
        
        private float survivalTime;
        public float SurvivalTime
        {
            set {survivalTime=value;}
            get {return survivalTime;}
        }

        // Animations
        protected Animation runAnimation;
        protected Animation idleAnimation;
        protected AnimationPlayer sprite;

        #endregion
        
        #region methods

        public Spell(string spriteSet, Vector2 _origin,  Level level)
        {
            //spellTexture = _texture;
            Origin = _origin;
            Velocity = Vector2.Zero;
            Force = 0;
            Position = _origin;
            time = 0;
            this.level = level;
            spellState = State.CREATING;
            LoadContent(spriteSet);
        }
        


        /// <summary>
        /// Loads a particular enemy sprite sheet and sounds.
        /// </summary>
        public virtual void LoadContent(string spriteSet)
        {
            
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(Position.X - sprite.Origin.X) + Size.X;
                int top = (int)Math.Round(Position.Y - sprite.Origin.Y) + Size.Y;

                return new Rectangle(left, top, Size.Width, Size.Height);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) 
        {
            // Flip the sprite to face the way we are moving.
            if (direction > 0)
                flip = SpriteEffects.FlipHorizontally;
            else if (direction < 0)
                flip = SpriteEffects.None;

            // Draw that sprite.
            sprite.Draw(gameTime, spriteBatch, Position, flip);
        }
        public virtual void Update(GameTime gameTime)
        {
            
        }
        public void Grow()
        {
        }
        public void Shrink()
        {
        }
        #endregion
    }
}
