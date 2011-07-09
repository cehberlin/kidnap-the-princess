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
    class LevelTransitionScreen : GameScreen
    {
        #region Fields

        string message;
        Texture2D gradientTexture;

        #endregion

        #region Events

        public event EventHandler<PlayerIndexEventArgs> Accepted;
   

        #endregion

        #region Initialization


        
        /// <summary>
        /// Constructor lets the caller specify whether to include the standard
        /// "A=ok, B=cancel" usage text prompt.
        /// </summary>
        public LevelTransitionScreen(string message)
            
        {
            this.message = message;
 
            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
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
            string tempString = "aaaaaaaaaaaaaaa\na\na\na\n";

            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            // Center the message text in the viewport.
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(tempString);
            Vector2 textPosition = (viewportSize - textSize) / 2;
            Vector2 vecIncrease = new Vector2(0, 30);

            

           

            // Fade the popup alpha during transitions.
            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin();            

            // Draw the message box text.
            spriteBatch.DrawString(font, message, textPosition, color,0,Vector2.Zero,1.0f,SpriteEffects.None,0);
            textPosition += vecIncrease*3;            
            spriteBatch.DrawString(font, "Your performance", textPosition, color, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            textPosition += vecIncrease;
            spriteBatch.DrawString(font, "Elapsed Time: " + ScreenManager.Game.GameData.Time.ToString("#0.0") + "s", textPosition, color, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            textPosition += vecIncrease;
            spriteBatch.DrawString(font, "Items collected: " + ScreenManager.Game.GameData.ItemsCollected.ToString(), textPosition, color, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            textPosition += vecIncrease;
            spriteBatch.DrawString(font, "Items available: " + ScreenManager.Game.GameData.TotalItems.ToString(), textPosition, color, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);


            spriteBatch.End();
        }


        #endregion
    }
}
