#region Using Statements
using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MagicWorld
{
    /// <summary>
    /// A popup message box screen, used to display "are you sure?"
    /// confirmation messages.
    /// </summary>
    class CreditsScreen : GameScreen
    {
        #region Fields

        string message;
        Texture2D gradientTexture;
        bool showMessage = false;
        Vector2 pos;

        #endregion

        #region Events

        public event EventHandler<PlayerIndexEventArgs> Accepted;
   

        #endregion

        #region Initialization


        
        /// <summary>
        /// Constructor lets the caller specify whether to include the standard
        /// "A=ok, B=cancel" usage text prompt.
        /// </summary>
        public CreditsScreen(ScreenManager screenManager)
            
        {
            message = "Magic World\n";            
            message += "\n\nMade in Berlin";            
            message += "\nGame Programming";
            message += "\nTU-Berlin";
            message += "\n\nCredits";
            message += "\n\n Developers";
            message += "\n\nChristopher Hrabia";
            message += "\nPascal Schwenke";
            message += "\nJohn Carlo";
            message += "\nMarian Volk";
            message += "\nAmauri Albuquerque";
                        
            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
            Viewport viewport = screenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            pos = new Vector2(0,viewport.Height);
        }


        /// <summary>
        /// Loads graphics content for this screen. This uses the shared ContentManager
        /// provided by the Game class, so the content will remain loaded forever.
        /// Whenever a subsequent MessageBoxScreen tries to load this same content,
        /// it will just get back another reference to the already loaded data.
        /// </summary>
        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            gradientTexture = content.Load<Texture2D>("MenuScreen/gradient");
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Responds to user input, accepting or cancelling the message box.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;

            if (!showMessage)
            {
                return;
            }

            if (input.IsContinuePress(ControllingPlayer,out playerIndex))
            {
                if (Accepted != null)
                {
                    ScreenManager.RemoveScreen(this);
                    Accepted(this, null);
                    
                }
            }
        }


        #endregion

        #region Draw


        /// <summary>
        /// Draws the message box.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            // Center the message text in the viewport.
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(message);
            Vector2 textPosition = (viewportSize - textSize) / 2;
            Vector2 vecIncrease = new Vector2(0, 1);

            // Fade the popup alpha during transitions.
            Color color = Color.White * TransitionAlpha;

            //update the position

            if (!showMessage)
            {
                pos.X = textPosition.X;
                pos.Y -= vecIncrease.Y * (float)(gameTime.ElapsedGameTime.Milliseconds*0.04);

                if (pos.Y + textSize.Y < 0)
                {
                    showMessage = true;
                }
            }
            else            
            {
                pos = textPosition;
                message = "Press any key.";
            }

            spriteBatch.Begin();            

            // Draw the message box text.
            spriteBatch.DrawString(font, message, pos, color,0,Vector2.Zero,1.0f,SpriteEffects.None,0);
            
            spriteBatch.End();
        }


        #endregion
    }
}
