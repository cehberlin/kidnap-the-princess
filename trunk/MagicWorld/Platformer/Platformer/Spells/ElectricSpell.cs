using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MagicWorld.Spells;
using MagicWorld.Constants;
using MagicWorld.HelperClasses;

namespace MagicWorld
{
    class ElectricSpell:Spell 
    {

        public override Bounds Bounds
        {
            get
            {
                return new Bounds(position, currentScale*20);
            }
        }

        public ElectricSpell(string spriteSet, Vector2 _origin, Level level)
            : base(spriteSet, _origin, level, SpellConstantsValues.ElectricSpellConstants.BasicCastingCost, SpellConstantsValues.ElectricSpellConstants.CastingCostPerSecond, SpellType.ElectricSpell)
        {
            survivalTimeMs = SpellConstantsValues.ElectricSpell_survivalTimeMs;
            MoveSpeed = SpellConstantsValues.ElectricSpell_MoveSpeed;
        
            durationOfActionMs = SpellConstantsValues.ElectricSpell_durationOfActionMs;
        }

        int currentParticles = 0;

        public override void Update(GameTime gameTime)
        {
            currentParticles++;
            if (SpellState == State.CREATING)
            {
                if (currentParticles%4==0) //only every 4 update cycle
                {
                    level.Game.LightningCreationParticleSystem.AddParticles(position);
                }
            }
            else
            {

                if (currentParticles % 2 == 0) //only every 2 update cycle
                {
                    level.Game.LightningCreationParticleSystem.AddParticles(position);
                }
            }
            base.Update(gameTime);
        }

        public override void AddOnCreationParticles()
        {
            //do not
        }
    }
}
