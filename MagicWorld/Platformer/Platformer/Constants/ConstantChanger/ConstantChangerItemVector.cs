using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MagicWorld.Constants
{
    class ConstantChangerItemVector: IConstantChangerItem
    {

        public Vector2 value;

        float changeFactor;

        public string name;

        bool changeX = true;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="name">name of item</param>
        /// <param name="value">current value of item</param>
        /// <param name="changeFactor">should be a positive value</param>
        public ConstantChangerItemVector(string name, ref Vector2 value, float changeFactor)
        {
            this.value = value;
            this.changeFactor = changeFactor;
            this.name = name;
        }

        #region IConstantChangerItem Member

        public void Increment()
        {
            if (changeX)
            {
                value.X += changeFactor;
            }
            else
            {
                value.Y += changeFactor;
            }
        }

        public void Decrement()
        {
            if (changeX)
            {
                value.X -= changeFactor;
            }
            else
            {
                value.Y -= changeFactor;
            }
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Vector2 position, Microsoft.Xna.Framework.Graphics.SpriteFont font, Color color)
        {
                spriteBatch.DrawString(font, name + ": " + value, position, color);
        }

        public void switchInternalValues()
        {
            changeX = !changeX;
        }

        public string Name()
        {
            return this.name;
        }

        #endregion
    }
}
