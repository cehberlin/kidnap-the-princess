using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace KidnapThePrincess
{
    class Templar:Enemy
    {
        public Templar(Texture2D tex, List<Hero> heroes)
            : base(tex,heroes)
        {
            Speed = 0.8f;
        }
    }
}
