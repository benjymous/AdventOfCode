namespace AoC.Advent2024;
public class Day22 : IPuzzle
{
    private const long Prune = 16777216;

    public static IEnumerable<long> SecretSequence(long secret)
    {
        while (true)
        {
            yield return secret;
            secret = (secret ^ (secret * 64)) % Prune;
            secret = (secret ^ (secret / 32)) % Prune;
            secret = (secret ^ (secret * 2048)) % Prune;
        }
    }

    private static IEnumerable<(int, int)> Diffs(int[] input)
    {
        for (int i = 1; i < input.Length; ++i)
        {
            yield return (input[i], input[i] - input[i - 1]);
        }
    }

    private static (int val, int diff)[][] BuildTables(IEnumerable<int> data)
    {
        return data.AsParallel().Select(seed => SecretSequence(seed).Take(2000).Select(n => (int)(n % 10)).ToArray())
                                .Select(ones => Diffs(ones).ToArray()).ToArray();
    }

    public static long Part1(Parser.AutoArray<int> input) => input.AsParallel().Sum(seed => SecretSequence(seed).ElementAt(2000));

    public static int Part2(Parser.AutoArray<int> input)
    {
        var tables = BuildTables(input);
        var t2 = tables.SelectMany(t => t.Windows(4).Select(t => (key: HashCode.Combine(t[0].diff, t[1].diff, t[2].diff, t[3].diff), t[3].val)).DistinctBy(v => v.key));

        return t2.AggregateBy(item => item.key, 0, (acc, item) => acc + item.val).Max(v => v.Value);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}