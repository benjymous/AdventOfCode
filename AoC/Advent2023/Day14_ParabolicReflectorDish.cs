namespace AoC.Advent2023;
public class Day14 : IPuzzle
{
    static IEnumerable<(List<(int x, int y)> positions, int rockCount)> FindZones_(HashSet<(int x, int y)> rocks, HashSet<(int x, int y)> obstacles, int gridSize)
    {
        for (int x = 0; x < gridSize; ++x)
        {
            (List<(int x, int y)> positions, int rockCount) entry = ([], 0);
            for (int y = 0; y < gridSize; ++y)
            {
                if (obstacles.Contains((x, y)))
                {
                    if (entry.rockCount > 0) yield return entry;

                    entry = ([], 0);
                }
                else
                {
                    if (rocks.Contains((x, y))) entry.rockCount++;

                    entry.positions.Add((x, y));
                }
            }
            if (entry.rockCount > 0) yield return entry;
        }
    }

    static IEnumerable<List<(int x, int y)>> FindZones(HashSet<(int x, int y)> obstacles, int gridSize)
    {
        for (int x = 0; x < gridSize; ++x)
        {
            List<(int x, int y)> entry = [];
            for (int y = 0; y < gridSize; ++y)
            {
                if (obstacles.Contains((x, y)))
                {
                    if (entry.Any()) yield return entry;

                    entry = [];
                }
                else
                {
                    entry.Add((x, y));
                }
            }
            if (entry.Any()) yield return entry;
        }
    }

    static (List<(int x, int y)> positions, int rockCount) CountRocksInZone(List<(int x, int y)> zone, HashSet<(int x, int y)> rocks)
    {
        int count = 0;
        foreach (var position in zone)
        {
            if (rocks.Contains(position)) count++;
        }
        return (zone, count);
    }

    public static HashSet<(int x, int y)> RotateGrid(HashSet<(int x, int y)> input, int gridSize) => input.Select(p => (gridSize - 1 - p.y, p.x)).ToHashSet();

    public static int Part1(string input)
    {
        var grid = Util.ParseSparseMatrix<char>(input);
        var gridSize = grid.Max(kvp => kvp.Key.x) + 1;

        HashSet<(int x, int y)> rocks = grid.Where(kvp => kvp.Value == 'O').Select(kvp => kvp.Key).ToHashSet();
        HashSet<(int x, int y)> obstacles = grid.Where(kvp => kvp.Value == '#').Select(kvp => kvp.Key).ToHashSet();

        return SlideRocks(rocks, obstacles, gridSize).Sum(el => gridSize - el.y);
    }

    private static HashSet<(int x, int y)> SlideRocks(HashSet<(int x, int y)> rocks, HashSet<(int x, int y)> obstacles, int gridSize)
        => FindZones(obstacles, gridSize).Select(z => CountRocksInZone(z, rocks))
            .SelectMany(z => z.positions.Take(z.rockCount)).ToHashSet();

    public static int Part2(string input)
    {
        var grid = Util.ParseSparseMatrix<char>(input);
        var gridSize = grid.Max(kvp => kvp.Key.x) + 1;

        HashSet<(int x, int y)> rocks = grid.Where(kvp => kvp.Value == 'O').Select(kvp => kvp.Key).ToHashSet();

        Dictionary<int, int> seen = [];

        HashSet<(int x, int y)>[] obstacles = [grid.Where(kvp => kvp.Value == '#').Select(kvp => kvp.Key).ToHashSet(), null, null, null];
        List<(int x, int y)>[][] zones = [null, null, null, null];
        for (int i = 0; i < 4; ++i)
        {
            if (i > 0) obstacles[i] = RotateGrid(obstacles[i - 1], gridSize);
            zones[i] = FindZones(obstacles[i], gridSize).ToArray();
        }

        int target = 0;

        for (int j = 0; j < 500; ++j)
        {
            for (int i = 0; i < 4; ++i)
            {
                var rockCount = zones[i].Select(z => CountRocksInZone(z, rocks));
                var newRockPositions = rockCount.SelectMany(z => z.positions.Take(z.rockCount)).ToHashSet();

                rocks = RotateGrid(newRockPositions, gridSize);
            }

            if (target == 0)
            {
                int checkSum = rocks.Sum(el => el.x * el.y);

                if (seen.TryGetValue(checkSum, out int prev))
                {
                    int cycle = j - prev;
                    if (target == 0) target = ((1000000000 - prev) % cycle) + j - 1;
                }

                seen[checkSum] = j;
            }
            else if (j == target) return rocks.Sum(el => gridSize - el.y);
        }

        return 0;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}