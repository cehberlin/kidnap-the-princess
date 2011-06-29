using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.StaticLevelContent;
using Microsoft.Xna.Framework;
using MagicWorld.HelperClasses;

namespace MagicWorld.DynamicLevelContent.SwitchRiddles
{
    /// <summary>
    /// this switch could only be used one time after activated with warm spell it will be destroyed
    /// </summary>
    class OneTimeDestroySwitch : AbstractSwitch
    {

        public OneTimeDestroySwitch(String texture, Level level, Vector2 position, string id)
            : base(texture, CollisionType.Platform, level, position,id)
        {         
        }


        public override bool SpellInfluenceAction(Spell spell)
        {
            if (spell.GetType() == typeof(WarmSpell))
            {
                Activate();
                isRemovable = true;
                return true;
            }
            return base.SpellInfluenceAction(spell);
        }     
    }
}
