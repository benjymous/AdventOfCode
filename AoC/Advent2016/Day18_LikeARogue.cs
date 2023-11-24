namespace AoC.Advent2016;
public class Day18 : IPuzzle
{
    private static int Step(bool[] current, bool[] next)
    {
        next[0] = current[1] == true;
        for (var i = 0; i < current.Length - 2; ++i)
        {
            next[i + 1] = current[i] == current[i + 2];
        }
        next[^1] = current[^2] == true;

        return Count(next);
    }

    static int Count(IEnumerable<bool> line) => line.Count(x => x);

    public static int BuildMap(string input, int numLines)
    {
        var states = new bool[2][]
        {
            input.Select(c => c=='.').ToArray(),
            new bool[input.Length]
        };

        int count = Count(states[0]);
        for (int i = 1; i < numLines; ++i)
        {
            count += Step(states[(i + 1) % 2], states[i % 2]);
        }

        return count;
    }

    public static int Part1(string input) => BuildMap(input.Trim(), 40);

    public static int Part2(string input) => BuildMap(input.Trim(), 400000);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}