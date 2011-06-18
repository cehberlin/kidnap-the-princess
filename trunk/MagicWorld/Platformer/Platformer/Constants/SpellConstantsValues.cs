using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MagicWorld.Constants
{
    public class SpellConstantsValues
    {
        private SpellConstantsValues() { }

        /// <summary>
        /// value how fast the spell is rotated while in casting mode
        /// </summary>
        public static double spellAimingRotationSpeed = 150.0f;

        /// <summary>
        /// speed of the powering up and down of the spell in casting mode
        /// </summary>
        public static double spellPowerUpDownSpeed = 500.0f;

        /// <summary>
        /// how fast the spell grows if more mana is pumped into it
        /// </summary>
        public static float DefaultSpellGrowRate = 0.006f;//0.02f;

        /// <summary>
        /// Max mana a player can have
        /// </summary>
        public static int MAX_MANA = 1000;

        /// <summary>
        /// regeneration rate of players mana
        /// </summary>
        public static float MANA_REGENERATION_RATE = 0.25f;

        /// <summary>
        /// distance of casting from the player mid point
        /// </summary>
        public static double spellDistanceToPlayerMidPoint = 60;

        /// <summary>
        /// step for increaseing decreasing the aiming angle
        /// </summary>
        public static double spellAngleChangeStep = Math.PI / 64;

        

        public static Spells.SpellConstants ColdSpellConstants = new Spells.SpellConstants(150, 1, 200, new TimeSpan(0, 0, 1));
        public static float ColdSpell_MoveSpeed = 150f;
        public static int ColdSpell_survivalTimeMs = 5000;
        public static int ColdSpell_durationOfActionMs = 5000;
        public static int ColdSpell_Force = 1;
        public static Vector2 ColdSpellGravity = new Vector2(0f, 0.2f);

        public static Spells.SpellConstants WarmSpellConstants = new Spells.SpellConstants(150, 1, 200, new TimeSpan(0, 0, 1));
        public static float WarmSpell_MoveSpeed = 160f;
        public static int WarmSpell_survivalTimeMs = 3000;
        public static int WarmSpell_durationOfActionMs = 5000;
        public static int WarmSpell_Force = 1;
        public static Vector2 WarmSpellGravity = new Vector2(0f, 0.2f);

        public static Spells.SpellConstants CreateMatterSpellConstants = new Spells.SpellConstants(150, 1, 200, new TimeSpan(0, 0, 1));
        public static float CreateMatterSpell_MoveSpeed = 80f;
        public static int CreateMatterSpell_Force = 1;
        public static float CreateMatterSpell_currentScale = 0.7f;
        public static float CreateMatterSpell_accelarationChangeFactor = -0.1f;

        public static Spells.SpellConstants NoGravitationSpellConstants = new Spells.SpellConstants(150, 1, 200, new TimeSpan(0, 0, 1));
        public static float NoGravitationSpell_MoveSpeed = 10f;
        public static int NoGravitationSpell_survivalTimeMs = 5000;
        public static int NoGravitationSpell_durationOfActionMs = 5000;
        public static int NoGravitationSpell_Force = 1;

        public static Spells.SpellConstants ElectricSpellConstants = new Spells.SpellConstants(150, 1, 200, new TimeSpan(0, 0, 1));
        public static float ElectricSpell_MoveSpeed = 500f;
        public static int ElectricSpell_survivalTimeMs = 5000;
        public static int ElectricSpell_durationOfActionMs = 5000;
        public static int ElectricSpell_Force = 1;


        public static Spells.SpellConstants PushSpellConstants = new Spells.SpellConstants(150, 1, 200, new TimeSpan(0, 0, 1));
        public static int PushSpell_survivalTimeMs = 1000;
        public static int PushSpell_durationOfActionMs = 5000;
        public static int PushSpell_Force = 1;
        public static float PushSpell_MaxSize = 20f;
        public static float PushSpell_GrowRate = 0.52f;

        public static Spells.SpellConstants PullSpellConstants = PushSpellConstants;//new Spells.SpellConstants(150, 1, 200, new TimeSpan(0, 0, 1));
        public static int PullSpell_survivalTimeMs = PushSpell_survivalTimeMs;
        public static int PullSpell_durationOfActionMs = PushSpell_durationOfActionMs;
        public static int PullSpell_Force = PushSpell_Force;
        public static float PullSpell_MaxSize = PushSpell_MaxSize;
        public static float PullSpell_GrowRate =0.52f;

    }
}
