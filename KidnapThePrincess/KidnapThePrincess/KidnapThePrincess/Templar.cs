using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace KidnapThePrincess
{
    class Templar:Enemy
    {
        public Templar(Texture2D tex)
            : base(tex)
        {
            Speed = 1.75f;
        }
    }
}
