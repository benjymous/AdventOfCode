namespace AoC.Advent2021;
public class Day07 : IPuzzle
{
    public static int Solve(string input, Func<int, int> FuelCost)
    {
        var positions = Util.ParseNumbers<int>(input).Order().ToArray();

        return Enumerable.Range(positions.First(), positions.Last())
                         .Min(x => positions.Sum(crab => FuelCost(Math.Abs(crab - x))));
    }

    public static int Part1(string input) => Solve(input, x => x);

    public static int Part2(string input) => Solve(input, x => x * (x + 1) / 2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}