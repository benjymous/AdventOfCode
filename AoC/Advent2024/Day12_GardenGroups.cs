namespace AoC.Advent2024;
public class Day12 : IPuzzle
{
    static readonly (int, int)[] neighbours = [(-1, 0), (1, 0), (0, -1), (0, 1)];

    static IEnumerable<HashSet<(int, int)>> GetRegions(string input)
    {
        var map = Util.ParseSparseMatrix<char>(input);

        return map.Select(kvp => FloodFill(map, kvp.Key, kvp.Value).ToHashSet());
    }

    static IEnumerable<(int x, int y)> FloodFill(Dictionary<(int, int), char> map, (int x, int y) pos, char expected)
    {
        if (!map.TryGetValue(pos, out var next) || next != expected) return [];

        map.Remove(pos);
        return [pos, .. neighbours.SelectMany(d => FloodFill(map, pos.OffsetBy(d), expected))];
    }

    static IEnumerable<((int, int), (int, int))> GetPerimeter(HashSet<(int, int)> region)
        => region.SelectMany(cell => neighbours.Where(n => !region.Contains(cell.OffsetBy(n))).Select(n => (cell, n)));

    static bool IsAdjacent(((int x, int y) p, (int, int) d) a, ((int x, int y) p, (int, int) d) b)
        => !(a.d != b.d) && (a.p.x == b.p.x ? Math.Abs(a.p.y - b.p.y) == 1 : a.p.y == b.p.y && Math.Abs(a.p.x - b.p.x) == 1);

    static int CountEdges(IEnumerable<((int, int) pos, (int, int))> perim)
    {
        List<List<((int, int), (int, int))>> edges = [];

        foreach (var (face, edge) in perim.OrderBy(v => v.pos)
            .Select(section => (section, edges.FirstOrDefault(e => e.Any(p => IsAdjacent(section, p))))))
        {
            if (edge != default) edge.Add(face);
            else edges.Add([face]);
        }

        return edges.Count;
    }

    public static int Part1(string input)
        => GetRegions(input).Sum(region => GetPerimeter(region).Count() * region.Count);

    public static int Part2(string input)
        => GetRegions(input).Sum(region => CountEdges(GetPerimeter(region)) * region.Count);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}