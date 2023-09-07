using AoC.Utils;
using System;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day05 : IPuzzle
    {
        public string Name => "2015-05";

        public static bool HasVowels(string line, int count) => line.Count(c => c.IsVowel()) >= count;

        public static bool HasRun(string line) => line.OverlappingPairs().Any(pair => pair.first == pair.second);

        static readonly string[] bads = new string[] { "ab", "cd", "pq", "xy" };
        public static bool NoBads(string line) => !bads.Any(line.Contains);

        public static bool IsNice1(string line) => HasVowels(line, 3) && HasRun(line) && NoBads(line);

        public static bool HasNonOverlappingPair(string line)
        {
            for (int i = 0; i < line.Length - 3; ++i)
            {
                for (int j = i + 2; j < line.Length - 1; ++j)
                {
                    if (line[i] == line[j] && line[i + 1] == line[j + 1])
                        return true;
                }
            }
            return false;
        }

        public static bool HasGapRepeat(string line)
        {
            for (int i = 0; i < line.Length - 2; ++i)
            {
                if (line[i] == line[i + 2]) return true;
            }
            return false;
        }

        public static bool IsNice2(string line) => HasNonOverlappingPair(line) && HasGapRepeat(line);

        public static int Part1(string input)
        {
            return Util.Split(input).Count(IsNice1);
        }

        public static int Part2(string input)
        {
            return Util.Split(input).Count(IsNice2);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}