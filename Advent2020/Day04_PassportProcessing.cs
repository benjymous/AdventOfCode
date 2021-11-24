using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2020
{
    public class Day04 : IPuzzle
    {
        public string Name { get { return "2020-04"; } }

        private static IEnumerable<Dictionary<string, string>> ParseData(string input, bool validate)
        {
            return input.Split("\n\n")
                .Select(line => (line.Replace("\n", " ").Trim()).Split(" "))
                .Select(entries => entries.Select(v => v.Split(":"))
                .Where(pair => validate == false || ValidateEntry(pair[0],pair[1]))
                .ToDictionary(pair => pair[0], pair => pair[1]));
        }

        static readonly HashSet<string> eyeCols = new HashSet<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
        private static bool ValidateEntry(string key, string val)
        {
            try
            {
                switch (key)
                {
                    case "byr":
                        // Birth Year - four digits; at least 1920 and at most 2002.
                        {
                            var year = int.Parse(val);
                            return year >= 1920 && year <= 2002;
                        }

                    case "iyr":
                        // Issue Year - four digits; at least 2010 and at most 2020.
                        {
                            var year = int.Parse(val);
                            return year >= 2010 && year <= 2020;
                        }

                    case "eyr":
                        // Expiration Year - four digits; at least 2020 and at most 2030.
                        {
                            var year = int.Parse(val);
                            return year >= 2020 && year <= 2030;
                        }

                    case "hgt":
                        // Height - a number followed by either cm or in:
                        {
                            var height = int.Parse(val.Substring(0, val.Length - 2));
                            var unit = val.Substring(val.Length - 2);
                            switch (unit)
                            {
                                case "cm":
                                    // If cm, the number must be at least 150 and at most 193.
                                    return height >= 150 && height <= 193;
                                case "in":
                                    // If in, the number must be at least 59 and at most 76.
                                    return height >= 59 && height <= 76;
                            }

                            return false;
                        }

                    case "hcl":
                        // Hair Color - a # followed by exactly six characters 0-9 or a-f.
                        {
                            val = val.ToLower();
                            if (val[0] != '#') return false;
                            if (val.Length != 7) return false;
                            for (var i = 1; i < 7; ++i)
                            {
                                if (val[i] < '0') return false;
                                if (val[i] > 'f') return false;
                                if (val[i] > '9' && val[i] < 'a') return false;
                            }
                            return true;
                        }

                    case "ecl":
                        // Eye Color - exactly one of: amb blu brn gry grn hzl oth.
                        {
                            return eyeCols.Contains(val);
                        }

                    case "pid":
                        // Passport ID - a nine-digit number, including leading zeroes
                        {
                            if (val.Length != 9) 
                                return false;
                            foreach (var c in val)
                            {
                                if (c < '0') 
                                    return false;
                                if (c > '9') 
                                    return false;
                            }
                            return true;
                        }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        static readonly string[] expectedFields = new string[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" /*, "cid"*/ };

        private static int CountValid(IEnumerable<Dictionary<string, string>> records)
        {            
            return records.Where(r => r.Keys.Intersect(expectedFields).Count() == expectedFields.Count()).Count();
        }

        public static int Part1(string input)
        {
            return CountValid(ParseData(input, false));
        }



        public static int Part2(string input)
        {
            return CountValid(ParseData(input, true));
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}