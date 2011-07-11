using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.StaticLevelContent;
using Microsoft.Xna.Framework;
using MagicWorld.HelperClasses;
using MagicWorld.Constants;

namespace MagicWorld.DynamicLevelContent.SwitchRiddles
{
    /// <summary>
    /// this switch could only be used one time after activated with warm spell it will be destroyed
    /// </summary>
    class OneTimeDestroySwitch : AbstractSwitch
    {

        public OneTimeDestroySwitch(String texture, Level level, Vector2 position, string id,Color drawColor)
            : base(texture, CollisionType.Impassable, level, position, id, drawColor)
        {         
        }


        public override bool SpellInfluenceAction(Spell spell)
        {
            if (spell.GetType() == typeof(WarmSpell))
            {
                Activate();
                isRemovable = true;
                audioService.playSound(Audio.SoundType.destroy);
                return true;
            }
            return base.SpellInfluenceAction(spell);
        }     
    }
}
