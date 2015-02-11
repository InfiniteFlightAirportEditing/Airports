/*
 * Script for calculating points around 2 LatLng points (runway)
 * Used before creating a convex hull
 * By Cameron Carmichael Alonso
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AirportParser
{
    class CreateBoundingBox
    {
        public static double EARTH_RADIUS = 6371.0;

        public static List<PointF> CalculateBoundingBox(List<PointF> Points, float RunwayHeading)
        {
            List<PointF> PointsToReturn = new List<PointF>();

            List<double> PointAngles = new List<double> {45.0, 135.0, 225.0, 315.0};

            for (int i = 0; i < 4; i++)
            {
                double headingOriginal = RunwayHeading - (PointAngles[i]);
                double distanceOriginal = 0.5;
                double distance = distanceOriginal /= EARTH_RADIUS;
                double heading = ((Math.PI / 180) * headingOriginal);
                Console.WriteLine("Calculating with heading " + headingOriginal + " and distance " + distanceOriginal);

                for (int y = 0; y < Points.Count; y++) {

                    double fromLat = ConvertToRadians(Points[y].X);
                    double fromLng = ConvertToRadians(Points[y].Y);
                    double cosDistance = Math.Cos(distance);
                    double sinDistance = Math.Cos(distance);
                    double sinFromLat = Math.Sin(fromLat);
                    double cosFromLat = Math.Cos(fromLat);
                    double sinLat = cosDistance * sinFromLat + sinDistance * cosFromLat * Math.Cos(heading);
                    double dLng = Math.Atan2(
                            sinDistance * cosFromLat * Math.Sin(heading),
                            cosDistance - sinFromLat * sinLat);

                    float ArcSinLat = ConvertToFloat(Math.Asin(sinLat));

                    if (!(ArcSinLat != ArcSinLat))
                    {
                        PointF Point = new PointF();
                        Point.X = ConvertToDegrees(ArcSinLat);
                        Point.Y = ConvertToDegrees(ConvertToFloat(fromLng + dLng));
                        PointsToReturn.Add(Point);
                        Console.WriteLine("Added point: " + Point.X + ", " + Point.Y);

                    }

                }
            }

            return PointsToReturn;
        }

        public static float ConvertToRadians(float angle)
        {
            return (ConvertToFloat(Math.PI) / 180) * angle;
        }

        public static float ConvertToDegrees(float angle)
        {
            return angle / (ConvertToFloat(Math.PI) / 180);
        }

        public static float ConvertToFloat(double doubleValue)
        {
            return float.Parse(doubleValue.ToString(), System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }

    }

}
