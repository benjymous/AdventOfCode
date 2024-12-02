namespace AoC.Advent2024;
public class Day01 : IPuzzle
{
    private static (int[], int[]) Parse(string input)
    {
        var data = Util.ParseNumberList<int>(input);
        return (data.Select(v => v[0]).ToArray(), data.Select(v => v[1]).ToArray());
    }

    public static int Part1(string input)
    {
        var (a, b) = Parse(input);

        return a.Order().Zip(b.Order()).Sum((v) => Math.Abs(v.First - v.Second));
    }

    public static int Part2(string input)
    {
        var (a, b) = Parse(input);

        return a.Sum(i => i * b.Count(j => j == i));
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}