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
    class ElectricSpell:Spell 
    {

        public ElectricSpell(string spriteSet, Vector2 _origin, Level level)
            : base(spriteSet, _origin, level, SpellConstantsValues.ElectricSpellConstants.BasicCastingCost, SpellConstantsValues.ElectricSpellConstants.CastingCostPerSecond, SpellType.ColdSpell)
        {
            Force = SpellConstantsValues.ElectricSpell_Force;
            survivalTimeMs = SpellConstantsValues.ElectricSpell_survivalTimeMs;
            MoveSpeed = SpellConstantsValues.ElectricSpell_MoveSpeed;
            LoadContent(spriteSet);
            sprite.PlayAnimation(idleAnimation);
            durationOfActionMs = SpellConstantsValues.ElectricSpell_durationOfActionMs;
        }

        public override void LoadContent(string spriteSet)
        {
            // Load animations.
            spriteSet = "Sprites/ElectricSpell/";//"Sprites/" + spriteSet + "/";
            //runAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Run"), 0.1f, true,3);
            runAnimation = new Animation("Content/Sprites/ElectricSpell/Run", 0.1f, 3, level.Content.Load<Texture2D>(spriteSet + "Run"), 0);
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
