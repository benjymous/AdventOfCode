namespace AoC.Advent2022;
public class Day12 : IPuzzle
{
    public class MapData : IMap<PackedPos32>
    {
        public MapData(string input)
        {
            Grid = Util.ParseSparseMatrix<PackedPos32, char>(input);
            Index = Grid.InvertFrozen();

            Start = Index['S'].First();
            End = Index['E'].First();

            Grid[Start] = 'a';
            Grid[End] = 'z';
        }

        public readonly Dictionary<PackedPos32, char> Grid;
        public readonly FrozenDictionary<char, IEnumerable<PackedPos32>> Index;

        public readonly PackedPos32 Start, End;

        static readonly PackedPos32[] neighbours = [(0, 1), (0, -1), (1, 0), (-1, 0)];
        public virtual IEnumerable<PackedPos32> GetNeighbours(PackedPos32 center)
        {
            var maxClimb = Grid[center] + 1;

            return neighbours.Select(delta => center + delta)
                             .Where(pt => Grid.TryGetValue(pt, out var height) && height <= maxClimb);
        }
    }

    public static int Part1(string input)
    {
        var map = new MapData(input);

        return map.FindPath(map.Start, map.End).Length;
    }

    public static int Part2(string input)
    {
        var map = new MapData(input);
        var allStarts = map.Index['a'];
        var goodStarts = allStarts.Where(pos => map.GetNeighbours(pos).Any(pos => map.Grid[pos] == 'b')).ToHashSet();
        allStarts.Except(goodStarts).ForEach(pos => map.Grid.Remove(pos));

        return Solver<PackedPos32>.Solve(goodStarts, (pos, solver) =>
        {
            var route = map.FindPath(pos, map.End);
            if (route.Length != 0)
            {
                int routeLength = route.Length;

                var intersect = route.Reverse().Intersect(goodStarts).ToArray();
                if (intersect.Length != 0)
                {
                    goodStarts.ExceptWith(intersect);
                    return routeLength - Array.IndexOf(route, intersect[0]);
                }

                return routeLength;
            }

            return default;
        }, Math.Min);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}