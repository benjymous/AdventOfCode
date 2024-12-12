namespace AoC.Advent2024;
public class Day11 : IPuzzle
{
    static int CountDigits(long number) => ((int)Math.Log10(number)) + 1;

    static long[] SplitNumber(long number, int digits)
    {
        int divisor = (int)Math.Pow(10, digits / 2);
        return [number / divisor, number % divisor];
    }

    static long[] Blink(long input)
    {
        if (input == 0) return [1];
        int digits = CountDigits(input);
        if (digits % 2 == 0) return SplitNumber(input, digits);
        else return [input * 2024];
    }

    private static long PerformBlinks(string input, int blinkCount)
    {
        var buckets = Util.ExtractNumbers<long>(input).CountUniqueElements<long, long>();

        for (int i = 0; i < blinkCount; ++i)
        {
            Dictionary<long, long> next = [];
            buckets.ForEach(b => Blink(b.Key).ForEach(n => next.IncrementAtIndex(n, b.Value)));
            buckets = next;
        }

        return buckets.Values.Sum();
    }

    public static long Part1(string input) => PerformBlinks(input, 25);
    public static long Part2(string input) => PerformBlinks(input, 75);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}