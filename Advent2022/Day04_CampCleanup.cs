using System;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day04 : IPuzzle
    {
        public string Name => "2022-04";

        public readonly struct Pair
        {
            [Regex(@"(\d+)-(\d+),(\d+)-(\d+)")]
            public Pair(int min1, int max1, int min2, int max2) => (Min1, Max1, Min2, Max2) = (min1, max1, min2, max2);

            readonly int Min1, Max1, Min2, Max2;

            public bool SubsetContained => Min1 >= Min2 && Max1 <= Max2 || Min2 >= Min1 && Max2 <= Max1;

            public bool HasOverlap => Min1 <= Max2 && Min2 <= Max1;
        }

        public static int Part1(string input)
        {
            var pairs = Util.RegexParse<Pair>(input);
            return pairs.Count(p => p.SubsetContained);
        }

        public static int Part2(string input)
        {
            var pairs = Util.RegexParse<Pair>(input);
            return pairs.Count(p => p.HasOverlap);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}