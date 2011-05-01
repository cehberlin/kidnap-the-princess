using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace KidnapThePrincess
{
    class GameObjectManager
    {
        List<GameObject> gameObjects;

        public List<GameObject> GameObjects
        {
            get { return gameObjects; }
            set { gameObjects = value; }
        }

        public GameObjectManager()
        {
            gameObjects = new List<GameObject>();
        }

        public void Update(GameTime gameTime)
        {
            RemoveDestroyedObjects();
            foreach (GameObject go in gameObjects)
            {
                go.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (GameObject g in gameObjects)
            {
                g.Draw(sb);
            }
        }

        public void AddObject(GameObject g)
        {
            gameObjects.Add(g);
        }

        public void RemoveDestroyedObjects()
        {
            for (int i=0;i<gameObjects.Count;i++)
            {
                if (gameObjects[i].Hitpoints < 0)
                {
                    //SpawnCoin(gameObjects[i].Position);
                    gameObjects.Remove(gameObjects[i]);
                }
            }
        }
    }
}
