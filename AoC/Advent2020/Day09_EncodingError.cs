namespace AoC.Advent2020;
public class Day09 : IPuzzle
{
    private static bool ValidateNumber(int index, int preamble, long[] numbers) =>
        numbers.Skip(index - preamble).Take(preamble)
            .Pairs().Any(p => p.Item1 + p.Item2 == numbers[index]);

    private static long FindInvalid(long[] numbers, int preamble) =>
        Enumerable.Range(preamble, numbers.Length)
            .Where(i => !ValidateNumber(i, preamble, numbers))
            .Select(i => numbers[i]).First();

    public static long Part1(string input, int preamble = 25)
    {
        var numbers = Util.ParseNumbers<long>(input);

        return FindInvalid(numbers, preamble);
    }

    public static long Part2(string input, int preamble = 25)
    {
        var numbers = Util.ParseNumbers<long>(input);

        long invalid = FindInvalid(numbers, preamble);

        for (var i = 0; i < numbers.Length; ++i)
        {
            var accumulator = new Accumulator<long>(numbers[i]);
            foreach (var n in numbers.Skip(i + 1))
            {
                accumulator.Add(n);
                if (accumulator.Sum == invalid)
                {
                    return accumulator.Max + accumulator.Min;
                }
                else if (accumulator.Sum > invalid)
                {
                    break;
                }
            }
        }

        return 0;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}