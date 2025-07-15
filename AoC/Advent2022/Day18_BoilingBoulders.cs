namespace AoC.Advent2022;
public class Day18 : IPuzzle
{
    [Regex("(.+)")]
    public record class Node([Regex("(.+),(.+),(.+)")] (int x, int y, int z) Pos)
    {
        public ((int, int, int), (int, int, int))[] Edges() =>
        [
            ((Pos.x - 1, Pos.y, Pos.z), Pos),
            (Pos, (Pos.x + 1, Pos.y, Pos.z)),
            ((Pos.x, Pos.y - 1, Pos.z), Pos),
            (Pos, (Pos.x, Pos.y + 1, Pos.z)),
            ((Pos.x, Pos.y, Pos.z - 1), Pos),
            (Pos, (Pos.x, Pos.y, Pos.z + 1)),
        ];
    }

    private static int CountOuterEdges(IEnumerable<Node> cells)
        => cells.SelectMany(cell => cell.Edges()).GetUniqueItems().Count();

    private static readonly (int dx, int dy, int dz)[] Neighbours = [(-1, 0, 0), (1, 0, 0), (0, -1, 0), (0, 1, 0), (0, 0, -1), (0, 0, 1)];
    private static void FloodFill((int x, int y, int z) pos, HashSet<(int x, int y, int z)> matrix)
    {
        if (matrix.Remove(pos))
        {
            foreach (var (dx, dy, dz) in Neighbours)
                FloodFill((pos.x + dx, pos.y + dy, pos.z + dz), matrix);
        }
    }

    public static int Part1(string input) => CountOuterEdges(Parser.Parse<Node>(input));

    public static int Part2(string input)
    {
        var cells = Parser.Parse<Node>(input).ToArray();

        var range = cells.GetRange(v => v.Pos);
        var airPositions = Util.Range3DInclusive(range).Except(cells.Select(c => c.Pos)).ToHashSet();
        var boundaries = airPositions.Where(c => c.x == range.minX || c.x == range.maxX || c.y == range.minY || c.y == range.maxY || c.z == range.minZ || c.z == range.maxZ);

        while (boundaries.Any()) FloodFill(boundaries.First(), airPositions);

        return CountOuterEdges(cells.Union(airPositions.Select(p => new Node(p))));
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}