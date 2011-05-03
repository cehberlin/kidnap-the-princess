using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace KidnapThePrincess
{
    class GameObject
    {
        Texture2D sprite;

        private Vector2 pos;

        public Vector2 Position
        {
            get { return pos; }
            set { pos = value; }
        }

        private Rectangle area;

        public Rectangle Area
        {
            get { return area; }
            set { area = value; }
        }

        public Rectangle CollisionArea
        {
            get { return new Rectangle(
                (int)(area.X + area.Width * 0.125),
                (int)(area.Y + area.Height * 0.25),
                (int)(area.Width * 0.75),
                (int)(area.Height * 0.75));
            }
            set { area = value; }
        }

        private int hitPoints;
        public int Hitpoints
        {
            get { return hitPoints; }
            set
            {
                hitPoints = value;
                if (hitPoints <= value && value>0)
                {
                    hitpoitsStore = hitPoints;
                }
            }
        }

        /// <summary>
        /// store highest hitpoints
        /// </summary>
        private int hitpoitsStore;

        public int HitpoitsMax
        {
            get { return hitpoitsStore; }
        }

        public GameObject(Texture2D tex)
        {
            sprite = tex;
            Hitpoints = 1;
        }
        public virtual void Draw(SpriteBatch sb) 
        {
            sb.Draw(sprite, Area, Color.White);
        }

        public virtual void Update(GameTime time) { }
    }
}
