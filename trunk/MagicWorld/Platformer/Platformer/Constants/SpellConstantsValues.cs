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
            grouptSpellCasting.Add(matterSpellGrowRate);
            Constants.Add(new ConstantGroup("SpellCasting", grouptSpellCasting));

            //group mana
            List<IConstantChangerItem> groupMana = new List<IConstantChangerItem>();
            groupMana.Add(max_mana);
            groupMana.Add(mana_regenration_rate);
            Constants.Add(new ConstantGroup("Mana", groupMana));


            //group pushpull
            List<IConstantChangerItem> groupPushPull = new List<IConstantChangerItem>();
            groupPushPull.Add(pushspell_maxsize);
            groupPushPull.Add(pushspell_growrate);
            groupPushPull.Add(pullspell_maxsize);
            groupPushPull.Add(pullspell_growrate);           
            groupPushPull.Add(pushpull_default_start_acceleration);
            groupPushPull.Add(pushpull_default_acceleration_change_factor);
            groupPushPull.Add(pushpull_moveabel_platforms_start_acceleration);            
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
        static ConstantChangerItemDouble spellAimingRotationSpeedInternal = new ConstantChangerItemDouble("spellAimingRotationSpeed", 70, 1);

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
        /// how fast the spell matter grows if more mana is pumped into it
        /// </summary>
        public static float MatterSpellGrowRate
        {
            get { return matterSpellGrowRate.value; }
        }
        static ConstantChangerItemFloat matterSpellGrowRate = new ConstantChangerItemFloat("MatterSpellGrowRate", 0.004f, 0.0001f);


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

        

        public static Spells.SpellConfiguration ColdSpellConstants = new Spells.SpellConfiguration(150, 1, 200, new TimeSpan(0, 0, 1));
        public static float ColdSpell_MoveSpeed = 150f;
        public static int ColdSpell_survivalTimeMs = 5000;
        public static int ColdSpell_durationOfActionMs = 7; //-->muliplication factor for used mana
        public static Vector2 ColdSpellGravity = new Vector2(0f, 0.2f);
        public static float ColdSpell_MoveSpeedManaFactor = 0.004f; //describes the ratio between used mana and move speed increase

        public static Spells.SpellConfiguration WarmSpellConstants = new Spells.SpellConfiguration(150, 1, 200, new TimeSpan(0, 0, 1));
        public static float WarmSpell_MoveSpeed = 160f;
        public static int WarmSpell_survivalTimeMs = 3000;
        public static int WarmSpell_durationOfActionMs = 7;//-->muliplication factor for used mana
        public static Vector2 WarmSpellGravity = new Vector2(0f, 0.2f);
        public static float WarmSpell_MoveSpeedManaFactor = 0.005f;  //describes the ratio between used mana and move speed increase

        public static Spells.SpellConfiguration CreateMatterSpellConstants = new Spells.SpellConfiguration(300, 1, 200, new TimeSpan(0, 0, 1));
        public static float CreateMatterSpell_MoveSpeed = 130;
        public static float CreateMatterSpell_currentScale = 0.7f;
        public static float CreateMatterSpell_accelarationChangeFactor = -0.1f;
        public static int   MATTER_EXISTENCE_TIME = 25; // time that created Matter exist -->muliplication factor for used mana

        public static Spells.SpellConfiguration NoGravitationSpellConstants = new Spells.SpellConfiguration(150, 1, 200, new TimeSpan(0, 0, 1));
        //public static float NoGravitationSpell_MoveSpeed = 120;
        public static int NoGravitationSpell_survivalTimeMs = 9000;
        public static int NoGravitationSpell_durationOfActionMs = 1;//-->muliplication factor for used mana
        public static float NoGravitationSpell_accelarationChangeFactor = -0.05f;

        public static Spells.SpellConfiguration ElectricSpellConstants = new Spells.SpellConfiguration(150, 1, 200, new TimeSpan(0, 0, 1));
        public static float ElectricSpell_MoveSpeed = 500f;
        public static int ElectricSpell_survivalTimeMs = 5000;
        public static int ElectricSpell_durationOfActionMs = 5;//-->muliplication factor for used mana


        public static Spells.SpellConfiguration PushSpellConstants = new Spells.SpellConfiguration(1, 1, 1000, new TimeSpan(0, 0, 1));
        public static int PushSpell_survivalTimeMs = 1000;
        public static int PushSpell_durationOfActionMs = 5;//-->muliplication factor for used mana


        public static float PushSpell_MaxSize
        {
            get { return pushspell_maxsize.value; }
        }
        static ConstantChangerItemFloat pushspell_maxsize = new ConstantChangerItemFloat("PushSpell_MaxSize", 100, 0.1f); //TODO was not reached

        public static float PushSpell_GrowRate
        {
            get { return pushspell_growrate.value; }
        }
        static ConstantChangerItemFloat pushspell_growrate = new ConstantChangerItemFloat("PushSpell_GrowRate", DefaultSpellGrowRate, 0.1f);

   
        public static Spells.SpellConfiguration PullSpellConstants = PushSpellConstants;//new Spells.SpellConstants(150, 1, 200, new TimeSpan(0, 0, 1));
        public static int PullSpell_survivalTimeMs = PushSpell_survivalTimeMs;
        public static int PullSpell_durationOfActionMs = PushSpell_durationOfActionMs;

        public static float PullSpell_MaxSize
        {
            get { return pullspell_maxsize.value; }
        }
        static ConstantChangerItemFloat pullspell_maxsize = new ConstantChangerItemFloat("PullSpell_MaxSize", PushSpell_MaxSize, 0.1f);

        public static float PullSpell_GrowRate
        {
            get { return pullspell_growrate.value; }
        }
        static ConstantChangerItemFloat pullspell_growrate = new ConstantChangerItemFloat("PullSpell_GrowRate", PushSpell_GrowRate, 0.01f);

   
        /// <summary>
        /// start acceleration for push and pull handler, used default
        /// </summary>
        public static float PUSHPULL_DEFAULT_START_ACCELERATION
        {
            get { return pushpull_default_start_acceleration.value; }
        }
        static ConstantChangerItemFloat pushpull_default_start_acceleration = new ConstantChangerItemFloat("PUSHPULL_DEFAULT_START_ACCELERATION", 1.12f, 0.01f);

        /// <summary>
        /// start acceleration for push and pull handler, used default
        /// </summary>
        public static float PUSHPULL_MOVEABEL_PLATFORMS_START_ACCELERATION
        {
            get { return pushpull_moveabel_platforms_start_acceleration.value; }
        }
        static ConstantChangerItemFloat pushpull_moveabel_platforms_start_acceleration = new ConstantChangerItemFloat("PUSHPULL_MOVEABEL_PLATFORMS_START_ACCELERATION", 1.1f, 0.01f);


        /// <summary>
        /// start acceleration for push and pull handler, used default
        /// </summary>
        public static float PUSHPULL_DEFAULT_ACCELERATION_CHANGE_FACTOR
        {
            get { return pushpull_default_acceleration_change_factor.value; }
        }
        static ConstantChangerItemFloat pushpull_default_acceleration_change_factor = new ConstantChangerItemFloat("PUSHPULL_DEFAULT_ACCELERATION_CHANGE_FACTOR", -0.2f, 0.001f);

        /// <summary>
        /// particle color for push pull influenced objects
        /// </summary>
        public static Color PUSH_COLOR = Color.Crimson;
        public static Color PULL_COLOR = Color.DarkGoldenrod;

    }
}
