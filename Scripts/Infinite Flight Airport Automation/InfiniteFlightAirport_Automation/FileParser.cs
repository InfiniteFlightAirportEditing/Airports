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

        public static Dictionary<string, string> ParseContent(string Content)
        {

            if ((Regex.IsMatch(Content, "^111 ")) || (Regex.IsMatch(Content, "^112 ")) || (Regex.IsMatch(Content, "^113 ")) || (Regex.IsMatch(Content, "^114 ")) || (Regex.IsMatch(Content, "^115 ")) || (Regex.IsMatch(Content, "^116 ")) || (Regex.IsMatch(Content, "^10 "))) //111-116 are current nodes. 10 is a deprecated yet common factor for a taxiway from 750 version
            {
                //is node
                Dictionary<string, string> dict = ParseNode(Content);
                Console.WriteLine("LatLng dict for node: " + dict + '\n');

                return dict;
            }
            else if (Regex.IsMatch(Content, "^100 "))
            {
                //is runway
                Dictionary<string, string> dict = ParseRunway(Content);
                Console.WriteLine("LatLng dict for runway: " + dict + '\n');

                return dict;
            }
            {
                //something else not defined - return nil
                return null;

            }
                

        }

        public static Dictionary<string, string> ParseRunway(string Runway)
        {
            string Latitude = Runway.Substring (35, 12);
			string Longitude = Runway.Substring (48, 12);

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("Latitude", Latitude);
            dictionary.Add("Longitude", Longitude);

            return dictionary;

        }

        public static Dictionary<string, string> ParseNode(string Runway)
        {

            string Latitude = Runway.Substring(4, 12);
            string Longitude = Runway.Substring(18, 12);

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("Latitude", Latitude);
            dictionary.Add("Longitude", Longitude);

            return dictionary;

        }


    }
}
