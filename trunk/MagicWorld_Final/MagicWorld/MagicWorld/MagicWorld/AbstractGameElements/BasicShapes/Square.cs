using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MagicWorld.AbstractGameElements.BasicShapes
{
    /// <summary>
    /// A simple 2D Square.
    /// </summary>
    public class Square : Rectangle
    {
        public Square(Vector2 position, float sidelength)
            :
            base(position, sidelength, sidelength)
        {
           
        }
    }
}
