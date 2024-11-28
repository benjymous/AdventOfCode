namespace AoC.Advent2018;
public class Day20 : IPuzzle
{
    static readonly (int dx, int dy)[] Directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];

    public class Cell
    {
        public bool[] Exits = [false, false, false, false];

        public int DoorDistance { get; set; } = int.MaxValue;
    }

    static IEnumerable<char[]> SplitOptions(char[] input)
    {
        List<List<char>> parts = [];

        int close = 1, depth = 1;
        List<char> part = [];
        while (depth > 0)
        {
            var c = input[close++];
            switch (c)
            {
                case '(': depth++; part.Add(c); break;
                case ')': depth--; part.Add(c); break;
                case '|':
                    if (depth == 1)
                    {
                        parts.Add(part);
                        part = [];
                    }
                    else part.Add(c);
                    break;
                default: part.Add(c); break;
            }
        }
        parts.Add(part);

        var rest = input[close..];

        foreach (var p in parts) yield return [.. p, .. rest];
    }

    static Dictionary<(int x, int y), Cell> BuildMap(string input)
    {
        var map = new Dictionary<(int x, int y), Cell> { { (0, 0), new() { DoorDistance = 0 } } };

        Solver<((int x, int y) position, char next, char[] tape), object>.Solve(((0, 0), input[1], input[1..].ToArray()), (state, solver) =>
        {
            if (state.next == '(') solver.EnqueueRange(SplitOptions(state.tape).Select(option => (state.position, option[0], option)));
            else if ("NESW".TryGetIndex(state.next, out int forward))
            {
                map[state.position].Exits[forward] = true;
                var newPos = state.position.OffsetBy(Directions[forward]);

                map.GetOrCreate(newPos).Exits[(forward + 2) % 4] = true;
                solver.Enqueue((newPos, state.tape[1], state.tape[1..]));
            }
        });

        CalcFurthestDistance(map, (0, 0), map[(0, 0)]);

        return map;
    }

    private static void CalcFurthestDistance(Dictionary<(int x, int y), Cell> map, (int x, int y) pos, Cell cell)
    {
        foreach (var (neighbour, neighbourPos) in cell.Exits.Index().Where(e => e.Item).Select(e => pos.OffsetBy(Directions[e.Index])).Select(neighbourPos => (neighbour: map[neighbourPos], neighbourPos)).Where(v => v.neighbour.DoorDistance > cell.DoorDistance + 1))
        {
            neighbour.DoorDistance = cell.DoorDistance + 1;
            CalcFurthestDistance(map, neighbourPos, neighbour);
        }
    }

    public static int Part1(string input, Dictionary<(int x, int y), Cell> map = null)
    {
        map ??= BuildMap(input.Trim());

        return map.Values.Max(c => c.DoorDistance);
    }

    public static int Part2(string input, Dictionary<(int x, int y), Cell> map = null)
    {
        map ??= BuildMap(input.Trim());

        return map.Values.Count(c => c.DoorDistance >= 1000);
    }

    public void Run(string input, ILogger logger)
    {
        var map = BuildMap(input.Trim());

        logger.WriteLine("- Pt1 - " + Part1(input, map));
        logger.WriteLine("- Pt2 - " + Part2(input, map));
    }
}