namespace AoC.Advent2023;
public class Day14 : IPuzzle
{
    static IEnumerable<List<(int x, int y)>> FindZones(HashSet<(int x, int y)> obstacles, int gridSize)
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

    public static HashSet<(int x, int y)> RotateGrid(IEnumerable<(int x, int y)> input, int gridSize) => input.Select(p => (gridSize - 1 - p.y, p.x)).ToHashSet();

    static (HashSet<(int x, int y)> rocks, HashSet<(int x, int y)> obstacles, int gridSize) Init(string input)
    {
        var grid = Util.ParseSparseMatrix<char>(input);

        return (rocks: grid.Where(kvp => kvp.Value == 'O').Select(kvp => kvp.Key).ToHashSet(),
            obstacles: grid.Where(kvp => kvp.Value == '#').Select(kvp => kvp.Key).ToHashSet(),
             gridSize: grid.Max(kvp => kvp.Key.x) + 1);
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

        Dictionary<int, int> seen = [];

        List<(int x, int y)>[][] zones = [null, null, null, null];
        for (int i = 0; i < 4; ++i)
        {
            zones[i] = FindZones(obstacles, gridSize).ToArray();
            if (i < 3) obstacles = RotateGrid(obstacles, gridSize);
        }

        int originalTarget = 1000000000;
        int target = originalTarget;

        for (int j = 0; j <= target; ++j)
        {
            for (int i = 0; i < 4; ++i)
                rocks = RotateGrid(zones[i].SelectMany(z => z.Take(z.Count(rocks.Contains))), gridSize);

            if (target == originalTarget)
            {
                int checkSum = rocks.Sum(el => el.x * el.y);

                if (seen.TryGetValue(checkSum, out int prev))
                {
                    int cycle = j - prev;
                    target = ((originalTarget - prev) % cycle) + j - 1;
                }
                else seen[checkSum] = j;
            }
        }

        return rocks.Sum(el => gridSize - el.y);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}