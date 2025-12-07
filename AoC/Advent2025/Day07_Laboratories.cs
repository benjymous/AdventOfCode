namespace AoC.Advent2025;

public class Day07 : IPuzzle
{
    static long CountTimelines(int x, int i, string[] states) => Memoize((x, i), _ =>
    {
        if (i == states.Length) return 1;

        return states[i][x] == '^' 
            ? CountTimelines(x - 1, i, states) + CountTimelines(x + 1, i, states) 
            : CountTimelines(x, i + 1, states);
    });

    public static int Part1(string input)
    {
        var states = Util.Split(input).Select(r => r.ToArray()).ToArray();
        var width = states[0].Length;
        int splits = 0;

        for (int i = 1; i < states.Length; i++)
        {
            for (int x = 0; x < width; x++)
            {
                switch (states[i][x])
                {
                    case '.' when states[i - 1][x] is 'S' or '|':
                        states[i][x] = '|';
                        break;
                    case '^' when states[i - 1][x] == '|':
                        splits++;
                        states[i][x - 1] = '|';
                        states[i][x + 1] = '|';
                        break;
                }
            }
        }

        return splits;
    }

    public static long Part2(string input) =>
        CountTimelines(Util.Split(input)[0].IndexOf('S'), 1, Util.Split(input));

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}