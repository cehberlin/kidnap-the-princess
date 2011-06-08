using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.HelperClasses;
using MagicWorld.DynamicLevelContent;
using System.Collections.Generic;
using MagicWorld.Spells;

namespace MagicWorld
{
    /// <summary>
    /// Base class that determines a spell
    /// </summary>
    public abstract class Spell : BasicGameElement
    {
        #region "mana"
            /// <summary>
            /// Basic Cost to pay if 
            /// </summary>
            public int ManaBasicCost {get{return manaBasicCost;}}
            private int manaBasicCost;
            /// <summary>
            /// Costs while casting the spell
            /// </summary>
            public  float ManaCastingCost{get{return manaCastingCost;}}
            private float manaCastingCost;
        #endregion

            public SpellType SpellType { get; protected set; }

        #region properties

            public SpellType SpellSlot_A { get; set; }
            public SpellType SpellSlot_B { get; set; }

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
                        isRemovable = true;
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

        public override Bounds Bounds
        {
            get
            {
                // Calculate bounds within texture size.
                float width = (sprite.Animation.FrameWidth * currentScale);
                float height = (sprite.Animation.FrameHeight * currentScale);
                return new Bounds(position, width, height);
            }
        }

        protected float currentScale = 1.0f;
        protected float MaxScale = 3.0f;
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

        public Spell(string spriteSet, Vector2 _origin, Level level, int manaBasicCost, float manaCastingCost, SpellType spellType)
            : base(level)
        {
            SpellType = spellType;
            this.manaBasicCost = manaBasicCost;
            this.manaCastingCost = manaCastingCost;
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
        public override void LoadContent(string spriteSet)
        {
            base.LoadContent("");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Flip the sprite to face the way we are moving.
            if (direction.X > 0)
                flip = SpriteEffects.FlipHorizontally;
            else if (direction.X < 0)
                flip = SpriteEffects.None;
            Vector2 paintPosition = Bounds.Position;

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
            base.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (SpellState == State.WORKING)
            {
                HandleMovement(gameTime);
                HandleLiveTime(gameTime);
                //does not work proper dont know why at the moment
                //if (direction == new Vector2()) { //if spell has no direction and stay on currnt position use idle animation
                //    if (sprite.Animation != idleAnimation)
                //    {
                //        sprite.PlayAnimation(idleAnimation);
                //    }
                //}
                //else
                {
                    //only start playing if animation changes because frame position is reseted
                    if (sprite.Animation != runAnimation)
                    {
                        sprite.PlayAnimation(runAnimation);
                    }
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
            float posX = Position.X + Bounds.Width / 2 * (int)direction.X;

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


        /// <summary>
        /// throw away current spell
        /// </summary>
        public virtual void FireUp()
        {
            SpellState = State.WORKING;
        }

        ///Every type of collision has its own virtual method, so its easy to override special behavior in subclasses
        #region collision

        /// <summary>
        /// handels collision with enemie
        /// </summary>
        public virtual void HandleEnemyCollision()
        {
            List<Enemy> collisionEnemies = new List<Enemy>();

            level.CollisionManager.CollidateWithEnemy(this, ref collisionEnemies);
            //enemy collision
            foreach (Enemy enemy in collisionEnemies)
            {                
                    if (enemy.SpellInfluenceAction(this))
                    {
                        SpellState = State.REMOVE;
                    }                
            }
        }

        /// <summary>
        /// handels collision with tiles
        /// </summary>
        public virtual void HandleCollisionWithLevelElements()
        {
            List<BasicGameElement> collisionObjects = new List<BasicGameElement>();

            level.CollisionManager.CollidateWithGeneralLevelElements(this, ref collisionObjects);
            //enemy collision
            foreach (BasicGameElement tile in collisionObjects)
            {
                if (tile.SpellInfluenceAction(this))
                {
                    SpellState = State.REMOVE;
                }
            }
        }

        /// <summary>
        /// handels collision with level bounds
        /// </summary>
        public virtual void HandleOutOfLevelCollision()
        {
            if (level.CollisionManager.CollidateWithLevelBounds(this))
            {
                SpellState = State.REMOVE;
            }
        }

        /// <summary>
        /// handels collision with player
        /// </summary>
        public virtual void HandlePlayerCollision()
        {
            //check if collision is occured
            if (level.CollisionManager.CollidateWithPlayer(this))
            {
                if (level.Player.SpellInfluenceAction(this))
                {
                    SpellState = State.REMOVE;
                }
            }
        }

        /// <summary>
        /// handels collision with other objects
        /// </summary>
        public virtual void HandleObjectsCollision()
        {

            List<BasicGameElement> levelElements = new List<BasicGameElement>();

            level.CollisionManager.CollidateWithGeneralLevelElements(this, ref levelElements);
            //enemy collision
            foreach (BasicGameElement elem in levelElements)
            {
                if (elem.SpellInfluenceAction(this))
                {
                    SpellState = State.REMOVE;
                }
            }
        }

        /// <summary>
        /// handels collision with tiles and enemies and level bounds
        /// </summary>
        public virtual void HandleCollision()
        {
            //player collision
            HandlePlayerCollision();

            //enemy collision
            HandleEnemyCollision();

            //Tile collision
            HandleCollisionWithLevelElements();        

            //check if spells leaves the level
            HandleOutOfLevelCollision();

            //objects collision
            HandleObjectsCollision();
        }

        #endregion collision

        #endregion
    }
}
