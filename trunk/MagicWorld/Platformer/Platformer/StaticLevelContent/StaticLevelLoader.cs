using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.DynamicLevelContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace MagicWorld.StaticLevelContent
{
    /// <summary>
    /// Static fixed implementation of a test level
    /// </summary>
    class StaticLevelLoader:ILevelLoader
    {
        Level level;

        public StaticLevelLoader()
        {
        }

        #region LevelLoader Member

        public List<Enemy> getEnemies()
        {
            List<Enemy> enemies = new List<Enemy>();

            return enemies;
        }

        public List<BasicGameElement> getGeneralObjects()
        {
            List<BasicGameElement> elements = new List<BasicGameElement>();

            elements.Add(LoadBlock("BlockA0", BlockCollision.Impassable,new Vector2(0f,200),200,32));
            elements.Add(LoadBlock("BlockA3", BlockCollision.Impassable, new Vector2(220f, 300), 150, 45));
            elements.Add(LoadBlock("BlockA4", BlockCollision.Impassable, new Vector2(400f, 250)));
            elements.Add(LoadBlock("BlockA1", BlockCollision.Impassable, new Vector2(500f, 200),300,10));
            elements.Add(new IceBlockElement(level,new Vector2(100f, 130)));
            elements.Add(new IceBlockElement(level, new Vector2(100f, 100)));            

            return elements;
        }

        
        public Vector2 getPlayerStartPosition()
        {
            Vector2 startPos = new Vector2(5, 5);

            return startPos;
        }

        public BasicGameElement getLevelExit()
        {
            BasicGameElement exit = LoadBlock("Exit", BlockCollision.Passable,new Vector2(650,160));
            return exit;
        }

        public double getMaxLevelTime()
        {
            return 99;
        }

        public Song getBackgroundMusic()
        {
            return level.Content.Load<Song>("Sounds/Backgroundmusic");
        }

        public HelperClasses.Bounds getLevelBounds()
        {
            return new HelperClasses.Bounds(0, 0, 5000, 5000);
        }


        public Level Level
        {
            get
            {
                return level;
            }
            set
            {
                this.level = value;
            }
        }

        #endregion

        private BlockElement LoadBlock(string name, BlockCollision collision, Vector2 position)
        {
            return new BlockElement("Tiles/" + name, collision, level, position);
        }

        private BlockElement LoadBlock(string name, BlockCollision collision, Vector2 position, int width, int height)
        {
            return new BlockElement("Tiles/" + name, collision, level, position,width,height);
        }

    }
}
