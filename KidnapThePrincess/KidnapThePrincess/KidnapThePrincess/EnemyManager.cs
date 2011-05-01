using System;
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

        public EnemyManager(Texture2D tex,Vector2 castlePos,List<Hero> heroes)
        {
            enemies = new List<Enemy>();
            sprite = tex;
            spawnPoint = new Vector2();
            spawnPoint= castlePos;
            lastWave = TimeSpan.Zero;
            this.heroes = heroes;
            destOffset = new Vector2(30, 66);
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

        public void SpawnEnemy(GameTime time)
        {
            Enemy e = new Templar(sprite);
            e.Position = spawnPoint;
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
    }
}
