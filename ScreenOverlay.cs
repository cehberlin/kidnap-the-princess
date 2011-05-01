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
    /// <summary>
    /// game component for a simple text overlay
    /// </summary>
    public class ScreenOverlay : DrawableGameComponent
    {
        #region Fields and Properties

        private ContentManager content;
        private SpriteBatch spriteBatch;

        Game1 game;

        // For font rendering
        private int fontSpacing = 3;
        private SpriteFont font;
        private SpriteFont smallFont;

        // Some strings and positioning data
        const string xnaString = "Kidnap the Princess";
        const string fpsString = "FPS: {0}";
        const string pointsString = "Points: {0:F0}";
        const string startString = "Start \nPress Enter to continue";
        const string gameoverString = "GAMEOVER \nPress ENTER to continue";
        const string winString = "CONGRATULATION \nPress ENTER to continue";


        Vector2 xnaStringPos;
        Vector2 xnaStringOrigin;
        Vector2 titleSafeUpperLeft;
        Vector2 pointsPos;
        Vector2 pausePos;
        Vector2 gameoverPos;
        Vector2 beginPos;
        Vector2 winPos;

        // For the frame rate counter
        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        // for detecting the visible region on the xbox 360 (this is set from Sandbox.cs)
        Rectangle titleSafe;
        /// <summary>
        /// The title safe rectangle used for both the game and the text overlay
        /// </summary>
        public Rectangle TitleSafe
        {
            get { return titleSafe; }
            set { titleSafe = value; }
        }


        Texture2D outlineTex;
        Rectangle top;
        Rectangle bottom;
        Rectangle left;
        Rectangle right;
        int borderWidth = 3;
        #endregion

        #region Construction and Initialization

        public ScreenOverlay(Game1 game)
            : base(game)
        {
            this.game = game;
            content = new ContentManager(game.Services);
            content.RootDirectory = "Content";
        }

        public override void Initialize()
        {

            // The center of the string
            xnaStringPos = new Vector2(TitleSafe.Width - 10, TitleSafe.Height);
            titleSafeUpperLeft = new Vector2(TitleSafe.X + 10, TitleSafe.Y + 5);

            pointsPos = new Vector2(TitleSafe.X + 10, TitleSafe.Y + 75);
            gameoverPos = game.Center + new Vector2(-320, 0);
            pausePos = game.Center + new Vector2(-240, 0);
            beginPos = game.Center + new Vector2(-320, 0);
            winPos = gameoverPos;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Load font and set spacing
            font = content.Load<SpriteFont>("Fonts\\Font");
            font.Spacing = fontSpacing;
            xnaStringOrigin = font.MeasureString(xnaString);
            smallFont = content.Load<SpriteFont>("Fonts\\FontSmall");
            outlineTex = content.Load<Texture2D>("white");

            top = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, borderWidth);
            left = new Rectangle(0, 0, borderWidth, game.GraphicsDevice.Viewport.Height);
            right = new Rectangle(game.GraphicsDevice.Viewport.Width - borderWidth, 0, borderWidth, game.GraphicsDevice.Viewport.Height);
            bottom = new Rectangle(0, game.GraphicsDevice.Viewport.Height-borderWidth, game.GraphicsDevice.Viewport.Width, borderWidth);
        }


        protected override void UnloadContent()
        {
            content.Unload();
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            // Update the framerate timer
            elapsedTime += gameTime.ElapsedGameTime;

            // Update the frame counter every second
            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }

        #endregion

        #region Rendering

        public override void Draw(GameTime gameTime)
        {
            // Draw text
            spriteBatch.Begin();

            // Font color
            Color fontColor = Color.DarkBlue;

            // Draw the lower right string
            spriteBatch.DrawString(font, xnaString, xnaStringPos, fontColor,
                0, xnaStringOrigin, 1.0f, SpriteEffects.None, 0.5f);

            // Update the framerate counter
            frameCounter++;

            if (game.StateMachine.Status == GameState.State.RUN)
            {

                // Format data strings
                string fps = string.Format(fpsString, frameRate);

                // Draw data strings
                spriteBatch.DrawString(font, string.Format(fpsString, frameRate),
                    titleSafeUpperLeft, fontColor, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);


                spriteBatch.DrawString(smallFont, string.Format(pointsString, game.StateMachine.Points),
                pointsPos, fontColor, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);

            }
            else if (game.StateMachine.Status == GameState.State.GAMEOVER)
            {
                spriteBatch.DrawString(font, gameoverString,
                gameoverPos, Color.Red, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
            }
            else if (game.StateMachine.Status == GameState.State.START)
            {
                spriteBatch.DrawString(font, startString,
                beginPos, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
            }
            else if (game.StateMachine.Status == GameState.State.WIN)
            {
                spriteBatch.DrawString(font, winString,
                winPos, Color.Yellow, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
            }
            //Draw screen outline
            spriteBatch.Draw(outlineTex, top, Color.White);
            spriteBatch.Draw(outlineTex, left, Color.White);
            spriteBatch.Draw(outlineTex, right, Color.White);
            spriteBatch.Draw(outlineTex,bottom,Color.White);
            spriteBatch.End();
        }

        #endregion
    }
}