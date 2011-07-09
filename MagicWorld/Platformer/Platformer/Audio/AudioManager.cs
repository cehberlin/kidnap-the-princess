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
        playerjump,
        playerkilled,
        powerup
    }

    class AudioManager : GameComponent, IAudioService
    {
        String path = "Sounds/";
        Hashtable hashtable;

        public AudioManager(Game game)
            : base(game)
        {
            hashtable = new Hashtable(); ;
            game.Services.AddService(typeof(IAudioService), this);
        }

        public void Initialize()
        {
            //sounds.Add(Game.Content.Load<SoundEffect>(path + "CreateSpell"));
            SoundEffect s = Game.Content.Load<SoundEffect>(path + "CreateSpell");
            hashtable.Add(SoundType.createSpell, s);
            //sounds.Add(s);
            //sounds.Add(Game.Content.Load<SoundEffect>(path + "ExitReached"));
            //sounds.Add(Game.Content.Load<SoundEffect>(path + "GemCollected"));
            //sounds.Add(Game.Content.Load<SoundEffect>(path + "Icehit"));
            //sounds.Add(Game.Content.Load<SoundEffect>(path + "Monsterkilled"));
            //sounds.Add(Game.Content.Load<SoundEffect>(path + "PlayerFall"));
            //sounds.Add(Game.Content.Load<SoundEffect>(path + "PlayerJump"));
            //sounds.Add(Game.Content.Load<SoundEffect>(path + "PlayerKilled"));
            //sounds.Add(Game.Content.Load<SoundEffect>(path + "Powerup"));
            //base.Initialize();
        }

        public object GetService(Type serviceType)
        {
            return this;
        }

        public void playSound(SoundType soundType)
        {
            SoundEffect soundEffect = null;

            IDictionaryEnumerator enumerator = hashtable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key.Equals(soundType))
                {
                    soundEffect = (SoundEffect)enumerator.Value;
                }
                break;
            }
            if(soundEffect != null){
                soundEffect.Play();
            }
        }
    }
}

