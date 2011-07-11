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
    class MainMenuScreen : MenuScreen
    {                
        MenuEntry playGameMenuEntry;
        //Video video;
        //VideoPlayer player;
        //Texture2D videoTexture;

       
        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("")
        {
            // Create our menu entries.
            playGameMenuEntry = new MenuEntry("Play Game");            
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");
            

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;            
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;
            

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);            
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
           
            
        }
        
        #endregion       

        public override void LoadContent()
        {
            //ContentManager content = ScreenManager.Game.Content;
            //Texture2D teste = content.Load<Texture2D>("frozen");

            //video = content.Load<Video>("Video/Intro.wmv");
            //player = new VideoPlayer();

        }      

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new LoadGameScreen(this.ScreenManager), e.PlayerIndex);
        }        

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit this sample?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        /*
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
         * */

        public override void Draw(GameTime gameTime)
        {
            /*
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
            */


            base.Draw(gameTime);
        }

       
        
    }
}
