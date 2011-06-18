using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MagicWorld.DynamicLevelContent;
using System.Diagnostics;
using MagicWorld.Spells;
using MagicWorld.HelperClasses;
using MagicWorld.Constants;

namespace MagicWorld
{
    class MatterSpell:Spell 
    {
        private const int MATTER_EXISTENCE_TIME = 500; // time that created Matter exist

        Texture2D matterTexture;

        protected Boolean gravityIsSetOffBySpell = false;
        
        protected PushPullHandler pushPullHandler = new PushPullHandler();


        /// <summary>
        /// Created Tile lifetime depends on Force (spell creation time) also the life time of the spell itself(so it flies a shorter time)
        /// </summary>
        /// <param name="spriteSet"></param>
        /// <param name="_origin"></param>
        /// <param name="level"></param>
        public MatterSpell(string spriteSet, Vector2 _origin, Level level)
            : base(spriteSet, _origin, level, SpellConstantsValues.CreateMatterSpellConstants.BasicCastingCost, SpellConstantsValues.CreateMatterSpellConstants.CastingCostPerSecond, SpellType.CreateMatterSpell)
        {            
            Force = SpellConstantsValues.CreateMatterSpell_Force;
            survivalTimeMs = MATTER_EXISTENCE_TIME;
            MoveSpeed = SpellConstantsValues.CreateMatterSpell_MoveSpeed;
            currentScale = SpellConstantsValues.CreateMatterSpell_currentScale;
            accelarationChangeFactorX = SpellConstantsValues.CreateMatterSpell_accelarationChangeFactor;
            accelarationChangeFactorY = 0;
            this.Collision = CollisionType.Platform;
        }

        public override Bounds Bounds
        {
            get
            {
                // Calculate bounds within texture size.
                float radius = ((matterTexture.Width + matterTexture.Height)/4 * currentScale); 
                return new Bounds(position, radius);
            }
        }

        public override void LoadContent(string spriteSet)
        {
            matterTexture = level.Content.Load<Texture2D>("Sprites\\MatterSpell\\matter");

            base.LoadContent(spriteSet);
            oldBounds = this.Bounds;
        }

        Bounds oldBounds;
        bool isOnGround = false;
        public override void Update(GameTime gameTime)
        {            
            if (SpellState == State.WORKING)
            {
                pushPullHandler.Update(gameTime);
                if (!gravityIsSetOffBySpell)
                {
                    level.PhysicsManager.ApplyGravity(this, PhysicValues.DEFAULT_GRAVITY, gameTime);
                }
                else
                {
                    gravityIsSetOffBySpell = false;
                }
            }

            base.Update(gameTime);

            if (SpellState == State.WORKING)
            {
                level.CollisionManager.HandleGeneralCollisions(this, ref oldBounds, ref isOnGround);
                if (isOnGround)
                {
                    ResetVelocity();
                }
            }
        }

        protected override void OnWorkingStart()
        {
            survivalTimeMs *= Force;
            Debug.WriteLine("Matter starts working TIme:" +survivalTimeMs);
            base.OnWorkingStart();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(matterTexture, Bounds.getRectangle(), Color.White);
            base.Draw(gameTime, spriteBatch);
        }

        protected override void ResetVelocity()
        {
            velocity = Vector2.Zero;
        }

        /// <summary>
        /// cold spell increases lifetime warm spell shortens on 10%
        /// </summary>
        /// <param name="spell"></param>
        /// <returns></returns>
        public override bool SpellInfluenceAction(Spell spell)
        {
            if (spell.GetType() == typeof(WarmSpell))
            {
                survivalTimeMs *= 0.7;
            }
            else if (spell.GetType() == typeof(ColdSpell))
            {
                survivalTimeMs *= 1.3;
            }
            else if (spell.GetType() == typeof(NoGravitySpell))
            {
                gravityIsSetOffBySpell = true;
                return false; //do not remove spell
            }
            else if (spell.SpellType == SpellType.PushSpell)
            {
                //TODO put values into constant class
                Vector2 push = this.Position - spell.Position;
                push.Normalize();
                pushPullHandler.setXAcceleration(1.1f, 0, 2, -0.2f);
                pushPullHandler.start(this,1000, push);
                return false;
            }
            else if (spell.SpellType == SpellType.PullSpell)
            {
                //TODO put values into constant class
                Vector2 pull = spell.Position - this.Position;
                pull.Normalize();
                pushPullHandler.setXAcceleration(1f,0, 1.1f, 0.07f);                
                pushPullHandler.start(this, 1000,pull);
                return false;
            }
            return base.SpellInfluenceAction(spell);
        }


        public override void AddOnCreationParticles()
        {
            if (level.Game.MatterCreationParticleSystem.CurrentParticles() < 10)
            {
                level.Game.MatterCreationParticleSystem.AddParticles(position);
            }
        }

        public override void HandleCollision()
        {   
            //check if spells leaves the level
            HandleOutOfLevelCollision();
         }

    }
}
