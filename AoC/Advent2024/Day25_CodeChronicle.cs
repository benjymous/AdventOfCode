namespace AoC.Advent2024;
public class Day25 : IPuzzle
{
    public static int Part1(string input)
    {
        var parts = input.SplitSections();

        List<int[]> locks = [];
        List<int[]> keys = [];

        foreach (var part in parts)
        {
            var grid = Util.ParseMatrix<bool>(part);
            var r = grid.Row(0);
            if (r.All(v => v))
            {
                var sig = grid.Columns().Select(c => c.Count(v => !v));
                locks.Add(sig.ToArray());
            }
            else
            {
                var sig = grid.Columns().Select(c => c.Count(v => v));
                keys.Add(sig.ToArray());
            }
        }

        int count = 0;
        foreach (var l in locks)
        {
            foreach (var k in keys)
            {
                bool fit = true;
                for (int i = 0; i < 5; ++i)
                {
                    if (l[i] < k[i])
                    {
                        fit = false; break;
                    }
                }
                if (fit) count++;
            }
        }

        return count;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
    }
}