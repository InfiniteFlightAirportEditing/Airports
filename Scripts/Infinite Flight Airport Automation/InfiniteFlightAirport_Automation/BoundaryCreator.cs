/*
 * Script for adding boundary to airports
 * Boundary created based on outer coordinates
 * By Cameron Carmichael Alonso
*/

using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace AirportParser
{

    public static class BoundaryCreator
    {

        public static void CreateBoundary()
        {
            //list regions
            List<string> RegionsList = new List<string> { "Amsterdam", "Caribbean", "Charlotte", "Chicago", "Denver", "London", "NewYork", "Oshkosh", "Paris", "SanFrancisco", "Seattle", "SoCal", "SouthFlorida", "Sydney" };

            for (int RL = 0; RL < RegionsList.Count; RL++ )
            {

                string RPath = (@"C:\Users\Cameron\Documents\InfiniteFlight\Airports\" + RegionsList[RL]);

                DirectoryInfo dInfo = new DirectoryInfo(RPath);
                DirectoryInfo[] subdirs = dInfo.GetDirectories();

                for (int Dir = 0; Dir < subdirs.Length; Dir++)
                {
                    //get individual directories
                    DirectoryInfo DirectoryAtIndex = subdirs[Dir];
                    
                    string apt = (RPath + '\\' + DirectoryAtIndex.Name + "\\apt.dat");
                    LoadFile(apt);


                }

            }
        }

        static void LoadFile(string Path)
        {
            string[] data = System.IO.File.ReadAllLines(Path);

            for (int i = 0; i < data.Length; i++)
            {
                //search for ^130
                string Line = data[i];
                if (Regex.IsMatch(Line, "^130 "))
                {
                    //found boundary
                    break;

                    //uncomment to check differences

                }

                if (i == (data.Length - 1))
                {
                    //final one, hasn't broken yet 
                    GetAllCoordinates(data, Path);
                }

            }


        }

        static void GetAllCoordinates(string[] Lines, string Path)
        {

            List<PointF> PointsEnum = new List<PointF>();
            Dictionary<string, float> CoordinateDictionary = new Dictionary<string, float>();
            double RunwayHeadingDouble = new double();

            //loop through each line and return various coordinates
            for (int i = 0; i < Lines.Length; i++)
            {
                string line = Lines[i];
                Console.WriteLine("Line " + i + ": " + line); //print line contents

                Dictionary<string, string> CurrentDictionary = FileParser.ParseContent(line, false);

                if (!(CurrentDictionary == null))
                {
                    if (CurrentDictionary["Format"] == "100")
                    {
                        //runway - has 2 points

                        try
                        {

                            Dictionary<string, string> CurrentDictionaryNew = FileParser.ParseContent(line, true);
                            float Latitude1 = float.Parse(CurrentDictionaryNew["Latitude1"], CultureInfo.InvariantCulture.NumberFormat);
                            float Longitude1 = float.Parse(CurrentDictionaryNew["Longitude1"], CultureInfo.InvariantCulture.NumberFormat);

                            PointF p1 = new PointF();
                            p1.X = Latitude1;
                            p1.Y = Longitude1;

                            PointsEnum.Add(p1);

                            float Latitude2 = float.Parse(CurrentDictionaryNew["Latitude2"], CultureInfo.InvariantCulture.NumberFormat);
                            float Longitude2 = float.Parse(CurrentDictionaryNew["Longitude2"], CultureInfo.InvariantCulture.NumberFormat);

                            PointF p2 = new PointF();
                            p2.X = Latitude2;
                            p2.Y = Longitude2;

                            PointsEnum.Add(p2);

                            RunwayHeadingDouble = Convert.ToDouble(CurrentDictionaryNew["RunwayNumber"]);

                            Console.WriteLine("Added 2 new points");


                        }
                        catch (System.FormatException)
                        {
                            /*//exception - uses Older format
                            Console.WriteLine("Older format");


                            float Latitude1 = float.Parse(CurrentDictionary["Latitude1"], CultureInfo.InvariantCulture.NumberFormat);
                            float Longitude1 = float.Parse(CurrentDictionary["Longitude1"], CultureInfo.InvariantCulture.NumberFormat);

                            PointF p1 = new PointF();
                            p1.X = Latitude1;
                            p1.Y = Longitude1;

                            PointsEnum.Add(p1);

                            float Latitude2 = float.Parse(CurrentDictionary["Latitude2"], CultureInfo.InvariantCulture.NumberFormat);
                            float Longitude2 = float.Parse(CurrentDictionary["Longitude2"], CultureInfo.InvariantCulture.NumberFormat);

                            PointF p2 = new PointF();
                            p2.X = Latitude2;
                            p2.Y = Longitude2;

                            PointsEnum.Add(p2);
                            */
                        }
                    }
                    else
                    {
                        //not rwy - only 1 point
                        
                        float Latitude = float.Parse(CurrentDictionary["Latitude"], CultureInfo.InvariantCulture.NumberFormat);
                        float Longitude = float.Parse(CurrentDictionary["Longitude"], CultureInfo.InvariantCulture.NumberFormat);

                        PointF p = new PointF();
                        p.X = Latitude;
                        p.Y = Longitude;

                        PointsEnum.Add(p);

                    }
                }

                if (line == "99")
                {

                    //is a 99 - footer. delete and read outside of if
                    Lines[i] = "";

                }
                else if (Regex.IsMatch(line, "^1000 Ge")) 
                {

                    //is a header, append /AB (automatic boundary)
                    Lines[i] = (Lines[i] + "/AB");

                }

                if (i == (Lines.Length - 1))
                {

                    //check number of points - if there are 2, then it's a single runway apt with no edits made. Interpolate an area 5m around the airfield and add points
                    if (PointsEnum.Count <= 2)
                    {
                        //commented out - creates a massive triangle
                        /*float RunwayHeadingFloat = ConvertToFloat(RunwayHeadingDouble * 100);
                        List<PointF> ReturnedList = CreateBoundingBox.CalculateBoundingBox(PointsEnum, RunwayHeadingFloat);

                        for (int x = 0; x < ReturnedList.Count; x++)
                        {
                            PointsEnum.Add(ReturnedList[x]);

                        }*/

                    }


                    //finished for loop. Create Convex Hull
                    PointF[] ReturnedPoints = CreateConvexHull.ComputeConvexHull(PointsEnum);
                    Console.WriteLine("Returned convex hull: " + ReturnedPoints);
                    for (int y = 0; y < ReturnedPoints.Length; y++)
                    {

                        PointF point = ReturnedPoints[y];
                        Console.WriteLine("Point at coordinates: " + point.X + ", " + point.Y);

                    }
                    WriteToFile(Lines, ReturnedPoints, Path);


                }
            }
        }

        static void WriteToFile(string[] Lines, PointF[] Points, string Path)
        {

            //add items in string[] to List<string>
            List<string> FileContent = new List<string>();
            for (int i = 0; i < Lines.Length; i++)
            {
                FileContent.Add(Lines[i]);
            }

            //add lines with points to end
            FileContent.Add("130 Airport Boundary");
            for (int i = 0; i < Points.Length; i++)
            {
                PointF PAtIndex = Points[i];
                double Latitude = Convert.ToDouble(PAtIndex.X);
                double Longitude = Convert.ToDouble(PAtIndex.Y);

                string LatString = Latitude.ToString("000.00000000");
                string LngString = Longitude.ToString("000.00000000");

                //construct line
                if (i == (Points.Length - 1))
                {
                    //final i - set closed loop 113

                    if (Latitude < 0)
                    {

                        if (Longitude < 0)
                        {

                            //both are negative
                            string Line = ("113 " + LatString + " " + LngString);
                            FileContent.Add(Line);

                        }
                        else
                        {

                            //lat is negative, lng is positive
                            string Line = ("113 " + LatString + "  " + LngString);
                            FileContent.Add(Line);
                        }

                    }
                    else
                    {

                        if (Longitude < 0)
                        {

                            //lat is positive, lng is negative
                            string Line = ("113  " + LatString + " " + LngString);
                            FileContent.Add(Line);

                        }
                        else
                        {

                            //both are positive
                            string Line = ("113  " + LatString + "  " + LngString);
                            FileContent.Add(Line);
                        }

                    }

                }
                else
                {

                    //not final - set 111
                    //final i - set closed loop 113

                    if (Latitude < 0)
                    {

                        if (Longitude < 0)
                        {

                            //both are negative
                            string Line = ("111 " + LatString + " " + LngString);
                            FileContent.Add(Line);

                        }
                        else
                        {

                            //lat is negative, lng is positive
                            string Line = ("111 " + LatString + "  " + LngString);
                            FileContent.Add(Line);
                        }

                    }
                    else
                    {

                        if (Longitude < 0)
                        {

                            //lat is positive, lng is negative
                            string Line = ("111  " + LatString + " " + LngString);
                            FileContent.Add(Line);

                        }
                        else
                        {

                            //both are positive
                            string Line = ("111  " + LatString + "  " + LngString);
                            FileContent.Add(Line);
                        }


                    }


                }

            }

            //for loops finished, move 99, 50, 51, 52, 53, 54, 55, 56 to bottom.

                FileContent.Add("99");
                System.IO.File.WriteAllLines(Path, FileContent);

        }

        public static float ConvertToFloat(double doubleValue)
        {
            return float.Parse(doubleValue.ToString(), System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }

    }
}