using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld
{
    class ResolutionMenuScreen : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public ResolutionMenuScreen()
            : base("Options")
        {
            // Create our menu entries.
            MenuEntry mnu800x600 = new MenuEntry("800X600");
            MenuEntry mnu1024x768 = new MenuEntry("1024X768");
            MenuEntry mnu1152x864 = new MenuEntry("1152X864");
            MenuEntry mnu1366x768 = new MenuEntry("1366X768");

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            mnu800x600.Selected  += ResolutionMenu800x600EntrySelected;
            mnu1024x768.Selected += ResolutionMenu1024x768EntrySelected;
            mnu1152x864.Selected += ResolutionMenu1152x864EntrySelected;
            mnu1366x768.Selected += ResolutionMenu1366x768EntrySelected;

            back.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(mnu800x600);
            MenuEntries.Add(mnu1024x768);
            MenuEntries.Add(mnu1152x864);
            MenuEntries.Add(mnu1366x768);
            
            MenuEntries.Add(back);
        }

        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>
        void ResolutionMenu800x600EntrySelected(object sender, PlayerIndexEventArgs e, int width, int height)
        {
            ScreenManager.setScreenResolution(800, 600);
        }

        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void ResolutionMenu1024x768EntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.setScreenResolution(1024, 768);
        }

        void ResolutionMenu1152x864EntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.setScreenResolution(1152,864);
        }

        void ResolutionMenu1366x768EntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.setScreenResolution(1366, 768);
        }

        #endregion
    }
}
