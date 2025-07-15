namespace AoC.Advent2015;
public class Day25 : IPuzzle
{
    private static int FindCode(int row, int col)
    {
        int iterTarget = ((row + col - 2) * (row + col - 1) / 2) + col;

        return (int)(20151125 * BigInteger.ModPow(252533, iterTarget - 1, 33554393) % 33554393);
    }

    public static int Part1(string input)
    {
        var numbers = Util.ExtractNumbers(input);

        return FindCode(numbers[0], numbers[1]);
    }

    public void Run(string input, ILogger logger) => logger.WriteLine("- Pt1 - " + Part1(input));
}