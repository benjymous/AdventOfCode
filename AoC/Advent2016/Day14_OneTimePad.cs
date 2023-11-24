namespace AoC.Advent2016;
public class Day14 : IPuzzle
{
    static IEnumerable<(int run, char repeat, string hash)> CheckRuns(int index, string input, bool stretch)
    {
        string hash = $"{input}{index}".GetMD5String(true);
        if (stretch)
        {
            for (int i = 0; i < 2016; ++i)
            {
                hash = hash.GetMD5String(true);
            }
        }
        var ch = '\0';

        int run = 1;
        for (var i = 0; i <= hash.Length; ++i)
        {
            if (i < hash.Length && hash[i] == ch)
            {
                run++;
            }
            else
            {
                if (run >= 3)
                {
                    yield return (run, ch, hash);
                }
                run = 1;
                ch = i < hash.Length ? hash[i] : '\0';
            }
        }
    }

    private static int FillCache(string input, bool stretch, Queue<(int index, (int run, char repeat, string hash)[] runs)> cache, int last, int target)
    {
        while (last <= target)
        {
            var run = CheckRuns(last, input, stretch).ToArray();
            if (run.Length > 0) cache.Add((last, run));
            last++;
        }

        return last;
    }

    private static int GenKeys(string input, bool stretch)
    {
        input = input.Trim();
        Queue<(int index, (int run, char repeat, string hash)[] runs)> cache = [];

        int found = 0;

        int last = FillCache(input, stretch, cache, 0, 10000);

        while (true)
        {
            var (index, runs) = cache.Dequeue();

            last = FillCache(input, stretch, cache, last, index + 1000);

            var triplet = runs[0].repeat;
            if (cache.Where(v => v.index <= index + 1000 && v.runs.Any(r => r.run >= 5 && r.repeat == triplet)).Any() && ++found == 64)
                return index;
        }
    }

    public static int Part1(string input) => GenKeys(input, false);

    public static int Part2(string input) => GenKeys(input, true);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}