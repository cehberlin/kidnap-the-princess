using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MagicWorld.Gleed2dLevelContent
{
    public class RectangleItem:Item
    {
        public int Width;
        public int Height;
        public Color FillColor;

        public RectangleItem() { }
    }
}
