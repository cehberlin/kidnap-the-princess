using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld.Constants
{
    public class SpellConstantsValues
    {
        private SpellConstantsValues() { }

        /// <summary>
        /// Max mana a player can have
        /// </summary>
        public static int MAX_MANA = 1000;

        /// <summary>
        /// regeneration rate of players mana
        /// </summary>
        public static float MANA_REGENERATION_RATE = 0.25f;

        public static Spells.SpellConstants ColdSpellConstants = new Spells.SpellConstants(150, 1, 200, new TimeSpan(0, 0, 1));

        public static Spells.SpellConstants WarmSpellConstants = new Spells.SpellConstants(150, 1, 200, new TimeSpan(0, 0, 1));

        public static Spells.SpellConstants CreateMatterSpellConstants = new Spells.SpellConstants(150, 1, 200, new TimeSpan(0, 0, 1));

        public static Spells.SpellConstants NoGravitationSpellConstants = new Spells.SpellConstants(150, 1, 200, new TimeSpan(0, 0, 1));

    }
}
