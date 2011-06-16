using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.Services;

namespace MagicWorld.BlendInClasses
{
    class AimingAid : DrawableGameComponent
    {
        ContentManager content;
        SpriteBatch spriteBatch;

        Texture2D arrowTex;
        Texture2D circleTex;

        IPlayerService playerService;
        ICameraService cameraService;
        Vector2 origin;
        bool servicesAcquired;

        public AimingAid(Game game)
            : base(game)
        {
            content = game.Content;
            servicesAcquired = false;
        }

        protected override void LoadContent()
        {
            circleTex = content.Load<Texture2D>("AimingAid/aidCircle");
            arrowTex = content.Load<Texture2D>("AimingAid/aimArrow");

            origin = new Vector2(arrowTex.Width / 2, arrowTex.Height / 2);

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            playerService = (IPlayerService)Game.Services.GetService(typeof(IPlayerService));
            cameraService = (ICameraService)Game.Services.GetService(typeof(ICameraService));
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (playerService == null || cameraService == null)
            {
                playerService = (IPlayerService)Game.Services.GetService(typeof(IPlayerService));
                cameraService = (ICameraService)Game.Services.GetService(typeof(ICameraService));
            }
            else
                servicesAcquired = true;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (servicesAcquired)
            {
                if (playerService.IsCasting)
                {
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, cameraService.TransformationMatrix);
                    spriteBatch.Draw(circleTex, playerService.Position, null, Color.White, 0, origin, 1.0f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(arrowTex, playerService.Position, null, Color.White, (float)playerService.SpellAimAngle, origin, 1.0f, SpriteEffects.FlipVertically, 0f);
                    spriteBatch.End();
                }
            }
            base.Draw(gameTime);
        }
    }
}
