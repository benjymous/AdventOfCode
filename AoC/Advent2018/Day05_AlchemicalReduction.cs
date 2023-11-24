namespace AoC.Advent2018;
public class Day05 : IPuzzle
{
    static int Reduce(IEnumerable<char> inp)
    {
        var input = inp.ToList();
        bool replaced;
        do
        {
            replaced = false;

            for (var i = 0; i < input.Count - 1; ++i)
            {
                if (input[i] != input[i + 1] && ((input[i] & 0x1f) == (input[i + 1] & 0x1f)))
                {
                    input.RemoveAt(i);
                    input.RemoveAt(i);
                    i--;

                    replaced = true;
                }
            }
        } while (replaced);

        return input.Count;
    }

    public static int Part1(string input) => Reduce(input.Trim());

    public static int ShrinkReduce(char c, IEnumerable<char> input)
    {
        var check = c & 0x1f;
        return Reduce(input.Where(ch => (ch & 0x1f) != check));
    }

    public static int Part2(string input) => ParallelEnumerable.Range('a', 26).Select(alpha => ShrinkReduce((char)alpha, input.Trim())).Min();

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}