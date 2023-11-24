namespace AoC.Advent2017;
public class Day15 : IPuzzle
{
    private static int RunDuel(string input, int pairs, bool picky)
    {
        var (valueA, valueB) = Util.ExtractNumbers(input).Decompose2();
        var (multipleA, multipleB) = picky ? (4, 8) : (1, 1);
        int count = 0;
        while (pairs-- > 0)
        {
            do valueA = (int)((long)valueA * 16807 % 2147483647); while (valueA % multipleA != 0);
            do valueB = (int)((long)valueB * 48271 % 2147483647); while (valueB % multipleB != 0);
            if ((valueA & 0xffff) == (valueB & 0xffff)) count++;
        }
        return count;
    }

    public static int Part1(string input) => RunDuel(input, 40000000, false);

    public static int Part2(string input) => RunDuel(input, 5000000, true);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}