using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using System.Collections;

namespace MagicWorld.Audio
{
    public enum SoundType
    {
        createSpell,
        exitreached,
        gemCollected,
        icehit,
        monsterkill,
        playerfall,
        playerjump,
        playerkilled,
        powerup
    }

    //TODO add MediaPlayer/loop sounds for backgroundmusic
    class AudioManager : GameComponent, IAudioService
    {
        String path = "Sounds/";
        Hashtable sounds;

        bool isEffectMuted = false;

        bool isMusicMuted = false;

        public AudioManager(Game game)
            : base(game)
        {
            sounds = new Hashtable(); ;
            game.Services.AddService(typeof(IAudioService), this);
        }

        public new void Initialize()
        {
            //TODO add all new sounds and add also calls into corresponding gamecode, all basicgameelemts have a reference to this service
            sounds.Add(SoundType.createSpell, Game.Content.Load<SoundEffect>(path + "CreateSpell"));
            sounds.Add(SoundType.exitreached, Game.Content.Load<SoundEffect>(path + "GemCollected"));
            sounds.Add(SoundType.gemCollected, Game.Content.Load<SoundEffect>(path + "Icehit"));
            sounds.Add(SoundType.icehit, Game.Content.Load<SoundEffect>(path + "Monsterkilled"));
            sounds.Add(SoundType.playerfall, Game.Content.Load<SoundEffect>(path + "PlayerFall"));
            sounds.Add(SoundType.playerjump, Game.Content.Load<SoundEffect>(path + "PlayerJump"));
            sounds.Add(SoundType.playerkilled, Game.Content.Load<SoundEffect>(path + "PlayerKilled"));
            sounds.Add(SoundType.powerup, Game.Content.Load<SoundEffect>(path + "Powerup"));
        }

        public object GetService(Type serviceType)
        {
            return this;
        }

        public void playSound(SoundType soundType)
        {
            if (!IsEffectMuted)
            {
                SoundEffect soundEffect = null;

                soundEffect = (SoundEffect)sounds[soundType];

                if (soundEffect != null)
                {
                    soundEffect.Play();
                }
            }
        }

        #region IAudioService Member


        public bool IsEffectMuted
        {
            get
            {
                return isEffectMuted;
            }
            set
            {
                isEffectMuted = value;
            }
        }

        #endregion

        #region IAudioService Member


        public bool IsMusicMuted
        {
            get
            {
                return isMusicMuted;
            }
            set
            {
                isMusicMuted = value;
            }
        }

        #endregion
    }
}

