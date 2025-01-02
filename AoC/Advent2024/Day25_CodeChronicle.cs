namespace AoC.Advent2024;
public class Day25 : IPuzzle
{
    static bool CheckFit(int[] l, int[] k)
    {
        for (int i = 0; i < 5; ++i)
        {
            if (l[i] < k[i]) return false;            
        }

        return true;
    }

    public static int Part1(string input)
    {
        var grids = input.SplitSections().Select(Util.ParseMatrix<bool>);

        var locks = grids.Where(g => g.Row(0).All(v => v)).Select(g => g.Columns().Select(c => c.Count(v => !v)).ToArray()).ToList();
        var keys = grids.Where(g => !g.Row(0).Any(v => v)).Select(g => g.Columns().Select(c => c.Count(v => v)).ToArray()).ToList();

        return locks.SelectMany(l => keys.Select(k => (l, k))).Count(v => CheckFit(v.l, v.k));
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
    }
}