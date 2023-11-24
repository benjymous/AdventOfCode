namespace AoC.Advent2018;
public class Day01 : IPuzzle
{
    public static int Part1(string input) => Util.ParseNumbers<int>(input).Sum();

    public static int Part2(string input)
    {
        var seen = new HashSet<int>();

        var nums = Util.ParseNumbers<int>(input);

        int freq = 0;

        while (true)
        {
            foreach (var num in nums)
            {
                seen.Add(freq);

                freq += num;

                if (seen.Contains(freq))
                {
                    return freq;
                }
            }
        }
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
