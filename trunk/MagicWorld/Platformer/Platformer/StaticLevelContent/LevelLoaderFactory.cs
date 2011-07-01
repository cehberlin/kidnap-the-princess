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
            //return new StaticLevelLoader(); //testing
            //return new XMLLevelLoader(5);//TEST
            if (nr <= 7) 
            {
                return new XMLLevelLoader(nr); 
            } 
            else 
            {
                return new XMLLevelLoader(1); 
            }
            //switch (nr) { 
            //    case 1:
            //    case 2:
            //        return new XMLLevelLoader(nr);
            //    default:
            //        return new XMLLevelLoader(1);
            //}
    }
    }
}
