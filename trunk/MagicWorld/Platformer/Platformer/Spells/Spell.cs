using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    /// <summary>
    /// Base class that determines a spell
    /// </summary>
    abstract class Spell : IAutonomusGameObject
    {
        #region properties
        /// <summary>
        /// Origin from the object in the screen
        /// </summary>
        public Vector2 Origin;

        public enum State { CREATING, WORKING, REMOVE };
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
            set { spellTexture = value; }
        }

        /// <summary>
        /// describes how long a spell is alive
        /// </summary>
        protected double survivalTimeMs;
        public double SurvivalTimeMs
        {
            set { survivalTimeMs = value; }
            get { return survivalTimeMs; }
        }

        // Animations
        protected Animation runAnimation;
        protected Animation idleAnimation;
        protected AnimationPlayer sprite;


        /// <summary>
        /// describes how long the magic of a spell influences a attacked object
        /// </summary>
        protected double durationOfActionMs = 0;

        public double DurationOfActionMs
        {
            get { return durationOfActionMs; }
        }

        #endregion

        #region methods

        public Spell(string spriteSet, Vector2 _origin, Level level)
        {
            //spellTexture = _texture;
            Origin = _origin;
            Velocity = Vector2.Zero;
            Force = 0;
            Position = _origin;
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

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
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
            //remove a spell after his time is come
            if (survivalTimeMs < 0)
            {
                this.spellState = State.REMOVE;
            }
            survivalTimeMs -= gameTime.ElapsedGameTime.TotalMilliseconds;
            HandleCollision();
        }

        public virtual void Grow()
        {
        }
        public virtual void Shrink()
        {
        }


        public virtual void HandleCollision()
        {
            Rectangle bounds = BoundingRectangle;

            // Calculate tile position based on the side we are walking towards.
            float posX = Position.X + bounds.Width / 2 * (int)direction;
            int x = (int)Math.Floor(posX / Tile.Width) - (int)direction;
            int y = (int)Math.Floor(Position.Y / Tile.Height);


            // If this tile is collidable,
            TileCollision collision = level.GetCollision(x, y);
            if (collision == TileCollision.OutOfLevel)
            {
                spellState = State.REMOVE;
            }
            else if (collision == TileCollision.Impassable || collision == TileCollision.Platform)
            {
                Tile tile = level.GetTile(x, y);
                if (tile.SpellInfluenceAction(this))
                {
                    spellState = State.REMOVE;
                }
            }
        }
        #endregion
    }
}
