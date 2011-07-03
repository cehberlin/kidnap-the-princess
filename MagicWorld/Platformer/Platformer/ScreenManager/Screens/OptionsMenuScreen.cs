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

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            mnuControls.Selected += ControlsMenuEntrySelected;
            mnuSound.Selected += SoundMenuEntrySelected;
            mnuResolution.Selected += ResolutionMenuEntrySelected;
            
            back.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(mnuControls);
            MenuEntries.Add(mnuSound);
            MenuEntries.Add(mnuResolution);
            
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
        void SoundMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            
        }

        void ResolutionMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ResolutionMenuScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            bool a;
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
