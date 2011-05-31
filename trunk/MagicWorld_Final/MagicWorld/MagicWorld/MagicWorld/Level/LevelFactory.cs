using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld.Level
{
    /// <summary>
    /// provides Level objects
    /// </summary>
    public class LevelFactory
    {

        private LevelFactory() { }

        private static LevelFactory INSTANCE;
        public static LevelFactory GET_INSTANCE() {
            if(INSTANCE == null)
            {
                INSTANCE = new LevelFactory();
            }
            return INSTANCE;
        }

        /// <summary>
        /// creates Level instances
        /// returns null if the level is not available
        /// <param name="number"></param>
        /// <returns></returns>
        public Level getLevel(int number)
        {
            throw new NotImplementedException();
        }
    }
}
