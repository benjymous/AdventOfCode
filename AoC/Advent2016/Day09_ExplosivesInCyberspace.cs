namespace AoC.Advent2016;
public class Day09 : IPuzzle
{
    private static (int numChars, int numRepeats) Parse(string cmd) => Util.ParseNumbers<int>(cmd, "x").Decompose2();

    private static long Decompress(string input, bool recurse)
    {
        long length = 0;
        int i = 0;
        while (i < input.Length)
        {
            var c = input[i];

            if (c == '(')
            {
                int start = i;
                while (input[i++] != ')') ;

                var (numChars, numRepeats) = Parse(input.Substring(start + 1, i - start - 2));

                length += (recurse ? Decompress(input.Substring(i, numChars), true) : numChars) * numRepeats;
                i += numChars;
            }
            else
            {
                i++;
                length++;
            }
        }
        return length;
    }

    public static long Part1(string input) => Decompress(input.Trim(), false);

    public static long Part2(string input) => Decompress(input.Trim(), true);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));  // 107035
        logger.WriteLine("- Pt2 - " + Part2(input));  // 11451628995
    }
}