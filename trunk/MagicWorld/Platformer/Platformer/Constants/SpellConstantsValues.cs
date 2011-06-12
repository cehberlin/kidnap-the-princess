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
        public static float ColdSpell_MoveSpeed = 100f;
        public static int ColdSpell_survivalTimeMs = 5000;
        public static int ColdSpell_durationOfActionMs = 5000;
        public static int ColdSpell_Force = 1;

        public static Spells.SpellConstants WarmSpellConstants = new Spells.SpellConstants(150, 1, 200, new TimeSpan(0, 0, 1));
        public static float WarmSpell_MoveSpeed = 100f;
        public static int WarmSpell_survivalTimeMs = 3000;
        public static int WarmSpell_durationOfActionMs = 5000;
        public static int WarmSpell_Force = 1;

        public static Spells.SpellConstants CreateMatterSpellConstants = new Spells.SpellConstants(150, 1, 200, new TimeSpan(0, 0, 1));
        public static float CreateMatterSpell_MoveSpeed = 80f;
        public static int CreateMatterSpell_Force = 1;
        public static float CreateMatterSpell_currentScale = 0.7f;
        public static float CreateMatterSpell_accelarationChangeFactor = -0.2f;

        public static Spells.SpellConstants NoGravitationSpellConstants = new Spells.SpellConstants(150, 1, 200, new TimeSpan(0, 0, 1));
        public static float NoGravitationSpell_MoveSpeed = 40f;
        public static int NoGravitationSpell_survivalTimeMs = 5000;
        public static int NoGravitationSpell_durationOfActionMs = 5000;
        public static int NoGravitationSpell_Force = 1;

        public static Spells.SpellConstants ElectricSpellConstants = new Spells.SpellConstants(150, 1, 200, new TimeSpan(0, 0, 1));
        public static float ElectricSpell_MoveSpeed = 500f;
        public static int ElectricSpell_survivalTimeMs = 5000;
        public static int ElectricSpell_durationOfActionMs = 5000;
        public static int ElectricSpell_Force = 1;

    }
}
