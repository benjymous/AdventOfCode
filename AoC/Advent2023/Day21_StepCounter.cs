namespace AoC.Advent2023;

public class Day21 : IPuzzle
{
    static (IEnumerable<int> counts, int gridSize, int distanceToEdge) ParseData(string input)
    {
        var map = Util.ParseSparseMatrix<char>(input);
        var gridSize = map.Width + 1;

        return (counts: CalcTileDistances(map.Single(kvp => kvp.Value == 'S').Key, map.Where(kvp => kvp.Value != '#').Select(kvp => kvp.Key).ToHashSet()), gridSize, gridSize == 11 ? 7 : (gridSize / 2));
    }

    public static IEnumerable<int> CalcTileDistances((int x, int y) start, HashSet<(int, int)> walkable)
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

        return visited.Values;
    }

    public static int Part1(string input)
    {
        var (counts, gridSize, distanceToEdge) = ParseData(input);

        return counts.Count(d => d < distanceToEdge && (d % 2 == 0));
    }

    public static long Part2(string input)
    {
        var (counts, gridSize, distanceToEdge) = ParseData(input);

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