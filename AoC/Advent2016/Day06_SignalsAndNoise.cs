namespace AoC.Advent2016;
public class Day06 : IPuzzle
{
    public static string Part1(string input)
    {
        var dat = Util.ParseMatrix<char>(input);

        return dat.Columns().Select(col => col.MostCommon().value).AsString();
    }

    public static string Part2(string input)
    {
        var dat = Util.ParseMatrix<char>(input);

        return dat.Columns().Select(col => col.LeastCommon().value).AsString();
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}