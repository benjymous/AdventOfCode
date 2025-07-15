namespace AoC.Advent2024;
public class Day20 : IPuzzle
{
    private static readonly PackedPos32[] neighbours = [(-1, 0), (1, 0), (0, -1), (0, 1)];

    public static int FindCheats(string input, int maxCheatSteps, int minSaving)
    {
        var grid = Util.ParseSparseMatrix<PackedPos32, char>(input, new Util.Convertomatic.SkipChars('#'));

        var walkable = grid.Keys.ToHashSet();
        var fromStart = FloodFill(walkable.Select(k => (k, int.MaxValue)).ToDictionary(), grid.SingleWithValue('S'));

        var cheatNeighbours = GetNeighbourOffsets(maxCheatSteps).ToList();

        int count = 0;

        foreach (var (from, fromDist) in fromStart)
        {
            foreach (var (offset, dist) in cheatNeighbours)
            {
                if (fromStart.TryGetValue(from + offset, out var toDist) && toDist - fromDist - dist >= minSaving)
                    count++;
            }
        }

        return count;
    }

    private static IEnumerable<(PackedPos32 offset, int dist)> GetNeighbourOffsets(int length)
    {
        for (int y = -length; y <= length; ++y)
            for (int x = -length; x <= length; ++x)
            {
                var d = Math.Abs(x) + Math.Abs(y);
                if (d > 1 && d <= length) yield return ((x, y), d);
            }
    }

    private static Dictionary<PackedPos32, int> FloodFill(Dictionary<PackedPos32, int> map, PackedPos32 pos, int current = 0)
    {
        if (!map.TryGetValue(pos, out var prev) || prev <= current) return map;

        map[pos] = current;
        foreach (var d in neighbours) FloodFill(map, pos + d, current + 1);
        return map;
    }

    public static int Part1(string input)
        => FindCheats(input, 2, 100);

    public static int Part2(string input)
        => FindCheats(input, 20, 100);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}