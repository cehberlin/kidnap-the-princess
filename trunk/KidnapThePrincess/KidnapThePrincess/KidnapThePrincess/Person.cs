using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace KidnapThePrincess
{
    class Person
    {
        public Texture2D sprite;
        private float speed;
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        private Vector2 direction;
        public Vector2 Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public float X
        {
            get { return position.X; }
            set { position.X = value; }
        }
        public float Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }
        public Person(Texture2D tex)
        {
            sprite = tex;
            direction = Vector2.Zero;
            position = Vector2.Zero;
        }

        public virtual void Initialize() { }
        public virtual void Load() { }
        public virtual void UnLoad() { }
        public virtual void Update() 
        {
            Position += Speed * Direction;
        }
        public virtual void Draw(SpriteBatch sb) 
        {
            sb.Draw(sprite, position, Color.White);
        }
    }
}
