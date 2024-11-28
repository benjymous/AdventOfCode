namespace AoC.Advent2022;
public class Day04 : IPuzzle
{
    [Regex(@"(\d+)-(\d+),(\d+)-(\d+)")]
    public record struct Pair(int Min1, int Max1, int Min2, int Max2)
    {
        public readonly bool SubsetContained => (Min1 >= Min2 && Max1 <= Max2) || (Min2 >= Min1 && Max2 <= Max1);

        public readonly bool HasOverlap => Min1 <= Max2 && Min2 <= Max1;
    }

    public static int Part1(Parser.AutoArray<Pair> input) => input.Count(p => p.SubsetContained);

    public static int Part2(Parser.AutoArray<Pair> input) => input.Count(p => p.HasOverlap);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}