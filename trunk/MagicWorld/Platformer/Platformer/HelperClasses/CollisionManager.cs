using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Platformer.DynamicLevelContent;

namespace Platformer.HelperClasses
{
    class CollisionManager
    {

        Level level;

        public CollisionManager(Level level)
        {
            this.level = level;
        }

        public static bool Intersects(Bounds c1, Bounds c2)
        {
            ContainmentType type = c1.Contains(c2);
            if (type == ContainmentType.Contains || type == ContainmentType.Intersects)
                return true;
            else
                return false;
        }
        
        public static bool Intersects(BasicGameElement b1, BasicGameElement b2)
        {
            return Intersects(b1.Bounds, b2.Bounds);
        }

        public static bool Contains(Bounds c1, Bounds c2)
        {
            ContainmentType type = c1.Contains(c2);
            if (type == ContainmentType.Contains)
                return true;
            else
                return false;
        }

        public static bool Contains(BasicGameElement b1, BasicGameElement b2)
        {
            return Contains(b1.Bounds, b2.Bounds);
        }



        /// <summary>
        /// Checks for collision with enemies
        /// </summary>
        /// <param name="elem">the game element which should be checked</param>
        /// <param name="enemiesColliadingWith">if you want a list of all enemies colliading with ..if not set null</param>
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
        /// <param name="tilesColliadingWith">if you want a list of all enemies colliading with ..if not set null</param>
        /// <returns></returns>
        public bool CollidateWithTiles(BasicGameElement elem, ref List<Tile> tilesColliadingWith)
        {

            bool isCollision = false;

            foreach (Tile tile in level.Tiles)
            {
                if (Intersects(tile.Bounds, elem.Bounds))
                {
                    isCollision = true;
                    if (tilesColliadingWith != null)
                    {
                        tilesColliadingWith.Add(tile);
                    }
                }
            }
            return isCollision;
        }

        /// <summary>
        /// Checks for collision with tiles --> later remove or rename....
        /// </summary>
        /// <param name="elem">the game element which should be checked</param>
        /// <param name="tilesColliadingWith">if you want a list of all enemies colliading with ..if not set null</param>
        /// <returns></returns>
        public bool CollidateWithGeneralLevelElements(BasicGameElement elem, ref List<BasicGameElement> elementsColliadingWith)
        {

            bool isCollision = false;

            foreach (BasicGameElement element in level.GeneralGameElements)
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


        public bool ColliadateWithLevelBounds(BasicGameElement elem)
        {
            //TODO
            return false;
        }

    }
}
