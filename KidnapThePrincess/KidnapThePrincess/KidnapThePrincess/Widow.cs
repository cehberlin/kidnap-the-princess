using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace KidnapThePrincess
{
    class Widow:Hero
    {
        public Widow(Texture2D tex, Rectangle area) :
            base(tex, area)
        {
            Speed = 0.8f;
        }
    }
}
