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
    /// this switch reacts on a electric spell and puts the switch on for a defined time 
    /// </summary>
    class TimedElectricitySwitch : AbstractSwitch
    {

        double resetTimeMs;
        double currentTimeMs;

        public TimedElectricitySwitch(String texture, Level level, Vector2 position, string id, double resetTimeMs)
            : base(texture, CollisionType.Impassable, level, position)
        {
            this.ID = id;

            //calculate bounds
            int left = (int)Math.Round(position.X);
            int top = (int)Math.Round(position.Y);

            //the bounds are only a small rectangle at bottom middle of the texture
            int boundsWith = 30;
            int boundsHeight = 30;
            this.bounds = new Bounds(left + Width / 2 - boundsWith / 2, top + Height - boundsHeight, boundsWith, boundsHeight);

            this.resetTimeMs = resetTimeMs;
        }


        public override void Update(GameTime gameTime)
        {
            CheckForPushDown(gameTime);
            base.Update(gameTime);
        }

        public override void Activate()
        {
            base.Activate();
            //reset activation time
            currentTimeMs = resetTimeMs;
        }

        public override bool SpellInfluenceAction(Spell spell)
        {
            if (spell.GetType() == typeof(ElectricSpell))
            {
                Activate();
                return true;
            }
            return base.SpellInfluenceAction(spell);
        }

        /// <summary>
        /// check if switch is pushed down
        /// </summary>
        void CheckForPushDown(GameTime time)
        {
            if (Activated)
            {
                currentTimeMs -= time.ElapsedGameTime.TotalMilliseconds;
                if (currentTimeMs < 0)
                {
                    Deactivate();
                }
            }
        }

    }
}
