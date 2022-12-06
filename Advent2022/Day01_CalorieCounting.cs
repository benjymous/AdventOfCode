using AoC.Utils;
using System;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day01 : IPuzzle
    {
        public string Name => "2022-01";

        public static int Part1(string input)
        {
            var groups = input.Split("\n\n");
            var elves = groups.Select(group => Util.ParseNumbers<int>(group));
            var max = elves.Max(cals => cals.Sum());
            return max;
        }

        public static int Part2(string input)
        {
            var groups = input.Split("\n\n");
            var elves = groups.Select(group => Util.ParseNumbers<int>(group));
            var totalthree = elves.Select(cals => cals.Sum()).OrderDescending().Take(3).Sum();
            return totalthree;
        }

        public void Run(string input, ILogger logger)
        {
            Console.WriteLine("- Pt1 - " + Part1(input));
            Console.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}