using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.DynamicLevelContent;
using Microsoft.Xna.Framework;

namespace MagicWorld.HelperClasses.Collision
{
    /// <summary>
    /// this class provides reusable functions for physics influeced behavior
    /// </summary>
    public class PhysicsManager
    {
        Level level;
        public PhysicsManager(Level level)
        {
            this.level = level;
        }

        /// <summary>
        /// Apply Gravity to an object; no collision handling
        /// Be careful that the velocity is increasing all time you call this method. you need to reset the object
        /// velocity on ground collision; if not at some time the object will fall through the ground (wehen velocity is greater than object hight)
        /// </summary>
        /// <param name="elem">the game element</param>
        /// <param name="acceleration">the vector with which strength and direction the gravity goes</param>
        public virtual void ApplyGravity(BasicGameElement elem, Vector2 acceleration, GameTime time)
        {
            elem.Velocity += acceleration * (float)time.ElapsedGameTime.TotalMilliseconds;
        }
    }
        
}
