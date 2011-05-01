using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace KidnapThePrincess
{
    /// <summary>
    /// The generic Entity class. Extended by Circle and Square.
    /// </summary>
    abstract class Entity
    {
        public Entity(Vector2 position, int halfSize)
        {
            Position = position;
            HalfSize = halfSize;
        }

        private int halfSize;

        /// <summary>
        /// Halfsize doubles as half of the square edge length, 
        /// as well as the circle radius of the respective entity.
        /// </summary>
        public int HalfSize
        {
            get { return halfSize; }
            set { halfSize = value; }
        }

        /// <summary>
        /// Edge length of square, or diameter of circle.
        /// </summary>
        public int Size
        {
            get { return 2 * halfSize; }
        }

        protected Vector2 position;

        /// <summary>
        /// Screen space position of center point of this entitiy.
        /// </summary>
        public virtual Vector2 Position
        {
            get { return position; }
            set { position = value;
            OnPositionUpdate();
            }
        }

        protected virtual void OnPositionUpdate()
        {
        }

        /// <summary>
        /// Used for assigning a value to the x position of entity
        /// </summary>
        public virtual float PosX
        {
            get { return position.X; }
            set { position.X = value;
            OnPositionUpdate();
            }
        }

        /// <summary>
        /// Used for assigning a value to the y position of entity
        /// </summary>
        public virtual float PosY
        {
            get { return position.Y; }
            set { position.Y = value;
            OnPositionUpdate();
            }
        }

        private Vector2 speed;

        /// <summary>
        /// This entity's velocity.
        /// </summary>
        public Vector2 Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        /// <summary>
        /// Returns a unit vector of the speed direction.
        /// </summary>
        public void NormalizeSpeed()
        {
            speed.Normalize();
        }

        /// <summary>
        /// Used for assigning a value to the x component of speed
        /// </summary>
        public float VelX
        {
            get { return speed.X; }
            set { speed.X = value; }
        }

        /// <summary>
        /// Used for assigning a value to the y component of speed
        /// </summary>
        public float VelY
        {
            get { return speed.Y; }
            set { speed.Y = value; }
        }

    }
}
