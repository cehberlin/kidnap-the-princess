#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MagicWorld.StaticLevelContent;
using MagicWorld.Gleed2dLevelContent;
using Microsoft.Xna.Framework.Input.Touch;
#endregion

namespace MagicWorld
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;        
        private HelperClasses.Camera2d camera = new HelperClasses.Camera2d(600, 800);

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
        private KeyboardState oldKeyboardState;


        // The number of levels in the Levels directory of our content. We assume that
        // levels in our content are 0-based and that all numbers under this constant
        // have a level file present. This allows us to not need to check for the file
        // or handle exceptions, both of which can add unnecessary time to level loading.
        private const int numberOfLevels = 1;

        float pauseAlpha;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);            
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            // Load fonts
            hudFont = content.Load<SpriteFont>("Fonts/Hud");

            // Load overlay textures
            winOverlay = content.Load<Texture2D>("Overlays/you_win");
            loseOverlay = content.Load<Texture2D>("Overlays/you_lose");
            diedOverlay = content.Load<Texture2D>("Overlays/you_died");

            LoadNextLevel();

            //Needed for Testing TODO
            //Gleed2dLevelLoader level = Gleed2dLevelLoader.FromFile("level1.xml", Content);

            //gameFont = content.Load<SpriteFont>("gamefont");

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // update our level, passing down the GameTime along with all of our input states
            level.Update(gameTime, keyboardState, gamePadState, ScreenManager.Game.Window.CurrentOrientation);

            camera.Pos = new Vector2(level.Player.Position.X, level.Player.Position.Y - 150);




            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                // Apply some random jitter to make the enemy move around.
                
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            keyboardState = input.CurrentKeyboardStates[playerIndex];
            gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
                       

            bool continuePressed =keyboardState.IsKeyDown(Player.JumpKey) || keyboardState.IsKeyDown(Player.JumpKeyAlternative) ||
                gamePadState.IsButtonDown(Player.JumpButton);

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
                ScreenManager.Graphics.ToggleFullScreen();
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
                        
            wasContinuePressed = continuePressed;
            oldKeyboardState = keyboardState;

        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            //ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,Color.CornflowerBlue, 0, 0);

            // Our player and enemy are both actually just text strings.            

            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Immediate,
            BlendState.AlphaBlend,
            null,
            null,
            null,
            null,
            camera.get_transformation(ScreenManager.Graphics.GraphicsDevice));

            level.Draw(gameTime, ScreenManager.SpriteBatch);

            DrawHud();

            ScreenManager.SpriteBatch.End();
            

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        #region level
        private void LoadNextLevel()
        {
            // move to the next level
            levelIndex = (levelIndex + 1) % numberOfLevels;

            // Unloads the content for the current level before loading the next one.
            if (level != null)
                level.Dispose();

            // Load the level.
            level = new Level(ScreenManager.Game.Services, LevelLoaderFactory.getLevel(levelIndex));
        }

        private void ReloadCurrentLevel()
        {
            --levelIndex;
            LoadNextLevel();
        }
        #endregion


        private void DrawHud()
        {
            Rectangle titleSafeArea = ScreenManager.Game.GraphicsDevice.Viewport.TitleSafeArea;
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

            if (ScreenManager.Graphics.IsFullScreen)
            {
                stringpositionX = camera._pos.X - camera.Width * 3 / 5;
            }
            else
            {
                stringpositionX = camera._pos.X - camera.Width * 2 / 3;
            }

            DrawShadowedString(hudFont, timeString, new Vector2(stringpositionX, stringpositionTimeY), timeColor);
            level.Player.Mana.drawHud(ScreenManager.SpriteBatch, hudFont, new Vector2(stringpositionX, stringpositionManaY));
           
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
                ScreenManager.SpriteBatch.Draw(status, center - statusSize / 2, Color.White);
            }
        }

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color)
        {
            ScreenManager.SpriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            ScreenManager.SpriteBatch.DrawString(font, value, position, color);
        }
    }

        #endregion
    
}
