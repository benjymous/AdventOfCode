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

        static bool IsHash(int num, string baseStr, string prefix)
        {
            string tryHash = baseStr + num.ToString();
            string hashed = tryHash.GetMD5String();
            return hashed.StartsWith(prefix);
        }
 
        public static IEnumerable<int> Forever()
        {
            int i=0;
            while(true) { yield return i++; }
        }

        public static int FindHash2(string baseStr, int numZeroes)
        {
            var prefix = String.Join("", Enumerable.Repeat('0', numZeroes));
            int num=0;
            while (true)
            {
                if (IsHash(num, baseStr, prefix)) return num;
                num++;
            }
        }

        public static int FindHash(string baseStr, int numZeroes)
        {
            var prefix = String.Join("", Enumerable.Repeat('0', numZeroes));
            
            return Forever().Where(n => IsHash(n, baseStr, prefix)).First();
        }

        public static int Part1(string input)
        {
            return FindHash(input.Trim(), 5);
        }

        public static int Part2(string input)
        {
            return FindHash(input.Trim(), 6);
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