using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    interface ISpellInfluenceable
    {
        /// <summary>
        /// The class needs to implement the action which is forced by a spell on its own
        /// </summary>
        /// <param name="spell"></param>
        public virtual void SpellInfluenceAction(Spell spell);
    }
}
