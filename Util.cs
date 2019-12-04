using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Cryptography;


namespace Advent
{
    public class Util
    {
        public static string[] Split(string input)
        {
            int commaCount = input.Count( c => c == ',');
            int linefeedCount = input.Count( c => c == '\n');
            if (linefeedCount > commaCount)
            {
                return input.Split("\n").Where(x => !string.IsNullOrEmpty(x)).ToArray();
            }
            else
            {
                return input.Split(",").Select(e => e.Replace("\n","")).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            }
        }

        public static int[] Parse(string input) => Parse(Split(input));

        public static int[] Parse(string[] input) => input.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => Int32.Parse(s)).ToArray();

        public static string GetInput(IPuzzle puzzle) => System.IO.File.ReadAllText(System.IO.Path.Combine("Data",puzzle.Name+".txt")).Replace("\r","");   
    
        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }

    public static class Extensions
    {
        public static string[] args;

        public static bool ShouldRun(this IPuzzle puzzle)
        {
            if (args.Length == 0) return true;

            foreach (var line in args)
            {
                if (puzzle.Name.Contains(line.Trim())) return true;
            }

            return false;
        }

        public static long TimeRun(this IPuzzle puzzle)
        {
            var watch = new System.Diagnostics.Stopwatch();        
            watch.Start();
            Console.WriteLine(puzzle.Name);
            var input = Util.GetInput(puzzle);
            puzzle.Run(input);
            return watch.ElapsedMilliseconds;
        }

        public static void IncrementAtIndex<T>(this Dictionary<T,int> dict, T key)
        {
            if (dict.ContainsKey(key))
            {
                dict[key]++;
            }
            else
            {
                dict[key] = 1;
            }
        }
    }
}
