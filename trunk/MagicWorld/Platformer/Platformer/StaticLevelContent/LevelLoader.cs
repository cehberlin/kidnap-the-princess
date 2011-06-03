using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld;
using MagicWorld.DynamicLevelContent;
using Microsoft.Xna.Framework;

namespace MagicWorld.StaticLevelContent
{
    /// <summary>
    /// Interface for new Level Loaders
    /// </summary>
    interface LevelLoader
    {
        Level Level
        {
            get;
            set;
        }

        List<Enemy> getEnemies();
        
        List<BasicGameElement> getGeneralObjects();
        
        Vector2 getPlayerStartPosition();

        BasicGameElement getLevelExit();

        double getMaxLevelTime();
    }
}
