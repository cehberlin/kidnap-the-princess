using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.HelperClasses;

namespace MagicWorld.Spells
{
    public class PushSpell : Spell
    {

        //public override Bounds Bounds
        //{
        //    get
        //    {
        //        float radius = 0;
        //        if (sprite.Animation != null)
        //        {
        //            // Calculate bounds within texture size.
        //            radius = (sprite.Animation.FrameWidth + sprite.Animation.FrameHeight) / 2 * currentScale;
        //        }
        //        return new Bounds(position, radius);
        //    }
        //}

        public PushSpell(string spriteSet, Vector2 _origin, Level level)
            : base(spriteSet, _origin, level, SpellConstantsValues.PushSpellConstants.BasicCastingCost, SpellConstantsValues.PushSpellConstants.CastingCostPerSecond, SpellType.PushSpell)
        {
            survivalTimeMs = SpellConstantsValues.PushSpell_survivalTimeMs;
            MoveSpeed = 0;
            sprite.PlayAnimation(idleAnimation);
            durationOfActionMs = SpellConstantsValues.PullSpell_durationOfActionMs;
            base.Position = level.Player.Position;
            MaxScale = SpellConstantsValues.PushSpell_MaxSize;
            growFactor = SpellConstantsValues.PushSpell_GrowRate;
            currentScale = ManaBasicCost * SpellConstantsValues.PushSpell_GrowRate;
        }

        public override void LoadContent(string spriteSet)
        {
            // Load animations.
            spriteSet = "Sprites/PullSpell/";//"Sprites/" + spriteSet + "/";
            runAnimation = new Animation("Content/Sprites/PullSpell/push", 0.1f,12, level.Content.Load<Texture2D>(spriteSet + "push"), 0);
            runAnimation.Opacity = 1f;
            idleAnimation = runAnimation;

            base.LoadContent(spriteSet);
        }

        public override void Update(GameTime gameTime)
        {
            if (SpellState == State.CREATING)
            {
                base.Position = level.Player.Position;

                SetScaleDependingOnManaCost();
                //only start playing if animation changes because frame position is reseted
                if (sprite.Animation != idleAnimation)
                {
                    sprite.PlayAnimation(idleAnimation);
                }
            }
        }

        /// <summary>
        /// only check collision after casting is over then remove spell
        /// </summary>
        protected override void OnWorkingStart()
        {
            level.CollisionManager.HandleCollisionWithoutRestrictions(this, collisionCallback);
            SpellState = State.REMOVE;
        } 

    }
}
