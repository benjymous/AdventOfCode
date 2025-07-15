namespace AoC.Advent2015;
public class Day10 : IPuzzle
{
    public static IEnumerable<int> SayIt(int[] input)
    {
        for (int i = 0, run; i < input.Length; i += run)
        {
            run = 0;

            while ((run + i < input.Length) && (input[i] == input[i + run])) run++;

            yield return run;
            yield return input[i];
        }
    }

    public static int GetNth(string input, int iterations)
    {
        var result = input.Trim().Select(ch => ch.AsDigit()).ToArray();

        for (int i = 0; i < iterations; ++i)
        {
            result = [.. SayIt(result)];
        }

        return result.Length;
    }

    public static int Part1(string input) => GetNth(input, 40);

    public static int Part2(string input) => GetNth(input, 50);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}