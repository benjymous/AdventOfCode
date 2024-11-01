namespace AoC.Advent2023;
public class Day23 : IPuzzle
{
    static readonly Direction2[] neighbours = [Direction2.East, Direction2.West, Direction2.North, Direction2.South];

    class Map : IMap<(int x, int y)>
    {
        public readonly Dictionary<(int x, int y), char> Data;
        readonly List<(int x, int y)> Waypoints;
        public readonly Dictionary<int, Dictionary<int, int>> Routes = [];
        public readonly int EndKey = -1;

        public (int x, int y) Destination = (0, 0);

        public Map(string input, QuestionPart part)
        {
            Data = Util.ParseSparseMatrix<char>(input, new Util.Convertomatic.SkipChars('#'));
            int mapSize = Data.Max(kvp => kvp.Key.y);

            Waypoints = [Data.Single(kvp => kvp.Key.y == 0 && kvp.Value == '.').Key,
            ..Data.Keys.Where(pos => neighbours.Select(d => pos.OffsetBy(d)).Count(p => Data.ContainsKey(p)) > 2),
            Data.Single(kvp => kvp.Key.y == mapSize && kvp.Value == '.').Key];

            EndKey = Waypoints.Count - 1;

            if (!part.One())
            {
                Data.Where(kvp => kvp.Value is not '.').ForEach(kvp => Data[kvp.Key] = '.');
            }

            for (int i = 0; i < Waypoints.Count - 1; ++i)
            {
                Routes[i] = [];
                for (int j = Waypoints.Count - 1; j >= 1; --j)
                {
                    Destination = Waypoints[j];
                    var route = this.FindPath(Waypoints[i], Waypoints[j]).Length;
                    if (route > 0)
                    {
                        Routes[i][j] = route;

                        if (j == EndKey) break;
                    }
                }
            }
        }

        public IEnumerable<(int x, int y)> GetNeighbours((int x, int y) location) => neighbours.Select(dir => (dir, pos: location.OffsetBy(dir))).Where(v => ((Data.TryGetValue(v.pos, out var c) && c == '.') || c == v.dir.AsChar()) && (v.pos == Destination || !Waypoints.Contains(v.pos))).Select(v => v.pos);
    }

    private static int FindScenicRoute(string input, QuestionPart part)
    {
        var map = new Map(input, part);

        return Solver<(int position, ulong visited, int steps), int>.Solve((0, 0, 0), (state, solver) =>
        {
            foreach (var next in map.Routes[state.position])
            {
                if ((state.visited & (1UL << next.Key)) == 0)
                {
                    if (next.Key == map.EndKey) return state.steps + next.Value;

                    solver.Enqueue((next.Key, state.visited + (1UL << next.Key), state.steps + next.Value));
                }
            }

            return default;
        }, Math.Max);
    }

    public static int Part1(string input) => FindScenicRoute(input, QuestionPart.Part1);
    public static int Part2(string input) => FindScenicRoute(input, QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}