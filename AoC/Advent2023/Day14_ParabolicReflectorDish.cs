namespace AoC.Advent2023;
public class Day14 : IPuzzle
{
    private static IEnumerable<List<(int x, int y)>> FindZones(HashSet<(int x, int y)> obstacles, int gridSize)
    {
        for (int x = 0; x < gridSize; ++x)
        {
            List<(int x, int y)> entry = [];
            for (int y = 0; y < gridSize; ++y)
            {
                if (obstacles.Contains((x, y)))
                {
                    if (entry.Count != 0) yield return entry;

                    entry = [];
                }
                else entry.Add((x, y));
            }
            if (entry.Count != 0) yield return entry;
        }
    }

    public static HashSet<(int x, int y)> RotateGrid(IEnumerable<(int x, int y)> input, int gridSize) => [.. input.Select(p => (gridSize - 1 - p.y, p.x))];

    private static (HashSet<(int x, int y)> rocks, HashSet<(int x, int y)> obstacles, int gridSize) Init(string input)
    {
        var grid = Util.ParseSparseMatrix<char>(input);

        return (rocks: [.. grid.KeysWithValue('O')],
            obstacles: [.. grid.KeysWithValue('#')],
             gridSize: grid.Width + 1);
    }

    public static int Part1(string input)
    {
        var (rocks, obstacles, gridSize) = Init(input);

        return FindZones(obstacles, gridSize)
            .SelectMany(z => z.Take(z.Count(rocks.Contains)))
            .Sum(el => gridSize - el.y);
    }

    public static int Part2(string input)
    {
        var (rocks, obstacles, gridSize) = Init(input);

        List<(int x, int y)>[][] zones = [null, null, null, null];
        for (int i = 0; i < 4; ++i)
        {
            zones[i] = [.. FindZones(obstacles, gridSize)];
            if (i < 3) obstacles = RotateGrid(obstacles, gridSize);
        }

        int originalTarget = 1000000000;

        return Util.FindCycle(originalTarget, rocks,
            r => r.Sum(e => e.x * e.y),
            r =>
            {
                for (int i = 0; i < 4; ++i)
                    r = RotateGrid(zones[i].SelectMany(z => z.Take(z.Count(r.Contains))), gridSize);

                return r;
            }).Sum(el => gridSize - el.y);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}