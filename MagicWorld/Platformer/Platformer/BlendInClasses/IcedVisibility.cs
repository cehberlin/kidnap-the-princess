using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MagicWorld.Services;
using MagicWorld.Audio;

namespace MagicWorld.BlendInClasses
{
    class IcedVisibility : DrawableGameComponent, IVisibility
    {
        Texture2D icedTexture;
        ContentManager content;
        SpriteBatch spriteBatch;
        ICameraService camera;
        IPlayerService playerService;
        IAudioService audioService;

        List<IIcedVisibility> positions;
        Effect effect;

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
            effect = content.Load<Effect>("HelperShader"); 
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (camera != null)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, camera.TransformationMatrix);
                if(playerService != null){
                    if (playerService.IsCasting)
                        effect.CurrentTechnique.Passes[0].Apply();
                }
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
            audioService.playSound(SoundType.icecrack);
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

        public void Init()
        {
            camera = (ICameraService)Game.Services.GetService(typeof(ICameraService));
            playerService = (IPlayerService)Game.Services.GetService(typeof(IPlayerService));
            audioService = (IAudioService)Game.Services.GetService(typeof(IAudioService));
        }
    }
}
