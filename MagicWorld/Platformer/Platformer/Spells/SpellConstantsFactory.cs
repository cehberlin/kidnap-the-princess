using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.Constants;

namespace MagicWorld.Spells
{
    /// <summary>
    /// returns the SPellConstants for a spell
    /// </summary>
    public class SpellConstantsFactory
    {
        private SpellConstantsFactory() { }

        public static SpellConfiguration getSpellConstants(SpellType type)
        {
            switch (type)
            {
                case SpellType.ColdSpell: return SpellConstantsValues.ColdSpellConstants; 
                case SpellType.CreateMatterSpell: return SpellConstantsValues.CreateMatterSpellConstants; 
                case SpellType.NoGravitySpell: return SpellConstantsValues.NoGravitationSpellConstants; 
                case SpellType.WarmingSpell: return SpellConstantsValues.WarmSpellConstants;
                case SpellType.ElectricSpell: return SpellConstantsValues.ElectricSpellConstants;
                case SpellType.PullSpell: return SpellConstantsValues.PullSpellConstants;
                case SpellType.PushSpell: return SpellConstantsValues.PushSpellConstants; 

                default: throw new NotImplementedException();

            }
        }
    }
}
