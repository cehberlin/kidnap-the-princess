using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MagicWorld.AbstractGameElements.BasicShapes
{
    /// <summary>
    /// The generic Entity class. Extended by Circle and Square.
    /// </summary>
    public abstract class Entity
    {
        public Entity(Vector2 position, float width, float height)
        {
            Position = position;
            this.width = width;
            this.height = height;
        }

        private float width;

        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        private float height;

        public float Height
        {
            get { return height; }
            set { height = value; }
        }



        /// <summary>
        /// Edge length of square, or diameter of circle.
        /// </summary>
        public abstract double Size
        {
            get;
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

        //DOES IT REALLY MAKE SENSE IN THIS CLASS CONTEXT?
        //private Vector2 speed;

        ///// <summary>
        ///// This entity's velocity.
        ///// </summary>
        //public Vector2 Speed
        //{
        //    get { return speed; }
        //    set { speed = value; }
        //}

        ///// <summary>
        ///// Returns a unit vector of the speed direction.
        ///// </summary>
        //public void NormalizeSpeed()
        //{
        //    speed.Normalize();
        //}

        ///// <summary>
        ///// Used for assigning a value to the x component of speed
        ///// </summary>
        //public float VelX
        //{
        //    get { return speed.X; }
        //    set { speed.X = value; }
        //}

        ///// <summary>
        ///// Used for assigning a value to the y component of speed
        ///// </summary>
        //public float VelY
        //{
        //    get { return speed.Y; }
        //    set { speed.Y = value; }
        //}

    }
}
