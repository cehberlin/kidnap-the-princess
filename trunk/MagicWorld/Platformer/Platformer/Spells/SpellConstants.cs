using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld.Spells
{
    /// <summary>
    /// Constant Values for a spell
    /// </summary>
    public class SpellConstants
    {
        /// <summary>
        /// Basic Cost when start casting a spell
        /// </summary>
        virtual public int BasicCastingCost { get; protected set; }

        /// <summary>
        /// casting cost for growing the spell per second
        /// </summary>
        virtual public int CastingCostPerSecond { get; protected set; }

        /// <summary>
        /// maximum amount of mana that can be used for this spell
        /// limits the spell power/size
        /// </summary>
        virtual public int MaximalSpellPower { get; protected set; }

        /// <summary>
        /// cooldown between casting the same spell
        /// </summary>
        virtual public TimeSpan CoolDown { get; protected set; }

        public SpellConstants(int basicCastingCost, int castingCostPerSecond, int maximalSpellPower, TimeSpan coolDown)
        {
            this.BasicCastingCost = basicCastingCost;
            this.CastingCostPerSecond = castingCostPerSecond;
            this.MaximalSpellPower = maximalSpellPower;
            this.CoolDown = coolDown;
        }
    }
}
