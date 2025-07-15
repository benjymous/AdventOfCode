namespace AoC.Advent2024;
public class Day07 : IPuzzle
{
    [Regex(@"(\d+): (.+)")]
    private record struct Record(long TestValue, [Split(" ")] long[] Values);

    private static long Value(QuestionPart part, Record r)
        => IsValid(part, r.TestValue, r.Values.First(), r.Values.Skip(1)) ? r.TestValue : 0;

    private static bool IsValid(QuestionPart part, long lhs, long next, IEnumerable<long> rest)
        => rest.Any()
             ? next <= lhs && (
                 (part.Two && IsValid(part, lhs, Concat(next, rest.First()), rest.Skip(1))) ||
                 IsValid(part, lhs, next * rest.First(), rest.Skip(1)) ||
                 IsValid(part, lhs, next + rest.First(), rest.Skip(1)))
             : next == lhs;

    private static long Concat(long a, long b)
    {
        long multiplier = 10;
        while (b >= multiplier) multiplier *= 10;
        return (a * multiplier) + b;
    }

    private static long Solve(string input, QuestionPart part)
        => Parser.Parse<Record>(input).AsParallel().Sum(r => Value(part, r));

    public static long Part1(string input) => Solve(input, QuestionPart.Part1);
    public static long Part2(string input) => Solve(input, QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}