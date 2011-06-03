using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld.StaticLevelContent
{
    class LevelLoaderFactory
    {
        /// <summary>
        /// Factory which gives you a new LevelLoader
        /// </summary>
        /// <param name="nr"></param>
        /// <returns></returns>
        public static ILevelLoader getLevel(int nr){

            switch (nr) { 
                //case 1:
                //    throw new NotImplementedException();                   
                default:
                    return new StaticLevelLoader();
            }
    }
    }
}
