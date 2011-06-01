using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.AbstractGameElements;

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

        public Spell createSpell(SpellType type)
        {
            throw new NotImplementedException();
        }
    }
}
