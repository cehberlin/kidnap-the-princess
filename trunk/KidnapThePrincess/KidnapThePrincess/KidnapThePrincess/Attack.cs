using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace KidnapThePrincess
{
    class Attack
    {
        Texture2D sprite;

        private int dmg;

        public int Damage
        {
            get { return dmg; }
            set { dmg = value; }
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

        public Attack(Texture2D tex,Vector2 position,int dmg)
        {
            sprite=tex;
            pos=position;
            collisionArea = new Rectangle((int)pos.X, (int)pos.Y, sprite.Width, sprite.Height);
            this.dmg=dmg;
        }

        public void Update(GameTime time)
        {
            duration = duration.Add(time.ElapsedGameTime);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, pos, Color.White);
        }
    }
}
