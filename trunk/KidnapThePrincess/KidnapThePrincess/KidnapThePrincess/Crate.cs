using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace KidnapThePrincess
{
    class Crate : GameObject
    {
        public Crate(SpriteFont defaultTextFont,Texture2D textureFullHitPoints, Texture2D textureMediumHitPoints, Texture2D textureLowHitPoints, Vector2 pos)
            : base(10,defaultTextFont,textureFullHitPoints, textureMediumHitPoints, textureLowHitPoints)
        {
            Position = pos;
            Area = new Rectangle((int)pos.X, (int)pos.Y, textureFullHitPoints.Width, textureFullHitPoints.Height);
        }
    }
}
