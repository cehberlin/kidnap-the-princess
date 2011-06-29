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
    /// this switch reacts on a electric spell and puts the switch on or off depending on state
    /// </summary>
    class OnOffElectricitySwitch : AbstractSwitch
    {

        public OnOffElectricitySwitch(String texture, Level level, Vector2 position, string id)
            : base(texture, CollisionType.Platform, level, position,id)
        {

            //calculate bounds
            int left = (int)Math.Round(position.X);
            int top = (int)Math.Round(position.Y);

            //the bounds are only a small rectangle at bottom middle of the texture
            int boundsWith = 30;
            int boundsHeight = 30;
            this.bounds = new Bounds(left + Width / 2 - boundsWith / 2, top + Height - boundsHeight, boundsWith, boundsHeight);

        }


        public override bool SpellInfluenceAction(Spell spell)
        {
            if (spell.GetType() == typeof(ElectricSpell))
            {
                if (Activated)
                {
                    Deactivate();
                }
                else
                {
                    Activate();
                }
                return true;
            }
            return base.SpellInfluenceAction(spell);
        }     
    }
}
