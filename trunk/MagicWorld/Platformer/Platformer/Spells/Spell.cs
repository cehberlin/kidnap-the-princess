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
        private State spellState;

        public State SpellState
        {
            get { return spellState; }
            set
            {
                switch (value)
                {
                    case State.REMOVE:
                        OnRemove();
                        break;
                    case State.CREATING:
                        OnCreateStart();
                        break;
                    case State.WORKING:
                        OnWorkingStart();
                        break;
                }
                spellState = value;

            }
        }
        /// <summary>
        /// Actual size of the object
        /// the idea is that the object on the moment of creation grows
        /// </summary>
        private Rectangle size;

        protected Rectangle Size
        {
            get
            {
                // Calculate bounds within texture size.
                int width = (int)(sprite.Animation.FrameWidth * currentScale);
                int height = (int)(sprite.Animation.FrameHeight * currentScale);
                int top = 0;
                int left = 0;
                return new Rectangle(left, top, width, height);
            }
            set { size = value; }
        }

        protected float currentScale = 1.0f;
        protected float MaxScale = 3.0f;

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

        protected float rotation = 0.0f;

        /// <summary>
        /// velocity of the movement of the spell
        /// </summary>
        protected Vector2 velocity;
        public Vector2 Velocity
        {
            set { velocity = value; }
            get { return velocity; }
        }

        protected Vector2 direction;

        public Vector2 Direction
        {
            set { direction = value; }
            get { return direction; }
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

        protected Texture2D debugTexture;

        /// <summary>
        /// describes how long the magic of a spell influences a attacked object
        /// </summary>
        protected double durationOfActionMs = 0;

        /// <summary>
        /// duration depents on spell force
        /// </summary>
        public double DurationOfActionMs
        {
            get { return durationOfActionMs * Force; }
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
            SpellState = State.CREATING;
            LoadContent(spriteSet);
        }



        /// <summary>
        /// Loads a particular enemy sprite sheet and sounds.
        /// </summary>
        public virtual void LoadContent(string spriteSet)
        {
            debugTexture = level.Content.Load<Texture2D>("Sprites\\white");
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                //its correct, but dont know why, could be checked in debug mode
                int left = (int)Math.Round(Position.X - Size.Width / 2);
                int top = (int)Math.Round(Position.Y - Size.Height);

                return new Rectangle(left, top, (int)(Size.Width), (int)(Size.Height));
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Flip the sprite to face the way we are moving.
            if (direction.X > 0)
                flip = SpriteEffects.FlipHorizontally;
            else if (direction.X < 0)
                flip = SpriteEffects.None;
            Vector2 paintPosition = new Vector2(BoundingRectangle.X, BoundingRectangle.Y);

            if (direction.X == 0)
            {
                if (direction.Y < 0)
                {
                    rotation = (float)Math.PI * 0.5f;

                }
                else if (direction.Y > 0)
                {
                    rotation = -(float)Math.PI * 0.5f ;

                }
            }
            else if (direction.X > 0)
            {
                if (direction.Y < 0)
                    rotation = -(float)Math.PI * 1/4;
                else if (direction.Y > 0)
                    rotation = (float)Math.PI * 1/4;
            }
            else if (direction.X < 0)
            {
                if (direction.Y < 0)
                    rotation = (float)Math.PI * 1/4;
                else if (direction.Y > 0)
                    rotation = -(float)Math.PI * 1/4;
            }
            // Draw that sprite.
            sprite.Draw(gameTime, spriteBatch, Position, flip, rotation);
            if (GlobalValues.DEBUG)
            {
                spriteBatch.Draw(debugTexture, BoundingRectangle, Color.Pink);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (SpellState == State.WORKING)
            {
                HandleMovement(gameTime);
                HandleLiveTime(gameTime);
                //only start playing if animation changes because frame position is reseted
                if (sprite.Animation != runAnimation)
                {
                    sprite.PlayAnimation(runAnimation);
                }
            }
            else if (SpellState == State.CREATING)
            {
                Grow(gameTime);
                //only start playing if animation changes because frame position is reseted
                if (sprite.Animation != idleAnimation)
                {
                    sprite.PlayAnimation(idleAnimation);
                }
            }
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
            float posX = Position.X + Size.Width / 2 * (int)direction.X;

            // Move in the current direction.
            velocity = new Vector2((int)direction.X * MoveSpeed * elapsed, (int)direction.Y * MoveSpeed * elapsed);
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
                this.SpellState = State.REMOVE;
            }
            survivalTimeMs -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }


        protected virtual void OnRemove()
        {
        }

        protected virtual void OnCreateStart()
        {
        }

        protected virtual void OnWorkingStart()
        {
        }

        public virtual void Grow(GameTime gameTime)
        {

            if (currentScale <= MaxScale)
            {
                currentScale += 0.02f;
                idleAnimation.Scale = currentScale;
                runAnimation.Scale = currentScale;
                Force++;
            }
        }

        //Makes shrinking sense?
        //public virtual void Shrink()
        //{
        //}

        /// <summary>
        /// throw away current spell
        /// </summary>
        public virtual void FireUp()
        {
            SpellState = State.WORKING;
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
                        SpellState = State.REMOVE;
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
                            SpellState = State.REMOVE;
                        }
                    }
                }
            }

            //check if spells leaves the level

            Rectangle bounds = BoundingRectangle;

            // Calculate tile position based on the side we are walking towards.
            float posX = Position.X + bounds.Width / 2 * (int)direction.X;
            int x = (int)Math.Floor(posX / Tile.Width) - (int)direction.Y;
            int y = (int)Math.Floor(Position.Y / Tile.Height);

            if (x > level.Width || x < 0 || y > level.Height || y < 0)
            {
                SpellState = State.REMOVE;
            }

        }
        #endregion
    }
}
