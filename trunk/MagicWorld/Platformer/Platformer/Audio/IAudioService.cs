using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld.Audio
{
    public interface IAudioService : IServiceProvider
    {
        void playSound(SoundType soundType);

        void playBackgroundmusic();

        void stopBackgroundmusic();

        bool IsEffectMuted { get; set; }

        bool IsMusicMuted { get; set; }

        void stopSoundLoop(SoundType soundType);

        void playSoundLoop(SoundType soundType, float volume);

        void setBackgroundVolume(float volume);
    }
}
