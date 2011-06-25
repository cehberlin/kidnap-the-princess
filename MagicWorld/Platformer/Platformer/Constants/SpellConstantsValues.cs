using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MagicWorld.BlendInClasses;

namespace MagicWorld.Constants
{
    class SpellConstantsValues
    {
        private SpellConstantsValues() { }

        /// <summary>
        /// Please add constants from the bottom if you want to change them online
        /// </summary>
        /// <returns></returns>
        public static List<ConstantGroup> getChangeableConstants()
        {
            List<ConstantGroup> Constants = new List<ConstantGroup>();

            //group spell casting
            List<IConstantChangerItem> grouptSpellCasting = new List<IConstantChangerItem>();
            grouptSpellCasting.Add(spellAimingRotationSpeedInternal);
            grouptSpellCasting.Add(spellPowerUpDownSpeedInternal);
            grouptSpellCasting.Add(defaultSpellGrowRate);            
            Constants.Add(new ConstantGroup("SpellCasting", grouptSpellCasting));

            //group mana
            List<IConstantChangerItem> groupMana = new List<IConstantChangerItem>();
            groupMana.Add(max_mana);
            groupMana.Add(mana_regenration_rate);
            groupMana.Add(mana_regenration_rate);
            Constants.Add(new ConstantGroup("Mana", groupMana));


            //group pushpull
            List<IConstantChangerItem> groupPushPull = new List<IConstantChangerItem>();
            groupPushPull.Add(pushpull_default_start_acceleration);
            groupPushPull.Add(pushpull_default_acceleration_change_factor);
            Constants.Add(new ConstantGroup("PushPull", groupPushPull));                      

            return Constants;
        }

        /// <summary>
        /// value how fast the spell is rotated while in casting mode
        /// </summary>
        public static double spellAimingRotationSpeed
        {
            get { return spellAimingRotationSpeedInternal.value; }
        }
        static ConstantChangerItemDouble spellAimingRotationSpeedInternal = new ConstantChangerItemDouble("spellAimingRotationSpeed", 100, 1);

        /// <summary>
        /// speed of the powering up and down of the spell in casting mode
        /// </summary>
        public static double spellPowerUpDownSpeed
        {
            get { return spellPowerUpDownSpeedInternal.value; }
        }
        static ConstantChangerItemDouble spellPowerUpDownSpeedInternal = new ConstantChangerItemDouble("spellPowerUpDownSpeed", 500, 5);


        /// <summary>
        /// how fast the spell grows if more mana is pumped into it
        /// </summary>
        public static float DefaultSpellGrowRate
        {
            get { return defaultSpellGrowRate.value; }
        }
        static ConstantChangerItemFloat defaultSpellGrowRate = new ConstantChangerItemFloat("DefaultSpellGrowRate", 0.006f, 0.0001f);

        /// <summary>
        /// Max mana a player can have
        /// </summary>
        public static int MAX_MANA
        {
            get { return max_mana.value; }
        }
        static ConstantChangerItemInteger max_mana = new ConstantChangerItemInteger("MAX_MANA", 1000, 10);
 
        /// <summary>
        /// regeneration rate of players mana
        /// </summary>
        public static float MANA_REGENERATION_RATE
        {
            get { return mana_regenration_rate.value; }
        }
        static ConstantChangerItemFloat mana_regenration_rate = new ConstantChangerItemFloat("MANA_REGENERATION_RATE", 0.25f, 0.001f);

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
        public static float ColdSpell_MoveSpeedManaFactor = 0.004f; //describes the ratio between used mana and move speed increase

        public static Spells.SpellConstants WarmSpellConstants = new Spells.SpellConstants(150, 1, 200, new TimeSpan(0, 0, 1));
        public static float WarmSpell_MoveSpeed = 160f;
        public static int WarmSpell_survivalTimeMs = 3000;
        public static int WarmSpell_durationOfActionMs = 5000;
        public static int WarmSpell_Force = 1;
        public static Vector2 WarmSpellGravity = new Vector2(0f, 0.2f);
        public static float WarmSpell_MoveSpeedManaFactor = 0.005f;  //describes the ratio between used mana and move speed increase

        public static Spells.SpellConstants CreateMatterSpellConstants = new Spells.SpellConstants(150, 1, 200, new TimeSpan(0, 0, 1));
        public static float CreateMatterSpell_MoveSpeed = 130;
        public static int CreateMatterSpell_Force = 1;
        public static float CreateMatterSpell_currentScale = 0.7f;
        public static float CreateMatterSpell_accelarationChangeFactor = -0.1f;

        public static Spells.SpellConstants NoGravitationSpellConstants = new Spells.SpellConstants(150, 1, 200, new TimeSpan(0, 0, 1));
        public static float NoGravitationSpell_MoveSpeed = 100;
        public static int NoGravitationSpell_survivalTimeMs = 9000;
        public static int NoGravitationSpell_durationOfActionMs = 10;
        public static int NoGravitationSpell_Force = 1;
        public static float NoGravitationSpell_accelarationChangeFactor = -0.05f;

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


        /// <summary>
        /// start acceleration for push and pull handler, used default
        /// </summary>
        public static float PUSHPULL_DEFAULT_START_ACCELERATION
        {
            get { return pushpull_default_start_acceleration.value; }
        }
        static ConstantChangerItemFloat pushpull_default_start_acceleration = new ConstantChangerItemFloat("PUSHPULL_DEFAULT_START_ACCELERATION", 1.1f, 0.01f);

        /// <summary>
        /// start acceleration for push and pull handler, used default
        /// </summary>
        public static float PUSHPULL_DEFAULT_ACCELERATION_CHANGE_FACTOR
        {
            get { return pushpull_default_acceleration_change_factor.value; }
        }
        static ConstantChangerItemFloat pushpull_default_acceleration_change_factor = new ConstantChangerItemFloat("PUSHPULL_DEFAULT_ACCELERATION_CHANGE_FACTOR", -0.2f, 0.001f);


    }
}
