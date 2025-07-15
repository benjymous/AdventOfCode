namespace AoC.Advent2024;
public class Day25 : IPuzzle
{
    private static int Parse(string grid)
        => (grid[0] == '.' ? 1 : 0) | (grid.Skip(5).Take(25).Aggregate(0, (res, c) => res = (res << 1) | (c == '#' ? 1 : 0)) << 1);

    public static int Part1(string input)
    {
        var (keys, locks) = input.SplitSections().Select(l => Parse(l.Replace("\n", ""))).Partition(v => (v & 1) > 0);

        return locks.Sum(l => keys.Count(k => (l & k) == 0));
    }

    public void Run(string input, ILogger logger) => logger.WriteLine("- Pt1 - " + Part1(input));
}