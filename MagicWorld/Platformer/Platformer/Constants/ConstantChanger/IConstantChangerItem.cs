using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicWorld.Constants
{
    interface IConstantChangerItem
    {
        /// <summary>
        /// increment value
        /// </summary>
        void Increment();
        /// <summary>
        /// decrement value
        /// </summary>
        void Decrement();
        /// <summary>
        /// draw value
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="position"></param>
        /// <param name="font"></param>
        void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteFont font,Color color);

        /// <summary>
        /// if value has more than one internal value we could switch inside
        /// </summary>
        void switchInternalValues();

        string Name();
    }
}
