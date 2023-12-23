namespace AoC.Advent2023;
public class Day23 : IPuzzle
{
    private static int FindScenicRoute(string input, bool part1)
    {
        var map = Util.ParseSparseMatrix<char>(input);

        var maxY = map.Max(kvp => kvp.Key.y);

        var start = map.Where(kvp => kvp.Key.y == 0 && kvp.Value == '.').Single().Key;
        var end = map.Where(kvp => kvp.Key.y == maxY && kvp.Value == '.').Single().Key;

        if (!part1)
        {
            var slopes = map.Where(kvp => kvp.Value != '#' && kvp.Value !='.');
            slopes.ForEach(kvp => map[kvp.Key] = '.');
        }


        Direction2[] neighbours = [Direction2.East, Direction2.West, Direction2.North, Direction2.South];

        Dictionary<(int, int), int> cache = [];

        return Solver<((int x, int y) pos, HashSet<(int x, int y)> visited), int>.Solve((start, new HashSet<(int, int)>()), (state, solver) =>
        {
            if (state.pos == end)
            {
                Console.WriteLine($"Found path {state.visited.Count} length .. {solver.CurrentBest}");

                //for (int y=0; y<=maxY; ++y)
                //{
                //    for (int x=0; x<=maxY; ++x)
                //    {
                //        if (state.visited.Contains((x,y)))
                //        {
                //            Console.Write("O");
                //        }
                //        else
                //        {
                //            Console.Write(map[(x, y)]);
                //        }
                //    }
                //    Console.WriteLine();
                //}

                //Console.WriteLine();

                return state.visited.Count;
            }

            foreach (var dir in neighbours)
            {
                var next = state.pos.OffsetBy(dir);
                if (part1)
                {
                    if (cache.TryGetValue(next, out var prev) && prev > state.visited.Count+1) continue;
                    cache[next] = state.visited.Count+1;
                }
                if (state.visited.Contains(next)) continue;
                if (!map.TryGetValue(next, out var cell)) continue;
                if ( (cell != '.' && cell != dir.AsChar())) continue;
                //if (!part1 && cell == '#') continue;

                solver.Enqueue((next, [.. state.visited, state.pos]), -(state.visited.Count+1));
            }

            return default;
        }, Math.Max);
    }

    public static int Part1(string input)
    {
        return FindScenicRoute(input, true);
    }

    public static int Part2(string input)
    {
        return FindScenicRoute(input, false);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input)); 
        logger.WriteLine("- Pt2 - " + Part2(input)); // 4330 << too low
    }
}
