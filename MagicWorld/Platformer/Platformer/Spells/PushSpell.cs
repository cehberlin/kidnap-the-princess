using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicWorld.Spells
{
    public class PushSpell : Spell
    {


        public PushSpell(string spriteSet, Vector2 _origin, Level level)
            : base(spriteSet, _origin, level, SpellConstantsValues.PushSpellConstants.BasicCastingCost, SpellConstantsValues.PushSpellConstants.CastingCostPerSecond, SpellType.PushSpell)
        {
            Force = SpellConstantsValues.PushSpell_Force;
            survivalTimeMs = SpellConstantsValues.PushSpell_survivalTimeMs;
            MoveSpeed = 0;
            sprite.PlayAnimation(idleAnimation);
            durationOfActionMs = SpellConstantsValues.PullSpell_durationOfActionMs;
            base.Position = level.Player.Position;
            MaxScale = SpellConstantsValues.PushSpell_MaxSize;
            growFactor = SpellConstantsValues.PushSpell_GrowRate;
        }

        public override void LoadContent(string spriteSet)
        {
            // Load animations.
            spriteSet = "Sprites/NoGravitySpell/";//"Sprites/" + spriteSet + "/";
            //runAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Run"), 0.1f, true,3);
            runAnimation = new Animation("Content/Sprites/NoGravitySpell/Run", 0.1f, 3, level.Content.Load<Texture2D>(spriteSet + "Run"), 0);
            //idleAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Idle"), 0.15f, true,3);
            idleAnimation = runAnimation;

            base.LoadContent(spriteSet);
        }

        public override void Update(GameTime gameTime)
        {
            base.Position = level.Player.Position;
            base.Update(gameTime);
        }               

    }
}
