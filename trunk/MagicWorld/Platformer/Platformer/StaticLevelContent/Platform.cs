using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.DynamicLevelContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.HelperClasses;
using MagicWorld.Constants;

namespace MagicWorld.StaticLevelContent
{
    /// <summary>
    /// platform the player can stand on
    /// </summary>
    class Platform : BlockElement
    {

        private Rectangle drawRec;

        public Rectangle DrawRec
        {
            get { return drawRec; }
            set { drawRec = value;}
        }

        public Platform(String texture, CollisionType collision,Level level,Vector2 position,Color drawColor)
            : base(texture, collision, level, position, drawColor)
        {

            int left = (int)Math.Round(position.X);
            int top = (int)Math.Round(position.Y);
            
            drawRec = new Rectangle(left, top, Width, Height);
        }

        public override HelperClasses.Bounds Bounds
        {
            get
            {
                if (positionChanged)
                {
                    int left = (int)Math.Round(position.X);
                    int top = (int)Math.Round(position.Y);
                    int yOffset = 10;
                    this.bounds = new Bounds(left + yOffset / 2, top, Width - yOffset, 20);
                    drawRec = new Rectangle(left, top, Width, Height);
                }
                return this.bounds;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(base.Texture, drawRec, drawColor);
            if (DebugValues.DEBUG)
            {
                spriteBatch.Draw(debugTexture, Bounds.getRectangle(), debugColor);
            }
        }
    }
}
