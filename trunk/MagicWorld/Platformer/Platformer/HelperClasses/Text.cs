using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MagicWorld.HelperClasses
{
    class Text
    {
        public static void DrawOutlinedText(SpriteBatch spriteBatch, SpriteFont font, String text, Vector2 position, Color colorText){
            DrawOutlinedText(spriteBatch, font, text, position, colorText, Vector2.Zero, 1f,Color.White);
        }
        public static void DrawOutlinedText(SpriteBatch spriteBatch, SpriteFont font, String text, Vector2 position, Color color, Vector2 origin, float scale)
        {
            DrawOutlinedText(spriteBatch, font, text, position, color, origin, scale, Color.White);
        }
        

        public static void DrawOutlinedText(SpriteBatch spriteBatch, SpriteFont font, String text, Vector2 position, Color color,Vector2 origin,float scale,Color outlineColor)
        {

            int offset = 1;
            spriteBatch.DrawString(font, text, position + new Vector2(-offset, -offset), outlineColor, 0,
                                   origin, scale, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, text, position + new Vector2(offset, offset), outlineColor, 0,
                                   origin, scale, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, text, position + new Vector2(0, offset), outlineColor, 0,
                       origin, scale, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, text, position + new Vector2(offset, 0), outlineColor, 0,
                 origin, scale, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, text, position, color, 0,
                 origin, scale, SpriteEffects.None, 0);
        }
    }
}
