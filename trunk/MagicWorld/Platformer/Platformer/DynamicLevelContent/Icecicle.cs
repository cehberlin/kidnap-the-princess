using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Platformer.DynamicLevelContent;
using Platformer.HelperClasses;
using System.Collections.Generic;

namespace Platformer
{
    enum IcecicleState { NORMAL, FALLING, DESTROYED };
    class Icecicle : BasicGameElement
    {

        private Texture2D texture;
        private Vector2 origin;
        private SoundEffect hitSound;
        protected AnimationPlayer sprite;
        protected Animation idleAnimation;

        public IcecicleState icecicleState ;
        private float fallVelocity;

        public const int Width = 40;
        public const int Height = 41;

        public override Bounds Bounds
        {
            get
            {
                int left = (int)Math.Round(position.X - Width / 2);
                int top = (int)Math.Round(position.Y - Height);

                return new Bounds(left, top, Width, Height);
            }
        }


        public Icecicle(Level level, Vector2 position)
            : base(level)
            
        {
            this.position = position;
            icecicleState = IcecicleState.NORMAL;
            LoadContent("Sprites/Icecicle");
            sprite.PlayAnimation(idleAnimation);
        }

        public override void LoadContent(string spriteSet)
        {
            texture = level.Content.Load<Texture2D>(spriteSet);
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            hitSound = level.Content.Load<SoundEffect>("Sounds/Icehit");
            idleAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet), 0.15f, true, 1);            
            fallVelocity = 0.8f;
            debugTexture = level.Content.Load<Texture2D>("Sprites\\white");
        }

     
        public override Boolean SpellInfluenceAction(Spell spell)
        {
            if (spell.GetType() == typeof(WarmSpell))
            {
                hitSound.Play();
                icecicleState = IcecicleState.FALLING;                
                return true;
            }
            else return false;
        }   
   
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            SpriteEffects flip = SpriteEffects.None;
            // Draw it in screen space.                
            if (icecicleState != IcecicleState.DESTROYED)
            {
                sprite.Draw(gameTime, spriteBatch, position, flip);
            }

            base.Draw(gameTime, spriteBatch);            
        }

        public override void Update(GameTime gameTime)
        {
            if (icecicleState == IcecicleState.FALLING)
            {
                position.Y += (float)(position.Y * gameTime.ElapsedGameTime.TotalSeconds* fallVelocity);          
            }            
            HandleCollision();
        }
 
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

            List<Enemy> collisionEnemies = new List<Enemy>();

            level.CollisionManager.CollidateWithEnemy(this, ref collisionEnemies);
            //enemy collision
            foreach (Enemy enemy in collisionEnemies)
            {
                //destroy enemy
                hitSound.Play();
                icecicleState = IcecicleState.DESTROYED;
                level.Enemies.Remove(enemy);
            }
        }

        /// <summary>
        /// handels collision with tiles
        /// </summary>
        public virtual void HandleTileCollision()
        {
            //Tile collision
            List<Tile> collisionTiles = new List<Tile>();
            if (level.CollisionManager.CollidateWithTiles(this, ref collisionTiles))
            {
                //destroy the icecicle
                icecicleState = IcecicleState.DESTROYED;
            }                        
        }

        /// <summary>
        /// handels collision with level bounds
        /// </summary>
        public virtual void HandleOutOfLevelCollision()
        {
            if (level.CollisionManager.CollidateWithLevelBounds(this))
            {
                icecicleState = IcecicleState.DESTROYED;
            }
        }

        #endregion  colision
    }
}
