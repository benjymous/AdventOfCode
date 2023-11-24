namespace AoC.Advent2021;
public class Day20 : IPuzzle
{
    class State
    {
        private readonly HashSet<PackedPos32> data = [];
        int MinX, MaxX, MinY, MaxY;

        public State(IEnumerable<(int x, int y)> input) => Set(input);

        public void Set(IEnumerable<(int x, int y)> input)
        {
            MinX = 0; MaxX = 0; MinY = 0; MaxY = 0;
            data.Clear();
            input.ForEach(p =>
            {
                MinX = Math.Min(MinX, p.x);
                MinY = Math.Min(MinY, p.y);
                MaxX = Math.Max(MaxX, p.x);
                MaxY = Math.Max(MaxY, p.y);
                data.Add(p);
            });
        }

        public int Get(PackedPos32 pos) => data.Contains(pos) ? 1 : 0;

        public (int minY, int maxY, int minX, int maxX) GetRange(int margin) => (MinY - margin, MaxY + margin, MinX - margin, MaxX + margin);

        public int Count => data.Count;
    }

    static (int x, int y)[] Step(State input, bool[] rules, int margin, bool crop, (int minY, int maxY, int minX, int maxX) cropSize)
        => Util.Range2DInclusive(input.GetRange(margin))
               .Where(pos => rules[GetRuleIndex(input, pos)] && (!crop || (pos.x >= cropSize.minX && pos.x <= cropSize.maxX && pos.y >= cropSize.minY && pos.y <= cropSize.maxY))).ToArray();

    static readonly PackedPos32[] Offsets = [(-1, -1), (0, -1), (1, -1), (-1, 0), (0, 0), (1, 0), (-1, 1), (0, 1), (1, 1)];

    static int GetRuleIndex(State input, PackedPos32 key)
    {
        int result = 0;
        for (int i = 0; i < 9; ++i)
        {
            result = (result << 1) + input.Get(key + Offsets[i]);
        }
        return result;
    }

    public static int Simulate(string input, int steps, int margin = 1, bool crop = false)
    {
        var bits = input.Split("\n\n");
        var rules = bits[0].Select(b => b == '#').ToArray();

        var map = new State(Util.ParseSparseMatrix<bool>(bits[1]).Keys);

        var range = map.GetRange(steps);

        for (int i = 0; i < steps; i++)
        {
            map.Set(Step(map, rules, margin, crop && (i % 2) == 1, range));
        }

        return map.Count;
    }

    public static int Part1(string input) => Simulate(input, steps: 2, margin: 4, crop: true);

    public static int Part2(string input) => Simulate(input, steps: 50, margin: 75, crop: true);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}