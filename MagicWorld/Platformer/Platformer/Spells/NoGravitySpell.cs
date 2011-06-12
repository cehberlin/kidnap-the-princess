using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.Constants;

namespace MagicWorld.Spells
{
    class NoGravitySpell : Spell 
    {

        public NoGravitySpell(string spriteSet, Vector2 _origin, Level level)
            : base(spriteSet, _origin, level, SpellConstantsValues.NoGravitationSpellConstants.BasicCastingCost, SpellConstantsValues.NoGravitationSpellConstants.CastingCostPerSecond, SpellType.NoGravitySpell)
        {
            Force = SpellConstantsValues.NoGravitationSpell_Force;
            survivalTimeMs = SpellConstantsValues.NoGravitationSpell_survivalTimeMs;;
            MoveSpeed = SpellConstantsValues.NoGravitationSpell_MoveSpeed;
            LoadContent(spriteSet);
            sprite.PlayAnimation(idleAnimation);
            durationOfActionMs = SpellConstantsValues.NoGravitationSpell_durationOfActionMs;
        }
        
        public override void LoadContent(string spriteSet)
        {
            // Load animations.
            spriteSet = "Sprites/" + spriteSet + "/";
            //runAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Run"), 0.1f, true,3);
            runAnimation = new Animation("Content/Sprites/NoGravitySpell/Run", 0.1f, 3, level.Content.Load<Texture2D>(spriteSet + "Run"), 0);
            //idleAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Idle"), 0.15f, true,3);
            idleAnimation = runAnimation;

            base.LoadContent(spriteSet);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}
