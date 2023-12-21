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

    public static int CountSteps(string input, int targetSteps)
    {
        var (start, walkable, _) = ParseData(input);

        return WalkMap(targetSteps, start, walkable);
    }

    private static int WalkMap(int targetSteps, (int x, int y) start, HashSet<(int x, int y)> walkable)
    {
        var ends = new HashSet<(int x, int y)>();
        var visited = new HashSet<(int x, int y)>();

        (int dx, int dy)[] neighbours = [(-1, 0), (1, 0), (0, -1), (0, 1)];

        Queue<((int x, int y) pos, int steps)> queue = [];
        queue.Enqueue((start, 0));

        while (queue.TryDequeue(out var state))
        {
            if (state.steps % 2 == 0)
            {
                ends.Add(state.pos);

                if (state.steps == targetSteps || visited.Contains(state.pos))
                {
                    continue;
                }
            }

            visited.Add(state.pos);

            foreach (var delta in neighbours)
            {
                var next = state.pos.OffsetBy(delta);
                if (walkable.Contains(next))
                {
                    queue.Enqueue((next, state.steps + 1));
                }
            }
        }

        return ends.Count;
    }

    public static long SolvePart2(string input, int maxDist)
    {
        var (start, walkable, gridSize) = ParseData(input);
        var fullGridOdd = walkable.Where(v => (v.Distance(start) % 2) == 1).Count();
        var fullGridEven = walkable.Where(v => (v.Distance(start) % 2) == 0).Count();

        var oddEven = maxDist % 2;
        int range = (maxDist / gridSize) + 1;
        Console.WriteLine($"oddEven: {oddEven}");
        Console.WriteLine($"distance: {maxDist}");
        Console.WriteLine($"gridSize: {gridSize}");
        Console.WriteLine($"range: {range}");

        //range = 0;

        long count = 0;

        for (int y = -range; y <= range; ++y)
        {
            for (int x = -range; x <= range; ++x)
            {
                var cell = (x, y);

                var realCentre = (x: cell.x * gridSize, y: cell.y * gridSize);

                int centreDistance = realCentre.Distance(0, 0);

                if (centreDistance - gridSize > maxDist)
                {
                    continue;
                }

                int thisOddEven = (centreDistance % 2) + oddEven;

                //Console.WriteLine();
                //Console.WriteLine($"{cell} : {realCentre} : {centreDistance} : {(thisOddEven == 0 ? "even" : "odd")}");

                if (centreDistance < maxDist - gridSize)
                {

                    //Console.WriteLine($"{cell} is a full cell");
                    count += thisOddEven == 0 ? fullGridEven : fullGridOdd;
                }
                else
                {

                    var translated = walkable.Select(p => (p.x - start.x + realCentre.x, p.y - start.y + realCentre.y));
                    var distances = translated.Select(p => p.Distance(0, 0));
                    var checks = distances.Where(d => d <= maxDist && ((d % 2) == thisOddEven));

                    var subCount = checks.Count();
                    //                    .Distance(0, 0)).Where(d => d <= maxDist && ((d % 2) == thisOddEven)).Count();

                    count += subCount;

                    //Console.WriteLine($"Need to count in {cell}");
                }
            }
        }

        //for (int y = 0; y < gridSize; ++y)
        //{
        //    for (int x = 0; x < gridSize; ++x)
        //    {
        //        if ((x, y) == start) Console.Write("S");
        //        else if (fullGridOdd.Contains((x, y))) Console.Write("O");
        //        else if (walkable.Contains((x, y))) Console.Write(".");
        //        else Console.Write("#");
        //    }
        //    Console.WriteLine();
        //}

        return count;
    }

    public static int Part1(string input) => CountSteps(input, 64);

    public static long Part2(string input) => SolvePart2(input, 26501365);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
