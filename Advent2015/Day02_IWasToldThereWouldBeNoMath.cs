using System;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day02 : IPuzzle
    {
        public string Name => "2015-02";

        public static int Wrap(string line)
        {
            var bits = Util.ParseNumbers<int>(line, 'x');

            const int LENGTH = 0;
            const int WIDTH = 1;
            const int HEIGHT = 2;

            var side1 = bits[LENGTH] * bits[WIDTH];
            var side2 = bits[WIDTH] * bits[HEIGHT];
            var side3 = bits[HEIGHT] * bits[LENGTH];

            var extra = Math.Min(Math.Min(side1, side2), side3);

            return extra + (2 * side1) + (2 * side2) + (2 * side3);
        }

        public static int Ribbon(string line)
        {
            var bits = Util.ParseNumbers<int>(line, 'x').Order().ToList();

            return (bits[0] * 2) + (bits[1] * 2) + (bits[0] * bits[1] * bits[2]);
        }

        public static int Part1(string input)
        {
            var lines = Util.Split(input);
            return lines.Select(Wrap).Sum();
        }

        public static int Part2(string input)
        {
            var lines = Util.Split(input);
            return lines.Select(Ribbon).Sum();
        }

        public void Run(string input, ILogger logger)
        {
            // Console.WriteLine(Wrap("2x3x4")); // 58
            // Console.WriteLine(Wrap("1x1x10")); // 43

            // Console.WriteLine(Ribbon("2x3x4")); // 34
            // Console.WriteLine(Ribbon("1x1x10")); // 14

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}