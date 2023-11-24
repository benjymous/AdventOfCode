namespace AoC.Advent2017;
public class Day11 : IPuzzle
{
    public static int Part1(string input)
    {
        var steps = input.Trim().Split(",");

        var pos = new HexVectorFlat();

        steps.ForEach(step => pos.TranslateHex(step));

        return pos.Distance(new HexVectorFlat());
    }

    public static int Part2(string input)
    {
        var origin = new HexVectorFlat();
        var steps = input.Trim().Split(",");

        var pos = new HexVectorFlat();

        int furthest = 0;

        foreach (var step in steps)
        {
            pos.TranslateHex(step);

            furthest = Math.Max(furthest, pos.Distance(origin));
        }

        return furthest;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}