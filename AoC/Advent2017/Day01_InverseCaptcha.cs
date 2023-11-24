namespace AoC.Advent2017;
public class Day01 : IPuzzle
{
    public static int Captcha(string input, int offset)
    {
        var data = input.Trim().Select(c => c - '0').ToArray();
        return data.Where((v, i) => v == data[(i + offset) % data.Length]).Sum();
    }

    public static int Part1(string input) => Captcha(input, 1);

    public static int Part2(string input) => Captcha(input, input.Length / 2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}