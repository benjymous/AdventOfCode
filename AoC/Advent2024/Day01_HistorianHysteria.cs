namespace AoC.Advent2024;
public class Day01 : IPuzzle
{
    private static (int[], int[]) Parse(string input)
    {
        var data = Util.ParseNumberList<int>(input);
        return ([.. data.Select(v => v[0])], [.. data.Select(v => v[1])]);
    }

    public static int Part1(string input)
    {
        var (a, b) = Parse(input);

        return a.Order().Zip(b.Order()).Sum((v) => Math.Abs(v.First - v.Second));
    }

    public static int Part2(string input)
    {
        var (a, b) = Parse(input);

        var aCount = a.CountUniqueElements<int, int>();
        var bCount = b.CountUniqueElements<int, int>();

        return aCount.Keys.Intersect(bCount.Keys)
                          .Sum(key => key * aCount[key] * bCount[key]);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}