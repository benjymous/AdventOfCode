namespace AoC.Advent2023;
public class Day10 : IPuzzle
{
    static Dictionary<(int x, int y), Direction2[]> GetLoop(string input) => Memoize(input, _ =>
    {
        var grid = Util.ParseSparseMatrix<char>(input);
        var pos = grid.Single(kvp => kvp.Value == 'S').Key;

        Dictionary<(int x, int y), Direction2[]> visited = [];

        Direction2 lastDir = Direction2.Null;

        while (true)
        {
            var current = grid[pos];
            var dir = PossibleDirections(current).Select(d => (Direction2)d).First(dir => (current == 'S' || (lastDir - dir) != 2) && ValidateDirection(grid, pos, dir));

            var next = pos.OffsetBy(dir);
            visited[pos] = current == 'S' ? [dir] : [lastDir, dir];
            if (grid[next] == 'S') return visited;

            pos = next;
            lastDir = dir;
        }
    });

    static string PossibleDirections(char current) => current switch
    {
        'S' => "^>v<",
        '|' => "^v",
        '-' => "<>",
        'J' => "^<",
        '7' => "v<",
        'F' => "v>",
        'L' => "^>",
        _ => "",
    };

    static bool ValidateDirection(Dictionary<(int x, int y), char> map, (int x, int y) position, Direction2 dir) => PossibleDirections(map[position.OffsetBy(dir)]).Contains(dir + 2);

    static int FloodFill((int x, int y) pos, Dictionary<(int x, int y), Direction2[]> matrix)
    {
        if (matrix.ContainsKey(pos)) return 0;
        matrix[pos] = [];
        return 1 + FloodFill(pos.OffsetBy((0, 1)), matrix) + FloodFill(pos.OffsetBy((0, -1)), matrix) + FloodFill(pos.OffsetBy((1, 0)), matrix) + FloodFill(pos.OffsetBy((-1, 0)), matrix);
    }

    public static int Part1(string input) => GetLoop(input).Count / 2;

    public static int Part2(string input)
    {
        var loop = GetLoop(input);

        int minY = loop.Min(v => v.Key.y);

        var clockwise = loop.Any(kvp => kvp.Key.y == minY && kvp.Value[0] == Direction2.Right);

        var loopKeys = loop.Keys.ToArray();
        return loopKeys.SelectMany(pos => loop[pos].Distinct().Select(dir => FloodFill(pos.OffsetBy(dir + (clockwise ? 1 : -1)), loop))).Sum();
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}