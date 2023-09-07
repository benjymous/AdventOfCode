using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day11 : IPuzzle
    {
        public string Name => "2015-11";

        static readonly HashSet<char> bads = new () { 'i', 'o', 'l' };

        public static bool IsBad(char c) => bads.Contains(c);

        public static char[] Increment(char[] pwd)
        {
            var newPwd = pwd.ToArray();
            int i = pwd.Length - 1;
            while (true)
            {
                newPwd[i]++;
                if (newPwd[i] <= 'z')
                {
                    if (!IsBad(newPwd[i])) return newPwd;
                }
                else newPwd[i--] = 'a';
            }
        }

        public static bool HasStraight(char[] line)
        {
            for (int i = 0; i < line.Length - 2; ++i)
            {
                if (line[i] == line[i + 1] - 1 && line[i] == line[i + 2] - 2) return true;
            }
            return false;
        }


        public static bool NoBads(char[] line) => !bads.Any(line.Contains);

        public static bool HasTwoNonOverlappingPairs(char[] line)
        {
            int pairs = 0;
            for (int i = 0; i < line.Length - 1; ++i)
            {
                if (line[i] == line[i + 1])
                {
                    pairs++;
                    i++;
                }
            }
            return pairs > 1;
        }

        public static bool IsValid(char[] line) => HasStraight(line) && HasTwoNonOverlappingPairs(line) && NoBads(line);

        public static char[] FindNextValid(char[] input)
        {
            do input = Increment(input); while (!IsValid(input));

            return input;
        }

        public static string Part1(string input)
        {
            return FindNextValid(input.Trim().ToArray()).AsString();
        }

        public static string Part2(string input)
        {
            return FindNextValid(FindNextValid(input.Trim().ToArray())).AsString();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}