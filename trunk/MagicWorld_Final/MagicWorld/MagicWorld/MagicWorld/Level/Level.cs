using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.AbstractGameElements;

namespace MagicWorld.Level
{
    /// <summary>
    /// contains all static level content and initialized dynamic level content (startpoint and starting state)
    /// </summary>
    public class Level
    {

        /// <summary>
        /// available spells the player can use
        /// </summary>
        public Spell AvailableSpells { get; private set; }

    }
}
