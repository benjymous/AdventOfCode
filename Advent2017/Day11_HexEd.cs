using Advent.Utils.Vectors;
using System;

namespace AoC.Advent2017
{
    public class Day11 : IPuzzle
    {
        public string Name => "2017-11";

        public static int Part1(string input)
        {
            var steps = input.Trim().Split(",");

            var pos = new HexVector(0, 0, 0, false);

            foreach (var step in steps)
            {
                pos.TranslateHex(step);
            }

            return pos.Distance(new HexVector(0, 0, 0, false));
        }

        public static int Part2(string input)
        {
            var origin = new HexVector(0, 0, 0, false);
            var steps = input.Trim().Split(",");

            var pos = new HexVector(0, 0, 0, false);

            int furthest = 0;

            foreach (var step in steps)
            {
                pos.TranslateHex(step);

                furthest = Math.Max(furthest, pos.Distance(origin));
            }

            return furthest;

        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}