//Code by Cameron Alonso and Imran Ahmed

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Airport_Parser
{
    public class Parking:IComparable<Parking>
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public double Heading { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string[] Aircraft  { get; set; }
        public int CompareTo(Parking another)
        {
            return Title.CompareTo(another.Title);
        }
        public void PrintComplete()
        {
            if (Type == null)
            {
                Console.WriteLine("Parking spot: {0} with Heading: {1} , Latitude: {2} and Longitude: {3}", Title, Heading.ToString(), Latitude.ToString(), Longitude.ToString());
            }
            else
            {
                Console.WriteLine("Parking spot: {0} with Heading: {1} , Latitude: {2} and Longitude: {3}. Space is a {4} and takes {5}", Title, Heading.ToString(), Latitude.ToString(), Longitude.ToString(),Type,String.Join(",",Aircraft));
            }
          }
        public void PrintTitle()
        {
            // "Parking spot: {0}"
            Console.WriteLine("{0}", Title);
        }
    }
    public class TerminalCollection:IComparable<TerminalCollection>
    {
        public string Name { get; set; }
        public List<Parking> Slots { get; set; }
        public int CompareTo(TerminalCollection another)
        {
            return Name.CompareTo(another.Name);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //Instantiate Airport Data list
            List<Parking> Airport = new List<Parking>();
            //Read file on desktop
            string[] data = System.IO.File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\apt.dat");
            foreach (string line in data)
            {
                if (RawtoParking(line) != null)
                {
                    Airport.Add(RawtoParking(line));
                }
            }
            Airport.Sort();
            List<TerminalCollection> sorted = new List<TerminalCollection>();
            sorted = Terminals(Airport);
            foreach (TerminalCollection terminal in sorted)
            {
                Console.WriteLine(terminal.Name);
                Console.WriteLine("");
                foreach (Parking i in terminal.Slots)
                {
                    i.PrintTitle();
                }
                Console.WriteLine("");
            }
            Console.ReadKey();
        }
        //Converts Raw String to "Parking" Object
        private static Parking RawtoParking(string input)
        {
            if (Regex.IsMatch(input, "^15  "))
            {
                input = input.Remove(0, "^15  ".Length);
                float latitude = float.Parse(Regex.Match(input, @"[-+]?\d*\.\d* +").Value, System.Globalization.CultureInfo.InvariantCulture);
                input = input.Remove(0, Regex.Match(input, @"[-+]?\d*\.\d* +").Value.Length);
                float longitude = float.Parse(Regex.Match(input, @"[-+]?\d*\.\d* +").Value, System.Globalization.CultureInfo.InvariantCulture);
                input = input.Remove(0, Regex.Match(input, @"[-+]?\d*\.\d* +").Value.Length);
                double heading = double.Parse(Regex.Match(input, @"[-+]?\d*\.\d* +").Value, System.Globalization.CultureInfo.InvariantCulture);
                input = input.Remove(0, Regex.Match(input, @"[-+]?\d*\.\d* +").Value.Length);
                Parking output = new Parking();
                output.Latitude = latitude;
                output.Longitude = longitude;
                output.Heading = heading;
                output.Title = input.Trim();
                return output;
            }
            else if (Regex.IsMatch(input, "^1300  "))
            {                
                Parking output = new Parking();
                input = input.Remove(0, "^1300  ".Length);
                float latitude = float.Parse(Regex.Match(input, @"[-+]?\d*\.\d* +").Value, System.Globalization.CultureInfo.InvariantCulture);
                input = input.Remove(0, Regex.Match(input, @"[-+]?\d*\.\d* +").Value.Length);
                float longitude = float.Parse(Regex.Match(input, @"[-+]?\d*\.\d* +").Value, System.Globalization.CultureInfo.InvariantCulture);
                input = input.Remove(0, Regex.Match(input, @"[-+]?\d*\.\d* +").Value.Length);
                double heading = double.Parse(Regex.Match(input, @"[-+]?\d*\.\d* +").Value, System.Globalization.CultureInfo.InvariantCulture);
                input = input.Remove(0, Regex.Match(input, @"[-+]?\d*\.\d* +").Value.Length);
                output.Type = Regex.Match(input, @"^[^\s]+ +").Value;
                input = input.Remove(0, output.Type.Length);
                output.Type = output.Type.Trim();
                string aircrafts = Regex.Match(input, @"^[^\s]+ +").Value;
                input = input.Remove(0, aircrafts.Length);
                aircrafts = aircrafts.Trim();
                output.Type = output.Type.Trim();
                output.Aircraft = aircrafts.Split('|');
                output.Latitude = latitude;
                output.Longitude = longitude;
                output.Heading = heading;
                output.Title = input.Trim();
                return output;
            }
            else
            {
                return null;
            }
        }
        //Compiles List of Terminals
        private static List<TerminalCollection> Terminals(List<Parking> input)
        {
            List<TerminalCollection> Terminals = new List<TerminalCollection>();
            //Populate an "Other" field for parking spaces that do not match a group
            TerminalCollection MiscellaneousGroup = new TerminalCollection();
            List<Parking> miscellaneous = new List<Parking>();
            MiscellaneousGroup.Name = "Other";
            MiscellaneousGroup.Slots = miscellaneous;
            Terminals.Add(MiscellaneousGroup);
            //Create dummy collections of items for grouping
            TerminalCollection CurrentTerminal = new TerminalCollection();
            List<Parking> currentcollection = new List<Parking>();
            string previousname = "";
            bool unlooped = true;
            foreach (Parking attempt in input)
            {
                string[] stubs = new string[2];
                stubs = GetStub(attempt.Title);
                if (unlooped)
                {
                    previousname = stubs[0];
                    attempt.Title = stubs[1];
                    currentcollection.Add(attempt);
                    CurrentTerminal.Name = previousname;
                    CurrentTerminal.Slots = currentcollection;
                    unlooped = false;
                }
                else
                {
                    if (stubs[0] == previousname && !(stubs[0]=="Other"))
                    {
                        attempt.Title = stubs[1];
                        currentcollection.Add(attempt);
                    }
                    else
                    {
                        if (stubs[0] == "Other")
                        {
                            attempt.Title = stubs[1];
                            miscellaneous.Add(attempt);
                        }
                        else
                        {
                            attempt.Title = stubs[1];
                            Terminals.Add(CurrentTerminal);
                            CurrentTerminal = new TerminalCollection();
                            currentcollection = new List<Parking>();
                            currentcollection.Add(attempt);
                            previousname = stubs[0];
                            CurrentTerminal.Name = previousname;
                            CurrentTerminal.Slots = currentcollection;
                        }
                    }
                }
            }
            Terminals.Add(CurrentTerminal);
            //Group Together Lone Terminals (Under "other")
            List<int> remove = new List<int>();
            for (int i=0; i<Terminals.Count; i++) {
                if (Terminals[i].Slots.Count == 1) {
                    Terminals[i].Slots[0].Title = Terminals[i].Name;
                    Terminals[0].Slots.Add(Terminals[i].Slots[0]);
                    remove.Add(i);
                }
                Terminals[i].Slots.Sort();
            }
            int current = 0;
            foreach (int i in remove) {
              Terminals.RemoveAt(i-current);
              current++;
            }
            //Clear 'Other' if it is empty
            if (Terminals[0].Slots.Count == 0)
            {
                Terminals.RemoveAt(0);
            }
            //Sort each Terminal
            foreach (TerminalCollection terminals in Terminals)
            {
                terminals.Slots.Sort();
            }
            return Terminals;
        }
        private static string[] GetStub(string input)
        {
            input = input.Trim();
            MatchCollection words = Regex.Matches(input, @"[\S]+");
            string[] output;
            output = new string[2];
            Match check = Regex.Match(input, @"\d+");
            // Goes through fomatting possibilites
            if (words.Count <= 1 && input.Contains("-") == false)
            {
                // Last categorization check (A21 -> Terminal A, Gate 21)
                if(Regex.IsMatch(input, @"[a-zA-Z]\d+.*$")) {
                    output[0] = "Terminal " + (input.Substring(0,Regex.Match(input,@"\d").Index)).ToUpper();
                    output[1] = "Gate " + (Regex.Match(input,@"\d+.*$").Value);
                } else {
                // If not, likely to be miscellaneous
                output[0] = "Other";
                output[1] = input;
                }
            } else
            if (check.Value!="" && (check.Index + check.Length) == input.Length) {
               string stub = input.Substring(0, Regex.Match(input, @"\d+").Index).Trim();
               if (stub.Substring(0, Regex.Match(stub, "[-,]", RegexOptions.RightToLeft).Index) != "")
               {
                   stub = stub.Substring(0, Regex.Match(stub, "[-,]", RegexOptions.RightToLeft).Index).Trim();
                   Match ending = Regex.Match(input.Trim(), "[-,]", RegexOptions.RightToLeft);
                   string endstub = input.Trim().Substring(ending.Index + 1, input.Length - ending.Index - 1).Trim();
                   output[0] = stub;
                   output[1] = endstub;
               }
               else
               {
                   string endstub = input.Substring(Regex.Match(input, @"\d+").Index, input.Length - Regex.Match(input, @"\d+").Index).Trim();
                   output[0] = stub;
                   output[1] = endstub;
               }
            }
            else if(Regex.IsMatch(input, ".*(?=[-,].*$)"))
            {
                Match nonumber = Regex.Match(input, ".*(?=[-,].*$)");
                Match ending = Regex.Match(input, "^(?:.*[-,]+)");
                string stub = input.Substring(0, nonumber.Value.Length).Trim();
                output[0] = stub;
                string endstub = (input.Substring(ending.Length, input.Length - ending.Length)).Trim();
                output[1] = endstub;
            }
            else if (Regex.IsMatch(input, @"\d+ "))
            {
                Match first = Regex.Match(input, @"\d+ ");
                string stub = input.Substring(0, first.Index + first.Value.Length);
                output[0] = stub.Trim();
                string endstub = input.Substring(first.Index + first.Value.Length, input.Length - (first.Index + first.Value.Length));
                output[1] = endstub.Trim();
            }
            else if (Regex.IsMatch(input, @"\d+[a-zA-z]$"))
            {
                Match first = Regex.Match(input, @"\d+[a-zA-z]$");
                string stub = input.Substring(0, first.Index);
                output[0] = stub.Trim();
                string endstub = input.Substring(first.Index, first.Value.Length);
                output[1] = endstub.Trim();
            }
            else
            {
                output[0] = input;
                output[1] = input;
            }
            //Correct Ramp & Stand Discrepancies by finding last-word
            string[] parts = output[0].Split(' ');
            string lastWord = parts[parts.Length - 1];
            if (lastWord.ToLower() == "ramp" && !(lastWord==parts[0]))
            {
                output[0] = output[0].Remove(output[0].Length - 4, 4);
                output[1] = "Ramp " + output[1].Trim();
            }
            if (lastWord.ToLower() == "stand" && !(lastWord==parts[0]))
            {
                output[0] = output[0].Remove(output[0].Length - 5, 5);
                output[1] = "Stand " + output[1].Trim();
            }
            //Code adds "Gate" to a lone number -> Can cause errors with "Runups" to Runways
            //if (Regex.IsMatch(output[1],@"^\d+[A-Za-z]*$"))
            //{
            //    output[1] = "Gate " + Regex.Match(output[1], @"\d+[A-Za-z]*$").Value;
            //}
            return output;
        }
        private static bool IsDigit(string input)
        {
            foreach (char c in input)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
      }
    }
