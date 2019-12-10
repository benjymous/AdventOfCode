using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Advent.MMXV
{
    public class Day04 : IPuzzle
    {
        public string Name { get { return "2015-04";} }

        // public string 
 
        public static IEnumerable<int> FindHashes(string baseStr, int numZeroes)
        {
            var prefix = String.Join("", Enumerable.Repeat('0', numZeroes));
            int num=0;
            while (true)
            {
                string tryHash = baseStr + num.ToString();
                string hashed = tryHash.GetMD5String();
                if (hashed.StartsWith(prefix))
                {
                    yield return num;
                }
                num++;
            }
        }

        public static int Part1(string input)
        {
            return FindHashes(input.Trim(), 5).First();
        }

        public static int Part2(string input)
        {
            return FindHashes(input.Trim(), 6).First();
        }

        public void Run(string input, System.IO.TextWriter console)
        {
            //Console.WriteLine("abcdef609043".GetMD5String());
            //Console.WriteLine(FindHashes("abcdef", 5).First());

            console.WriteLine("- Pt1 - "+Part1(input));
            console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}