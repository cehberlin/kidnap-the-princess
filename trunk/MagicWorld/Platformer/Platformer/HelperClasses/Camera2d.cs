using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MagicWorld.Services;
using IndependentResolutionRendering;

namespace MagicWorld.HelperClasses
{
    public class Camera2d : GameComponent, ICameraService
    {
        public Camera2d(Game game)
            : base(game)
        {

            zoom = 0.55f;
            game.Services.AddService(typeof(ICameraService), this);
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public Matrix TransformationMatrix
        {
            get
            {
                return  Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                    Resolution.getTransformationMatrix() *
                    Matrix.CreateRotationZ(rotation) *
                    Matrix.CreateScale(new Vector3(zoom, zoom, 0)) *
                    Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
            }
        }
        Vector2 position;
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        float zoom;
        public float Zoom
        {
            get
            {
                return zoom;
            }
            set
            {
                zoom = value;
            }
        }
        float rotation;
        public float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }
        public GraphicsDevice graphicsDevice
        {
            get { return getGraphicsDevice(); }
        }
        private GraphicsDevice getGraphicsDevice()
        {
            IGraphicsDeviceService graphicsDeviceService = (IGraphicsDeviceService)Game.Services.GetService(typeof(IGraphicsDeviceService));
            return graphicsDeviceService.GraphicsDevice;
        }
        public object GetService(Type serviceType)
        {
            return this;
        }


        public void Init()
        {
            throw new NotImplementedException();
        }
    }
}
