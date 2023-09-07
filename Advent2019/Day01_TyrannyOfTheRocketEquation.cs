using System;
using System.Linq;

namespace AoC.Advent2019
{
    public class Day01 : IPuzzle
    {
        public string Name => "2019-01";

        public static int GetFuelRequirement(int moduleWeight) => Math.Max(0, (moduleWeight / 3) - 2);

        public static int GetFullFuelRequirement(int moduleWeight)
        {
            int fuel = GetFuelRequirement(moduleWeight);
            if (fuel > 0) fuel += GetFullFuelRequirement(fuel);
            return fuel;
        }

        public static int Part1(string input) => Util.ParseNumbers<int>(input).Sum(GetFuelRequirement);
        public static int Part2(string input) => Util.ParseNumbers<int>(input).Sum(GetFullFuelRequirement);

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
