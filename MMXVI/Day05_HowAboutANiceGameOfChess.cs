using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVI
{
    public class Day05 : IPuzzle
    {
        public string Name { get { return "2016-05";} }
 
        public static string CrackPassword1(string doorId)
        {
            var sb = new StringBuilder();
            int hashNumber = 0;

            while (sb.Length < 8)
            {
                hashNumber = HashBreaker.FindHash(doorId, 5, hashNumber+1);
                var hashString = HashBreaker.GetHashChars(hashNumber, doorId);
                char c = hashString.Skip(5).First();
                sb.Append(c);
            }

            return sb.ToString().ToLower();
        }

        public static string CrackPassword2(string doorId)
        {
            var outpass = "________".ToCharArray();
            int hashNumber = 0;

            while (outpass.Contains('_'))
            {
                hashNumber = HashBreaker.FindHash(doorId, 5, hashNumber+1);
                var hashString = HashBreaker.GetHashChars(hashNumber, doorId).Take(7).ToArray();
                char c = hashString[6];
                char p = hashString[5];

                int pos = p - '0';

                if (pos >=0 && pos <=7 && outpass[pos] == '_')
                {
                    outpass[pos] = c;
                }

                Console.WriteLine($"{hashNumber} [{pos}]:{c} {outpass.AsString()}");
            }

            return outpass.AsString().ToLower();
        }

        public static string Part1(string input) => CrackPassword1(input.Trim());

        public static string Part2(string input) => CrackPassword2(input.Trim());

        public void Run(string input, System.IO.TextWriter console)
        {
            //Console.WriteLine(CrackPassword("abc"));

            //Console.WriteLine(CrackPassword2("abc"));

            console.WriteLine("- Pt1 - "+Part1(input));
            console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}