using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace KidnapThePrincess
{
    class Coin
    {
        Texture2D sprite;

        private int worth=1;

        public int Worth
        {
            get { return worth; }
            set { worth = value; }
        }

        private Vector2 pos;

        public Vector2 Position
        {
            get { return pos; }
            set { pos = value; }
        }

        private Rectangle collisionArea;

        public Rectangle CollisionArea
        {
            get { return collisionArea; }
            set { collisionArea = value; }
        }

        private TimeSpan duration;

        public TimeSpan Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        private TimeSpan lifeSpan=new TimeSpan(0,0,3);
        

        public Coin(Texture2D tex,Vector2 pos)
        {
            sprite = tex;
            this.pos = pos;
            duration=TimeSpan.Zero;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite,collisionArea,Color.White);
        }

        public void Update(GameTime time)
        {
            duration = duration.Add(time.ElapsedGameTime);
        }
    }
}
