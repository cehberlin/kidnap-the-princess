using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace KidnapThePrincess
{
    class CollisionManager
    {
        public void ObstacleCollisionResolution(List<GameObject> immovables, List<Hero> heroes)
        {
            List<GameObject> obstacle = new List<GameObject>();
            foreach (GameObject go in immovables)
            {
                foreach (Hero h in heroes)
                {
                    if (go.CollisionArea.Intersects(h.CollisionArea))//collision check
                    {
                        h.Position += GetCorrectionVector(h.CollisionArea, go.CollisionArea);
                    }
                }
            }
        }

        public void PrincessCollisionCheck(List<Enemy> enemies, Hero princessCarrier)
        {
            int enemyCarriers = 0;
            foreach (Enemy e in enemies)
            {
                if (princessCarrier.CollisionArea.Intersects(e.CollisionArea))
                    enemyCarriers++;
            }
            //princessCarrier.Direction = new Vector2(0, 1 - (enemyCarriers * 0.5f));
            princessCarrier.Direction = new Vector2(0,1);
        }

        public void HeroEnemyCollisionResolution(List<Enemy> enemies, List<Hero> heroes)
        {
            List<Hero> toResolve;
            toResolve = new List<Hero>();
            toResolve = heroes.ToList<Hero>();
            toResolve.Remove(heroes[0]);
            foreach (Enemy e in enemies)
            {
                foreach (Hero h in heroes)
                {
                    if (e.CollisionArea.Intersects(h.CollisionArea))//collision check
                    {
                        e.Position += GetCorrectionVector(e.CollisionArea, h.CollisionArea);
                        h.Freezed = true;
                    }
                }
            }
        }

        /// <summary>
        /// Checks if a heros attack has hit the enemy
        /// </summary>
        /// <param name="enemies">all enemies</param>
        /// <param name="attacks">all attacks</param>
        /// <returns>Null if the attack did not hit anything, otherwise the attack that has hit something</returns>
        public Attack AttackEnemyCollision(List<Enemy> enemies, List<Attack> attacks)
        {
            foreach (Attack a in attacks)
            {
                foreach (Enemy e in enemies)
                {
                    if (a.CollisionArea.Intersects(e.CollisionArea))
                    {
                        a.Hero.attack(e,a);
                        return a;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Checks if an heros attack has hit a GameObject
        /// </summary>
        /// <param name="gameObjects">all gameobjects</param>
        /// <param name="attacks">all attacks</param>
        /// <returns>Null if the attack did not hit anything, otherwise the attack that has hit something</returns>
        public Attack AttackGameObjectCollision(List<GameObject> gameObjects, List<Attack> attacks)
        {
            foreach (Attack a in attacks)
            {
                foreach (GameObject go in gameObjects)
                {
                    if (a.CollisionArea.Intersects(go.CollisionArea))
                    {            
                        a.Hero.attack(go,a);
                        return a;
                    }
                }
            }
            return null;
        }

        private Vector2 GetCorrectionVector(Rectangle a, Rectangle b)
        {
            Vector2 correctionVector;
            Vector2 helper;
            int x1 = Math.Abs(a.Right - b.Left);
            int x2 = Math.Abs(a.Left - b.Right);
            int y1 = Math.Abs(a.Bottom - b.Top);
            int y2 = Math.Abs(a.Top - b.Bottom);

            helper.X = x1 < x2 ? -x1 : x2;
            helper.Y = y1 < y2 ? -y1 : y2;
            correctionVector.X = Math.Abs(helper.X) > Math.Abs(helper.Y) ? 0 : helper.X;
            correctionVector.Y = Math.Abs(helper.X) < Math.Abs(helper.Y) ? 0 : helper.Y;

            return correctionVector;
        }
    }
}
