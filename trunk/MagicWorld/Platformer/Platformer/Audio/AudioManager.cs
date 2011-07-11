using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using System.Collections;
using Microsoft.Xna.Framework.Media;

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
        powerup,
        laugh,
        startFireAndIce,
        destroy,
        collect,
        enemy_hit,
        rockslide
    }

    //TODO add MediaPlayer/loop sounds for backgroundmusic
    class AudioManager : GameComponent, IAudioService
    {
        String path = "Sounds/";
        Hashtable sounds;
        Hashtable loopSounds;

        bool isEffectMuted = false;

        bool isMusicMuted = false;

        public AudioManager(Game game)
            : base(game)
        {
            sounds = new Hashtable();
            loopSounds = new Hashtable();
        }

        public new void Initialize()
        {
            //TODO add all new sounds and add also calls into corresponding gamecode, all basicgameelemts have a reference to this service
            sounds.Add(SoundType.createSpell, Game.Content.Load<SoundEffect>(path + "spellcast2"));
            sounds.Add(SoundType.exitreached, Game.Content.Load<SoundEffect>(path + "level_exit1"));
            sounds.Add(SoundType.icehit, Game.Content.Load<SoundEffect>(path + "Icehit"));
            sounds.Add(SoundType.monsterkill, Game.Content.Load<SoundEffect>(path + "enemy-dies"));
            sounds.Add(SoundType.playerfall, Game.Content.Load<SoundEffect>(path + "PlayerFall"));
            sounds.Add(SoundType.playerjump, Game.Content.Load<SoundEffect>(path + "jump"));
            sounds.Add(SoundType.playerkilled, Game.Content.Load<SoundEffect>(path + "player_dies"));
            sounds.Add(SoundType.powerup, Game.Content.Load<SoundEffect>(path + "Powerup"));
            sounds.Add(SoundType.laugh, Game.Content.Load<SoundEffect>(path + "laugh"));
            sounds.Add(SoundType.startFireAndIce, Game.Content.Load<SoundEffect>(path + "start_fire_and_ice_begin_sound"));
            sounds.Add(SoundType.destroy, Game.Content.Load<SoundEffect>(path + "destroy_fire_spell_and_destroy_switch"));
            sounds.Add(SoundType.collect, Game.Content.Load<SoundEffect>(path + "collect"));
            sounds.Add(SoundType.enemy_hit, Game.Content.Load<SoundEffect>(path + "spell_hits_creature"));
            sounds.Add(SoundType.rockslide, Game.Content.Load<SoundEffect>(path + "rockslide"));
        }

        public object GetService(Type serviceType)
        {
            return this;
        }

        public void stopSoundLoop(SoundType soundType)
        {
            SoundEffectInstance sound = (SoundEffectInstance)loopSounds[soundType];
            if (sound != null)
            {
                sound.Stop();
                loopSounds.Remove(soundType);
            }
        }

        public void playSoundLoop(SoundType soundType)
        {
            if (!IsEffectMuted)
            {
                if (!loopSounds.Contains(soundType))
                {
                    SoundEffect soundEffect = null;
                    soundEffect = (SoundEffect)sounds[soundType];
                    if (soundEffect != null)
                    {
                        SoundEffectInstance sound = soundEffect.CreateInstance();
                        sound.IsLooped = true;
                        sound.Play();
                        loopSounds.Add(soundType, sound);
                    }
                }
            }
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

        public void playBackgroundmusic()
        {
            try
            {
                MediaPlayer.IsMuted = isMusicMuted;
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(Game.Content.Load<Song>("Sounds/music"));
            }
            catch { }
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
                MediaPlayer.IsMuted = value;
            }
        }

        #endregion
    }
}

