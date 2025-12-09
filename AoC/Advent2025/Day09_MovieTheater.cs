namespace AoC.Advent2025;

public class Day09 : IPuzzle
{
    static IEnumerable<(long area, (int x, int y) p1, (int x, int y) p2)> GetAreas((int x, int y)[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            var pi = points[i];
            for (int j = i + 1; j < points.Length; j++)
            {
                var pj = points[j];
                yield return ((Math.Abs(pi.x - pj.x) + 1) * ((long)Math.Abs(pi.y - pj.y) + 1), pi, pj);
            }
        }
    }

    static void FloodFill(char[,] grid, int x, int y) =>
    Solver<(int x, int y)>.Solve((x, y), (pos, solver) =>
    {
        if (!grid.TryGetValue(pos, out var c) || c != 0) return;

        grid[pos.x, pos.y] = '-';

        solver.Enqueue((pos.x + 1, pos.y));
        solver.Enqueue((pos.x, pos.y + 1));
        solver.Enqueue((pos.x - 1, pos.y));
        solver.Enqueue((pos.x, pos.y - 1));
    });

    public class PrefixSum
    {
        private readonly int[,] psum;

        public PrefixSum(char[,] grid)
        {
            var (w, h) = (grid.GetLength(0), grid.GetLength(1));
            psum = new int[w + 1, h + 1];

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    psum[x + 1, y + 1] = (grid[x, y] == '-' ? 1 : 0) + psum[x, y + 1] + psum[x + 1, y] - psum[x, y];
        }

        public bool HasOutside((int a, int b) xv, (int a, int b) yv) =>
            psum[xv.b + 1, yv.b + 1] -
            psum[xv.a, yv.b + 1] -
            psum[xv.b + 1, yv.a] +
            psum[xv.a, yv.a] > 0;
    }

    static (int a, int b) Remap(Dictionary<int, int> map, int v1, int v2) => Util.MinMax(map[v1], map[v2]);

    public static long Part1(string input)
        => GetAreas(new Parser.AutoArray<(int x, int y)>(input)).Max(v => v.area);

    public static long Part2(string input)
    {
        var points = new Parser.AutoArray<(int x, int y)>(input);

        var remapX = points.Select(p => p.x).Distinct().Order().Index().ToDictionary(x => x.Item, x => x.Index + 1);
        var remapY = points.Select(p => p.y).Distinct().Order().Index().ToDictionary(y => y.Item, y => y.Index + 1);

        var (w, h) = (remapX.Count + 2, remapY.Count + 2);
        var grid = new char[w, h];

        for (int i = 0; i < points.Length; i++)
        {
            var (p1, p2) = (points[i], points[(i + 1) % points.Length]);

            var (xa, xb) = Remap(remapX, p1.x, p2.x);
            var (ya, yb) = Remap(remapY, p1.y, p2.y);

            for (int y = ya; y <= yb; ++y)
                grid[xa, y] = '#';

            for (int x = xa; x <= xb; ++x)
                grid[x, ya] = '#';
        }

        FloodFill(grid, 0, 0);

        var psum = new PrefixSum(grid);

        foreach (var (area, p1, p2) in GetAreas(points).OrderByDescending(v => v.area))
        {
            if (!psum.HasOutside(Remap(remapX, p1.x, p2.x), Remap(remapY, p1.y, p2.y)))
                return area;
        }

        return 0;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}