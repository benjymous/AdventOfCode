namespace AoC.Advent2022;
public class Day06 : IPuzzle
{
    private static int FindSignal(string input, int size) =>
        input.Windows(size)
             .WithIndex()
             .First(kvp => kvp.Value.Distinct().Count() == size)
             .Index + size;

    public static int Part1(string input) => FindSignal(input, 4);

    public static int Part2(string input) => FindSignal(input, 14);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}