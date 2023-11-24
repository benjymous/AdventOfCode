namespace AoC.Advent2020;
public partial class Day24 : IPuzzle
{
    [GeneratedRegex("(ne|nw|se|sw|e|w)")] private static partial Regex DirectionSplitter();
    static IEnumerable<string> SplitCommands(string line) => DirectionSplitter().Split(line).WithoutNullOrWhiteSpace();

    static HexVectorPointy FollowPath(string path) => SplitCommands(path).Aggregate(new HexVectorPointy(), (prev, step) => prev.TranslateHex(step));

    public class State
    {
        public State(int reserve) => Cells = new HashSet<PackedPos32>(reserve);

        public State(HashSet<HexVectorPointy> initialState)
        {
            Cells = new HashSet<PackedPos32>(initialState.Count);
            foreach (var v in initialState) Set((v.X, v.Y));
        }

        public void Reset()
        {
            Cells.Clear();
            Range.Reset();
        }

        public readonly Accumulator2D<int> Range = new();
        public HashSet<PackedPos32> Cells { get; private set; }

        public void Set((int x, int y) pos)
        {
            Range.Add(pos.x, pos.y);
            Cells.Add(pos);
        }

        public bool Get((int x, int y) pos) => Cells.Contains(pos);

        public bool CheckDirection((int x, int y) pos, (int x, int y, int z) dir) =>
            Get((pos.x + dir.x, pos.y + dir.y));

        public int Neighbours((int x, int y) pos) =>
            HexVectorPointy.Directions.Values.Count(d => CheckDirection(pos, d));

        public void Tick(State oldState, (int x, int y) pos)
        {
            int neighbours = oldState.Neighbours(pos);
            var cellstate = oldState.Get(pos);

            if (cellstate && (neighbours == 0 || neighbours > 2)) cellstate = false;
            else if (!cellstate && neighbours == 2) cellstate = true;

            if (cellstate) Set(pos);
        }
    }

    public static (State, State) Tick(State oldState, State newState)
    {
        newState.Reset();
        foreach (var pos in oldState.Range.RangeBuffered(1)) newState.Tick(oldState, pos);
        return (oldState, newState);
    }

    public static int Run(HashSet<HexVectorPointy> initialState, int cycles)
    {
        State s1 = new(initialState), s2 = new(initialState.Count);

        while (cycles-- > 0) (s2, s1) = Tick(s1, s2);

        return s1.Cells.Count;
    }

    static HashSet<HexVectorPointy> GetInitialState(string input) => input.Trim().Split("\n").Select(line => FollowPath(line)).GroupBy(v => v).Where(group => group.Count() % 2 == 1).Select(group => group.First()).ToHashSet();

    public static int Part1(string input) => GetInitialState(input).Count;

    public static int Part2(string input) => Run(GetInitialState(input), 100);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}