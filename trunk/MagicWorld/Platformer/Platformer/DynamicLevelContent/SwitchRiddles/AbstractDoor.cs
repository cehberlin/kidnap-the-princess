using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MagicWorld.Audio;

namespace MagicWorld.DynamicLevelContent.SwitchRiddles
{
    /// <summary>
    /// general stuff for all doors
    /// </summary>
    abstract class AbstractDoor : BlockElement, IActivation
    {

        bool open;

        public bool Open
        {
            get { return open; }
        }

        bool closed;

        public bool Closed
        {
            get { return closed; }
        }

        public AbstractDoor(String texture, Level level, Vector2 position, bool opened = false)
            : base(texture, CollisionType.Impassable, level, position)
        {
            closed = !opened;
            open = opened;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


        #region IActivation Member
        /// <summary>
        /// open command
        /// </summary>
        public void activate()
        {
            toggleOpenClose();
        }

        /// <summary>
        /// close command
        /// </summary>
        public void deactivate()
        {
            toggleOpenClose();
        }

        protected virtual void toggleOpenClose()
        {
            open = !open;
            closed = !closed;
            if (open)
            {
                audioService.playSound(SoundType.doorOpen);
            }
            else
            {
                audioService.playSound(SoundType.doorClose);
            }
        }

        #endregion
    }
}
