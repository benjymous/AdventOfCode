namespace AoC.Advent2025;

public class Day08 : IPuzzle
{
    static IEnumerable<((int, int) pair, long dist)> GetDistances((long x, long y, long z)[] boxes)
    {
        for (int i = 0; i < boxes.Length; i++)
            for (int j = i + 1; j < boxes.Length; j++)
                yield return ((i, j), Util.SqDistance(boxes[i], boxes[j]));
    }

    public static long SolveCircuit(Parser.AutoArray<(long x, long y, long z)> boxes, int? connectionCount = null)
    {
        var distances = GetDistances(boxes).OrderBy(kvp => kvp.dist)
            .Select(kvp => kvp.pair)
            .If(connectionCount != null, vals => vals.Take(connectionCount.Value));

        HashSet<int>[] sets = [.. Enumerable.Range(0, boxes.Length).Select(i => new HashSet<int>() { i })];

        foreach (var c in distances)
        {
            if (sets[c.Item1] != sets[c.Item2])
            {
                var combined = sets[c.Item1];
                combined.UnionWith(sets[c.Item2]);

                if (combined.Count == boxes.Length)
                    return boxes[c.Item1].x * boxes[c.Item2].x;

                foreach (var j in combined) sets[j] = combined;
            }
        }

        return sets.Distinct().OrderByDescending(s => s.Count).Take(3).Product(s => s.Count);
    }
    public static long Part1(string input) => SolveCircuit(input, 1000);
    public static long Part2(string input) => SolveCircuit(input);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}