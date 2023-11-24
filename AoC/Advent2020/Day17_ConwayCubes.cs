namespace AoC.Advent2020;
public class Day17 : IPuzzle
{
    public class State(int dimensions)
    {
        public State(string input, int dimensions) : this(dimensions) => Cells = Util.ParseSparseMatrix<char>(input).Where(kvp => kvp.Value == '#').Select(kvp => (kvp.Key.x, kvp.Key.y, 0, 0)).ToHashSet();

        readonly int Dimensions = dimensions;
        public HashSet<(int x, int y, int z, int w)> Cells { get; private set; } = [];

        public IEnumerable<(int x, int y, int z, int w)> Positions()
        {
            var range = Cells.Aggregate((minx: 0, miny: 0, minz: 0, minw: 0, maxx: 0, maxy: 0, maxz: 0, maxw: 0), (curr, next) =>
                (Math.Min(curr.minx, next.x), Math.Min(curr.miny, next.y), Math.Min(curr.minz, next.z), Math.Min(curr.minw, next.w),
                 Math.Max(curr.maxx, next.x), Math.Max(curr.maxy, next.y), Math.Max(curr.maxz, next.z), Math.Max(curr.maxw, next.w)));

            if (Dimensions == 4) { range.minw--; range.maxw++; }
            range.minz--; range.maxz++;
            range.miny--; range.maxy++;
            range.minx--; range.maxx++;

            return range.Iterate();
        }
        readonly IEnumerable<(int x, int y, int z, int w)> directions = Directions(dimensions).ToList();

        public static IEnumerable<(int x, int y, int z, int w)> Directions(int dimensions) => (-1, -1, -1, dimensions == 4 ? -1 : 0, 1, 1, 1, dimensions == 4 ? 1 : 0).Iterate().Where(pos => pos.x != 0 || pos.y != 0 || pos.z != 0 || pos.w != 0);

        public bool CheckDirection((int x, int y, int z, int w) pos, (int x, int y, int z, int w) dir) =>
            Cells.Contains((pos.x + dir.x, pos.y + dir.y, pos.z + dir.z, pos.w + dir.w));

        public int Neighbours((int x, int y, int z, int w) pos) => directions.Count(d => CheckDirection(pos, d));

        public void Tick(State oldState, (int x, int y, int z, int w) pos)
        {
            int neighbours = oldState.Neighbours(pos);
            if (oldState.Cells.Contains(pos) ? (neighbours is 2 or 3) : (neighbours == 3)) Cells.Add(pos);
        }
    }

    static (State, State) Tick(State oldState, State newState)
    {
        newState.Cells.Clear();

        oldState.Positions().ToArray().ForEach(pos => newState.Tick(oldState, pos));

        return (oldState, newState);
    }

    public static int Run(string input, int cycles, int dimensions)
    {
        State s1 = new(input, dimensions), s2 = new(dimensions);

        while (cycles-- > 0) (s2, s1) = Tick(s1, s2);

        return s1.Cells.Count;
    }

    public static int Part1(string input) => Run(input, 6, 3);

    public static int Part2(string input) => Run(input, 6, 4);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}