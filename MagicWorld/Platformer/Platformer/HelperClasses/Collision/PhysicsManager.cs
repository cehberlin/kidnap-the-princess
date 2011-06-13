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
        /// </summary>
        /// <param name="elem">the game element</param>
        /// <param name="acceleration">the vector with which strength and direction the gravity goes</param>
        public virtual void ApplyGravity(BasicGameElement elem, Vector2 acceleration, GameTime time)
        {
            elem.Velocity += acceleration * (float)time.ElapsedGameTime.TotalMilliseconds;
        }
    }
        
}
