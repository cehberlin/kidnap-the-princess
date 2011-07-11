using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld
{
    class ResolutionMenuScreen : MenuScreen
    {
        #region Initialization

        MenuEntry mnuFullScreen;
        bool toogleScreen = false;
        /// <summary>
        /// Constructor.
        /// </summary>
        public ResolutionMenuScreen(ScreenManager screenManager)
            : base("Options")
        {
            // Create our menu entries.       
            toogleScreen = screenManager.Game.GameStatus.FullScreenMode;
            if (toogleScreen )
                mnuFullScreen = new MenuEntry("Full Screen <On>");
            else
                mnuFullScreen = new MenuEntry("Full Screen <Off>");


            MenuEntry mnu800x600 = new MenuEntry("800X600");
            MenuEntry mnu1024x768 = new MenuEntry("1024X768");
            MenuEntry mnu1152x864 = new MenuEntry("1152X864");
            MenuEntry mnu1366x768 = new MenuEntry("1366X768");
            MenuEntry mnu1280x800 = new MenuEntry("1280x800");
            MenuEntry mnu1680x1050 = new MenuEntry("1680x1050");

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            mnuFullScreen.Selected += ResolutionFullScreenEntrySelected;
            mnu800x600.Selected  += ResolutionMenu800x600EntrySelected;
            mnu1024x768.Selected += ResolutionMenu1024x768EntrySelected;
            mnu1152x864.Selected += ResolutionMenu1152x864EntrySelected;
            mnu1366x768.Selected += ResolutionMenu1366x768EntrySelected;
            mnu1280x800.Selected += ResolutionMenu1280x800EntrySelected;
            mnu1680x1050.Selected += ResolutionMenu1680x1050EntrySelected;

            back.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(mnuFullScreen);
            MenuEntries.Add(mnu800x600);
            MenuEntries.Add(mnu1024x768);
            MenuEntries.Add(mnu1152x864);
            MenuEntries.Add(mnu1280x800);
            MenuEntries.Add(mnu1366x768);
            MenuEntries.Add(mnu1680x1050);

            
            MenuEntries.Add(back);

            
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>
        void ResolutionFullScreenEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            toogleScreen = !toogleScreen;
            ScreenManager.Graphics.ToggleFullScreen();
            if (toogleScreen)
            {
                mnuFullScreen.Text = "Full Screen <On>";                
            }
            else
            {                
                mnuFullScreen.Text = "Full Screen <Off>";
            }
            
            ScreenManager.Game.GameStatus.FullScreenMode = toogleScreen;
        }
        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>
        void ResolutionMenu800x600EntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.setScreenResolution(800, 600);
            ScreenManager.Game.GameStatus.Resolution.X = 800;
            ScreenManager.Game.GameStatus.Resolution.Y = 600;
        }

        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void ResolutionMenu1024x768EntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.setScreenResolution(1024, 768);
            ScreenManager.Game.GameStatus.Resolution.X = 1024;
            ScreenManager.Game.GameStatus.Resolution.Y = 768;
        }

        void ResolutionMenu1152x864EntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.setScreenResolution(1152,864);
            ScreenManager.Game.GameStatus.Resolution.X = 1152;
            ScreenManager.Game.GameStatus.Resolution.Y = 864;
        }

        void ResolutionMenu1366x768EntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.setScreenResolution(1366, 768);
            ScreenManager.Game.GameStatus.Resolution.X = 1366;
            ScreenManager.Game.GameStatus.Resolution.Y = 768;
        }

        void ResolutionMenu1280x800EntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.setScreenResolution(1280, 800);
            ScreenManager.Game.GameStatus.Resolution.X = 1280;
            ScreenManager.Game.GameStatus.Resolution.Y = 800;
        }

        void ResolutionMenu1680x1050EntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.setScreenResolution(1680, 1050);
            ScreenManager.Game.GameStatus.Resolution.X = 1680;
            ScreenManager.Game.GameStatus.Resolution.Y = 1050;
        }

        #endregion
    }
}
