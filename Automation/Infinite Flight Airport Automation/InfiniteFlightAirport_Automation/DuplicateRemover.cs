using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AirportParser
{
    class DuplicateRemover
    {

        public static void RemoveDuplicateFixes()
        {

            string path = @"/Users/Cameron/Documents/InfiniteFlight/Airports/Navigation/earth_fix.dat";
            string pathN = @"/Users/Cameron/Documents/InfiniteFlight/Airports/Navigation/earth_fix2.dat";
            List<string> data = new List<string>(System.IO.File.ReadAllLines(path));

            var firstFixes = new List<string>();

            //remove duplicates
            for (int i = 2; i < data.Count; i++)
            {
                //11967 is end of first part
                //11971 is start of second part

                string current = data[i];

                if (i <= 111967)
                {
                    //first part

                    try
                    {
                        string name = current.Substring(26, 5);
                        //Console.WriteLine(name);
                        
                        firstFixes.Add(name);
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        
                    }
                        

                } else
                {
                    //second part

                    try
                    {
                        string name = current.Substring(23, 5);
                        //Console.WriteLine(name);

                        //check if exists in array:
                        if (firstFixes.Contains(name))
                        {
                            Console.WriteLine("Found duplicate: {0}", name);
                           data.RemoveAt(i);

                        }

                        //add 3
                        data[i].Insert(0, "   ");

                    }
                    catch (System.ArgumentOutOfRangeException)
                    {

                    }

                }

            }

            //write to file
            System.IO.File.WriteAllLines(pathN, data);

            Console.WriteLine("Done");

        }


    }


}
