using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace KidnapThePrincess
{
    class GameState
    {
        public enum State { START,RUN, GAMEOVER, WIN, EXIT,INIT };

        private State status;

        internal State Status
        {
            get { return status; }
            set { status = value; }
        }

        public int Points = 0;

        private Level level;


        public int SpwanTimeDiff = 8000;


        private GameState(Level level)
        {
            this.level = level;
            Status = State.START;
        }

        private static GameState instance;
        public static GameState getInstance(Level level)
        {
            if (instance == null)
            {
                instance = new GameState(level);
            }
            return instance;
        }


        public void Update(GameTime time)
        {
                //are we at the carriage yet?
                if (level.carriagRec.Contains((int)level.Heroes[0].Position.X, (int)level.Heroes[0].Position.Y))
                {
                    Status = State.WIN;
                }
                else if (level.PrincessCarrier.Bounds.Contains(new Vector3(level.CastlePosition.X, level.CastlePosition.Y, 0)) == ContainmentType.Contains)
                {
                    Status = State.GAMEOVER;
                }            
        }

    }
}
