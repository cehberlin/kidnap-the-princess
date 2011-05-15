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

        protected float MoveSpeed = 0;

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
            HandleMovement(gameTime);
            HandleLiveTime(gameTime);
            HandleCollision();
        }

        /// <summary>
        /// Handles spell movement
        /// </summary>
        /// <param name="gameTime"></param>
        private void HandleMovement(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Calculate tile position based on the side we are walking towards.
            float posX = Position.X + Size.Width / 2 * (int)direction;

            // Move in the current direction.
            velocity = new Vector2((int)direction * MoveSpeed * elapsed, 0.0f);
            Position = Position + velocity;
        }

        /// <summary>
        /// Handles how long a spell lives
        /// </summary>
        /// <param name="gameTime"></param>
        private void HandleLiveTime(GameTime gameTime)
        {
            //remove a spell after his time is come
            if (survivalTimeMs < 0)
            {
                this.spellState = State.REMOVE;
            }
            survivalTimeMs -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public virtual void Grow()
        {

        }

        public virtual void Shrink()
        {
        }

        /// <summary>
        /// handels collision with tiles and enemies
        /// </summary>
        public virtual void HandleCollision()
        {
            //enemy collision
            foreach (Enemy enemy in level.Enemies)
            {
                if (enemy.BoundingRectangle.Intersects(this.BoundingRectangle))
                {
                    if (enemy.SpellInfluenceAction(this))
                    {
                        spellState = State.REMOVE;
                    }
                }
            }

            //Tile collision

            foreach (Tile tile in level.Tiles)
            {
                 //first check if collision is possible
                if (tile.Collision == TileCollision.Impassable || tile.Collision == TileCollision.Platform)
                {
                    //check if collision is occured
                    if (tile.BoundingRectangle.Intersects(this.BoundingRectangle))
                    {
                            if (tile.SpellInfluenceAction(this))
                            {
                                spellState = State.REMOVE;
                            }
                    }                    
                }
            }

            //check if spells leaves the level

            Rectangle bounds = BoundingRectangle;

            // Calculate tile position based on the side we are walking towards.
            float posX = Position.X + bounds.Width / 2 * (int)direction;
            int x = (int)Math.Floor(posX / Tile.Width) - (int)direction;
            int y = (int)Math.Floor(Position.Y / Tile.Height);

            if (x > level.Width || x < 0 || y > level.Height || y < 0)
            {
                spellState = State.REMOVE;
            }

        }
        #endregion
    }
}
