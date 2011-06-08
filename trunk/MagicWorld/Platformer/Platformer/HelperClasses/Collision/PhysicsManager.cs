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
        /// Apply Gravity to an object; stops if collision occurs
        /// </summary>
        /// <param name="elem">the game element</param>
        /// <param name="acceleration">the vector with which strength and direction the gravity goes</param>
        /// <returns>True if has influence,false if not</returns>
        public virtual bool ApplyGravity(BasicGameElement elem, Vector2 acceleration,GameTime time)
        {
            Vector2 oldPos = elem.Position;

            elem.Position += acceleration*(float)time.ElapsedGameTime.TotalMilliseconds;

            List<BasicGameElement> collisionObjects = new List<BasicGameElement>();
            level.CollisionManager.CollidateWithGeneralLevelElements(elem, ref collisionObjects);

            foreach (BasicGameElement t in collisionObjects)
            {
                CollisionType collision = t.Collision;
                if (collision == CollisionType.Impassable || collision == CollisionType.Platform)
                {
                    Vector2 depth = CollisionManager.GetCollisionDepth(elem, t);

                    //increse distance until we have no more collision
                    if (depth.Y >= 0)
                    {
                        depth.Y++;
                    }
                    else
                    {
                        depth.Y--;
                    }                    
                    elem.Position += new Vector2(0,depth.Y);

                }
            }

            if (oldPos == elem.Position)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
