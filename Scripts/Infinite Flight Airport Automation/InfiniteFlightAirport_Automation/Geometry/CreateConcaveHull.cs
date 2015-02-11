 /*
 * Script for creating concave hull around LatLng points
 * Creates boundary for airports, based around node points.
 * Bends in towards points
 * Based on samples from StackOverflow - this is an excellent one http://stackoverflow.com/questions/16407533/translating-concave-hull-algorithm-to-c-sharp
 * By Cameron Carmichael Alonso
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportParser
{
    class CreateConcave
    {
        public static Vertex[] ConcaveHull(Vertex[] points, int k = 3)
        {
            if (k < 3)
                throw new ArgumentException("K is required to be 3 or more", "k");
            List<Vertex> hull = new List<Vertex>();
            //Clean first, may have lots of duplicates
            Vertex[] clean = RemoveDuplicates(points);
            if (clean.Length < 3)
                throw new ArgumentException("At least 3 dissimilar points reqired", "points");
            if (clean.Length == 3)//This is the hull, its already as small as it can be.
                return clean;
            if (clean.Length < k)
                throw new ArgumentException("K must be equal to or smaller then the amount of dissimilar points", "points");
            Vertex firstPoint = clean[0]; //TODO find mid point
            hull.Add(firstPoint);
            Vertex currentPoint = firstPoint;
            Vertex[] dataset = RemoveIndex(clean, 0);
            double previousAngle = 0;
            int step = 2;
            int i;
            while (((currentPoint != firstPoint) || (step == 2)) && (dataset.Length > 0))
            {
                if (step == 5)
                    dataset = Add(dataset, firstPoint);
                Vertex[] kNearestPoints = nearestPoints(dataset, currentPoint, k);
                Vertex[] cPoints = sortByAngle(kNearestPoints, currentPoint, previousAngle);
                bool its = true;
                i = 0;
                while ((its) && (i < cPoints.Length))
                {
                    i++;
                    int lastPoint = 0;
                    if (cPoints[0] == firstPoint)
                        lastPoint = 1;
                    int j = 2;
                    its = false;
                    while ((!its) && (j < hull.Count - lastPoint))
                    {
                        its = intersectsQ(hull[step - 1 - 1], cPoints[0], hull[step - i - j - 1], hull[step - j - 1]);
                        j++;
                    }
                }
                if (its)
                {
                    return ConcaveHull(points, k + 1);
                }
                currentPoint = cPoints[0];
                hull.Add(currentPoint);
                previousAngle = angle(hull[step - 1], hull[step - 2]);
                dataset = RemoveIndex(dataset, 0);
                step++;
            }
            bool allInside = true;
            i = dataset.Length;
            while (allInside && i > 0)
            {
                allInside = new Polygon(dataset).Contains(currentPoint); //TODO havent finished ray casting yet.
                i--;
            }
            if (!allInside)
                return ConcaveHull(points, k + 1);
            return hull.ToArray();
        }

        private static Vertex[] Add(Vertex[] vs, Vertex v)
        {
            List<Vertex> n = new List<Vertex>(vs);
            n.Add(v);
            return n.ToArray();
        }

        private static Vertex[] RemoveIndex(Vertex[] vs, int index)
        {
            List<Vertex> removed = new List<Vertex>();
            for (int i = 0; i < vs.Length; i++)
                if (i != index)
                    removed.Add(vs[i]);
            return removed.ToArray();
        }

        private static Vertex[] RemoveDuplicates(Vertex[] vs)
        {
            List<Vertex> clean = new List<Vertex>();
           /* foreach (Vertex v in vs)
            {
                if (!clean.Contains(v, vc))
                    clean.Add(v);
            }
            return clean.ToArray();*/
            return null;
        }

        private static Vertex[] nearestPoints(Vertex[] vs, Vertex v, int k)
        {
            Dictionary<double, Vertex> lengths = new Dictionary<double, Vertex>();
            List<Vertex> n = new List<Vertex>();
            double[] sorted = lengths.Keys.OrderBy(d => d).ToArray();
            for (int i = 0; i < k; i++)
            {
                n.Add(lengths[sorted[i]]);
            }
            return n.ToArray();
        }

        private static Vertex[] sortByAngle(Vertex[] vs, Vertex v, double angle)
        {
            
            return new Vertex[] { };
        }

        private static bool intersectsQ(Vertex v1, Vertex v2, Vertex v3, Vertex v4)
        {
            return intersectsQ(new Edge(v1, v2), new Edge(v3, v4));
        }

        private static bool intersectsQ(Edge e1, Edge e2)
        {
            double x1 = e1.A.X;
            double x2 = e1.B.X;
            double x3 = e2.A.X;
            double x4 = e2.B.X;

            double y1 = e1.A.Y;
            double y2 = e1.B.Y;
            double y3 = e2.A.Y;
            double y4 = e2.B.Y;

            var x = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4));
            var y = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4));
            if (double.IsNaN(x) || double.IsNaN(y))
            {
                return false;
            }
            else
            {
                if (x1 >= x2)
                {
                    if (!(x2 <= x && x <= x1)) { return false; }
                }
                else
                {
                    if (!(x1 <= x && x <= x2)) { return false; }
                }
                if (y1 >= y2)
                {
                    if (!(y2 <= y && y <= y1)) { return false; }
                }
                else
                {
                    if (!(y1 <= y && y <= y2)) { return false; }
                }
                if (x3 >= x4)
                {
                    if (!(x4 <= x && x <= x3)) { return false; }
                }
                else
                {
                    if (!(x3 <= x && x <= x4)) { return false; }
                }
                if (y3 >= y4)
                {
                    if (!(y4 <= y && y <= y3)) { return false; }
                }
                else
                {
                    if (!(y3 <= y && y <= y4)) { return false; }
                }
            }
            return true;
        }

        private static double angle(Vertex v1, Vertex v2)
        {
            // TODO fix
            Vertex v3 = new Vertex(v1.X, 0);
            if (Orientation(v3, v1, v2) == 0)
                return 180;

            double b = EuclideanDistance(v3, v1);
            double a = EuclideanDistance(v1, v2);
            double c = EuclideanDistance(v3, v2);
            double angle = Math.Acos((Math.Pow(a, 2) + Math.Pow(b, 2) - Math.Pow(c, 2)) / (2 * a * b));

            if (Orientation(v3, v1, v2) < 0)
                angle = 360 - angle;

            return angle;
        }

        private static double EuclideanDistance(Vertex v1, Vertex v2)
        {
            return Math.Sqrt(Math.Pow((v1.X - v2.X), 2) + Math.Pow((v1.Y - v2.Y), 2));
        }

        public static double Orientation(Vertex p1, Vertex p2, Vertex p)
        {
            double Orin = (p2.X - p1.X) * (p.Y - p1.Y) - (p.X - p1.X) * (p2.Y - p1.Y);
            if (Orin > 0)
                return -1;//Left
            if (Orin < 0)
                return 1;//Right
            return 0;//Colinier
        }
    } //end class

    public class Polygon
    {

        private Vertex[] vs;

        public Polygon(Vertex[] Vertexes)
        {
            vs = Vertexes;
        }

        public Polygon(Bounds bounds)
        {
            vs = bounds.ToArray();
        }

        public Vertex[] ToArray()
        {
            return vs;
        }

        public IEnumerable<Edge> Edges()
        {
            if (vs.Length > 1)
            {
                Vertex P = vs[0];
                for (int i = 1; i < vs.Length; i++)
                {
                    yield return new Edge(P, vs[i]);
                    P = vs[i];
                }
                yield return new Edge(P, vs[0]);
            }
        }

        public bool Contains(Vertex v)
        {
            return false;
        }
    }

    public class Edge
    {
        public Vertex A = new Vertex(0, 0);
        public Vertex B = new Vertex(0, 0);
        public Edge() { }
        public Edge(Vertex a, Vertex b)
        {
            A = a;
            B = b;
        }
        public Edge(double ax, double ay, double bx, double by)
        {
            A = new Vertex(ax, ay);
            B = new Vertex(bx, by);
        }
    }

    public class Bounds
    {
        public Vertex TopLeft;
        public Vertex TopRight;
        public Vertex BottomLeft;
        public Vertex BottomRight;
        public Bounds() { }

        public Bounds(Vertex TL, Vertex TR, Vertex BL, Vertex BR)
        {
            TopLeft = TL;
            TopRight = TR;
            BottomLeft = BL;
            BottomRight = BR;
        }

        public Vertex[] ToArray()
        {
            return new Vertex[] { TopLeft, TopRight, BottomRight, BottomLeft };
        }

    }

    public class Vertex 
    {
        public double X = 12;
        public double Y = 15;

        public Vertex() { }
        public Vertex(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vertex[] Convert(string vs)
        {
            vs = vs.Replace("[", "");
            vs = vs.Replace("]", "");
            string[] spl = vs.Split(';');
            List<Vertex> nvs = new List<Vertex>();
            foreach (string s in spl)
            {
                try
                {
                    nvs.Add(new Vertex(double.Parse(s, System.Globalization.CultureInfo.InvariantCulture), 0));
                }
                catch
                {

                }
            }
            return nvs.ToArray();
        }

        public static string Stringify(Vertex[] vs)
        {
            string res = "[";
            foreach (Vertex v in vs)
            {
                res += v.ToString();
                res += ";";
            }
            res = res.Remove(res.Length - 1);
            res += "]";
            return res;
        }

        public static string ToString(Vertex[] array)
        {
            string res = "[";
            foreach (Vertex v in array)
                res += v.ToString() + ",";
            return res.Remove(res.Length - 1) + "]";
        }

        /*
        //When x < y return -1
        //When x == y return 0
        //When x > y return 1
        public static int Compare(Vertex x, Vertex y)
        {
            //To find lowest
            if (x.X < y.X)
            {
                return -1;
            }
            else if (x.X == y.X)
            {
                if (x.Y < y.Y)
                {
                    return -1;
                }
                else if (x.Y == y.Y)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }
        */
        public static int CompareY(Vertex a, Vertex b)
        {
            if (a.Y < b.Y)
                return -1;
            if (a.Y == b.Y)
                return 0;
            return 1;
        }

        public static int CompareX(Vertex a, Vertex b)
        {
            if (a.X < b.X)
                return -1;
            if (a.X == b.X)
                return 0;
            return 1;
        }

        public double distance(Vertex b)
        {
            double dX = b.X - this.X;
            double dY = b.Y - this.Y;
            return Math.Sqrt((dX * dX) + (dY * dY));
        }

        public double slope(Vertex b)
        {
            double dX = b.X - this.X;
            double dY = b.Y - this.Y;
            return dY / dX;
        }

        public static int Compare(Vertex u, Vertex a, Vertex b)
        {
            if (a.X == b.X && a.Y == b.Y) return 0;

            Vertex upper = new Vertex();
            Vertex p1 = new Vertex();
            Vertex p2 = new Vertex();
            upper.X = (u.X + 180) * 360;
            upper.Y = (u.Y + 90) * 180;
            p1.X = (a.X + 180) * 360;
            p1.Y = (a.Y + 90) * 180;
            p2.X = (b.X + 180) * 360;
            p2.Y = (b.Y + 90) * 180;
            if (p1 == upper) return -1;
            if (p2 == upper) return 1;

            double m1 = upper.slope(p1);
            double m2 = upper.slope(p2);

            if (m1 == m2)
            {
                return p1.distance(upper) < p2.distance(upper) ? -1 : 1;
            }

            if (m1 <= 0 && m2 > 0) return -1;

            if (m1 > 0 && m2 <= 0) return -1;

            return m1 > m2 ? -1 : 1;
        }

        public static Vertex UpperLeft(Vertex[] vs)
        {
            Vertex top = vs[0];
            for (int i = 1; i < vs.Length; i++)
            {
                Vertex temp = vs[i];
                if (temp.Y > top.Y || (temp.Y == top.Y && temp.X < top.X))
                {
                    top = temp;
                }
            }
            return top;
        }

    }

}
