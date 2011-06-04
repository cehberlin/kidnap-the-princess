#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
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
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry mnuControls;
        MenuEntry mnuSound;      

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            // Create our menu entries.
            mnuControls = new MenuEntry("Controls");
            mnuSound = new MenuEntry("Sound");

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            mnuControls.Selected += ControlsMenuEntrySelected;
            mnuSound.Selected += SoundMenuEntrySelected;
            
            back.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(mnuControls);
            MenuEntries.Add(mnuSound);
            
            MenuEntries.Add(back);
        }

        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>
        void ControlsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            
        }

        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void SoundMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            
        }

        #endregion
    }
}
