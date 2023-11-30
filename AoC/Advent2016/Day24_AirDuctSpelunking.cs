namespace AoC.Advent2016;
public class Day24 : IPuzzle
{
    static uint LocationCode(char c) => 1U << (c - '0');
    static readonly uint HOME = LocationCode('0');

    public class MapData : IMap<PackedPos32>
    {
        readonly HashSet<PackedPos32> Data = [];
        public readonly Dictionary<uint, PackedPos32> Locations = [];
        public readonly Dictionary<uint, int> Paths = [];

        public readonly uint AllLocations = 0;
        public readonly int ShortestPath = 0;

        public MapData(string input)
        {
            var rawdata = Util.ParseSparseMatrix<PackedPos32, char>(input, new Util.Convertomatic.SkipChars('#')).Select(kvp => (ch: kvp.Value, pos: kvp.Key));

            Data = rawdata.Select(v => v.pos).ToHashSet();
            Locations = rawdata.Where(v => v.ch is >= '0' and <= '9').ToDictionary(kvp => LocationCode(kvp.ch), kvp => kvp.pos);
            AllLocations = Locations.Keys.Sum();
            Paths = (from loc1 in Locations
                     from loc2 in Locations
                     where loc1.Key < loc2.Key
                     select (loc1.Key + loc2.Key, this.FindPath(loc1.Value, loc2.Value).Length)).ToDictionary();

            ShortestPath = Paths.Min(kvp => kvp.Value);
        }

        public static uint[] Bits(uint input) => Memoize(input, _ => input.BitSequence().ToArray());

        static readonly PackedPos32[] Neighbours = [(1, 0), (0, 1), (-1, 0), (0, -1)];
        public IEnumerable<PackedPos32> GetNeighbours(PackedPos32 location) => Neighbours.Select(n => location + n).Where(n => Data.Contains(n));
    }

    public static int Solve(MapData map, bool returnHome)
    {
        return Solver<(uint location, uint visited, int distance), int>.Solve((HOME, HOME, 0), (state, solver) =>
        {
            uint tryLocations = map.AllLocations - state.visited;
            if (tryLocations > 0)
            {   // check locations not visited
                foreach (var location in MapData.Bits(tryLocations))
                {
                    if (map.Paths.TryGetValue(state.location | location, out var pathDistance))
                    {
                        var next = (location, visited: state.visited + location, distance: state.distance + pathDistance);

                        int remainingCount = (map.AllLocations - next.visited).CountBits();
                        if (!solver.IsBetterThanCurrentBest(next.distance + (map.ShortestPath * remainingCount))) continue;

                        if (solver.IsBetterThanSeen((next.location, next.visited), next.distance))
                        {
                            solver.Enqueue(next, next.distance * remainingCount);
                        }
                    }
                }
            }
            else // we have visited all the locations, so this is a possible solution
                return state.distance + (returnHome ? map.Paths[state.location + HOME] : 0);

            return default;
        }, Math.Min);
    }

    public static int Part1(string input) => Solve(new MapData(input), false);

    public static int Part2(string input) => Solve(new MapData(input), true);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}