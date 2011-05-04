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
        
        /// <summary>
        /// the attacking hero
        /// </summary>
        private Hero hero;

        internal Hero Hero
        {
            get { return hero; }
            set { hero = value; }
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

        public Attack(Texture2D tex,Vector2 position,Hero h)
        {
            sprite=tex;
            pos=position;
            collisionArea = new Rectangle((int)pos.X, (int)pos.Y, sprite.Width, sprite.Height);
            this.hero=h;
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
