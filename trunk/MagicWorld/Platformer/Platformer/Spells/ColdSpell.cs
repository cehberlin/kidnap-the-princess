using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MagicWorld.Spells;
using MagicWorld.Constants;

namespace MagicWorld
{
    class ColdSpell:Spell 
    {

        public ColdSpell(string spriteSet, Vector2 _origin, Level level)
            : base(spriteSet, _origin, level, SpellConstantsValues.ColdSpellConstants.BasicCastingCost, SpellConstantsValues.ColdSpellConstants.CastingCostPerSecond, SpellType.ColdSpell)
        {
            survivalTimeMs = SpellConstantsValues.ColdSpell_survivalTimeMs;
            MoveSpeed = SpellConstantsValues.ColdSpell_MoveSpeed;
            sprite.PlayAnimation(idleAnimation);
            durationOfActionMs = SpellConstantsValues.ColdSpell_durationOfActionMs;
        }

        public override void LoadContent(string spriteSet)
        {
            // Load animations.
            spriteSet = "Sprites/" + spriteSet + "/";
            //runAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Run"), 0.1f, true,3);
            runAnimation = new Animation("Content/Sprites/ColdSpell/Run", 0.1f, 3, level.Content.Load<Texture2D>(spriteSet + "Run"), 0);
            runAnimation.Opacity = 0.8f;
            //idleAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Idle"), 0.15f, true,3);
            idleAnimation = runAnimation;

            base.LoadContent(spriteSet);
        }

        protected override void HandleMovement(GameTime gameTime)
        {
            level.PhysicsManager.ApplyGravity(this, SpellConstantsValues.ColdSpellGravity, gameTime);
           
            base.HandleMovement(gameTime);
        }

        protected override void OnRemove()
        {
            level.Game.IceParticleSystem.AddParticles(Position);
            base.OnRemove();
        }


        /// <summary>
        /// speed depends on spell size/mana consumption
        /// </summary>
        protected override void OnWorkingStart()
        {
            MoveSpeed = MoveSpeed * SpellConstantsValues.ColdSpell_MoveSpeedManaFactor * UsedMana;
            base.OnWorkingStart();
        } 

        /// <summary>
        /// adds creation particles
        /// </summary>
        public override void AddOnCreationParticles()
        {
            if (level.Game.IceMagicParticleSystem.CurrentParticles() < 10)
            {
                level.Game.IceMagicParticleSystem.AddParticles(position);
            }
        }

    }
}
