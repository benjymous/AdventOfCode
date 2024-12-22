namespace AoC.Advent2024;
public class Day22 : IPuzzle
{
    public static int Prune(long val) => (int)(val % 16777216);

    public static int DoIter(int secret)
    {
        secret = Prune(secret ^ secret * 64L);
        secret = Prune(secret ^ secret / 32);
        secret = Prune(secret ^ secret * 2048L);
        return secret;
    }

    public static IEnumerable<int> SecretSequence(int initial)
    {
        yield return initial;
        while (true)
        {
            yield return initial = DoIter(initial);
        }
    }

    public static long Part1(string input)
    {
        var numbers = Util.Split(input).Select(int.Parse);

        return numbers.Sum(n => (long)SecretSequence(n).ElementAt(2000));
    }

    public static int Part2(string input)
    {
        var numbers = Util.Split(input).Select(int.Parse);

        List<(int val, int diff)[]> tables = [];

        foreach (var n in numbers)
        {
            var seq = SecretSequence(n).Take(2000);
            var ones = seq.Select(n => n % 10).ToArray();
            var diffs = ones.Windows(2).Select(p => p[1] - p[0]).ToArray();

            var res = new (int val, int diff)[1999];

            for (int i = 0; i < 1999; ++i)
            {
                res[i].val = ones[i + 1];
                res[i].diff = diffs[i];
            }

            tables.Add(res);
        }

        List<(int val, (int d0, int d1, int d2, int d3) diffs)[]> tables2 = [];

        foreach (var t in tables)
        {
            var w = t.Windows(4).Where(t => t[3].val >= 3).Select(t => (t[3].val, (t[0].diff, t[1].diff, t[2].diff, t[3].diff))).ToArray();
            tables2.Add(w);
        }

        List<Dictionary<(int d0, int d1, int d2, int d3), int>> dicts = [];
        foreach (var t2 in tables2)
        {
            var dict = t2.DistinctBy(v => v.diffs).ToDictionary(v => v.diffs, v => v.val);
            dicts.Add(dict);
        }

        int best = 0;

        HashSet<(int, int, int, int)> seen = [];

        for (int i = 0; i < dicts.Count; i++)
        {
            Dictionary<(int d0, int d1, int d2, int d3), int> d = dicts[i];
            foreach (var row in d)
            {
                if (seen.Add(row.Key))
                {
                    int sum = row.Value;
                    for (int i1 = i + 1; i1 < dicts.Count; i1++)
                    {
                        Dictionary<(int d0, int d1, int d2, int d3), int> d1 = dicts[i1];
                        if (d1.TryGetValue(row.Key, out int value))
                        {
                            sum += value;
                        }
                    }
                    if (sum > best)
                    {
                        Console.WriteLine($"{sum}: {row.Key}");
                    }
                    best = Math.Max(best, sum);
                }
            }
        }

        return best;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}