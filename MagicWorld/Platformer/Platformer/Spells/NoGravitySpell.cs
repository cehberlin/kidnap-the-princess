using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.Constants;
using MagicWorld.HelperClasses;

namespace MagicWorld.Spells
{
    class NoGravitySpell : Spell 
    {
        public override Bounds Bounds
        {
            get
            {
                float radius = 0;
                if (sprite.Animation != null)
                {
                    // Calculate bounds within texture size.
                    radius = (sprite.Animation.FrameWidth) / 2 * 1f * currentScale;
                }
                return new Bounds(Position, radius);
            }
        }

        protected double startSurvivalTime;


        public NoGravitySpell(string spriteSet, Vector2 _origin, Level level)
            : base(spriteSet, _origin, level, SpellConstantsValues.NoGravitationSpellConstants.BasicCastingCost, SpellConstantsValues.NoGravitationSpellConstants.CastingCostPerSecond, SpellType.NoGravitySpell)
        {
            survivalTimeMs = SpellConstantsValues.NoGravitationSpell_survivalTimeMs;;
            //MoveSpeed = SpellConstantsValues.NoGravitationSpell_MoveSpeed;
            MaxScale = 3;
            sprite.PlayAnimation(idleAnimation);
            durationOfActionMs = SpellConstantsValues.NoGravitationSpell_durationOfActionMs;
            accelarationChangeFactorX = SpellConstantsValues.NoGravitationSpell_accelarationChangeFactor;
            accelarationChangeFactorY = SpellConstantsValues.NoGravitationSpell_accelarationChangeFactor;
            startSurvivalTime = survivalTimeMs;
            growFactor = SpellConstantsValues.NoGravitySpellGrowRate;
        }
        
        public override void LoadContent(string spriteSet)
        {
            // Load animations.
            spriteSet = "Sprites/" + spriteSet + "/";
            runAnimation = new Animation("Content/Sprites/NoGravitySpell/AntiGravSpriteSheet", 0.1f, 12, level.Content.Load<Texture2D>(spriteSet + "AntiGravSpriteSheet"), 0);
            idleAnimation = runAnimation;

            base.LoadContent(spriteSet);
        }

        protected override void OnWorkingStart()
        {
            MoveSpeed = UsedMana/4;
            base.OnWorkingStart();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //+0.30 has the effect of longer full visibility and short time of very less visibility
            float opacity = (float)((survivalTimeMs / startSurvivalTime) + 0.30);
            sprite.Animation.Opacity = opacity;
        }


        protected override void HandleObjectCollision()
        {
            //do not resolve collision
            level.CollisionManager.HandleCollisionWithoutRestrictions(this, collisionCallback);
        }
    }
}
