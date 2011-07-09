using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld.Audio
{
    public interface IAudioService : IServiceProvider
    {
        void playSound(SoundType soundType);

        bool IsEffectMuted { get; set; }

        bool IsMusicMuted { get; set; }
    }
}
