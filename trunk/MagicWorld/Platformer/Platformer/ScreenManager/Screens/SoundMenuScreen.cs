using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MagicWorld.Audio;

namespace MagicWorld
{
    class SoundMenuScreen : MenuScreen
    {
        #region Initialization

        bool playingBackMusic = false;
        MenuEntry mnuPlayBackGroundMusic;
        public IAudioService audioService;
        /// <summary>
        /// Constructor.
        /// </summary>
        public SoundMenuScreen(ScreenManager screenManager)
            : base("Options")
        {
            audioService = (IAudioService)screenManager.Game.Services.GetService(typeof(IAudioService));
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
            if (!audioService.IsMusicMuted)
            {
                mnuPlayBackGroundMusic.Text = "Background Music <Off>";
                audioService.IsMusicMuted = true;
            }
            else
            {
                mnuPlayBackGroundMusic.Text = "Background Music <On>";
                audioService.IsMusicMuted = false;
            }
            audioService.playBackgroundmusic();

            ScreenManager.Game.GameStatus.PlayBackGroundMusic = !audioService.IsMusicMuted;

        }


        #endregion
    }
}
