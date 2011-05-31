using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MagicWorld.AbstractGameElements.BasicShapes
{
    /// <summary>
    /// A simple 2D circle.
    /// </summary>
    public class Circle : Entity
    {
        public Circle(Vector2 position, int halfSize)
            :
            base(position, halfSize)
        {
            // Vector3 needed for compatibility with (3D) BoundingSphere
            spherePosition = new Vector3(position, 0f);
            boundingSphere = new BoundingSphere(spherePosition, halfSize);
        }

        // Vector3 needed for compatibility with (3D) BoundingSphere
        protected Vector3 spherePosition;

        private BoundingSphere boundingSphere;
        /// <summary>
        /// The bounding sphere associated with this circle.
        /// </summary>
        public BoundingSphere BoundingSphere
        {
            get { return boundingSphere; }
        }

        /// <summary>
        /// Update of the bounding sphere center after a position change.
        /// </summary>
        private void UpdateBoundingSphere()
        {
            spherePosition.X = Position.X;
            spherePosition.Y = Position.Y;
            boundingSphere.Center = spherePosition;
        }


        protected override void OnPositionUpdate()
        {
            base.OnPositionUpdate();
            UpdateBoundingSphere();
        }       

    }
}
