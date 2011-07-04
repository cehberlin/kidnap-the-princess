using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace MagicWorld
{
    class SoundMenuScreen : MenuScreen
    {
        #region Initialization

        bool playingBackMusic=false;
        MenuEntry mnuPlayBackGroundMusic;
        /// <summary>
        /// Constructor.
        /// </summary>
        public SoundMenuScreen(ScreenManager screenManager)
            : base("Options")
        {
            // Create our menu entries.
            playingBackMusic = screenManager.Game.GameStatus.PlayBackGroundMusic;
            if (playingBackMusic)
            {
                mnuPlayBackGroundMusic = new MenuEntry("Background Music <On>");            
            }
            else
            {
                mnuPlayBackGroundMusic = new MenuEntry("Background Music <Off>");
            }

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            mnuPlayBackGroundMusic.Selected += PlayBackgroundMusic;          
            back.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(mnuPlayBackGroundMusic);
            
            MenuEntries.Add(back);
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>
        void PlayBackgroundMusic(object sender, PlayerIndexEventArgs e)
        {
            
            //Known issue that you get exceptions if you use Media PLayer while connected to your PC
            //See http://social.msdn.microsoft.com/Forums/en/windowsphone7series/thread/c8a243d2-d360-46b1-96bd-62b1ef268c66
            //Which means its impossible to test this from VS.
            //So we have to catch the exception and throw it away
            if (playingBackMusic)
            {
                mnuPlayBackGroundMusic.Text = "Background Music <Off>";
                MediaPlayer.Stop();
            }
            else
            {
                mnuPlayBackGroundMusic.Text = "Background Music <On>";
                try
                {
                    MediaPlayer.IsMuted = playingBackMusic;
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(ScreenManager.ContentManager.Load<Song>("Sounds/Backgroundmusic"));
                }
                catch { }
            }

            playingBackMusic=!playingBackMusic;
            ScreenManager.Game.GameStatus.PlayBackGroundMusic = playingBackMusic;

        }
       

        #endregion
    }
}
