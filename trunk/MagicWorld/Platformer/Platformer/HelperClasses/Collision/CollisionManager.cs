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

        public delegate void OnCollisionWithCallback(BasicGameElement element);

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
        public void HandleGeneralCollisions(BasicGameElement elem, Vector2 movement, OnCollisionWithCallback callback = null,  bool ignorePlatforms = false,bool resolveCollision = true)
        {
            Bounds dummy_bounds = new Bounds(Vector2.Zero, 0);
            bool isOnGroundDummy = false;
            HandleGeneralCollisionsInternal(elem, movement, ref dummy_bounds, ref isOnGroundDummy , callback, ignorePlatforms,resolveCollision,true);
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
        public void HandleGeneralCollisions(BasicGameElement elem, Vector2 movement, ref Bounds oldBounds, ref bool IsOnGround, OnCollisionWithCallback callback = null, bool ignorePlatforms = false,bool resolveCollision = true)
        {
            HandleGeneralCollisionsInternal(elem, movement, ref oldBounds, ref IsOnGround,callback, ignorePlatforms,resolveCollision,false);
        }

        /// <summary>
        /// Extract collision implementation from player handles all collision with enemies, level objects... not of level bounds and level exit
        /// </summary>
        /// <param name="elem">the element which should be handled</param>
        /// <param name="previousBottom">must be a member variable, same variable ref on every call</param>
        /// <param name="IsOnGround">give you information if the object is on ground or not</param>
        /// <param name="callback">delegate which called for every object which has collision</param>
        /// <param name="ignorePlatforms">set true if plattforms should ignored ->allows moving down thourogh plattforms</param>
        /// <param name="ignoreBounds">ignore position change of bounds, only a internal parameter see method above</param>
        protected void HandleGeneralCollisionsInternal(BasicGameElement elem, Vector2 movement, ref Bounds oldBounds, ref bool IsOnGround, OnCollisionWithCallback callback, bool ignorePlatforms, bool resolveCollision, bool ignoreBounds)
        {
            List<BasicGameElement> collisionObjects = new List<BasicGameElement>();
            level.CollisionManager.CollidateWithGeneralLevelElements(elem, ref collisionObjects);

            //// Reset flag to search for ground collision.
            IsOnGround = false;

            foreach (BasicGameElement t in collisionObjects)
            {                
                CollisionType collision = t.Collision;
                if (collision != CollisionType.Passable && t!=elem)
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

                        // Resolve the collision along the y axis.
                        if (absDepthY < absDepthX || collision == CollisionType.Platform )
                        {
                            if (oldBounds.Position.Y != elem.Bounds.Position.Y || ignoreBounds)
                            {
                                // If we crossed the top of a tile, we are on the ground.
                                if (oldBounds.getRectangle().Bottom <= t.Bounds.getRectangle().Top)
                                {
                                    IsOnGround = true;
                                }

                                // Ignore platforms, unless we are on the ground.
                                if (collision == CollisionType.Impassable
                                    //this condition is necessary if we ware on ground and have a collision with another plattform with upper player body
                                    || IsOnGround && t.Bounds.getRectangle().Bottom > elem.Bounds.getRectangle().Bottom)
                                {
                                    //Debug.WriteLine("Impassable or on Ground");
                                    if (depth.Y < 0 && movement.Y > 0 || depth.Y >= 0 && movement.Y < 0)
                                    {
                                        if (resolveCollision)
                                        {
                                            // Resolve the collision along the Y axis.
                                            elem.Position = new Vector2(elem.Position.X, elem.Position.Y + depth.Y);
                                        }
                                        if (callback != null)
                                        {
                                            callback.Invoke(t);
                                        }
                                    }
                                }
                            }
                        }
                        else if (collision == CollisionType.Impassable && (oldBounds.Position.X != elem.Bounds.Position.X || ignoreBounds)) // Ignore platforms. //only handles this if object is in move
                        {
                            //Debug.WriteLine("xCollision" + absDepthX + " " +absDepthY);
                            if (absDepthY > 10) //tolereance delta
                            {
                                if (resolveCollision)
                                {
                                    // Resolve the collision along the X axis.
                                    elem.Position = new Vector2(elem.Position.X + depth.X, elem.Position.Y);
                                }
                                if (callback != null)
                                {
                                    callback.Invoke(t);
                                }
                            }
                        }
                    }
                }
            }

            // Save the new bounds bottom.
            oldBounds = elem.Bounds;
        }

    }
}
