using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MagicWorld.Spells;
using System.Diagnostics;

namespace MagicWorld.DynamicLevelContent
{
    /// <summary>
    /// Base class for special tiles with some self dynamic properties
    /// </summary>
    class DynamicBlockElement:BlockElement
    {
        Boolean isGravity = true;

        public Boolean IsGravity
        {
            get { return isGravity; }
            set { isGravity = value; }
        }

        protected Boolean gravityIsSetOffBySpell = false;

        public DynamicBlockElement(String texture, CollisionType collision, Level level, Vector2 position) :
            base(texture, collision, level, position)
        {
        }

        public override bool SpellInfluenceAction(Spell spell)
        {
            if (spell.GetType() == typeof(NoGravitySpell))
            {
                gravityIsSetOffBySpell = true;
                return false; //do not remove spell
            }

            return base.SpellInfluenceAction(spell);
        }


        public override void Update(GameTime gameTime)
        {
            //every update cycle in the game the spells where updated before the tiles, so if a no gravity colliates with this tile
            //it resets the flag
            if (isGravity && !gravityIsSetOffBySpell)
            {
                level.PhysicsManager.ApplyGravity(this, Constants.PhysicValues.DEFAULT_GRAVITY,gameTime);
            }
            else
            {
                Debug.WriteLine("no gravity in this update cycle");
                gravityIsSetOffBySpell = false;
            }
        }
    }
}
