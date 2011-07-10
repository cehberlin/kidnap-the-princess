using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.DynamicLevelContent;
using MagicWorld.HelperClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.Constants;

namespace MagicWorld.StaticLevelContent
{
    class LevelExit : BlockElement
    {

        private Bounds shortBounds;
        private Rectangle drawRec;


        public LevelExit(String texture, Level level,Vector2 position)
            : base(texture, CollisionType.Passable, level, position)
        {
        }

        public override int Height
        {
            get
            {
                return base.Height;
            }
            set
            {
                calsBoundsAndDrawRec();
                base.Height = value;
            }
        }

        public override int Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                calsBoundsAndDrawRec();
                base.Width = value;
            }
        }

        public override HelperClasses.Bounds Bounds
        {
            get
            {
                return this.shortBounds;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, drawRec, drawColor);
            }   
            if (DebugValues.DEBUG)
            {
                if (Bounds.Type == Bounds.BoundType.BOX)
                {
                    spriteBatch.Draw(debugTexture, Bounds.getRectangle(), debugColor);
                }
                else
                {
                    spriteBatch.Draw(debugTextureCycle, Bounds.getRectangle(), debugColor);
                }
            }
        }

        private void calsBoundsAndDrawRec()
        {
            int x = (int)Math.Round(position.X);
            int y = (int)Math.Round(position.Y);

            this.shortBounds = new Bounds(x + 80 / 2, y + 20, 80, Height * 0.7f);
            drawRec = new Rectangle(x, y, Width, Height);
        }
    }
}
