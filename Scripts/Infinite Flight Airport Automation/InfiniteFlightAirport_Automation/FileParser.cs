using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AirportParser
{
    public class FileParser
    {

        public static Dictionary<string, string> ParseContent(string Content, bool New)
        {

            if ((Regex.IsMatch(Content, "^111 ")) || (Regex.IsMatch(Content, "^112 ")) || (Regex.IsMatch(Content, "^113 ")) || (Regex.IsMatch(Content, "^114 ")) || (Regex.IsMatch(Content, "^115 ")) || (Regex.IsMatch(Content, "^116 ")) || (Regex.IsMatch(Content, "^10 "))) //111-116 are current nodes. 10 is a deprecated yet common factor for a taxiway from 750 version
            {
                //is node
                Dictionary<string, string> dict = ParseNode(Content);
                Console.WriteLine("LatLng dict for node: " + dict["Latitude"] + ", " + dict["Longitude"] + '\n');

                return dict;
            }
            else if (Regex.IsMatch(Content, "^100 "))
            {
                //is runway - check if new

                if (New == true)
                {
                    Dictionary<string, string> dict = ParseRunwayNew(Content);
                    Console.WriteLine("LatLng dict for runway: " + dict["Latitude1"] + ", " + dict["Longitude1"] + "; " + dict["Latitude2"] + ", " + dict["Longitude2"] + '\n');

                    return dict;
                }
                else
                {

                    Dictionary<string, string> dict = ParseRunway(Content);
                    Console.WriteLine("LatLng dict for runway: " + dict["Latitude1"] + ", " + dict["Longitude1"] + "; " + dict["Latitude2"] + ", " + dict["Longitude2"] + '\n');

                    return dict;

                }
            }
            {
                //something else not defined - return nil
                return null;

            }
                

        }

        public static Dictionary<string, string> ParseRunway(string Runway)
        {
            string Latitude1 = Runway.Substring (29, 12);
			string Longitude1 = Runway.Substring (42, 12);
            string Latitude2 = Runway.Substring(77, 12);
            string Longitude2 = Runway.Substring(90, 12);
            

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("Latitude1", Latitude1);
            dictionary.Add("Longitude1", Longitude1);
            dictionary.Add("Latitude2", Latitude2);
            dictionary.Add("Longitude2", Longitude2);
            dictionary.Add("Format", "100");

            return dictionary;

        }

        public static Dictionary<string, string> ParseRunwayNew(string Runway)
        {
            string Latitude1 = Runway.Substring(34, 12);
            string Longitude1 = Runway.Substring(48, 12);
            string Latitude2 = Runway.Substring(90, 12);
            string Longitude2 = Runway.Substring(104, 12);
            string RunwayName = Runway.Substring(32, 3);

            string RunwayNumber = Regex.Replace(RunwayName, "[^0-9.]", "");

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("Latitude1", Latitude1);
            dictionary.Add("Longitude1", Longitude1);
            dictionary.Add("Latitude2", Latitude2);
            dictionary.Add("Longitude2", Longitude2);
            dictionary.Add("Format", "100");
            dictionary.Add("RunwayNumber", RunwayNumber);

            return dictionary;

        }

        public static Dictionary<string, string> ParseNode(string Runway)
        {

            string Latitude = Runway.Substring(3, 12);
            string Longitude = Runway.Substring(17, 12);

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("Latitude", Latitude);
            dictionary.Add("Longitude", Longitude);
            dictionary.Add("Format", "Other");

            return dictionary;

        }


    }
}
