using System.Linq;

namespace AoC.Advent2022
{
    public class Day04 : IPuzzle
    {
        public string Name => "2022-04";

        [Regex(@"(\d+)-(\d+),(\d+)-(\d+)")]
        record struct Pair(int Min1, int Max1, int Min2, int Max2)
        { 
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