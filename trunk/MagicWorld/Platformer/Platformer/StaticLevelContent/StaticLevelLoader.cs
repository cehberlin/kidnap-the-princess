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

            elements.Add(LoadTile("BlockA0", BlockCollision.Impassable,new Vector2(0f,7)));
            elements.Add(LoadTile("BlockA3", BlockCollision.Impassable, new Vector2(1f, 7)));
            elements.Add(LoadTile("BlockA1", BlockCollision.Impassable, new Vector2(2f, 7)));
            elements.Add(new IceBlockElement(level,new Vector2(2f, 6)));
            elements.Add(new IceBlockElement(level, new Vector2(2f, 5)));
            elements.Add(LoadTile("BlockA4", BlockCollision.Impassable, new Vector2(3f, 7)));

            return elements;
        }

        
        public Vector2 getPlayerStartPosition()
        {
            Vector2 startPos = new Vector2(5, 5);

            return startPos;
        }

        public BasicGameElement getLevelExit()
        {
            BasicGameElement exit = LoadTile("Exit", BlockCollision.Passable,new Vector2(3,6));
            return exit;
        }

        public double getMaxLevelTime()
        {
            return 99;
        }

        #endregion


        /// <summary>
        /// Creates a new tile. The other tile loading methods typically chain to this
        /// method after performing their special logic.
        /// </summary>
        /// <param name="name">
        /// Path to a tile texture relative to the Content/Tiles directory.
        /// </param>
        /// <param name="collision">
        /// The tile collision type for the new tile.
        /// </param>
        /// <returns>The new tile.</returns>
        private BlockElement LoadTile(string name, BlockCollision collision, Vector2 position)
        {
            return new BlockElement("Tiles/" + name, collision, level, position);
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

        #region ILevelLoader Member


        public Song getBackgroundMusic()
        {
            return level.Content.Load<Song>("Sounds/Backgroundmusic");
        }

        #endregion
    }
}
