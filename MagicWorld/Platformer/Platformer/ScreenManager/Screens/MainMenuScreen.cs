#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
#endregion

namespace MagicWorld
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {

        string[] files;
        bool filesLoaded = false;
        int selectedFile = 0;
        MenuEntry playGameMenuEntry;

        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("Main Menu")
        {
            // Create our menu entries.
            playGameMenuEntry = new MenuEntry("New Game Level1");            
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

        #region Handle Input
        public override void HandleInput(InputState input)
        {
            if (!filesLoaded)
            {
                LoadFiles();
                filesLoaded = true;
            }

            //check the selection of rigth and left when on "New game" menu
            if (input.IsMenuRight(ControllingPlayer))
            {
                if (SelectedEntry == 0) //New game
                {
                    if (files.Length > 0)
                    {
                        selectedFile++;
                        if (selectedFile > (files.Length - 1))
                        {
                            selectedFile = 0;
                        }
                        ScreenManager.Game.LoadGame(selectedFile + 1);
                        playGameMenuEntry.Text = "New Game " + files[selectedFile];
                        ShowGameInfo();
                    }

                }
            }

            //check the selection of rigth and left when on "New game" menu
            if (input.IsMenuLeft(ControllingPlayer))
            {
                if (SelectedEntry == 0) //New game
                {
                    if (files.Length > 0)
                    {
                        selectedFile--;
                        if (selectedFile < 0)
                        {
                            selectedFile = files.Length - 1;
                        }
                        ScreenManager.Game.LoadGame(selectedFile + 1);
                        playGameMenuEntry.Text = "New Game " + files[selectedFile];
                    }
                }
            }

            base.HandleInput(input);
        }


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            
            GameplayScreen gameScreen=new GameplayScreen();
            //LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, gameScreen);

            

            ScreenManager.AddScreen(gameScreen, e.PlayerIndex);
            //load the level      
            gameScreen.LoadContent();
            gameScreen.LoadLevel(selectedFile+1);//selectedfile is always one number less levelnumber            
            ScreenManager.Game.ResetElapsedTime();
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

        private void ShowGameInfo()
        {

        }

        #endregion

        #region load files
        /// <summary>
        /// Load saved files name to a list
        /// </summary>
        private void LoadFiles()
        {
            files=ScreenManager.Game.GetSavedFiles();
            if (files.Length > 1)
            {
                playGameMenuEntry.Text = "New Game " + files[0];
            }
            
        }
        #endregion
    }
}
