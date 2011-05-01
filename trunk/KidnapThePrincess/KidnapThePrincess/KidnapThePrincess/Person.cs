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
            set
            {
                position = value;
                OnPositionUpdate();
            }
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
            boundPosition = new Vector3(position, 0f);

            bounds = new BoundingSphere(boundPosition, 20); //20 is hacked TODO calculate or parameter
            rect.Width = tex.Width / 2;
            rect.Height = (int)(tex.Height / 1.2);
        }

        BoundingSphere bounds;

        public BoundingSphere Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }
        Vector3 boundPosition;

        private Rectangle rect;

        public Rectangle CollisionArea
        {
            get { return rect; }
            set { rect = value; }
        }


        protected virtual void OnPositionUpdate()
        {
            boundPosition.X = Position.X;
            boundPosition.Y = Position.Y;
            bounds.Center = boundPosition;
            rect.X = (int)Position.X + sprite.Width / 4;
            rect.Y = (int)(Position.Y+sprite.Height*0.1);
        }

        public virtual void Initialize() { }
        public virtual void Load() { }
        public virtual void UnLoad() { }
        public virtual void Update(GameTime time)
        {
            Position += Speed * Direction;    
        }
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, position, Color.White);
        }
    }
}
