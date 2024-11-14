namespace AoC.Advent2022;
public class Day01 : IPuzzle
{
    public static int Part1(string input)
    {
        return input.SplitSections()
                    .Select(group => Util.ParseNumbers<int>(group))
                    .Max(cals => cals.Sum());
    }

    public static int Part2(string input)
    {
        return input.SplitSections()
                    .Select(group => Util.ParseNumbers<int>(group))
                    .Select(cals => cals.Sum())
                    .OrderDescending()
                    .Take(3)
                    .Sum();
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}