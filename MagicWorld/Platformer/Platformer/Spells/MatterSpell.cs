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
using ParticleEffects;

namespace MagicWorld
{
    class MatterSpell:Spell 
    {       

        Texture2D matterTexture;

        protected Boolean gravityIsSetOffBySpell = false;
        
        protected PushPullHandler pushPullHandler = new PushPullHandler();

        protected double startSurvivalTime;

        /// <summary>
        /// Created Tile lifetime depends on Force (spell creation time) also the life time of the spell itself(so it flies a shorter time)
        /// </summary>
        /// <param name="spriteSet"></param>
        /// <param name="_origin"></param>
        /// <param name="level"></param>
        public MatterSpell(string spriteSet, Vector2 _origin, Level level)
            : base(spriteSet, _origin, level, SpellConstantsValues.CreateMatterSpellConstants.BasicCastingCost, SpellConstantsValues.CreateMatterSpellConstants.CastingCostPerSecond, SpellType.CreateMatterSpell)
        {
            survivalTimeMs = SpellConstantsValues.MATTER_EXISTENCE_TIME;
            startSurvivalTime = survivalTimeMs;
            MoveSpeed = SpellConstantsValues.CreateMatterSpell_MoveSpeed;                        
            accelarationChangeFactorX = SpellConstantsValues.CreateMatterSpell_accelarationChangeFactor;
            accelarationChangeFactorY = 0;
            this.Collision = CollisionType.Platform;
            this.growFactor = SpellConstantsValues.MatterSpellGrowRate; // max size
        }

        public override Bounds Bounds
        {
            get
            {
                // Calculate bounds within texture size.
                float width = (matterTexture.Width * 0.6f) * currentScale;
                float height = (matterTexture.Height * 0.9f) * currentScale;
                float left = (float)Math.Round(position.X - width / 2);
                float top = (float)Math.Round(position.Y - height / 2); 
                return new Bounds(left, top, width, height);
            }
        }

        /// <summary>
        /// calculates drawing rectangle
        /// </summary>
        /// <returns></returns>
        Rectangle getDrawingRectagle()
        {
                float width = (matterTexture.Width) * currentScale;
                float height = (matterTexture.Height ) * currentScale;
                float left = (float)Math.Round(position.X - width / 2);
                float top = (float)Math.Round(position.Y - height / 2);
                return new Rectangle((int)left, (int)top, (int)width, (int)height);
        }

        public override void LoadContent(string spriteSet)
        {
            matterTexture = level.Content.Load<Texture2D>("Sprites\\MatterSpell\\matter");

            base.LoadContent(spriteSet);
        }

        double nogravityInfluenceTime = 0;
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
            }

            if (gravityIsSetOffBySpell)
            {
                if (nogravityInfluenceTime <= 0)
                {
                    gravityIsSetOffBySpell = false;
                }
                else
                {
                    nogravityInfluenceTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }

            base.Update(gameTime);

            if (SpellState == State.WORKING)
            {
                level.CollisionManager.HandleGeneralCollisions(this, ref isOnGround,null,false,true,false);
                if (isOnGround)
                {
                    ResetVelocity();
                }
            }
        }

        protected override void OnWorkingStart()
        {
            survivalTimeMs *= UsedMana;
            startSurvivalTime = survivalTimeMs;
            Debug.WriteLine("Matter starts working TIme:" +survivalTimeMs);
            base.OnWorkingStart();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //+0.30 has the effect of longer full visibility and short time of very less visibility
            float opacity = (float)((survivalTimeMs / startSurvivalTime) + 0.30);
            spriteBatch.Draw(matterTexture, getDrawingRectagle(), Color.White * opacity);
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
                nogravityInfluenceTime = spell.DurationOfActionMs;
                ResetVelocity();
                return false; //do not remove spell
            }
            else if (spell.SpellType == SpellType.PullSpell)
            {
                Vector2 push = spell.Position - this.Position;
                push.Normalize();
                pushPullHandler.setXAcceleration(SpellConstantsValues.PUSHPULL_DEFAULT_START_ACCELERATION, 0, 2f, SpellConstantsValues.PUSHPULL_DEFAULT_ACCELERATION_CHANGE_FACTOR);
                pushPullHandler.start(this, 1000, push);
                return false;
            }
            else if (spell.SpellType == SpellType.PushSpell)
            {
                Vector2 pull = this.Position - spell.Position;
                pull.Normalize();
                pushPullHandler.setXAcceleration(SpellConstantsValues.PUSHPULL_DEFAULT_START_ACCELERATION, 0, 2f, SpellConstantsValues.PUSHPULL_DEFAULT_ACCELERATION_CHANGE_FACTOR);
                pushPullHandler.start(this, 1000, pull);
                return false;
            }
            return base.SpellInfluenceAction(spell);
        }


        public override void AddOnCreationParticles()
        {
            if (level.Game.MatterCreationParticleSystem.CurrentParticles() < 10)
            {
                Bounds bounds = Bounds;
                level.Game.MatterCreationParticleSystem.AddParticles(new ParticleSetting(position, (bounds.Width+bounds.Height)/4+40));
            }
        }

        public override void HandleCollision()
        {   
            //check if spells leaves the level
            HandleOutOfLevelCollision();
        }
    }
}
