using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;

namespace AirportParser
{
    public static class TowerHeightIncrease
    {
        public static void IncreaseHeight()
        {
            //will increase height at path  

            List<string> RegionsList = new List<string> { "Amsterdam", "Caribbean", "Charlotte", "Chicago", "Denver", "London", "NewYork", "Oshkosh", "Paris", "SanFrancisco", "Seattle", "SoCal", "SouthFlorida", "Sydney", "Singapore-KualaLumpur", "Hawaii" };

            for (int RL = 0; RL < RegionsList.Count; RL++)
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
                //search for ^14 
                string line = data[z];
                if (Regex.IsMatch(line, "^14 "))
                {
                    Console.WriteLine("\n\n");
                    Console.WriteLine("{0}", Path);

                    //check difference
                    Dictionary<string, double> DictToReturn = new Dictionary<string, double>();

                    if (line.StartsWith("14 "))
                    {
                        //found the tower
                        //example - '14   50.75734806  004.77850833    0 0 ATC Tower'
                        Console.WriteLine("Tower: {0}", line);


                        string OrigLat = line.Substring(3, 13);
                        string OrigLng = line.Substring(17, 13);
                        string OrigHeight = line.Substring(30, 5);

                        float Latitude = float.Parse(OrigLat.Trim(), CultureInfo.InvariantCulture.NumberFormat);
                        float Longitude = float.Parse(OrigLng.Trim(), CultureInfo.InvariantCulture.NumberFormat);
                        float Height = float.Parse(OrigHeight.Trim(), CultureInfo.InvariantCulture.NumberFormat);
                        Console.WriteLine("{0} / {1}, {2} / {3}, {4}", Height, Latitude, Longitude, OrigLat, OrigLng);

                        if (Height == 0)
                        {
                            //found a tower with 0 height
                            Console.Write("Found 0 height airport! ({0})", Path);

                            //construct new string
                            string NewLine = ("14 " + OrigLat + " " + OrigLng + " " + "0030 0 Tower");
                            Console.WriteLine("\nNew string: {0} \n\n", NewLine);

                            data[z] = NewLine;

                            WriteToFile(Path, data);
                        }

                        return;

                    }

                }

                if (z >= (data.Length - 1))
                {
                    //last one.
                    Console.WriteLine("Tower doesn't exist at this airport. ({0})", Path);
                    return;

                }

            }
        }

        //save airport to file
        public static void WriteToFile(string Path, string[] data)
        {

            //write to file
            System.IO.File.WriteAllLines(Path, data);

        }
    }
}
    
