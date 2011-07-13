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
        laugh,
        startFireAndIce,
        destroy,
        collect,
        enemy_hit,
        rockslide,
        menuClick,
        rockHitGround,
        iceHitEnemy,
        doorClose,
        doorOpen,
        icecrack,
        electric,
        menuSelect,
        menuValidate,
        noGravity,
        pause,
        switch_pressed,
        lava,
        wings, 
        electricSwitch,
        iceshatter,
        changeSpell
    }

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
            sounds.Add(SoundType.createSpell, Game.Content.Load<SoundEffect>(path + "spellcast2").CreateInstance());
            sounds.Add(SoundType.exitreached, Game.Content.Load<SoundEffect>(path + "level_exit1").CreateInstance());
            sounds.Add(SoundType.icehit, Game.Content.Load<SoundEffect>(path + "Icehit").CreateInstance());
            sounds.Add(SoundType.monsterkill, Game.Content.Load<SoundEffect>(path + "enemy-dies").CreateInstance());
            sounds.Add(SoundType.playerfall, Game.Content.Load<SoundEffect>(path + "PlayerFall").CreateInstance());
            sounds.Add(SoundType.playerjump, Game.Content.Load<SoundEffect>(path + "jump").CreateInstance());
            sounds.Add(SoundType.playerkilled, Game.Content.Load<SoundEffect>(path + "player_dies").CreateInstance());
            sounds.Add(SoundType.laugh, Game.Content.Load<SoundEffect>(path + "laugh").CreateInstance());
            sounds.Add(SoundType.startFireAndIce, Game.Content.Load<SoundEffect>(path + "start_fire_and_ice_begin_sound").CreateInstance());
            sounds.Add(SoundType.destroy, Game.Content.Load<SoundEffect>(path + "destroy_fire_spell_and_destroy_switch").CreateInstance());
            sounds.Add(SoundType.collect, Game.Content.Load<SoundEffect>(path + "collect").CreateInstance());
            sounds.Add(SoundType.enemy_hit, Game.Content.Load<SoundEffect>(path + "spell_hits_creature").CreateInstance());
            sounds.Add(SoundType.rockslide, Game.Content.Load<SoundEffect>(path + "rockslide").CreateInstance());
            sounds.Add(SoundType.menuClick, Game.Content.Load<SoundEffect>(path + "menuClick").CreateInstance());
            sounds.Add(SoundType.rockHitGround, Game.Content.Load<SoundEffect>(path + "rockHitGround").CreateInstance());
            sounds.Add(SoundType.iceHitEnemy, Game.Content.Load<SoundEffect>(path + "iceHitEnemy").CreateInstance());
            sounds.Add(SoundType.doorClose, Game.Content.Load<SoundEffect>(path + "doorClose").CreateInstance());
            sounds.Add(SoundType.doorOpen, Game.Content.Load<SoundEffect>(path + "openDoor").CreateInstance());
            sounds.Add(SoundType.icecrack, Game.Content.Load<SoundEffect>(path + "icecrack").CreateInstance());
            sounds.Add(SoundType.electric, Game.Content.Load<SoundEffect>(path + "electric").CreateInstance());
            sounds.Add(SoundType.menuSelect, Game.Content.Load<SoundEffect>(path + "menuSelect").CreateInstance());
            sounds.Add(SoundType.menuValidate, Game.Content.Load<SoundEffect>(path + "menuValidate").CreateInstance());
            sounds.Add(SoundType.noGravity, Game.Content.Load<SoundEffect>(path + "no_gravity").CreateInstance());
            sounds.Add(SoundType.pause, Game.Content.Load<SoundEffect>(path + "pause").CreateInstance());
            sounds.Add(SoundType.switch_pressed, Game.Content.Load<SoundEffect>(path + "switch_pressed").CreateInstance());
            sounds.Add(SoundType.lava, Game.Content.Load<SoundEffect>(path + "lava").CreateInstance());
            sounds.Add(SoundType.wings, Game.Content.Load<SoundEffect>(path + "wings").CreateInstance());
            sounds.Add(SoundType.electricSwitch, Game.Content.Load<SoundEffect>(path + "electric").CreateInstance());
            sounds.Add(SoundType.iceshatter, Game.Content.Load<SoundEffect>(path + "iceshatter").CreateInstance());
            sounds.Add(SoundType.changeSpell, Game.Content.Load<SoundEffect>(path + "changeSpell").CreateInstance());
        }

        public object GetService(Type serviceType)
        {
            return this;
        }

        public void stopSoundLoop(SoundType soundType, bool immediate)
        {
            SoundEffectInstance sound = (SoundEffectInstance)loopSounds[soundType];
            if (sound != null)
            {
                sound.Stop(immediate);
                loopSounds.Remove(soundType);
            }
        }

        public void playSoundLoop(SoundType soundType, float volume)
        {
            if (!IsEffectMuted)
            {
                if (!loopSounds.Contains(soundType))
                {
                    SoundEffectInstance soundEffect = null;
                    soundEffect = (SoundEffectInstance)sounds[soundType];
                    if (soundEffect != null)
                    {
                        if (!soundEffect.IsLooped)
                        {
                            soundEffect.IsLooped = true;
                        }
                        soundEffect.Volume = volume;                        
                        soundEffect.Play();                        
                        loopSounds.Add(soundType, soundEffect);
                    }
                }
            }
        }

        public void playSound(SoundType soundType,float volume)
        {
            if (!IsEffectMuted)
            {
                SoundEffectInstance soundEffect = null;
                soundEffect = (SoundEffectInstance)sounds[soundType];
                if (soundEffect != null)
                {
                    soundEffect.Volume = volume;
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
                MediaPlayer.Volume = 0.8f;
                MediaPlayer.Play(Game.Content.Load<Song>("Sounds/music"));
            }
            catch { }
        }

        public void setBackgroundVolume(float volume)
        {
            MediaPlayer.Volume = volume;
        }

        public void stopBackgroundmusic()
        {
            try
            {
                MediaPlayer.Stop();
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


        public void clearAllSounds()
        {
            foreach(SoundType st in System.Enum.GetValues( typeof(SoundType) ) )
            {
                stopSoundLoop(st,true);
            }
        }
    }
}

