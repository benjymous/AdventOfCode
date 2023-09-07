using AoC.Utils;
using System;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day02 : IPuzzle
    {
        public string Name => "2015-02";

        public static int Wrap(string line)
        {
            var (length, width, height) = Util.ParseNumbers<int>(line, 'x').Decompose3();

            var side1 = length * width;
            var side2 = width * height;
            var side3 = height * length;

            var extra = Math.Min(Math.Min(side1, side2), side3);

            return extra + (2 * side1) + (2 * side2) + (2 * side3);
        }

        public static int Ribbon(string line)
        {
            var (length, width, height) = Util.ParseNumbers<int>(line, 'x').Order().Decompose3();

            return (length * 2) + (width * 2) + (length * width * height);
        }

        public static int Part1(string input)
        {
            return Util.Split(input).Sum(Wrap);
        }

        public static int Part2(string input)
        {
            return Util.Split(input).Sum(Ribbon);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}