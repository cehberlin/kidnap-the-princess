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
        MenuEntry mnuResolution;
        MenuEntry mnuEraseProfile;

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
            mnuResolution = new MenuEntry("Display");
            mnuEraseProfile = new MenuEntry("Erase Profile");

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            mnuControls.Selected += ControlsMenuEntrySelected;
            mnuSound.Selected += SoundMenuEntrySelected;
            mnuResolution.Selected += ResolutionMenuEntrySelected;
            mnuEraseProfile.Selected += EraseProfileEntrySelected;
            
            back.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(mnuControls);
            MenuEntries.Add(mnuSound);
            MenuEntries.Add(mnuResolution);
            MenuEntries.Add(mnuEraseProfile);
            
            MenuEntries.Add(back);
        }

        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>
        void ControlsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ControlsMenu(), e.PlayerIndex);
        }

        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void EraseProfileEntrySelected(object sender, PlayerIndexEventArgs e)
        {

            const string message = "Are you sure you want to erase your profile?\nIt will delete all your saved files.";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += EraseProfileConfirmed;

            ScreenManager.AddScreen(confirmExitMessageBox, e.PlayerIndex);

            
        }

        void EraseProfileConfirmed(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.EraseProfile();
            mnuEraseProfile.Text = "Erase Profile <Done>";
        }

        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void SoundMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new SoundMenuScreen(ScreenManager), e.PlayerIndex);
        }

        void ResolutionMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ResolutionMenuScreen(ScreenManager), e.PlayerIndex);
        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            bool a;

            ScreenManager.Game.SaveGameConfig();

            foreach (GameScreen screen in ScreenManager.GetScreens())
            {
                if (screen.GetType().Equals(typeof(MainMenuScreen)))
                    a=screen.IsActive;
            }
            ExitScreen();
        }

        #endregion
    }
}
