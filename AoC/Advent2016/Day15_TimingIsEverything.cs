namespace AoC.Advent2016;
public class Day15 : IPuzzle
{
    [method: Regex(@"Disc #(\d) .+ (\d+) .+ (\d+).")]
    public class Disc(int discNum, int numPos, int initialPos)
    {
        public readonly int NumPos = numPos, Offset = discNum + initialPos;

        public bool CheckAlignment(int delay) => (delay + Offset) % NumPos == 0;
    }

    private static int FindAlignment(Parser.AutoArray<Disc> input)
    {
        int i = 0, inc = 1;
        foreach (var disc in input)
        {
            while (!disc.CheckAlignment(i += inc)) ;
            inc *= disc.NumPos;
        }
        return i;
    }

    public static int Part1(string input) => FindAlignment(input);

    public static int Part2(string input) => FindAlignment(input + "Disc #7 has 11 .. 0.");

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}