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

namespace MagicWorld
{
    class MatterSpell:Spell 
    {
        private const int manaBasicCost = 200;
        private const float manaCastingCost = 1f;

        private const int MATTER_EXISTENCE_TIME = 500; // time that created Matter exist

        Texture2D matterTexture;


        protected Boolean gravityIsSetOffBySpell = false;

        /// <summary>
        /// Created Tile lifetime depends on Force (spell creation time) also the life time of the spell itself(so it flies a shorter time)
        /// </summary>
        /// <param name="spriteSet"></param>
        /// <param name="_origin"></param>
        /// <param name="level"></param>
        public MatterSpell(string spriteSet, Vector2 _origin, Level level)
            : base(spriteSet, _origin, level, manaBasicCost, manaCastingCost, SpellType.CreateMatterSpell)
        {            
            Force = 1;
            survivalTimeMs = MATTER_EXISTENCE_TIME;
            MoveSpeed = 80.0f;
            currentScale = 0.7f;
            LoadContent(spriteSet);
            this.Collision = CollisionType.Platform;
        }

        public override Bounds Bounds
        {
            get
            {
                // Calculate bounds within texture size.
                float width = (matterTexture.Width * currentScale);
                float height = (matterTexture.Height * currentScale);
                return new Bounds(position, width, height);
            }
        }

        public override void LoadContent(string spriteSet)
        {
            matterTexture = level.Content.Load<Texture2D>("Sprites\\MatterSpell\\matter");

            base.LoadContent(spriteSet);
        }

        public override void Update(GameTime gameTime)
        {
            if (SpellState == State.WORKING)
            {
                accelaration -= (float)gameTime.ElapsedGameTime.TotalSeconds/5;
                if (accelaration < 0)
                    accelaration = 0;
                if (!gravityIsSetOffBySpell)
                {
                    if (!level.PhysicsManager.ApplyGravity(this, Constants.PhysicValues.DEFAULT_GRAVITY, gameTime))
                    {
                        Direction = Vector2.Zero;
                    }
                }
            }
            base.Update(gameTime);
        }

        protected override void OnWorkingStart()
        {
            survivalTimeMs *= Force;
            Debug.WriteLine("Matter starts working TIme:" +survivalTimeMs);
            base.OnWorkingStart();
        }

        protected override void OnRemove()
        {
            base.OnRemove();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(matterTexture, Bounds.getRectangle(), Color.White);
            base.Draw(gameTime, spriteBatch);
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
            }else if (spell.GetType() == typeof(ColdSpell))
            {
                survivalTimeMs *= 1.3;
            }else if (spell.GetType() == typeof(NoGravitySpell))
            {
                gravityIsSetOffBySpell = true;
                return false; //do not remove spell
            }
            return base.SpellInfluenceAction(spell);
        }


        public override void AddOnCreationParticles()
        {
            if (level.MatterCreationParticleSystem.CurrentParticles() < 10)
            {
                level.MatterCreationParticleSystem.AddParticles(getMidPointOfSpell());
            }
        }

    }
}
