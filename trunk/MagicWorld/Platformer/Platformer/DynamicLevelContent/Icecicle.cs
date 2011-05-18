using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Platformer
{
    enum IcecicleState { NORMAL, FALLING, DESTROYED };
    class Icecicle : ISpellInfluenceable,IAutonomusGameObject
    {
        protected Texture2D debugTexture;
        private Texture2D texture;
        private Vector2 origin;
        private SoundEffect hitSound;
        protected AnimationPlayer sprite;
        protected Animation idleAnimation;

        public IcecicleState icecicleState ;
        private float fallVelocity;

        public const int Width = 40;
        public const int Height = 41;

        /// <summary>
        /// Position in world space of the bottom center of this enemy.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
        }
        Vector2 position;

        public Level Level
        {
            get { return level; }
        }
        Level level;
        
        private Rectangle localBounds;
        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(position.X-Width/2);
                int top = (int)Math.Round(position.Y-Height);

                return new Rectangle(left, top, Width, Height);
            }
        }

        public Icecicle(Level level, Vector2 position)
            
        {
            this.level = level;
            this.position = position;
            icecicleState = IcecicleState.NORMAL;
            LoadContent("Sprites/Icecicle");
            sprite.PlayAnimation(idleAnimation);
        }

        public void LoadContent(string spriteSet)
        {
            texture = Level.Content.Load<Texture2D>(spriteSet);
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            hitSound = Level.Content.Load<SoundEffect>("Sounds/Icehit");
            idleAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet), 0.15f, true, 1);            
            fallVelocity = 0.8f;
            debugTexture = level.Content.Load<Texture2D>("Sprites\\white");
        }

        #region ISpellInfluenceable Member

        public virtual Boolean SpellInfluenceAction(Spell spell)
        {
            if (spell.GetType() == typeof(WarmSpell))
            {
                hitSound.Play();
                icecicleState = IcecicleState.FALLING;                
                return true;
            }
            else return false;
        }

        #endregion

        #region IAutonomusGameObject Member

        
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            SpriteEffects flip = SpriteEffects.None;
            // Draw it in screen space.                
            if (icecicleState != IcecicleState.DESTROYED)
            {
                sprite.Draw(gameTime, spriteBatch, position, flip);
            }

            if (GlobalValues.DEBUG)
            {
                spriteBatch.Draw(debugTexture, BoundingRectangle, Color.Pink);
            }           
            
        }

        public virtual void Update(GameTime gameTime)
        {
            if (icecicleState == IcecicleState.FALLING)
            {
                position.Y += (float)(position.Y * gameTime.ElapsedGameTime.TotalSeconds* fallVelocity);          
            }            
            HandleCollision();
        }
        #endregion

        #region collision
        /// <summary>
        /// handels collision with tiles and enemies and level bounds
        /// </summary>
        public virtual void HandleCollision()
        {
            if (icecicleState == IcecicleState.FALLING)
            {
                //player collision
                //HandlePlayerCollision();

                //enemy collision
                HandleEnemyCollision();

                //Tile collision
                HandleTileCollision();

                //check if spells leaves the level
                HandleOutOfLevelCollision();
            }
        }        

        /// <summary>
        /// handels collision with enemie
        /// </summary>
        public virtual void HandleEnemyCollision()
        {
            //enemy collision
            for (int i = 0; i < level.Enemies.Count; ++i)       
            
            {
                Enemy enemy = level.Enemies[i];
                if (enemy.BoundingRectangle.Intersects(this.BoundingRectangle))
                {
                    //destroy enemy
                    hitSound.Play();
                    icecicleState = IcecicleState.DESTROYED;
                    level.Enemies.Remove(enemy);
                }
            }
        }

        /// <summary>
        /// handels collision with tiles
        /// </summary>
        public virtual void HandleTileCollision()
        {
            //Tile collision

            

            foreach (Tile tile in level.Tiles)
            {
                //first check if collision is possible
                if (tile.Collision == TileCollision.Impassable || tile.Collision == TileCollision.Platform)
                {
                    //check if collision is occured
                    if (tile.BoundingRectangle.Intersects(this.BoundingRectangle))
                    {
                        //destroy the icecicle
                        icecicleState = IcecicleState.DESTROYED;
                    }
                }
            }
            
        }

        /// <summary>
        /// handels collision with level bounds
        /// </summary>
        public virtual void HandleOutOfLevelCollision()
        {
            Rectangle bounds = BoundingRectangle;

            // Calculate tile position based on the side we are walking towards.
            float posY = (int)Math.Floor((Position.Y+bounds.Height) / Tile.Height);          
            //int y = (int)Math.Floor(Position.Y / Tile.Height);

            if ( posY > level.Height )
            {
                icecicleState = IcecicleState.DESTROYED;
            }
        }

        #endregion  colision
    }
}
