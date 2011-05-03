﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace KidnapThePrincess
{
    class EnemyManager
    {
        List<Hero> heroes;
        Texture2D sprite;
        Vector2 spawnPoint;
        Vector2 destOffset;
        TimeSpan lastWave;
        TimeSpan spawnIntervall=TimeSpan.FromSeconds(3);
        private List<Enemy> enemies;

        public List<Enemy> Enemies
        {
            get { return enemies; }
            set { enemies = value; }
        }

        private Level level;

        public EnemyManager(Texture2D tex, Vector2 castlePos, List<Hero> heroes, Level level)
        {
            enemies = new List<Enemy>();
            sprite = tex;
            spawnPoint = new Vector2();
            spawnPoint= castlePos;
            lastWave = TimeSpan.Zero;
            this.heroes = heroes;
            destOffset = new Vector2(30, 66);
            this.level = level;
        }

        public void Update(GameTime time)
        {
            RemoveDeadEnemies();
            if (lastWave.Add(spawnIntervall) <= time.TotalGameTime)
                SpawnEnemy(time);
            foreach (Enemy e in enemies)
            {
                e.Destination = heroes[0].Position+destOffset;
                e.Update(time);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Enemy e in enemies)
            {
                e.Draw(sb);
            }
        }

        public void Init()
        {
            enemies.Clear();
        }

        /// <summary>
        /// randomly spawns a new enemy slightly outside the maximum camera zoom out position
        /// </summary>
        /// <param name="time"></param>
        public void SpawnEnemy(GameTime time)
        {
            Enemy e = new Templar(sprite);
            if (new Random().NextDouble() > 0.5) // randomly spawn enemies from castle or frome some point on the map
            {
                e.Position = getRandomSpawningPoint();
            }
            else
            {
                e.Position = spawnPoint;
            }

            enemies.Add(e);
            lastWave = time.TotalGameTime;
        }

        public void RemoveDeadEnemies()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].Hitpoints < 0)
                    enemies.Remove(enemies[i]);
                //SpawnCoin(gameObjects[i].Position);
            }
        }

        private Vector2 getRandomSpawningPoint()
        {
            Camera2d cam = level.PrincessCamera;
            List<Enemy> enemyList = Enemies;
            List<GameObject> gameObjectList = level.GameObjectManager.GameObjects;

            //Game1 game = null;
            //game.Window.ClientBounds.Center // 

            const int X_OFFSET = 10; //offset to let enemies spawn outside the cam

            Boolean spawnDownside = false; // true-> enemy spawns towards goal


            int minY = (int)cam.Pos.Y - cam.Height / 2 - X_OFFSET;
            int maxY = (int)cam.Pos.Y + cam.Height / 2 + X_OFFSET;


            //todo set spawnDownside manually if the player camera is at the castle or at the level end

            if (minY < 0) // view at strat  (castle)
            {
                spawnDownside = true;
            }
            else if (maxY > level.PlayArea.Bottom) {  // view at end (goal)
                spawnDownside = false;
            }
            else
            { // between start and end
                if (new Random().NextDouble() > 0.7) // more enemys spawn behind the players
                {
                    spawnDownside = true;
                }
            }

            int yPos;
            float minX = -(level.PlayArea.Width / 2); //TODO
            float maxX = level.PlayArea.Width / 2; //TODO

            if (spawnDownside)
            {
                yPos = maxY;
            }
            else
            {
                yPos = minY;
            }

            //find legal spawning point
            Random random = new Random();
            Boolean legalSpawningPoint = false;
            Rectangle enemySize = new Rectangle(0, yPos, 100, 100); //TODO
            do
            {
                legalSpawningPoint = true;
                enemySize.X = (int)randomBetween(minX, maxX);
                foreach (Hero hero in heroes)
                {
                    if (hero.CollisionArea.Intersects(enemySize))
                    {
                        legalSpawningPoint = false;
                        continue;
                    }
                }
                foreach (GameObject gameobject in gameObjectList)
                {
                    if (gameobject.CollisionArea.Intersects(enemySize))
                    {
                        legalSpawningPoint = false;
                        continue;
                    }
                }
                foreach (Enemy enemy in enemyList)
                {
                    if (enemy.CollisionArea.Intersects(enemySize))
                    {
                        legalSpawningPoint = false;
                        continue;
                    }
                }
            } while (!legalSpawningPoint);

            Vector2 spawningPoint = new Vector2(enemySize.X, enemySize.Y);


            return spawningPoint;
        }

        private double randomBetween(float min, float max)
        {
            Random random = new Random();
            return (min + random.NextDouble() * (max - min));
        }
    }
}