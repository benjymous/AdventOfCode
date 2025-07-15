namespace AoC.Advent2015;
public class Day18 : IPuzzle
{
    public class State
    {
        public State(State other) => (Width, Height, corners) = (other.Width, other.Height, other.corners);

        private readonly int Width, Height;
        private readonly HashSet<int> cells = [], corners = [];

        public State(string input, bool stuckCorners)
        {
            var raw = Util.ParseSparseMatrix<bool>(input);
            cells = [.. raw.Keys.Select(v => v.x + (v.y << 8))];

            Width = raw.Width;
            Height = raw.Height;

            if (stuckCorners) corners = [0, Width, Height << 8, Width + (Height << 8)];
        }

        private readonly int[] neighbours = [-1, +1, 1 << 8, -1 << 8, -1 + (1 << 8), +1 + (1 << 8), -1 - (1 << 8), +1 - (1 << 8),];
        private int Neighbours(int p)
        {
            int count = 0;
            foreach (var n in neighbours) if (cells.Contains(p + n)) count++;
            return count;
        }

        public void Tick(State oldState)
        {
            cells.Clear();
            for (var y = 0; y <= Height << 8; y += 1 << 8)
            {
                for (var key = y; key <= Width + y; ++key)
                {
                    int neighbours = oldState.Neighbours(key);
                    if (neighbours == 3 || (neighbours == 2 && oldState.cells.Contains(key))) cells.Add(key);
                }
            }
            cells.UnionWith(corners);
        }

        public int Count => cells.Count;
    }

    public static int Run(string input, int steps, bool stuckCorners = false)
    {
        State s1 = new(input, stuckCorners), s2 = new(s1);

        for (int i = 0; i < steps; ++i)
        {
            s2.Tick(s1);

            (s1, s2) = (s2, s1);
        }

        return s1.Count;
    }

    public static int Part1(string input) => Run(input, 100);
    public static int Part2(string input) => Run(input, 100, true);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}