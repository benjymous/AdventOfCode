namespace AoC.Advent2023;

public class Day21 : IPuzzle
{
    static ((int x, int y) start, HashSet<(int x, int y)> walkable, int gridSize) ParseData(string input)
    {
        var map = Util.ParseSparseMatrix<char>(input);
        var start = map.Where(kvp => kvp.Value == 'S').Single().Key;
        var walkable = map.Where(kvp => kvp.Value != '#').Select(kvp => kvp.Key).ToHashSet();

        return (start, walkable, map.Keys.Max(v => v.x) + 1);
    }

    public static List<int> CalcTileDistances((int x, int y) start, HashSet<(int, int)> walkable)
    {
        (int dx, int dy)[] neighbours = [(-1, 0), (1, 0), (0, -1), (0, 1)];

        Dictionary<(int x, int y), int> visited = [];

        Queue<(int distance, (int x, int y) pos)> queue = [];

        queue.Enqueue((0, start));

        while (queue.TryDequeue(out var next))
        {
            if (visited.ContainsKey(next.pos)) continue;

            visited[next.pos] = next.distance;

            foreach (var n in neighbours.Select(v => next.pos.OffsetBy(v)))
            {
                if (visited.ContainsKey(n) || !walkable.Contains(n)) continue;

                queue.Enqueue((next.distance + 1, n));
            }
        }

        return visited.Values.ToList();
    }

    public static int Part1(string input)
    {
        var (start, walkable, gridSize) = ParseData(input);

        var counts = CalcTileDistances(start, walkable);

        int distanceToEdge = gridSize == 11 ? 7 : (gridSize / 2);

        return counts.Where(d => d < distanceToEdge && (d % 2 == 0)).Count();
    }

    public static long Part2(string input)
    {
        var (start, walkable, gridSize) = ParseData(input);

        var counts = CalcTileDistances(start, walkable);

        int distanceToEdge = gridSize == 11 ? 7 : (gridSize / 2);

        long n = (26501365 - distanceToEdge) / gridSize;
        if (n != 202300)
        {
            throw new Exception($"n calc wrong, got {n}");
        }

        long numOddTiles = (n + 1) * (n + 1);
        long numEvenTiles = n * n;

        int oddCorners = counts.Count(v => v > distanceToEdge && v % 2 == 1);
        int evenCorners = counts.Count(v => v > distanceToEdge && v % 2 == 0);

        return (numOddTiles * counts.Count(v => v % 2 == 1))
                    + (numEvenTiles * counts.Count(v => v % 2 == 0))
                    - ((n + 1) * oddCorners)
                    + (n * evenCorners);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
