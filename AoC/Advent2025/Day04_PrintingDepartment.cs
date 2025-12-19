namespace AoC.Advent2025;

public class Day04 : IPuzzle
{
    private static List<(int x, int y)> PerformStep(HashSet<(int x, int y)> grid)
        => grid.Where(e => grid.Neighbours(e).Count() < 4).ToList();

    private static HashSet<(int, int)> Parse(string input) => Util.ParseSparseMatrix<bool>(input).Keys.ToHashSet();

    public static int Part1(string input) => PerformStep(Parse(input)).Count;

    public static int Part2(string input)
    {
        var grid = Parse(input);

        int total = 0;

        while (true)
        {
            var remove = PerformStep(grid);
            if (remove.Count == 0) break;
            total += remove.Count;

            grid.RemoveRange(remove);
        }

        return total;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}