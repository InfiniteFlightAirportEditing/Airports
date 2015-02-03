/*
 * Script for adding boundary to airports
 * Boundary created based on outer coordinates
 * By Cameron Carmichael Alonso
*/

using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace AirportParser
{

    public static class BoundaryCreator
    {

        public static void CreateBoundary()
        {
            //get paths
            string path = (@"C:\Users\Cameron\Documents\InfiniteFlight\Airports\Denver\KPUB\apt.dat");
            LoadFile(path);

        }

        static void LoadFile(string Path)
        {
            string[] data = System.IO.File.ReadAllLines(Path); 
            GetAllCoordinates(data);

        }

        static void GetAllCoordinates(string[] Lines)
        {

            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();

            Dictionary<string, float> CoordinateDictionary = new Dictionary<string, float>();

            //loop through each line and return various coordinates
            for (int i = 0; i < Lines.Length; i++) {
                string line = Lines[i];
                Console.WriteLine("Line " + i + ": " + line); //print line contents

                Dictionary<string, string> CurrentDictionary = FileParser.ParseContent(line);

                

            }
                            
            Vertex[] vertex = new Vertex[]();
            CreateConcave.ConcaveHull(vertex);

        } 

    }

    public class CoordinateWorker
    {

       /* public static GeoCoordinate GetCentralGeoCoordinate(
       IList<GeoCoordinate> geoCoordinates)
        {
            if (geoCoordinates.Count == 1)
            {
                return geoCoordinates.Single();
            }

            double x = 0;
            double y = 0;
            double z = 0;

            foreach (var geoCoordinate in geoCoordinates)
            {
                var latitude = geoCoordinate.Latitude * Math.PI / 180;
                var longitude = geoCoordinate.Longitude * Math.PI / 180;

                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
            }

            var total = geoCoordinates.Count;

            x = x / total;
            y = y / total;
            z = z / total;

            var centralLongitude = Math.Atan2(y, x);
            var centralSquareRoot = Math.Sqrt(x * x + y * y);
            var centralLatitude = Math.Atan2(z, centralSquareRoot);

            return new GeoCoordinate(centralLatitude * 180 / Math.PI, centralLongitude * 180 / Math.PI);
        }
        */
    }
}