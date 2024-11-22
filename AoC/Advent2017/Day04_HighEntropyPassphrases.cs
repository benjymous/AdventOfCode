namespace AoC.Advent2017;
public class Day04 : IPuzzle
{
    public static bool ValidationRule1(string passphrase) =>
        !passphrase.Split(" ").GroupBy(w => w).Any(group => group.Count() > 1);

    public static bool ValidationRule2(string passphrase) =>
        !passphrase.Split(" ").Select(x => x.Order().AsString()).GroupBy(w => w).Any(group => group.Count() > 1);

    public static int Part1(string input) => Util.Split(input, "\n").Count(ValidationRule1);

    public static int Part2(string input) => Util.Split(input, "\n").Where(ValidationRule1).Count(ValidationRule2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}