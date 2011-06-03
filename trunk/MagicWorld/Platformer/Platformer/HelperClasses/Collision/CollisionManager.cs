using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MagicWorld.DynamicLevelContent;

namespace MagicWorld.HelperClasses
{
    class CollisionManager
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
        /// <param name="enemiesColliadingWith">return a list of colliading objects</param>
        /// <returns></returns>
        public bool CollidateWithEnemy(BasicGameElement elem, ref List<Enemy> enemiesColliadingWith)
        {

            bool isCollision = false;

            foreach (Enemy enemy in level.Enemies)
            {
                if (Intersects(enemy.Bounds,elem.Bounds))
                {                
                    isCollision=true;
                    if (enemiesColliadingWith != null)
                    {
                        enemiesColliadingWith.Add(enemy);
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
                if (Intersects(element.Bounds, elem.Bounds))
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

    }
}
