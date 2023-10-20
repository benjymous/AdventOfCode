using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2017
{
    public class Day14 : IPuzzle
    {
        public string Name => "2017-14";

        public static IEnumerable<int> BuildBits(string input) =>
            ParallelEnumerable.Range(0, 128).SelectMany(y =>
                 Day10.KnotHash($"{input}-{y}").SelectMany(h => h.BinarySequence(0xff).Reverse()).Select((v, x) => (v, x)).Where(d => d.v == 1).Select(d => d.x + (y << 16))
            ).ToArray();

        static void FloodFill(int pos, HashSet<int> matrix)
        {
            if (!matrix.Contains(pos)) return;
            matrix.Remove(pos);
            FloodFill(pos + (1 << 16), matrix);
            FloodFill(pos - (1 << 16), matrix);
            FloodFill(pos + 1, matrix);
            FloodFill(pos - 1, matrix);
        }

        public static int Part1(IEnumerable<int> data)
        {
            return data.Count();
        }

        public static int Part2(IEnumerable<int> data)
        {
            var matrix = data.ToHashSet();

            int groups = 0;

            while (matrix.Count != 0)
            {
                var pos = matrix.First();
                FloodFill(pos, matrix);
                groups++;
            }

            return groups;
        }

        public static int Part1(string input)
        {
            return Part1(BuildBits(input.Trim()));
        }

        public static int Part2(string input)
        {
            return Part2(BuildBits(input.Trim()));
        }

        public void Run(string input, ILogger logger)
        {
            var data = BuildBits(input);
            logger.WriteLine("- Pt1 - " + Part1(data));
            logger.WriteLine("- Pt2 - " + Part2(data));
        }
    }
}