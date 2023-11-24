namespace AoC.Advent2020;
public class Day06 : IPuzzle
{

    public static int Part1(string input) => input.Split("\n\n").Sum(g => g.Replace("\n", "").Distinct().Count());

    public static int Part2(string input) => input.Split("\n\n").Sum(g => { var lines = g.Split("\n"); return lines.Aggregate(lines.First() as IEnumerable<char>, (s1, s2) => s1.Intersect(s2)).Count(); });

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}