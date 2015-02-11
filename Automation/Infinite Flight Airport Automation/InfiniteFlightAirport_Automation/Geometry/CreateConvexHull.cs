/*
 * Script for creating convex hull around LatLng points
 * Creates boundary for airports, based around node points.
 * Uses Monotone Chain algorithm
 * By Cameron Carmichael Alonso
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Loyc.Collections;
using Loyc.Utilities;
using System.ComponentModel;

namespace AirportParser
{
    public class CreateConvexHull
    {

        public static PointF[] ComputeConvexHull(List<PointF> points)
        {
            var list = new List<PointF>(points);
            return ComputeConvexHull(list, true);
        }

        public static PointF[] ComputeConvexHull(List<PointF> points, bool sortInPlace = false)
        {
            if (!sortInPlace)
                points = new List<PointF>(points);
            points.Sort((a, b) =>
              a.X == b.X ? a.Y.CompareTo(b.Y) : (a.X > b.X ? 1 : -1));

            DList<PointF> hull = new DList<PointF>();
            int L = 0, U = 0; // size of lower and upper hulls

            // Builds a hull such that the output polygon starts at the leftmost point.
            for (int i = points.Count - 1; i >= 0; i--)
            {
                PointF p = points[i], p1;

                // build lower hull (at end of output list)
                while (L >= 2 && cross((p1 = hull.Last),hull[hull.Count - 2], points[i]) >= 0)
                {
                    hull.RemoveAt(hull.Count - 1);
                    L--;
                }
                hull.PushLast(p);
                L++;

                // build upper hull (at beginning of output list)
                while (U >= 2 && cross((p1 = hull.First), (hull[1]), points[i]) <= 0)
                {
                    hull.RemoveAt(0);
                    U--;
                }
                if (U != 0) // when U=0, share the point added above
                    hull.PushFirst(p);
                U++;
            }
            hull.RemoveAt(hull.Count - 1);
            return hull.ToArray();
        }

        public static float cross(PointF O, PointF A, PointF B) {
            return (A.X - O.X) * (B.Y - O.Y) - (A.Y - O.Y) * (B.X - O.X);
        }

    }
}
