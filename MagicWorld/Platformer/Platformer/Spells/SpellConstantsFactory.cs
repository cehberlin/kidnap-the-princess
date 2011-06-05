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

        public static SpellConstants getSpellConstants(SpellType type)
        {
            switch (type)
            {
                case SpellType.ColdSpell: return SpellConstantsValues.ColdSpellConstants; 
                case SpellType.CreateMatterSpell: return SpellConstantsValues.CreateMatterSpellConstants; 
                case SpellType.NoGravitySpell: return SpellConstantsValues.NoGravitationSpellConstants; 
                case SpellType.WarmingSpell: return SpellConstantsValues.WarmSpellConstants; 

                default: throw new NotImplementedException();

            }
        }
    }
}
