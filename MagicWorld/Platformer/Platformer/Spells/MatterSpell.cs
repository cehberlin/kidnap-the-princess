using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MagicWorld.DynamicLevelContent;
using System.Diagnostics;
using MagicWorld.Spells;

namespace MagicWorld
{
    class MatterSpell:Spell 
    {
        private const int manaBasicCost = 200;
        private const float manaCastingCost = 1f;

        private const int MATTER_EXISTENCE_TIME = 500; // time that created Matter exist


        protected Boolean gravityIsSetOffBySpell = false;

        /// <summary>
        /// Created Tile lifetime depends on Force (spell creation time) also the life time of the spell itself(so it flies a shorter time)
        /// </summary>
        /// <param name="spriteSet"></param>
        /// <param name="_origin"></param>
        /// <param name="level"></param>
        public MatterSpell(string spriteSet, Vector2 _origin, Level level)
            : base(spriteSet, _origin, level, manaBasicCost, manaCastingCost)
        {            
            Force = 1;
            survivalTimeMs = MATTER_EXISTENCE_TIME;
            MoveSpeed = 40.0f;
            LoadContent(spriteSet);
            sprite.PlayAnimation(idleAnimation);
            this.Collision = CollisionType.Platform;
        }

        public override void LoadContent(string spriteSet)
        {
            // Load animations.
            spriteSet = "Sprites/" + spriteSet + "/";
            runAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Run"), 0.1f, true,3);
            idleAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Idle"), 0.15f, true,3);

            base.LoadContent(spriteSet);
        }

        public override void Update(GameTime gameTime)
        {
            if (!gravityIsSetOffBySpell)
            {
                if (!level.PhysicsManager.ApplyGravity(this, Constants.PhysicValues.DEFAULT_GRAVITY))
                {
                    Direction = new Vector2();
                }
            }
            base.Update(gameTime);
        }

        protected override void OnWorkingStart()
        {
            survivalTimeMs *= Force;
            Debug.WriteLine("Matter starts working TIme:" +survivalTimeMs);
            level.GeneralColliadableGameElements.Add(this);
            base.OnWorkingStart();
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



    }
}
