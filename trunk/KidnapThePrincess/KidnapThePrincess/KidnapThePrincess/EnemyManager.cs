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
        //Counters for enemy AI
        int carriers;
        Vector2[] carrierPositions;
        int escorts;
        Vector2[] escortPositions;
        Vector2[] escortOffsets;
        bool[] carrierAssigned;
        bool[] escortAssigned;
        
        List<Hero> heroes;
        Texture2D sprite;
        Vector2 spawnPoint;
        Vector2 destOffset;
        TimeSpan lastWave;
        TimeSpan spawnIntervall=TimeSpan.FromSeconds(1);
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
            carriers = 0;
            escorts = 0;
            carrierPositions=new Vector2[4];
            carrierAssigned = new bool[4];
            escortPositions= new Vector2[8];
            escortOffsets=new Vector2[8];
            escortAssigned=new bool[8];
            InitEscortOffsets();
            this.level = level;
        }

        public void Update(GameTime time)
        {
            RemoveDeadEnemies();
            UpdateAIPositions();
            if (lastWave.Add(spawnIntervall) <= time.TotalGameTime)
                SpawnEnemy(time);
            foreach (Enemy e in enemies)
            {
                e.Destination = GetDestination(e);
                e.Update(time);
            }
        }

        private void InitEscortOffsets()
        {
            escortOffsets[0]=new Vector2(80,0);
            escortOffsets[1]=new Vector2(-80,0);
            escortOffsets[2]=new Vector2(0,80);
            escortOffsets[3]=new Vector2(0,-80);
            escortOffsets[4]=new Vector2(80,80);
            escortOffsets[5]=new Vector2(-80,80);
            escortOffsets[6]=new Vector2(80,-80);
            escortOffsets[7] = new Vector2(-80,-80);
        }

        private Vector2 GetDestination(Enemy e)
        {
            Vector2 dest = new Vector2();
            if (e.IsCarrying) dest = carrierPositions[e.AINumber];
            else if (e.IsEscorting) dest = escortPositions[e.AINumber];
            else dest = heroes[1].Position;//improvement needed right now they constantly only attack Player ONE
            return dest;
        }

        private void UpdateAIPositions()
        {
            for (int i = 0; i < carrierPositions.Length; i++)//0-3
            {
                carrierPositions[i] = heroes[0].Position + new Vector2(-15+i%2*45,(i/2)*60-40);
            }
            for (int j = 0; j < escortPositions.Length; j++)//0-7
            {
                escortPositions[j] = heroes[0].Position+escortOffsets[j];
            }
        }

        private Vector2 GetAI(Enemy e)
        {
            Vector2 dest = new Vector2();
            if (carriers < 4)//carry the princess
            {
                dest=carrierPositions[carriers];
                e.IsCarrying = true;
                e.IsEscorting = false;
                e.AINumber = GetAINumber(false,true);//////////////////////////////////prob
                carriers++;
            }
            else if (carriers == 4 && escorts < 8)//escort the princess
            {
                dest=escortPositions[escorts];
                e.IsEscorting = true;
                e.AINumber = GetAINumber(true,false);/////////////////////////////////prob
                escorts++;
            }
            else//attack heroes
            {
                dest=heroes[1].Position;
            }
            return dest;
        }

        private int GetAINumber(bool escort, bool carrier)
        {
            int counter=0;
            if (carrier)
            {
                while (counter < 4)
                {
                    if (!carrierAssigned[counter])
                    {
                        carrierAssigned[counter] = true;
                        return counter;
                    }
                    counter++;
                }
            }
            else if (escort)
            {
                while (counter < 8)
                {
                    if (!escortAssigned[counter])
                    {
                        escortAssigned[counter] = true;
                        return counter;
                    }
                    counter++;
                }
            }
            return -1;
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


        private Boolean spawnFromCastle = false;

        /// <summary>
        /// randomly spawns a new enemy slightly outside the maximum camera zoom out position
        /// </summary>
        /// <param name="time"></param>
        public void SpawnEnemy(GameTime time)
        {
            Enemy e = new Templar(sprite);
            if (spawnFromCastle) // spawn enemies from castle or frome some point on the map
            {
                e.Position = getRandomSpawningPoint();
            }
            else
            {
                e.Position = spawnPoint;
            }
            spawnFromCastle = !spawnFromCastle;

            GetAI(e);
            enemies.Add(e);
            lastWave = time.TotalGameTime;
        }

        public void RemoveDeadEnemies()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].Hitpoints < 0)
                {
                    if (enemies[i].IsCarrying)
                    {
                        carriers--;
                        carrierAssigned[i] = false;
                    }
                    else if (enemies[i].IsEscorting)
                    {
                        escorts--;
                        escortAssigned[i] = false;
                    }
                    //carriers -= enemies[i].IsCarrying ? 1 : 0;//if the enemy was a carrier reduce current counter
                    //escorts -= enemies[i].IsEscorting ? 1 : 0;//if the enemy was a escort reduce escort counter
                    enemies.Remove(enemies[i]);
                    //SpawnCoin(gameObjects[i].Position);
                }
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
