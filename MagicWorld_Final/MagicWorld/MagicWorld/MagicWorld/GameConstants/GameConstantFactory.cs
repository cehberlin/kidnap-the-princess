using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld.GameConstants
{
    public class GameConstantFactory
    {
        private GameConstantFactory() { }

        private static GameConstantFactory INSTANCE;
        public static GameConstantFactory GET_INSTANCE()
        {
            if(INSTANCE == null)
            {
                INSTANCE = new GameConstantFactory();
            }
            return INSTANCE;
        }


        IPlayerConstants playerConstants=new DefaultPlayerConstants();
        public IPlayerConstants getPlayerConstants()
        {
            return playerConstants;
        }

    }
}
