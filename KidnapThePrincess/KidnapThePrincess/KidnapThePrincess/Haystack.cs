using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace KidnapThePrincess
{
    class Haystack:GameObject
    {
        public Haystack(SpriteFont defaultTextFont,Texture2D tex, Vector2 pos)
            : base(5,defaultTextFont,tex, tex, tex)
        {
            Position = pos;
            Area = new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Height);
        }
    }
}
