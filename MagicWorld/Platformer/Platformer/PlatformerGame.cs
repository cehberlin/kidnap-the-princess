using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;
using MagicWorld.StaticLevelContent;
using MagicWorld.Gleed2dLevelContent;
using MagicWorld.HUDClasses;

namespace MagicWorld
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PlatformerGame : Microsoft.Xna.Framework.Game
    {

        #region properties
         
        ScreenManager screenManager;
        HUD hud;
        // Resources for drawing.
        public GraphicsDeviceManager graphics;
        
        #endregion

        public PlatformerGame()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.PreferredBackBufferWidth = 1200; looks not good
            //graphics.PreferredBackBufferHeight = 800;
            
            //Content.RootDirectory = "Content";
            // Create the screen manager component.
            screenManager = new ScreenManager(this);
            
            //with this entry the framework call the update itself
            //don't need to add in update method
            Components.Add(screenManager);
            Services.AddService(typeof(ScreenManager), screenManager);

            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);
        }

        protected override void Initialize()
        {
            hud = new HUD(this);
            Components.Add(hud);
            base.Initialize();
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game from background to foreground.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            
            base.Draw(gameTime);
        }
        
    }
        
}
