using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MagicWorld.StaticLevelContent;
using MagicWorld.Constants;
using MagicWorld.Controls;
using MagicWorld.HelperClasses;
using Microsoft.Xna.Framework.Media;
using MagicWorld.Services;
using System.Diagnostics;

namespace MagicWorld
{
    /// <summary>
    /// This screen implements the actual game logic. 
    /// </summary>
    class GameplayScreen : GameScreen, IServiceProvider
    {
        ICameraService camera;
        #region Fields

        ContentManager content;
        // Meta-level game state.
        private int levelIndex = 0;
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
        //private const int numberOfLevels = 1;

        //check if the level was already added to a service
        bool levelAddedToService = false;

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
            camera = (ICameraService)ScreenManager.Game.Services.GetService(typeof(ICameraService));

            if(ScreenManager.Game.Services.GetService(typeof(GameplayScreen))==null){
                ScreenManager.Game.Services.AddService(typeof(GameplayScreen), this);
            }
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            
            LoadLevel(1);

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
            if (level != null)
                level.Dispose();
            content.Unload();
            ScreenManager.Game.Services.RemoveService(typeof(Level));
            ExitScreen();
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

            if (!coveredByOtherScreen)
            {
                if (!level.Player.IsAlive)
                {
                    ReloadCurrentLevel();
                }
                else if (level.ReachedExit && level.CollectedIngredients.Count >= level.NeededIngredients)
                {
                    LoadNextLevel();
                }

                // update our level, passing down the GameTime along with all of our input states
                level.Update(gameTime, keyboardState, gamePadState, ScreenManager.Game.Window.CurrentOrientation);

                camera.Position = new Vector2(level.Player.Position.X, level.Player.Position.Y - 150);
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

            IPlayerControl control = PlayerControlFactory.GET_INSTANCE().getPlayerControl();
            bool continuePressed = keyboardState.IsKeyDown(control.Keys_Up) ||
                gamePadState.IsButtonDown(control.GamePad_Up);

            // Perform the appropriate action to advance the game and
            // to get the player back to playing.
            //if (!wasContinuePressed && continuePressed)
            //{
            //    if (!level.Player.IsAlive)
            //    {
            //        ReloadCurrentLevel();
            //    }
            //    else if (level.TimeRemaining == TimeSpan.Zero)
            //    {
            //        if (level.ReachedExit)
            //            LoadNextLevel();
            //        else
            //            ReloadCurrentLevel();
            //    }
            //    else if (level.ReachedExit)
            //    {
            //        if(level.Ingredients.Count <= level.NeededIngredients)
            //        {

            //        }
            //        else
            //        {
            //            LoadNextLevel();
            //        }

            //    }
            //}

            //Options
            if (keyboardState.IsKeyUp(GameOptionsControls.ToggleSound) && oldKeyboardState.IsKeyDown(GameOptionsControls.ToggleSound))
            {
                MediaPlayer.IsMuted = !MediaPlayer.IsMuted;
            }


            if (keyboardState.IsKeyUp(GameOptionsControls.FullscreenToggleKey) && oldKeyboardState.IsKeyDown(GameOptionsControls.FullscreenToggleKey))
            {
                ScreenManager.Graphics.ToggleFullScreen();
            }

#if DEBUG


            if (keyboardState.IsKeyUp(GameOptionsControls.DEBUG_SHOW_BOUNDINGS) && oldKeyboardState.IsKeyDown(GameOptionsControls.DEBUG_SHOW_BOUNDINGS))
            {
                DebugValues.DEBUG = !DebugValues.DEBUG;
            }
            if (keyboardState.IsKeyUp(GameOptionsControls.DEBUG_NO_MANA_COST) && oldKeyboardState.IsKeyDown(GameOptionsControls.DEBUG_NO_MANA_COST))
            {
                DebugValues.DEBUG_NO_MANA_COST = !DebugValues.DEBUG_NO_MANA_COST;
            }

            if (keyboardState.IsKeyUp(GameOptionsControls.DEBUG_NEXT_LEVEL) && oldKeyboardState.IsKeyDown(GameOptionsControls.DEBUG_NEXT_LEVEL))
            {
                Debug.WriteLine("DEBUG: load level ");
                LoadNextLevel();
            }
            if (keyboardState.IsKeyUp(GameOptionsControls.DEBUG_PREV_LEVEL) && oldKeyboardState.IsKeyDown(GameOptionsControls.DEBUG_PREV_LEVEL))
            {
                Debug.WriteLine("DEBUG: Last level ");
                levelIndex--;
                if (levelIndex < 1)
                {
                    levelIndex = 1;
                }
                LoadLevel(levelIndex);
            }


            if (keyboardState.IsKeyUp(GameOptionsControls.DEBUG_RELOAD_LEVEL) && oldKeyboardState.IsKeyDown(GameOptionsControls.DEBUG_RELOAD_LEVEL))
            {
                level.OnExitReached();
            }

            if (keyboardState.IsKeyUp(GameOptionsControls.DEBUG_TOGGLE_GRAVITY_INFLUECE_ON_PLAYER) && oldKeyboardState.IsKeyDown(GameOptionsControls.DEBUG_TOGGLE_GRAVITY_INFLUECE_ON_PLAYER))
            {
                level.Player.nogravityHasInfluenceOnPlayer = !level.Player.nogravityHasInfluenceOnPlayer;
            }

            if (keyboardState.IsKeyDown(Keys.N))
            {
                camera.Zoom += -0.1f;
            }
            if (keyboardState.IsKeyDown(Keys.M))
            {
                camera.Zoom += 0.1f;
            }

#endif

            wasContinuePressed = continuePressed;
            oldKeyboardState = keyboardState;
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Immediate,
            BlendState.AlphaBlend,
            null,
            null,
            null,
            null,
            camera.TransformationMatrix);

            level.Draw(gameTime, ScreenManager.SpriteBatch);

            ScreenManager.SpriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        #region level
        public void LoadLevel(int num)
        {
            levelIndex = num;
            Debug.WriteLine("load level " + num);
            // Unloads the content for the current level before loading the next one.
            if (level != null)
                level.Dispose();

            // Load the level.
            level = new Level(ScreenManager.Game.Services, LevelLoaderFactory.getLevel(num), ScreenManager.Game);
            // if (!levelAddedToService)
            // {
            ScreenManager.Game.Services.RemoveService(typeof(Level));
            ScreenManager.Game.Services.AddService(typeof(Level), level);
            levelAddedToService = true;

            //actualize information to save the game
            ScreenManager.Game.GameData.Level = num;
            ScreenManager.Game.GameData.ItemsCollected = 0;
            ScreenManager.Game.GameData.TotalItems = level.MaxIngredientsCount;
            ScreenManager.Game.GameData.Completed = "Failed";
            ScreenManager.Game.SaveGame(num);

            // }
        }
        private void LoadNextLevel()
        {
            //save the game

            ScreenManager.Game.GameData.Level = levelIndex;
            ScreenManager.Game.GameData.ItemsCollected = level.CollectedIngredients.Count;            
            ScreenManager.Game.GameData.Completed = "Accomplished";
            ScreenManager.Game.SaveGame(levelIndex);

            LoadLevel(levelIndex + 1);
        }

        public void ReloadCurrentLevel()
        {
            LoadLevel(levelIndex);
        }
        #endregion

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color)
        {
            ScreenManager.SpriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            ScreenManager.SpriteBatch.DrawString(font, value, position, color);
        }

        #region IServiceProvider Member

        public object GetService(Type serviceType)
        {
            return this;
        }

        #endregion
    }

        #endregion

}
