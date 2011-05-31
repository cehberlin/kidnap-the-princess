using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld.GameElements.Spells
{
    /// <summary>
    /// creates spell objects
    /// </summary>
    public class SpellFactory
    {
        private SpellFactory()
        { 
        }

        private static SpellFactory INSTANCE;
        public static SpellFactory GET_INSTANCE()
        {
            if (INSTANCE == null)
            {
                INSTANCE = new SpellFactory();
            }
            return INSTANCE;
        }
    }
}
