using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicWorld.DynamicLevelContent.SwitchRiddles
{
    /// <summary>
    /// a door which switches it texture and collision type on activasion
    /// </summary>
    class OpenCloseDoor:AbstractDoor
    {
        Texture2D texOpen;
        Texture2D texClosed;


        public OpenCloseDoor(String textureOpen, String textureClose, Level level, Vector2 position, bool opened = false)
            : base(textureClose, level, position, opened)
        {
            texOpen = level.Content.Load<Texture2D>(textureOpen);
            texClosed = level.Content.Load<Texture2D>(textureClose);
            refreshOpenCloseTexture();
        }

        public override void Update(GameTime gameTime)
        {
            //no updating nesessary
        }

        protected override void toggleOpenClose()
        {
            base.toggleOpenClose();
            refreshOpenCloseTexture();
        }

        /// <summary>
        /// set the drawn texture on the opened or closed variation
        /// </summary>
        private void refreshOpenCloseTexture()
        {
            if (Open)
            {
                Collision = CollisionType.Passable;
                base.Texture = texOpen;
            }
            else
            {
                Collision = CollisionType.Impassable;
                base.Texture = texClosed;
            }
        }



    }
}
