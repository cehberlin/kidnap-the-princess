using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using MagicWorld.DynamicLevelContent;
using MagicWorld.HelperClasses;
using System.Collections.Generic;
using MagicWorld.Services;
using ParticleEffects;

namespace MagicWorld
{
    enum IcecicleState { NORMAL, FALLING, DESTROYED };
    class Icecicle : BlockElement
    {
        IEnemyService enemyService;

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

        public Icecicle(String texture, Level level, Vector2 position,Color drawColor)
            : base(texture,CollisionType.Impassable,level,position,drawColor)
            
        {
            icecicleState = IcecicleState.NORMAL;
            collisionCallback = HandleCollisionForOneObject;
            fallVelocity = 500f;
            enemyService = (IEnemyService)level.Game.Services.GetService(typeof(IEnemyService));
        }

        public override void LoadContent(string spriteSet)
        {           
            base.LoadContent(spriteSet);
        }

     
        public override Boolean SpellInfluenceAction(Spell spell)
        {
            if (spell.GetType() == typeof(WarmSpell))
            {
                icecicleState = IcecicleState.FALLING;                
                return true;
            }
            else return false;
        }   
   
       
        public override void Update(GameTime gameTime)
        {
            if (icecicleState == IcecicleState.FALLING)
            {
                position.Y += (float)(gameTime.ElapsedGameTime.TotalSeconds* fallVelocity);          
            }            
            HandleCollision();
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
                level.CollisionManager.HandleGeneralCollisions(this, collisionCallback);

                //check if spells leaves the level
                HandleOutOfLevelCollision();
            }
        }

        protected void HandleCollisionForOneObject(BasicGameElement element,bool xAxisCollision, bool yAxisCollision)
        {
            if (element.GetType().IsSubclassOf(typeof(Enemy)))
            {
                Enemy e = (Enemy)element; 
                if (! e.isFroozen)//enemyService.IsFroozen)
                {
                    //destroy enemy
                    audioService.playSound(Audio.SoundType.icehit);
                    IcecicleState = IcecicleState.DESTROYED;
                    e.IsRemovable = true;
                }
            }
            else
            {
                IcecicleState = IcecicleState.DESTROYED;
            }
            level.Game.IceParticleSystem.AddParticles(new ParticleSetting(position));
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
