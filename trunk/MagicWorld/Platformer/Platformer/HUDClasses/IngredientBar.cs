using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicWorld.HUDClasses
{
    /// <summary>
    /// shows needed and collected ingredients in hud
    /// </summary>
    class IngredientBar : HUDElement
    {
        /// <summary>
        /// Amount of ingredients that can be collected in the level.
        /// </summary>
        private int max;
        /// <summary>
        /// Amount of ingredients the player has right now.
        /// </summary>
        private int current;
        /// <summary>
        /// Amount of ingredients needed to finish the level.
        /// </summary>
        private int needed;

        Texture2D texture;

        const float NeededScale = 0.5f;

        const float MoreScale = 0.3f;

        public IngredientBar(Vector2 pos)
            : base(pos)
        {
            current = 0;
            max = 0;
            needed = 0;
        }

        public void LoadContent(Game game)
        {
            texture = game.Content.Load<Texture2D>("LevelContent/Cave/bone");
        }

        Rectangle getDrawRect(Vector2 pos, float scale)
        {
            return new Rectangle((int)pos.X, (int)pos.Y, (int)(texture.Width * scale), (int)(texture.Height * scale));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < max; i++)
            {
                Vector2 drawPos =Position+ new Vector2((texture.Width-25)*i,0);
                            
                if (i < needed)
                {
                    if (i < current)
                    {
                        spriteBatch.Draw(texture, getDrawRect(drawPos,NeededScale), Color.White);
                    }
                    else {
                        spriteBatch.Draw(texture, getDrawRect(drawPos, NeededScale), Color.Red * 0.3f);
                    }
                }
                else
                {
                    if (i < current)
                    {
                        spriteBatch.Draw(texture, getDrawRect(drawPos, MoreScale), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(texture, getDrawRect(drawPos, MoreScale), Color.LightGreen * 0.3f);
                    }
                }
            }
        }

        public void SetState(int cur, int need, int total)
        {
            current = cur;
            needed = need;
            max = total;
        }
    }
}
