using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MagicWorld.HelperClasses
{
    public class Camera2d
    {
        protected float _zoom; // Camera Zoom
        public Matrix _transform; // Matrix Transform
        public Vector2 _pos; // Camera Position
        protected float _rotation; // Camera Rotation

        private Vector2 speed;//Camera speed
        private Vector2 targetPosition;//Target position

        public Vector2 TargetPosition
        {
            get { return targetPosition; }
            set { targetPosition = value; }
        }
        private int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        private int height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        private int maxVelocity;

        public int MaxVelocity
        {
            get { return maxVelocity; }
            set { maxVelocity = value; }
        }

        private int acceleration;

        public int Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }



        /// <summary>
        /// The area the camera covers.
        /// </summary>
        public Rectangle Area
        {
            get { return new Rectangle((int)_pos.X - width / 2, (int)_pos.Y - height / 2, width, height); }
        }

        /// <summary>
        /// Constructor for camera class
        /// </summary>
        /// <param name="height">Height of the clientbounds of the game</param>
        /// <param name="width">Width of the clientbounds of the game</param>
        public Camera2d(int width, int height)
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
            this.width = width;
            this.height = height;
        }

        public void Update(GameTime time)
        {
        }

        // Sets and gets zoom
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        // Auxiliary function to move the camera
        public void Move(Vector2 amount)
        {
            _pos += amount;
        }

        //set/get Speed
        public Vector2 Speed
        {
            get { return speed; }
            set
            {
                if (speed.Length() > maxVelocity)
                {
                    speed.Normalize();
                    speed *= maxVelocity;
                }
                else speed = value;
            }
        }

        public void IncrementSpeed(Vector2 _acceleration)
        {
            speed += _acceleration;
            if (speed.Length() > maxVelocity)
            {
                speed.Normalize();
                speed *= maxVelocity;
            }
        }

        //Follow

        public void FollowTarget(float time)
        {
            Vector2 direction;
            //check if the target is reached
            if (Math.Abs(_pos.X - targetPosition.X) > 2 || Math.Abs(_pos.Y - targetPosition.Y) > 2)
            {
                //find direction
                if (_pos.X > targetPosition.X) direction.X = -1;
                else direction.X = 1;

                if (_pos.Y > targetPosition.Y) direction.Y = -1;
                else direction.Y = 1;

                _pos += speed * time * direction;

            }
        }
        // Get set position
        public Vector2 Pos
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public Matrix get_transformation(GraphicsDevice graphicsDevice)
        {
            _transform =       // Thanks to o KB o for this solution
              Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                         Matrix.CreateRotationZ(_rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 0)) *
                                         Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
            return _transform;
        }
    }
}
