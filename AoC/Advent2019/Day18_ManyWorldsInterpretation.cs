namespace AoC.Advent2019;
public class Day18 : IPuzzle
{
    public static int KeyCode(char c) => 1 << (char.ToLower(c) - 'a');

    public static int PlayerCode(int i) => 1 << (26 + i);

    public readonly struct RoomPath(IEnumerable<char> path)
    {
        public bool IsWalkable(int heldKeys) => (Doors & heldKeys) == Doors;
        readonly int Doors = path.Where(c => c is >= 'A' and <= 'Z').Sum(KeyCode);
        public readonly int Length = path.Count();
    }

    public class MapData : GridMap<char>
    {
        public int AllKeys { get; private set; } = 0;
        public int AllPlayers { get; private set; } = 0;

        public Dictionary<int, RoomPath> Paths { get; private set; } = [];

        public static int[] Bits(int input) => Memoize(input, _ => input.BitSequence().ToArray());

        public MapData(string input, QuestionPart part) : base(new AllWalkable<char>())
        {
            Data = Util.ParseSparseMatrix<char>(input, new Util.Convertomatic.SkipChars('#'));
            var startPositions = Data.Where(kvp => kvp.Value == '@').Select(kvp => kvp.Key).ToList();
            var keyPositions = Data.Where(kvp => kvp.Value is >= 'a' and <= 'z').Select(kvp => (KeyCode(kvp.Value), kvp.Key)).ToDictionary();
            AllKeys = keyPositions.Keys.Sum();

            if (part.Two() && startPositions.Count == 1)
            {
                var (x, y) = startPositions.First();
                startPositions = [(x - 1, y - 1), (x + 1, y - 1), (x - 1, y + 1), (x + 1, y + 1)];
                Data.RemoveRange([(x, y), (x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1)]);
            }

            AllPlayers = startPositions.Select((v, i) => PlayerCode(i)).Sum();
            Paths = keyPositions.SelectMany(kvp1 => startPositions.Select((player, index) => (pathKey: PlayerCode(index) | kvp1.Key, path: this.FindPath(player, kvp1.Value)))).Concat(keyPositions.SelectMany(kvp1 => keyPositions.Where(kvp => kvp.Key > kvp1.Key).Select(kvp => (pathKey: kvp1.Key | kvp.Key, path: this.FindPath(kvp1.Value, kvp.Value))))).Where(v => v.path.Length > 0).ToDictionary(v => v.pathKey, v => new RoomPath(v.path.Select(pos => Data[pos])));
        }
    }

    public static int Solve(MapData map)
    {
        var shortestPath = map.Paths.Values.Min(path => path.Length);

        return Solver<(int positions, int heldKeys, int distance)>.Solve((map.AllPlayers, 0, 0), (state, solver) =>
        {
            var (positions, heldKeys, distance) = state;

            if (solver.IsBetterThanCurrentBest(solver.PreviousPriority))
            {
                foreach (var position in MapData.Bits(positions))
                {
                    var tryKeys = MapData.Bits(map.AllKeys - heldKeys);
                    foreach (var key in tryKeys)
                    {
                        int remainingCount = tryKeys.Length - 1;

                        if (map.Paths.TryGetValue(position | key, out var path) && path.IsWalkable(heldKeys) && solver.IsBetterThanCurrentBest(path.Length + distance + (remainingCount * shortestPath)))
                        {
                            var next = (positions: positions - position + key, heldKeys: heldKeys + key, distance: distance + path.Length);

                            if (solver.IsBetterThanSeen((next.positions, next.heldKeys), next.distance))
                            {
                                var nextEstimatedDistance = next.distance + (remainingCount * shortestPath);

                                if (remainingCount == 0) return next.distance;
                                else if (solver.IsBetterThanCurrentBest(nextEstimatedDistance)) solver.Enqueue(next, nextEstimatedDistance);
                            }
                        }
                    }
                }
            }
            return default;
        }, Math.Min);
    }
    public static int Part1(string input)
    {
        var map = new MapData(input, QuestionPart.Part1);
        return Solve(map);
    }

    public static int Part2(string input)
    {
        var map = new MapData(input, QuestionPart.Part2);
        return Solve(map);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}