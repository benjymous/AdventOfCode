namespace AoC.Advent2019;
public class Day10 : IPuzzle
{
    public static double AngleBetween((int x, int y) vector1, (int x, int y) vector2)
    {
        if (vector1 == vector2) return 0;
        var angle = Math.Atan2(vector2.x - vector1.x, vector1.y - vector2.y);
        return angle < 0 ? angle + (Math.PI * 2) : angle;
    }

    public static int[] FindBestLocation(string input)
    {
        var data = Util.ParseSparseMatrix<bool>(input).Keys;

        return [.. data.Select(a1 => data.GroupBy(a2 => AngleBetween(a1, a2), a2 => (a2.x * 100) + a2.y))
            .MaxBy(e => e.Count())
            .OrderBy(group => group.Key)
            .Select(group => group.First())];
    }

    public static int Part1(string input) => FindBestLocation(input).Length;

    public static int Part2(string input, int position) => FindBestLocation(input)[position - 1];

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input, 200));
    }
}