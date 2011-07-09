using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MagicWorld.DynamicLevelContent;
using System.Diagnostics;

namespace MagicWorld.HelperClasses
{
    public class CollisionManager
    {
        Level level;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="xAxisCollision">if true collision on x axis</param>
        /// <param name="xAxisCollision">if true collision on y axis</param>
        public delegate void OnCollisionWithCallback(BasicGameElement element, bool xAxisCollision, bool yAxisCollision);

        public CollisionManager(Level level)
        {
            this.level = level;
        }

        /// <summary>
        /// Checks if two bounds intersects
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static bool Intersects(Bounds c1, Bounds c2)
        {
            ContainmentType type = c1.Contains(c2);
            if (type == ContainmentType.Contains || type == ContainmentType.Intersects)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if two basic game elements intersects
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        public static bool Intersects(BasicGameElement b1, BasicGameElement b2)
        {
            return Intersects(b1.Bounds, b2.Bounds);
        }

        /// <summary>
        /// check if c1 contains c2 Bounds
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static bool Contains(Bounds c1, Bounds c2)
        {
            ContainmentType type = c1.Contains(c2);
            if (type == ContainmentType.Contains)
                return true;
            else
                return false;
        }

        /// <summary>
        /// check if b1 contains b2
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        public static bool Contains(BasicGameElement b1, BasicGameElement b2)
        {
            return Contains(b1.Bounds, b2.Bounds);
        }

        /// <summary>
        /// get the depth of the collision from b1 and b2
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        public static Vector2 GetCollisionDepth(BasicGameElement b1, BasicGameElement b2)
        {
            return b1.Bounds.CollisionDepth(b2.Bounds);
        }

        /// <summary>
        /// Checks for collision with all game elemets (only internal use)
        /// </summary>
        /// <param name="elem">the game element which should be checked</param>
        /// <param name="tilesColliadingWith">return a list of colliading objects</param>
        /// <returns></returns>
        protected bool CollidateWithGeneralLevelElements(BasicGameElement elem, ref List<BasicGameElement> elementsColliadingWith)
        {
            bool isCollision = false;

            foreach (BasicGameElement element in level.GeneralColliadableGameElements)
            {
                if (elem != element && Intersects(element.Bounds, elem.Bounds))
                {
                    isCollision = true;
                    if (elementsColliadingWith != null)
                    {
                        elementsColliadingWith.Add(element);
                    }
                }
            }
            return isCollision;
        }

        public bool CollidateWithPlayer(BasicGameElement elem)
        {
            return Intersects(elem, level.Player);
        }

        public bool CollidateWithLevelBounds(BasicGameElement elem)
        {
            return !Intersects(elem.Bounds, level.LevelBounds);
        }

        public bool CollidateWithLevelExit(BasicGameElement elem)
        {
            return Intersects(elem, level.EndPoint);
        }

        /// <summary>
        /// Another version of collision handling at the bottom with less parameters
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="movement"></param>
        /// <param name="callback"></param>
        /// <param name="ignorePlatforms"></param>
        public void HandleGeneralCollisions(BasicGameElement elem, OnCollisionWithCallback callback = null, bool ignorePlatforms = false, bool resolveCollision = true)
        {
            bool isOnGroundDummy = false;
            HandleGeneralCollisionsInternal(elem, ref isOnGroundDummy, callback, ignorePlatforms, resolveCollision);
        }

        /// <summary>
        /// Extract collision implementation from player handles all collision with enemies, level objects... not of level bounds and level exit
        /// hiddes some parameters for outer scope
        /// </summary>
        /// <param name="elem">the element which should be handled</param>
        /// <param name="previousBottom">must be a member variable, same variable ref on every call</param>
        /// <param name="IsOnGround">give you information if the object is on ground or not</param>
        /// <param name="callback">delegate which called for every object which has collision</param>
        /// <param name="ignorePlatforms">set true if plattforms should ignored ->allows moving down thourogh plattforms</param>
        public void HandleGeneralCollisions(BasicGameElement elem, ref bool IsOnGround, OnCollisionWithCallback callback = null, bool ignorePlatforms = false, bool resolveCollision = true, bool resolveWithoutPositionUpdate = true)
        {
            HandleGeneralCollisionsInternal(elem, ref IsOnGround, callback, ignorePlatforms, resolveCollision, resolveWithoutPositionUpdate);
        }

        /// <summary>
        /// Extract collision implementation from player handles all collision with enemies, level objects... not of level bounds and level exit
        /// not useable for all types of collisions
        /// </summary>
        /// <param name="elem">the element which should be handled</param>
        /// <param name="previousBottom">must be a member variable, same variable ref on every call</param>
        /// <param name="IsOnGround">give you information if the object is on ground or not</param>
        /// <param name="callback">delegate which called for every object which has collision</param>
        /// <param name="ignorePlatforms">set true if plattforms should ignored ->allows moving down thourogh plattforms</param>
        /// <param name="ignoreBounds">ignore position change of bounds, only a internal parameter see method above</param>
        protected void HandleGeneralCollisionsInternal(BasicGameElement elem, ref bool IsOnGround, OnCollisionWithCallback callback, bool ignorePlatforms, bool resolveCollision, bool resolveWithoutPositionUpdate = true)
        {
            List<BasicGameElement> collisionObjects = new List<BasicGameElement>();
            level.CollisionManager.CollidateWithGeneralLevelElements(elem, ref collisionObjects);

            //// Reset flag to search for ground collision.
            IsOnGround = false;

            float resolvedYCollision=0;

            Rectangle elemRectangle;
            Rectangle obstacleRectangle;

            elemRectangle = elem.Bounds.getRectangle();
            int elemLeftXCorner = elemRectangle.X;
            int elemRightXCorner = elemLeftXCorner + elemRectangle.Width;

            foreach (BasicGameElement t in collisionObjects)
            {
                CollisionType collision = t.Collision;
                if (collision != CollisionType.Passable && t != elem)
                {
                    if (collision == CollisionType.Platform && ignorePlatforms)
                    {
                        continue;
                    }

                    //get depth of intersection
                    Vector2 depth = CollisionManager.GetCollisionDepth(elem, t);

                    if (depth != Vector2.Zero)
                    {
                        float absDepthX = Math.Abs(depth.X);
                        float absDepthY = Math.Abs(depth.Y);

                        obstacleRectangle = t.Bounds.getRectangle();

                        // Resolve the collision along the y axis.
                        if (absDepthY < absDepthX || collision == CollisionType.Platform)
                        {
                            int obstacleLeftXCorner = obstacleRectangle.X;
                            int obstacleRightXCorner = obstacleLeftXCorner + obstacleRectangle.Width;

                            bool localOnGround = false;

                            // If we crossed the top of a tile, we are on the ground.
                            if (obstacleRectangle.Top > elemRectangle.Bottom - 10 && (//is higher than colliding object 
                                //and the x bounds are in the same area like the colliding object
                                elemLeftXCorner > obstacleLeftXCorner && elemLeftXCorner < obstacleRightXCorner ||
                                elemRightXCorner > obstacleLeftXCorner && elemRightXCorner < obstacleRightXCorner))
                            {
                                IsOnGround = true;
                                localOnGround = true;
                            }

                            // Ignore platforms, unless we are on the ground.
                            if (collision == CollisionType.Impassable
                                //this condition is necessary if we ware on ground and have a collision with another plattform with upper player body
                                || localOnGround && obstacleRectangle.Bottom > elemRectangle.Bottom)
                            {
                                //if we already resolved a collision to the bottom do not resolve to the top
                                if (resolvedYCollision > 0 || depth.Y < 0)
                                { 
                                    if (resolveCollision && (resolveWithoutPositionUpdate || elem.OldPosition.Y != elem.Position.Y))
                                    {
                                        // Resolve the collision along the Y axis.
                                        elem.Position = new Vector2(elem.Position.X, elem.Position.Y + depth.Y);
                                        resolvedYCollision += depth.Y;
                                    }
                                }
                                if (callback != null)
                                {
                                    callback.Invoke(t, false, true);
                                }
                            }
                        }
                        else if (collision == CollisionType.Impassable) // Ignore platforms. //only handles this if object is in move
                        {
                            if (absDepthY > 10) //tolerance delta
                            {
                                if (resolveCollision && (resolveWithoutPositionUpdate || elem.OldPosition.X != elem.Position.X))
                                {
                                    // Resolve the collision along the X axis.
                                    elem.Position = new Vector2(elem.Position.X + depth.X, elem.Position.Y);
                                }
                                if (callback != null)
                                {
                                    callback.Invoke(t, true, false);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// very basic collision detection does only check if exists collision and call callback
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="callback"></param>
        public void HandleCollisionWithoutRestrictions(BasicGameElement elem, OnCollisionWithCallback callback)
        {
            List<BasicGameElement> collisionObjects = new List<BasicGameElement>();
            level.CollisionManager.CollidateWithGeneralLevelElements(elem, ref collisionObjects);

            foreach (BasicGameElement t in collisionObjects)
            {
                CollisionType collision = t.Collision;
                if (collision != CollisionType.Passable && t != elem)
                {
                    if (callback != null)
                    {
                        //get depth of intersection
                        Vector2 depth = CollisionManager.GetCollisionDepth(elem, t);

                        if (depth != Vector2.Zero)
                        {
                            callback.Invoke(t, depth.X != 0, depth.Y != 0);
                        }
                    }
                }
            }
        }

    }
}
