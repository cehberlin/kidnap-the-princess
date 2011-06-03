using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld.StaticLevelContent
{
    /// <summary>
    /// Static fixed implementation of a test level
    /// </summary>
    class StaticLevelLoader:LevelLoader
    {
        #region LevelLoader Member

        public List<Enemy> getEnemies()
        {
            throw new NotImplementedException();
        }

        public List<DynamicLevelContent.BasicGameElement> getGeneralObjects()
        {
            throw new NotImplementedException();
        }

        public List<Icecicle> getIcecicles()
        {
            throw new NotImplementedException();
        }

        public Microsoft.Xna.Framework.Vector2 getPlayerStartPosition()
        {
            throw new NotImplementedException();
        }

        public DynamicLevelContent.BasicGameElement getLevelExit()
        {
            throw new NotImplementedException();
        }

        public double getMaxLevelTime()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
