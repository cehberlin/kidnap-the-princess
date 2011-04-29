using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace KidnapThePrincess
{
    class GeometryHelper
    {
        public static Vector2 RotateAboutOrigin(Vector2 point, Vector2 origin, float rotationDegree)
        {

            float rotation = (float)(Math.PI * (double)rotationDegree / 180.0);
            Vector2 u = point - origin; //point relative to origin  

            if (u == Vector2.Zero)
                return point;

            float a = (float)Math.Atan2(u.Y, u.X); //angle relative to origin  
            a += rotation; //rotate  

            //u is now the new point relative to origin  
            u = u.Length() * new Vector2((float)Math.Cos(a), (float)Math.Sin(a));
            return u + origin;
        }

        #region intersection

        /// <summary>
        /// Returns true if the two circles intersect or are contained in one another.
        /// </summary>
        /// <param name="c1">Circle one.</param>
        /// <param name="c2">Circle two.</param>
        /// <returns></returns>
        public static bool Intersects(Circle c1, Circle c2)
        {
            return Intersects(c1.BoundingSphere, c2.BoundingSphere);
        }


        public static bool Intersects(BoundingSphere c1, BoundingSphere c2)
        {
            ContainmentType type = c1.Contains(c2);
            if (type == ContainmentType.Contains || type == ContainmentType.Intersects)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if the two squares intersect or are contained in one another.
        /// </summary>
        /// <param name="s1">Square one.</param>
        /// <param name="s2">Square two.</param>
        /// <returns></returns>
        public static bool Intersects(Square s1, Square s2)
        {
            ContainmentType type = s1.BoundingBox.Contains(s2.BoundingBox);
            if (type == ContainmentType.Contains || type == ContainmentType.Intersects)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if the square and circle intersect.
        /// </summary>
        /// <param name="s1">The square.</param>
        /// <param name="s2">The circle.</param>
        /// <returns></returns>
        public static bool Intersects(Square s, Circle c)
        {
            ContainmentType type = s.BoundingBox.Contains(c.BoundingSphere);
            // Note: "Contains" is impossible, since our bounding boxes have zero volume
            if (type == ContainmentType.Intersects)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if the square and circle intersect.
        /// </summary>
        /// <param name="s1">The circle.</param>
        /// <param name="s2">The square.</param>
        /// <returns></returns>
        public static bool Intersects(Circle c, Square s)
        {
            return Intersects(s, c);
        }

        #endregion intersection

    }
}
