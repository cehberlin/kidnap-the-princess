using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MagicWorld.Constants
{
    class ConstantChangerItemFloat:IConstantChangerItem
    {

        public float value;

        float changeFactor;

        public string name;

        float startValue;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="name">name of item</param>
        /// <param name="value">current value of item</param>
        /// <param name="changeFactor">should be a positive value</param>
        public ConstantChangerItemFloat(string name, float value, float changeFactor)
        {
            this.value = value;
            this.changeFactor = changeFactor;
            this.name = name;
            startValue = value;
        }

        #region IConstantChangerItem Member

        public void Increment()
        {
            value += changeFactor;
        }

        public void Decrement()
        {
            value -= changeFactor;
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Vector2 position, Microsoft.Xna.Framework.Graphics.SpriteFont font, Color color)
        {
            spriteBatch.DrawString(font, name +": " + value, position, color);
        }

        public void switchInternalValues()
        {
            //not used
        }

        public string Name()
        {
            return this.name;
        }
        public void resetToStart()
        {
            this.value = startValue;
        }

        #endregion
    }
}
