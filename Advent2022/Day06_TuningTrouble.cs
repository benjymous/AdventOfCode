using AoC.Utils;
using System;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day06 : IPuzzle
    {
        public string Name => "2022-06";

        private static int FindSignal(string input, int size)
        {
            return input.Windows(size).WithIndex().Where(kvp => kvp.Value.Distinct().Count() == size).First().Index + size;
        }

        public static int Part1(string input)
        {
            return FindSignal(input, 4);
        }

        public static int Part2(string input)
        {
            return FindSignal(input, 14);
        }

        public void Run(string input, ILogger logger)
        {
            Console.WriteLine("- Pt1 - " + Part1(input));
            Console.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}