namespace AoC.Advent2019;
public class Day24 : IPuzzle
{
    public class State(bool Infinite = false)
    {
        public State(string input, bool isInf = false) : this(isInf) => Util.ParseSparseMatrix<char>(input, new Util.Convertomatic.SkipChars('.')).Keys.ForEach(p => Set(p.x, p.y));

        public readonly Dictionary<int, uint> cells = [];

        static uint Bit(int x, int y) => 1U << ((y * 5) + x);

        void Set(int x, int y, int level = 0) => cells[level] = cells.TryGetValue(level, out uint value) ? value | Bit(x, y) : Bit(x, y);

        bool Get(int x, int y, int level = 0) => cells.TryGetValue(level, out var v) && x >= 0 && y >= 0 && x < 5 && y < 5 && (v & Bit(x, y)) > 0;

        (int x, int y, int level)[] GetNeighbours(int x, int y, int level) => this.Memoize(x + (y << 3) + (level << 6), _ => Infinite ? GetNeighboursInfinite(x, y, level).SelectMany(v => v).ToArray() : GetNeighboursFlat(x, y, level));

        static (int x, int y, int level)[] GetNeighboursFlat(int x, int y, int level) => [(x - 1, y, level), (x + 1, y, level), (x, y - 1, level), (x, y + 1, level)];

        static IEnumerable<(int x, int y, int level)[]> GetNeighboursInfinite(int x, int y, int level)
        {
            foreach (var n in GetNeighboursFlat(x, y, level))
            {
                if (n.x == 2 && n.y == 2) // Centre cell, need to recurse in
                {
                    if (x == 1) /* -> */ yield return [(0, 0, level + 1), (0, 1, level + 1), (0, 2, level + 1), (0, 3, level + 1), (0, 4, level + 1)];
                    else if (x == 3) /* <- */ yield return [(4, 0, level + 1), (4, 1, level + 1), (4, 2, level + 1), (4, 3, level + 1), (4, 4, level + 1)];
                    else if (y == 1) /* v */  yield return [(0, 0, level + 1), (1, 0, level + 1), (2, 0, level + 1), (3, 0, level + 1), (4, 0, level + 1)];
                    else if (y == 3) /* ^ */ yield return [(0, 4, level + 1), (1, 4, level + 1), (2, 4, level + 1), (3, 4, level + 1), (4, 4, level + 1)];
                }
                else if (n.x == -1) /* left edge */ yield return [(1, 2, level - 1)];
                else if (n.x == 5) /* right edge */ yield return [(3, 2, level - 1)];
                else if (n.y == -1) /* top edge */ yield return [(2, 1, level - 1)];
                else if (n.y == 5) /* bottom edge */ yield return [(2, 3, level - 1)];
                else yield return [n];
            }
        }

        public void Tick(State oldState, int x, int y, int level = 0)
        {
            int neighbours = oldState.GetNeighbours(x, y, level).Count(n => oldState.Get(n.x, n.y, n.level));
            if (neighbours == 1 || (neighbours == 2 && !oldState.Get(x, y, level))) Set(x, y, level);
        }
    }

    public static uint Part1(string input)
    {
        HashSet<uint> seen = [];

        State s1 = new(input), s2 = new();

        while (true)
        {
            if (!seen.Add(s1.cells[0])) return s1.cells[0];
            s2.cells.Clear();
            foreach (var (x, y) in Util.Range2DExclusive((0, 5, 0, 5))) s2.Tick(s1, x, y);
            (s1, s2) = (s2, s1);
        }
    }

    public static int Part2(string input, int runs = 200)
    {
        State s1 = new(input, true), s2 = new(true);

        for (var i = 0; i < runs; ++i)
        {
            s2.cells.Clear();
            var (min, max) = s1.cells.Keys.MinMax();
            foreach (var (x, y, level) in Util.Range3DExclusive((min - 1, max + 2, 0, 5, 0, 5)).Where(p => p.x != 2 || p.y != 2)) s2.Tick(s1, x, y, level);
            (s1, s2) = (s2, s1);
        }

        return s1.cells.Sum(level => level.Value.CountBits());
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input, 200));
    }
}