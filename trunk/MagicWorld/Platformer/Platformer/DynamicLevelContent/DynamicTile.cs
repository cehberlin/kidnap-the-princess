using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Platformer.DynamicLevelContent
{
    /// <summary>
    /// Base class for special tiles with some self dynamic properties
    /// </summary>
    class DynamicTile:Tile
    {
        Boolean isGravity = true;

        public Boolean IsGravity
        {
            get { return isGravity; }
            set { isGravity = value; }
        }

        const double FALLINGINTERVALL = 1000;

        /// <summary>
        /// defines in which time intervalls this tile falls down one tile
        /// </summary>
        double fallingTimeMS = FALLINGINTERVALL;

        public DynamicTile(String texture, TileCollision collision, Level level, Vector2 position) :
            base(texture, collision, level, position)
        {
        }

        public DynamicTile(String texture, TileCollision collision, Level level, int x, int y) :
            base(texture, collision, level, x,y)
        {
        }


        public override void Update(GameTime gameTime)
        {

            #region pseudogravity
            if (isGravity)
            {
                //falling is not smove it goes in tile steps because of problematic tile layout of game
                if (fallingTimeMS <= 0)
                {
                    fallingTimeMS = FALLINGINTERVALL;

                    //switch current tile with underlying one
                    Tile tmp = level.GetTile(X, Y + 1);
                    if (tmp.Texture == null)//empty tile
                    {
                        level.Tiles[X, Y + 1] = this;
                        level.Tiles[X, Y] = tmp;
                        this.Y++;
                        tmp.Y--;
                    }
                }
                else
                {
                    fallingTimeMS -= gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            #endregion pseudogravity
        }
    }
}
