using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day01 : IPuzzle
    {
        public string Name => "2021-01";

        static IEnumerable<int> Sliding3s(IEnumerable<int> input) =>
            input.Windows(3).Select(set => set.Sum());

        private static int AscendingDepths(IEnumerable<int> depths) =>
            depths.Skip(1)
             .Zip(depths, (curr, prev) => curr > prev ? 1 : 0)
             .Sum();

        public static int Part1(string input)
        {
            var depths = Util.Parse32(input);
            return AscendingDepths(depths);
        }

        public static int Part2(string input)
        {
            var depths = Sliding3s(Util.Parse32(input)).ToArray();
            return AscendingDepths(depths);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}