namespace AoC.Advent2015;
public class Day03 : IPuzzle
{
    public static int Part1(string input)
    {
        HashSet<(int x, int y)> visited = [(0, 0)];

        var santa = (0, 0);

        foreach (var dir in input)
        {
            visited.Add(santa = santa.OffsetBy(dir));
        }

        return visited.Count;
    }

    public static int Part2(string input)
    {
        HashSet<(int x, int y)> visited = [(0, 0)];

        var (santa1, santa2) = ((0, 0), (0, 0));

        foreach (var dir in input)
        {
            visited.Add(santa1 = santa1.OffsetBy(dir));
            (santa1, santa2) = (santa2, santa1);
        }

        return visited.Count;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}