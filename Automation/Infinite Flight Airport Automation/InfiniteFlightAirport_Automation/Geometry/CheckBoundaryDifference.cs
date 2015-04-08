using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;

namespace AirportParser
{
    class CheckBoundaryDifference
    {

        public static void CheckBoundary() {

            List<string> RegionsList = new List<string> { "Amsterdam", "Caribbean", "Charlotte", "Chicago", "Denver", "London", "NewYork", "Oshkosh", "Paris", "SanFrancisco", "Seattle", "SoCal", "SouthFlorida", "Sydney", "Singapore-KualaLumpur", "Hawaii" };

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

            for (int z = 0; z < data.Length; z++)
            {
                //search for ^130
                string line = data[z];
                if (Regex.IsMatch(line, "^130 "))
                {

                    //check difference
                        Dictionary<string, double> DictToReturn = new Dictionary<string, double>();

                        if (line.StartsWith("130 "))
                        {
                            //found start of def

                            for (int y = (z + 1); y < data.Length; y++)
                            {

                                //search for final line

                                string endLine = data[y];

                                if (endLine.StartsWith("113 "))
                                {
                                    //found end line

                                    string[] Airport = new List<string>(data).GetRange(z, (y - z)).ToArray();
                                    List<double> Latitudes = new List<double>();
                                    List<double> Longitudes = new List<double>();

                                    Console.WriteLine("Reading " + Path);

                                    foreach (string obj in Airport)
                                    {
                                      

                                        if (obj.StartsWith("130 "))
                                        {


                                        }
                                        else
                                        {

                                            try
                                            {

                                                Dictionary<string, string> dict = FileParser.ParseNode(obj);
                                                double lat = Convert.ToDouble(dict["Latitude"]);
                                                double lng = Convert.ToDouble(dict["Longitude"]);

                                                Latitudes.Add(lat);
                                                Longitudes.Add(lng);

                                            }
                                            catch (System.FormatException)
                                            {


                                            }
                                        }

                                    }

                                    double[] lats = Latitudes.ToArray();
                                    double[] lngs = Longitudes.ToArray();

                                    double HighestLat = lats.Max();
                                    double HighestLng = lngs.Max();

                                    double HighestDiffLat = 0.0;
                                    double HighestDiffLng = 0.0;

                                    for (int ii = 0; ii < lats.Length; ii++)
                                    {

                                        //try max - ii
                                        double calc = HighestLat - lats[ii];

                                        if (calc > HighestDiffLat)
                                        {
                                            //larger value - assign
                                            HighestDiffLat = calc;
                                        }

                                    }

                                    for (int ii = 0; ii < lngs.Length; ii++)
                                    {

                                        //try max - ii
                                        double calc = HighestLng - lngs[ii];

                                        if (calc > HighestDiffLng)
                                        {
                                            //larger value - assign
                                            HighestDiffLng = calc;
                                        }

                                    }
                                    try
                                    {

                                        DictToReturn.Add("Lng", HighestDiffLng);
                                        DictToReturn.Add("Lat", HighestDiffLat);

                                        Dictionary<string, double> DiffDict = DictToReturn;
                                        double LatDiff = Convert.ToDouble(DiffDict["Lat"]);
                                        double LngDiff = Convert.ToDouble(DiffDict["Lng"]);

                                        Console.WriteLine(LatDiff + LngDiff);

                                        if ((LatDiff > 0.05) || (LngDiff > 0.05))
                                        {

                                            Console.WriteLine("Largest differences for airfield at path: " + Path + '\n');

                                        }

                                    }
                                    catch (System.ArgumentException)
                                    {


                                    }

                                }

                            }

                    }


                }

            }


        }


    }
}
