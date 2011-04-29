using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace KidnapThePrincess
{
    class Brute:Hero
    {
        public Brute(Texture2D tex, Rectangle area) :
            base(tex, area)
        {
            Speed = 0.5f;
        }
    }
}
