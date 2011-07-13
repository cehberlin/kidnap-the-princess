using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MagicWorld.Spells;
using MagicWorld.Constants;
using MagicWorld.HelperClasses;
using ParticleEffects;
using MagicWorld.Audio;

namespace MagicWorld
{
    class ElectricSpell:Spell 
    {

        public override Bounds Bounds
        {
            get
            {
                return new Bounds(Position, getRadius());
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

        float getRadius()
        {
           return currentScale * 12;
        }

        public override void Update(GameTime gameTime)
        {
            currentParticles++;
            if (SpellState == State.CREATING)
            {
                if (currentParticles%8==0) //only every 4 update cycle
                {
                    level.Game.LightningCreationParticleSystem.AddParticles(new ParticleSetting(position, getRadius()));
                }
            }
            else
            {
                if (currentParticles % 4 == 0) //only every 2 update cycle
                {
                    level.Game.LightningCreationParticleSystem.AddParticles(new ParticleSetting(position, getRadius()));
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
