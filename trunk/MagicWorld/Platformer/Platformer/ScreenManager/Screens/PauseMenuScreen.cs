#region File Description
//-----------------------------------------------------------------------------
// PauseMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using System.Collections.Generic;
#endregion

namespace MagicWorld
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class PauseMenuScreen : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public PauseMenuScreen()
            : base("Paused")
        {
            // Create our menu entries.
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");
            MenuEntry restartLevelMenuEntry = new MenuEntry("Restart Level");
            MenuEntry optionsGameMenuEntry = new MenuEntry("Options");
            
            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;
            restartLevelMenuEntry.Selected += RestartLevelMenuEntrySelected;
            optionsGameMenuEntry.Selected += OptionsMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(restartLevelMenuEntry);
            MenuEntries.Add(optionsGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {            
            //List<GameScreen> screens=new List<GameScreen> (ScreenManager.GetScreens()); 

            //find the current Gameplay screen and delete it
           // for (int i = 0; i < screens.Count; i++)
           // {
           //     if ((screens[i]) is GameplayScreen)
           //     {
           //         screens[i].ExitScreen();
                    //ScreenManager.RemoveScreen(screens[i]);
           //     }                
          //  }

            GameplayScreen gameScreen;

            gameScreen = (GameplayScreen)ScreenManager.GetPlayScreen();
            gameScreen.UnloadContent();
            gameScreen.ExitScreen();
                
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen());
        }

        void RestartLevelMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            
            const string message = "Are you sure you want to restart the level?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += RestartLevelMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);

            
        }

        void RestartLevelMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {           

            GameplayScreen gameScreen = (GameplayScreen)ScreenManager.GetPlayScreen();
            
            gameScreen.ReloadCurrentLevel();

            ExitScreen();          
           
            
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }


        #endregion
    }
}
