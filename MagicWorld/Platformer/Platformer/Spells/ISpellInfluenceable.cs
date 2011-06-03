using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld
{
    interface ISpellInfluenceable
    {

        /// <summary>
        /// The class needs to implement the action which is forced by a spell on its own
        /// </summary>
        /// <param name="spell"></param>
        /// <returns>true if spell has done some influence and needs to be removed</returns>
        Boolean SpellInfluenceAction(Spell spell);
    }
}
