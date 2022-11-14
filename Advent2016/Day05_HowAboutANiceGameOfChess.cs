using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2016
{
    public class Day05 : IPuzzle
    {
        public string Name => "2016-05";

        public static string CrackPassword1(string doorId, ILogger logger)
        {
            var sb = new List<char>();
            int hashNumber = 0;
            while (sb.Count < 8)
            {
                hashNumber = HashBreaker.FindHash(doorId, 5, hashNumber + 1);
                var hashString = HashBreaker.GetHashChars(hashNumber, doorId);
                char c = hashString.Skip(5).First();
                sb.Add(c);
                logger?.WriteLine($"{c} {sb.AsString()}");
            }

            return sb.AsString().ToLower();
        }

        public static string CrackPassword2(string doorId, ILogger logger)
        {
            var outpass = "________".ToCharArray();
            int hashNumber = 0;

            while (outpass.Contains('_'))
            {
                hashNumber = HashBreaker.FindHash(doorId, 5, hashNumber + 1);
                var hashString = HashBreaker.GetHashChars(hashNumber, doorId).Skip(5).Take(2).ToArray();
                char c = hashString[1];
                char p = hashString[0];

                int pos = p - '0';

                if (pos >= 0 && pos <= 7 && outpass[pos] == '_')
                {
                    outpass[pos] = c;
                }

                logger?.WriteLine($"{hashNumber,8} [{pos,2}]:{c} {outpass.AsString()}");
            }

            return outpass.AsString().ToLower();
        }

        public static string Part1(string input, ILogger logger) => CrackPassword1(input.Trim(), logger);

        public static string Part2(string input, ILogger logger) => CrackPassword2(input.Trim(), logger);

        public void Run(string input, ILogger logger)
        {
            // var watch = new System.Diagnostics.Stopwatch();        
            // watch.Start();
            // for(int i=0; i<10; ++i)
            // {
            //     HashBreaker.FindHash("abcdefg", 4);
            //     logger.WriteLine(watch.ElapsedMilliseconds/(i+1));
            // }

            //Console.WriteLine(CrackPassword("abc"));

            //Console.WriteLine(CrackPassword2("abc"));

            logger.WriteLine("- Pt1 - " + Part1(input, logger));
            logger.WriteLine("- Pt2 - " + Part2(input, logger));
        }
    }
}