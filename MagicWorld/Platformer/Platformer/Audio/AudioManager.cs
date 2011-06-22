using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace MagicWorld.Audio
{
    class AudioManager : GameComponent
    {
        String path = "Sounds/";
        List<SoundEffect> sounds;

        public AudioManager(Game game)
            : base(game)
        {
            sounds = new List<SoundEffect>();
        }

        public override void Initialize()
        {
            //sounds.Add(Game.Content.Load<SoundEffect>(path + "CreateSpell"));
            SoundEffect s = Game.Content.Load<SoundEffect>(path + "CreateSpell");
            sounds.Add(s);
            sounds.Add(Game.Content.Load<SoundEffect>(path + "ExitReached"));
            sounds.Add(Game.Content.Load<SoundEffect>(path + "GemCollected"));
            sounds.Add(Game.Content.Load<SoundEffect>(path + "Icehit"));
            sounds.Add(Game.Content.Load<SoundEffect>(path + "Monsterkilled"));
            sounds.Add(Game.Content.Load<SoundEffect>(path + "PlayerFall"));
            sounds.Add(Game.Content.Load<SoundEffect>(path + "PlayerJump"));
            sounds.Add(Game.Content.Load<SoundEffect>(path + "PlayerKilled"));
            sounds.Add(Game.Content.Load<SoundEffect>(path + "Powerup"));
        }
    }
}
