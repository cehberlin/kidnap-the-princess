#region File Description
//-----------------------------------------------------------------------------
// PlatformerGame.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;
using MagicWorld.StaticLevelContent;


namespace MagicWorld
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PlatformerGame : Microsoft.Xna.Framework.Game
    {
        private HelperClasses.Camera2d camera = new HelperClasses.Camera2d(600,800);

        // Resources for drawing.
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        
        // Global content.
        private SpriteFont hudFont;

        private Texture2D winOverlay;
        private Texture2D loseOverlay;
        private Texture2D diedOverlay;

        // Meta-level game state.
        private int levelIndex = -1;
        private Level level;
        private bool wasContinuePressed;

        // When the time remaining is less than the warning time, it blinks on the hud
        private static readonly TimeSpan WarningTime = TimeSpan.FromSeconds(30);

        // We store our input states so that we only poll once per frame, 
        // then we use the same input state wherever needed
        private GamePadState gamePadState;
        private KeyboardState keyboardState;
        
        // The number of levels in the Levels directory of our content. We assume that
        // levels in our content are 0-based and that all numbers under this constant
        // have a level file present. This allows us to not need to check for the file
        // or handle exceptions, both of which can add unnecessary time to level loading.
        private const int numberOfLevels = 1;

        public PlatformerGame()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.PreferredBackBufferWidth = 1200; looks not good
            //graphics.PreferredBackBufferHeight = 800;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load fonts
            hudFont = Content.Load<SpriteFont>("Fonts/Hud");

            // Load overlay textures
            winOverlay = Content.Load<Texture2D>("Overlays/you_win");
            loseOverlay = Content.Load<Texture2D>("Overlays/you_lose");
            diedOverlay = Content.Load<Texture2D>("Overlays/you_died");

            LoadNextLevel();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Handle polling for our input and handling high-level input
            HandleInput();

            // update our level, passing down the GameTime along with all of our input states
            level.Update(gameTime, keyboardState, gamePadState, Window.CurrentOrientation);

            camera.Pos = new Vector2 (level.Player.Position.X,level.Player.Position.Y-150);

            base.Update(gameTime);
        }

        private KeyboardState oldKeyboardState;
        private void HandleInput()
        {
            // get all of our input states
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);

            // Exit the game when back is pressed.
            if (gamePadState.Buttons.Back == ButtonState.Pressed)
                Exit();

            bool continuePressed =
                keyboardState.IsKeyDown(Player.JumpKey) || keyboardState.IsKeyDown(Player.JumpKeyAlternative) ||
                gamePadState.IsButtonDown(Player.JumpButton) ;

            // Perform the appropriate action to advance the game and
            // to get the player back to playing.
            if (!wasContinuePressed && continuePressed)
            {
                if (!level.Player.IsAlive)
                {
                    ReloadCurrentLevel();
                }
                else if (level.TimeRemaining == TimeSpan.Zero)
                {
                    if (level.ReachedExit)
                        LoadNextLevel();
                    else
                        ReloadCurrentLevel();
                }
                else if (level.ReachedExit)
                {
                    LoadNextLevel();
                }
            }

            //Options
            if (keyboardState.IsKeyUp(Player.FullscreenToggleKey) && oldKeyboardState.IsKeyDown(Player.FullscreenToggleKey))
            {
                this.graphics.ToggleFullScreen();
            }

            if (keyboardState.IsKeyUp(Player.DebugToggleKey) && oldKeyboardState.IsKeyDown(Player.DebugToggleKey))
            {
                GlobalValues.DEBUG = !GlobalValues.DEBUG;
            }
            if (keyboardState.IsKeyUp(Player.DEBUG_NO_MANA_COST) && oldKeyboardState.IsKeyDown(Player.DEBUG_NO_MANA_COST))
            {
                GlobalValues.DEBUG_NO_MANA_COST = !GlobalValues.DEBUG_NO_MANA_COST;
            }

            if (keyboardState.IsKeyUp(Player.DEBUG_NEXT_LEVEL) && oldKeyboardState.IsKeyDown(Player.DEBUG_NEXT_LEVEL))
            {
                level.OnExitReached();
            }

            if (keyboardState.IsKeyUp(Player.DEBUG_TOGGLE_GRAVITY_INFLUECE_ON_PLAYER) && oldKeyboardState.IsKeyDown(Player.DEBUG_TOGGLE_GRAVITY_INFLUECE_ON_PLAYER))
            {
                level.Player.nogravityHasInfluenceOnPlayer = !level.Player.nogravityHasInfluenceOnPlayer;
            }

            if (keyboardState.IsKeyUp(Player.ExitGameKey) && oldKeyboardState.IsKeyDown(Player.ExitGameKey))
            {
                this.Exit();
            }

            wasContinuePressed = continuePressed;
            oldKeyboardState = keyboardState;
        }

        private void LoadNextLevel()
        {
            // move to the next level
            levelIndex = (levelIndex + 1) % numberOfLevels;

            // Unloads the content for the current level before loading the next one.
            if (level != null)
                level.Dispose();

            // Load the level.
            level = new Level(Services, LevelLoaderFactory.getLevel(levelIndex));
        }

        private void ReloadCurrentLevel()
        {
            --levelIndex;
            LoadNextLevel();
        }

        /// <summary>
        /// Draws the game from background to foreground.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Immediate,
            BlendState.AlphaBlend,
            null,
            null,
            null,
            null,
            camera.get_transformation(graphics.GraphicsDevice));

            level.Draw(gameTime, spriteBatch);

            DrawHud();

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawHud()
        {
            Rectangle titleSafeArea = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);

            Vector2 center = new Vector2(camera._pos.X,camera._pos.Y);

            // Draw time remaining. Uses modulo division to cause blinking when the
            // player is running out of time.
            string timeString = "TIME: " + level.TimeRemaining.Minutes.ToString("00") + ":" + level.TimeRemaining.Seconds.ToString("00");
            Color timeColor;
            if (level.TimeRemaining > WarningTime ||
                level.ReachedExit ||
                (int)level.TimeRemaining.TotalSeconds % 2 == 0)
            {
                timeColor = Color.Yellow;
            }
            else
            {
                timeColor = Color.Red;
            }

            float stringpositionX = 0;
            float stringpositionTimeY = camera._pos.Y - camera.Height / 5;
            float stringpositionManaY = camera._pos.Y - camera.Height / 4;

            if (this.graphics.IsFullScreen)
            {
                stringpositionX = camera._pos.X - camera.Width * 3 / 5;
            }
            else
            {
                stringpositionX = camera._pos.X - camera.Width * 2 / 3;
            }

            DrawShadowedString(hudFont, timeString, new Vector2(stringpositionX, stringpositionTimeY), timeColor);
            level.Player.Mana.drawHud(spriteBatch, hudFont, new Vector2(stringpositionX, stringpositionManaY));
           
            // Determine the status overlay message to show.
            Texture2D status = null;
            if (level.TimeRemaining == TimeSpan.Zero)
            {
                if (level.ReachedExit)
                {
                    status = winOverlay;
                }
                else
                {
                    status = loseOverlay;
                }
            }

            if (level.Player.IsAlive && level.ReachedExit)
            {
                status = winOverlay;
            }
            else if (!level.Player.IsAlive)
            {
                status = diedOverlay;
            }

            if (status != null)
            {
                // Draw status message.
                Vector2 statusSize = new Vector2(status.Width, status.Height);
                spriteBatch.Draw(status, center - statusSize / 2, Color.White);
            }
        }

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
        }
    }
}
