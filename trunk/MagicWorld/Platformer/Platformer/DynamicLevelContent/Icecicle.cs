using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using MagicWorld.DynamicLevelContent;
using MagicWorld.HelperClasses;
using System.Collections.Generic;
using MagicWorld.Services;

namespace MagicWorld
{
    enum IcecicleState { NORMAL, FALLING, DESTROYED };
    class Icecicle : BasicGameElement
    {
        IEnemyService enemyService;

        private Texture2D texture;
        private Vector2 origin;
        private SoundEffect hitSound;
        protected AnimationPlayer sprite;
        protected Animation idleAnimation;

        private IcecicleState icecicleState;

        public IcecicleState IcecicleState
        {
            get { return icecicleState; }
            set { icecicleState = value;
            if (icecicleState == MagicWorld.IcecicleState.DESTROYED)
            {
                this.isRemovable = true;
            }
            }
        }
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

            collisionCallback = HandleCollisionForOneObject;
        }

        public override void LoadContent(string spriteSet)
        {
            texture = level.Content.Load<Texture2D>(spriteSet);
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            hitSound = level.Content.Load<SoundEffect>("Sounds/Icehit");
            idleAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet), 0.15f, true, 1);            
            fallVelocity = 0.8f;
            debugTexture = level.Content.Load<Texture2D>("Sprites\\white");
            enemyService = (IEnemyService)level.Game.Services.GetService(typeof(IEnemyService));
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
            enemyService = (IEnemyService)level.Game.Services.GetService(typeof(IEnemyService));
        }
 
        #region collision

        /// <summary>
        /// callback delegate for collision with specific objects
        /// </summary>
        protected CollisionManager.OnCollisionWithCallback collisionCallback;


        /// <summary>
        /// handels collision with tiles and enemies and level bounds
        /// </summary>
        public virtual void HandleCollision()
        {
            if (icecicleState == IcecicleState.FALLING)
            {
                level.CollisionManager.HandleGeneralCollisions(this, velocity, collisionCallback);

                //check if spells leaves the level
                HandleOutOfLevelCollision();
            }
        }

        protected void HandleCollisionForOneObject(BasicGameElement element)
        {
            if (element.GetType() == typeof(Enemy))
            {
                Enemy e = (Enemy)element;
                if (!enemyService.IsFroozen)
                {
                    //destroy enemy
                    hitSound.Play();
                    icecicleState = IcecicleState.DESTROYED;
                    e.IsRemovable = true;
                }
            }
            else
            {
                this.isRemovable = true;
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
