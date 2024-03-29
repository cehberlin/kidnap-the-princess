﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MagicWorld.HelperClasses
{
    public class Bounds
    {

        public enum BoundType
        {
            BOX,
            SPHERE
        }

        BoundType type;

        public BoundType Type
        {
            get { return type; }
        }

        BoundingBox box;

        public BoundingBox Box
        {
            get { return box; }
        }

        BoundingSphere sphere;

        public BoundingSphere Sphere
        {
            get { return sphere; }
        }

        Vector3 position = Vector3.Zero;

        public Vector2 Position
        {
            get { return new Vector2(position.X,position.Y); }
            set { 
                position.X = value.X;
                position.Y = value.Y;
                updateBound();
            }
        }

        float radius;

        public float Radius
        {
            get { return radius; }
            set { 
                radius = value;
                updateBound();
            }
        }

        float width;

        public float Width
        {
            get { return width; }
            set { 
                width = value;
                updateBound();
            }
        }

        float height;

        public float Height
        {
            get { return height; }
            set { 
                height = value;
                updateBound();
            }
        }

        public Vector2 Center
        {
            get { 
                Rectangle rect=getRectangle();
                return new Vector2(rect.Center.X, rect.Center.Y);
            }
        }

        public Bounds()
            : this(0, 0, 0, 0)
        {
        }

        /// <summary>
        /// create cycle bound
        /// </summary>
        /// <param name="position">center position</param>
        /// <param name="radius"></param>
        public Bounds(Vector2 position, float radius) 
        {
            Init(position, radius);
        }

        /// <summary>
        /// create cycle bound
        /// </summary>
        /// <param name="radius"></param>
        public Bounds(float x,float y, float radius)
        {
            Init(x, y, radius);
        }

        /// <summary>
        /// create rectangle bound
        /// </summary>
        /// <param name="position">left upper corner position</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Bounds(Vector2 position, float width, float height)
        {
            Init(position, width, height);
        }

        /// <summary>
        /// create rectangle bound
        /// </summary>
        /// <param name="position">left upper corner position</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Bounds(float left, float top, float width, float height)            
        {
            Init(left, top, width, height);
        }

        /// <summary>
        /// rectangle init
        /// </summary>
        /// <param name="position"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void Init(Vector2 position, float width, float height)
        {
            type = BoundType.BOX;
            this.width = width;
            this.height = height;
            this.Position = position;//calls indirekt updateBound
        }

        public void Init(float left, float top, float width, float height)
        {
            type = BoundType.BOX;
            this.width = width;
            this.height = height;
            position.X = left;
            position.Y = top;
            updateBound();
        }

        /// <summary>
        /// cycle init
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        public void Init(Vector2 position, float radius)
        {
            sphere = new BoundingSphere();
            type = BoundType.SPHERE;
            this.radius = radius;
            this.Position = position;//calls indirekt updateBound
        }


        /// <summary>
        /// cycle init
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        public void Init(float left, float top, float radius)
        {
            sphere = new BoundingSphere();
            type = BoundType.SPHERE;
            this.radius = radius;
            position.X = left;
            position.Y = top;
            updateBound();
        }

        private Vector3 boxCreationVector = Vector3.Zero;
        private void updateBound()
        {
            if (this.type == BoundType.BOX)
            {
                boxCreationVector.X = width;
                boxCreationVector.Y = height;
                box = new BoundingBox(position, position + boxCreationVector);
            }
            else if (this.type == BoundType.SPHERE)
            {
                sphere.Center = position;
                sphere.Radius = radius;
            }
        }

        public Rectangle getRectangle()
        {
            if (this.type == BoundType.BOX)
            {
                return new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height);
            }
            else if (this.type == BoundType.SPHERE)
            {
                //build a rectangle around the cycle
                return new Rectangle((int)(position.X-radius), (int)(position.Y-radius), (int)(radius*2), (int)(radius*2));
            }
            throw new NotImplementedException();
        }


        public ContainmentType Contains(Bounds c)
        {
            if (c.Type == BoundType.BOX)
            {
                if (this.type == BoundType.BOX)
                {
                    return c.Box.Contains(this.box);
                }
                else if (this.type == BoundType.SPHERE)
                {
                    return c.Box.Contains(this.sphere);
                }
            }
            else if (c.Type == BoundType.SPHERE)
            {
                if (this.type == BoundType.BOX)
                {
                    return c.Sphere.Contains(this.box);
                }
                else if (this.type == BoundType.SPHERE)
                {
                    return c.Sphere.Contains(this.sphere);
                }
            }
            return ContainmentType.Disjoint;
        }

        public Vector2 CollisionDepth(Bounds c)
        {
            //at the moment the result for spheres are not exact because it is used a rectangle build arround the cycle
            return RectangleExtensions.GetIntersectionDepth(this.getRectangle(), c.getRectangle());
        }

    }
}
