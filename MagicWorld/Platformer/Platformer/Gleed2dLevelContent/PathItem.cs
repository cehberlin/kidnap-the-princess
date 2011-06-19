using System;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace MagicWorld.Gleed2dLevelContent
{
    public class PathItem : Item
    {
        public Vector2[] LocalPoints;
        public Vector2[] WorldPoints;
        public bool IsPolygon;
        public int LineWidth;
        public Color LineColor;

        public PathItem()
        {
        }
    }
}
