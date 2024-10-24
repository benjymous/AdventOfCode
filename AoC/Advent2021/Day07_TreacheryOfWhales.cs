namespace AoC.Advent2021;
public class Day07 : IPuzzle
{
    public static int Solve(string input, Func<int, int> FuelCost)
    {
        var positions = Util.ParseNumbers<int>(input).Order().ToArray();

#pragma warning disable CS9236 // Compiling requires binding the lambda expression many times. Consider declaring the lambda expression with explicit parameter types, or if the containing method call is generic, consider using explicit type arguments.
        return Enumerable.Range(positions.First(), positions.Last())
                         .Min(x => positions.Sum(crab => FuelCost(Math.Abs(crab - x))));
#pragma warning restore CS9236 // Compiling requires binding the lambda expression many times. Consider declaring the lambda expression with explicit parameter types, or if the containing method call is generic, consider using explicit type arguments.
    }

    public static int Part1(string input) => Solve(input, x => x);

    public static int Part2(string input) => Solve(input, x => x * (x + 1) / 2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}