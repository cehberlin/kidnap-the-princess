using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MagicWorld.DynamicLevelContent;

namespace MagicWorld.HelperClasses
{
    public class CollisionManager
    {

        Level level;

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
        /// Checks for collision with enemies
        /// </summary>
        /// <param name="elem">the game element which should be checked</param>
        /// <param name="enemiesColliadingWith">return a list of colliding objects</param>
        /// <returns></returns>
        public bool CollidateWithEnemy(BasicGameElement elem, ref List<Enemy> enemiesColliadingWith)
        {

            bool isCollision = false;

            foreach (BasicGameElement obj in level.GeneralColliadableGameElements)
            {
                if (obj.GetType() == typeof(Enemy))
                {
                    Enemy enemy = (Enemy)obj;
                    if (enemy != elem && Intersects(enemy.Bounds, elem.Bounds))
                    {
                        isCollision = true;
                        if (enemiesColliadingWith != null)
                        {
                            enemiesColliadingWith.Add(enemy);
                        }
                    }
                }
            }
            return isCollision;
        }


        /// <summary>
        /// Checks for collision with tiles --> later remove or rename....
        /// </summary>
        /// <param name="elem">the game element which should be checked</param>
        /// <param name="tilesColliadingWith">return a list of colliading objects</param>
        /// <returns></returns>
        public bool CollidateWithGeneralLevelElements(BasicGameElement elem, ref List<BasicGameElement> elementsColliadingWith)
        {
            bool isCollision = false;

            foreach (BasicGameElement element in level.GeneralColliadableGameElements)
            {
                if (elem!=element && Intersects(element.Bounds, elem.Bounds))
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
        /// extract collision implementation from player
        /// </summary>
        /// <param name="elem">the element which should be handled</param>
        /// <param name="previousBottom">must be a member variable, same variable ref on every call</param>
        /// <param name="IsOnGround">give you information if the object is on ground or not</param>
        public void HandleGeneralCollisions(BasicGameElement elem,Vector2 movement,ref float previousBottom,ref bool IsOnGround)
        {
            List<BasicGameElement> collisionObjects = new List<BasicGameElement>();
            level.CollisionManager.CollidateWithGeneralLevelElements(elem, ref collisionObjects);

            //// Reset flag to search for ground collision.
            IsOnGround = false;

            foreach (BasicGameElement t in collisionObjects)
            {
                CollisionType collision = t.Collision;
                if (collision != CollisionType.Passable)
                {
                    //get depth of intersection
                    Vector2 depth = CollisionManager.GetCollisionDepth(elem, t);

                    if (depth != Vector2.Zero)
                    {
                        float absDepthX = Math.Abs(depth.X);
                        float absDepthY = Math.Abs(depth.Y);

                        // Resolve the collision along the y axis.
                        if (absDepthY < absDepthX || collision == CollisionType.Platform)
                        {
                            // If we crossed the top of a tile, we are on the ground.
                            if (previousBottom <= t.Bounds.getRectangle().Top)
                            {
                                IsOnGround = true;
                                //Debug.WriteLine("On Ground");
                            }
                            //else
                            //{
                            //    Debug.WriteLine("Not On Ground");
                            //}

                            // Ignore platforms, unless we are on the ground.
                            if (collision == CollisionType.Impassable
                                //this condition is necessary if we ware on ground and have a collision with another plattform with upper player body
                                || IsOnGround && t.Bounds.getRectangle().Bottom > elem.Bounds.getRectangle().Bottom)
                            {
                                //Debug.WriteLine("Impassable or on Ground");
                                if (depth.Y < 0 && movement.Y > 0 || depth.Y >= 0 && movement.Y < 0)
                                {
                                    //Debug.WriteLine("Resolve Collision");
                                    // Resolve the collision along the Y axis.
                                    elem.Position = new Vector2(elem.Position.X, elem.Position.Y + depth.Y);
                                }
                                //Debug.WriteLine("Velocity Y " + velocity.Y);
                                //Debug.WriteLine("Depth Y " + depth.Y);
                            }
                        }
                        else if (collision == CollisionType.Impassable ) // Ignore platforms. //only handles this if player objects is in move
                        {
                            // Resolve the collision along the X axis.
                            elem.Position = new Vector2(elem.Position.X + depth.X, elem.Position.Y);
                        }
                    }
                }
            }

            // Save the new bounds bottom.
            previousBottom = elem.Bounds.getRectangle().Bottom;
        }

    }
}
