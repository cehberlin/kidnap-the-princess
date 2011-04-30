using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace KidnapThePrincess
{
    public class Camera2d
    {
        protected float _zoom; // Camera Zoom
        public Matrix _transform; // Matrix Transform
        public Vector2 _pos; // Camera Position
        protected float _rotation; // Camera Rotation
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

        /// <summary>
        /// The area the camera covers.
        /// </summary>
        public Rectangle Area
        {
            get { return new Rectangle((int)_pos.X-width/2,(int) _pos.Y-height/2, width, height); }
        }
        
        /// <summary>
        /// Constructor for camera class
        /// </summary>
        /// <param name="height">Height of the clientbounds of the game</param>
        /// <param name="width">Width of the clientbounds of the game</param>
        public Camera2d(int width,int height)
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
