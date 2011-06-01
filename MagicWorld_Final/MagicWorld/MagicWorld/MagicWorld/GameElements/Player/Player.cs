using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.AbstractGameElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MagicWorld.GameConstants;
using HelperClasses;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace MagicWorld.GameElements.Player
{
    class Player:DynamicGameElement
    {
        IPlayerConstants playerConstants;


        #region "Animation & sound"

        // Animations
        private Animation idleAnimation;
        private Animation runAnimation;
        private Animation jumpAnimation;
        private Animation celebrateAnimation;
        private Animation dieAnimation;
        private SpriteEffects flip = SpriteEffects.None;
        private AnimationPlayer sprite;
        private float rotation = 0.0f;

        // Sounds
        private SoundEffect killedSound;
        private SoundEffect jumpSound;
        private SoundEffect fallSound;
        private SoundEffect spellSound;
        #endregion


        public Player(ContentManager contentManager, Vector2 position):
            base(contentManager,position)
        {
            playerConstants = GameConstantFactory.GET_INSTANCE().getPlayerConstants();
        }
    }
}
