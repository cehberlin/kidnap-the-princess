#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;


#endregion

namespace MagicWorld
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class FilmScreen : GameScreen
    {                
        
        Video video;
        VideoPlayer player;
        Texture2D videoTexture;

       
        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public FilmScreen(ScreenManager screenManager)            
        {
            this.ScreenManager = screenManager;            
            
        }
        
        #endregion       

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;
            video = content.Load<Video>("Intro.wmv");
            player = new VideoPlayer();
        }      

       
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                        bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            if (player.State == MediaState.Stopped)
            {
                player.IsLooped = true;
                player.Play(video);
            }

        }
         

        public override void Draw(GameTime gameTime)
        {
            
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);
            Viewport viewPort = ScreenManager.GraphicsDevice.Viewport;

            // Only call GetTexture if a video is playing or paused
            if (player.State != MediaState.Stopped)
                videoTexture = player.GetTexture();

            // Drawing to the rectangle will stretch the 
            // video to fill the screen
            Rectangle screen = new Rectangle(viewPort.X,
                viewPort.Y,
                viewPort.Width,
                viewPort.Height);

            // Draw the video, if we have a texture to draw.
            if (videoTexture != null)
            {
                ScreenManager.SpriteBatch.Begin();
                ScreenManager.SpriteBatch.Draw(videoTexture, screen, Color.White);
                ScreenManager.SpriteBatch.End();
            }
            

            base.Draw(gameTime);
        }

        public override void HandleInput(InputState input)
        {

            PlayerIndex playerIndex;
            // We pass in our ControllingPlayer, which may either be null (to
            // accept input from any player) or a specific index. If we pass a null
            // controlling player, the InputState helper returns to us which player
            // actually provided the input. We pass that through to our Accepted and
            // Cancelled events, so they can tell which player triggered them.
            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                //player.State = MediaState.Stopped;
                player.Stop();
                ScreenManager.AddScreen(new BackgroundScreen(), null);
                ScreenManager.AddScreen(new MainMenuScreen(), null);
                ExitScreen();
            }
            
        }
       
        
    }
}
