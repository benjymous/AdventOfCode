namespace AoC.Advent2023;
public class Day09 : IPuzzle
{
    public static int GetNext(int[] sequence)
    {
        int last = sequence[sequence.Length - 1];
        return sequence.All(v => v == last) 
            ? last
            : last + GetNext([..sequence.Windows(2).Select(pair => pair[1] - pair[0])]);
    }

    public static int GetPrev(int[] sequence) => GetNext(sequence.Reverse().ToArray());

    static IEnumerable<int[]> ParseData(string input) 
        => Util.Split(input).Select(line => Util.ParseNumbers<int>(line, " "));

    public static int Part1(string input) => ParseData(input).Sum(GetNext);

    public static int Part2(string input) => ParseData(input).Sum(GetPrev);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
