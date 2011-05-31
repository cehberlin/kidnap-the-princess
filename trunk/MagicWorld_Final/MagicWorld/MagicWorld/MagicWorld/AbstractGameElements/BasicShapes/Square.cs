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
    public class Square : Entity
    {
        public Square(Vector2 position, int halfSize)
            :
            base(position, halfSize)
        {
            // Vector3 needed for compatibility with (3D) BoundingBox
            min = new Vector3(position.X - halfSize, position.Y - halfSize, 0f);
            max = new Vector3(position.X + halfSize, position.Y + halfSize, 0f);
            boundingBox = new BoundingBox(min, max);
        }

        // Vector3 needed for compatibility with (3D) BoundingBox
        protected Vector3 min;
        protected Vector3 max;

        private BoundingBox boundingBox;
        /// <summary>
        /// The bounding box associated with this square.
        /// </summary>
        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
        }

        /// <summary>
        /// Update of the bounding box after a position change.
        /// </summary>
        private void UpdateBoundingBox()
        {
            // Update bounding box
            min.X = Position.X - HalfSize;
            min.Y = Position.Y - HalfSize;
            max.X = Position.X + HalfSize;
            max.Y = Position.Y + HalfSize;
            boundingBox.Min = min;
            boundingBox.Max = max;
        }

        /// <summary>
        /// Screen space position of center point of this Square.
        /// </summary>
        public override Vector2 Position
        {
            set
            {
                position = value;
                UpdateBoundingBox();
            }
        }

        /// <summary>
        /// Used for assigning a value to the x position of this circle.
        /// </summary>
        public override float PosX
        {
            set
            {
                position.X = value;
                UpdateBoundingBox();
            }
        }

        /// <summary>
        /// Used for assigning a value to the y position of this circle.
        /// </summary>
        public override float PosY
        {
            set
            {
                position.Y = value;
                UpdateBoundingBox();
            }
        }

    }
}
