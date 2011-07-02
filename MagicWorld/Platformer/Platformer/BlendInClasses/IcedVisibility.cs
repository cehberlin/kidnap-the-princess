using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MagicWorld.Services;

namespace MagicWorld.BlendInClasses
{
    class IcedVisibility : DrawableGameComponent, IVisibility
    {
        Texture2D icedTexture;
        ContentManager content;
        SpriteBatch spriteBatch;
        ICameraService camera;
        List<IIcedVisibility> positions;

        public IcedVisibility(Game game)
            : base(game)
        {
            positions = new List<IIcedVisibility>();
            this.content = game.Content;
        }

        protected override void LoadContent()
        {
            icedTexture = content.Load<Texture2D>("frozen");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            camera = (ICameraService)Game.Services.GetService(typeof(ICameraService));
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            camera = (ICameraService)Game.Services.GetService(typeof(ICameraService));
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (camera != null)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, camera.TransformationMatrix);
                foreach (IIcedVisibility obj in positions)
                {
                    spriteBatch.Draw(icedTexture, obj.getDrawingArea(), Color.White);
                }
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        public void Add(IIcedVisibility obj)
        {
            if (!positions.Contains(obj))
                positions.Add(obj);
        }

        public void Remove(IIcedVisibility obj)
        {
            positions.Remove(obj);
        }

        public void Remove()
        {
            positions.Remove(positions[0]);
        }

        public object GetService(System.Type serviceType)
        {
            return this;
        }


        public void Clear()
        {
            positions.Clear();
        }
    }
}
