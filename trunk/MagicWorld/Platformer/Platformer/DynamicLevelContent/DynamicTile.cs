using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Platformer.DynamicLevelContent
{

    class DynamicTile:Tile
    {
        public DynamicTile(String texture, TileCollision collision, Level level, Vector2 position) :
            base(texture, collision, level, position)
        {
        }

        public DynamicTile(String texture, TileCollision collision, Level level, int x, int y) :
            base(texture, collision, level, x,y)
        {
        }
    }
}
