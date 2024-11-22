namespace AoC.Advent2020;
public class Day01 : IPuzzle
{
    public static int Part1(string input)
    {
        var allNumbers = Util.ParseNumbers<int>(input).ToHashSet();
        return allNumbers.Where(n1 => allNumbers.Contains(2020 - n1))
                         .Select(n1 => n1 * (2020 - n1))
                         .First();
    }

    public static int Part2(string input)
    {
        var allNumbers = Util.ParseNumbers<int>(input).ToHashSet();

        return allNumbers.SelectMany(n1 => allNumbers.Where(n2 => allNumbers.Contains(2020 - (n1 + n2)))
                                                     .Select(n2 => n1 * n2 * (2020 - (n1 + n2))))
                         .First();
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}