namespace AoC.Advent2025;

public class Day03 : IPuzzle
{
    public static long LargestPossible(int[] row, int digits)
    {
        var max = row[..^(digits - 1)].Max();
        if (digits == 1) return max;

        var nextIndex = row.IndexOf(max)+1;

        return (max * Util.Pow10(digits - 1)) + LargestPossible(row[nextIndex..], digits - 1);
    }

    public static long FindBestJoltage(string input, int digits)
        => Util.Split(input).Sum(r => LargestPossible([.. r.Select(v => v.AsDigit())], digits));

    public static long Part1(string input) => FindBestJoltage(input, 2);

    public static long Part2(string input) => FindBestJoltage(input, 12);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}