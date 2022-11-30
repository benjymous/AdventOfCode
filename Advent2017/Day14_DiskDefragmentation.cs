using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2017
{
    public class Day14 : IPuzzle
    {
        public string Name => "2017-14";

        public static IEnumerable<(int x, int y)> BuildBits(string input)
        {
            for (int y = 0; y < 128; ++y)
            {
                var hash = Day10.KnotHash($"{input}-{y}");
                var row = hash.BitSequenceFromHex().ToArray();
                for (int x = 0; x < 128; ++x)
                {
                    if (row[x]) yield return (x, y);
                }
            }
        }

        public static int Part1(string input)
        {
            return BuildBits(input).Count();
        }

        static void FloodFill((int x, int y) pos, HashSet<(int x, int y)> matrix)
        {
            if (!matrix.Contains(pos)) return;
            matrix.Remove(pos);
            FloodFill((pos.x, pos.y + 1), matrix);
            FloodFill((pos.x, pos.y - 1), matrix);
            FloodFill((pos.x + 1, pos.y), matrix);
            FloodFill((pos.x - 1, pos.y), matrix);
        }

        public static int Part2(string input)
        {
            var matrix = BuildBits(input).ToHashSet();

            int groups = 0;

            while (matrix.Any())
            {
                var pos = matrix.First();
                FloodFill(pos, matrix);
                groups++;
            }

            return groups;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}