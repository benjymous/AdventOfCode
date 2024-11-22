namespace AoC.Advent2023;

using PackedPos = PackedVect3<int, Pack8_8_16>;
public class Day22 : IPuzzle
{
    public class Brick
    {
        [Regex(@"(\d+),(\d+),(\d+)~(\d+),(\d+),(\d+)")]
        public Brick(int x1, int y1, int z1, int x2, int y2, int z2)
        {
            _Cubes = Util.Range3DInclusive<int, Pack8_8_16>((z1, z2, y1, y2, x1, x2)).ToArray();
            _Bottom = _Cubes.Where(c => c.Z == z1).Select(c => c - (0, 0, 1)).ToArray();
            Lowest = z1;
        }

        readonly PackedPos[] _Cubes = [], _Bottom = [];
        public int Lowest = 0, OffsetZ = 0;
        public IEnumerable<PackedPos> Cubes => _Cubes.Select(c => c + (0, 0, OffsetZ));

        public HashSet<Brick> Supporting = [], SupportedBy = [];

        bool Stable = false;

        public bool Update(Dictionary<PackedPos, Brick> index)
        {
            if (!(Stable = (Lowest + OffsetZ == 1) || _Bottom.Any(cube => index.TryGetValue(cube + (0, 0, OffsetZ), out var brick) && brick.Stable)))
            {
                index.RemoveRange(Cubes);
                OffsetZ--;
                index.AddRange(Cubes.Select(c => (c, this)));
                return true;
            }
            return false;
        }
    }

    public static Brick[] SimulateBricks(Util.AutoParse<Brick> input)
    {
        Brick[] bricks = [.. input.OrderBy(b => b.Lowest)];

        Dictionary<PackedPos, Brick> index = bricks.SelectMany(b => b.Cubes.Select(c => (c, b))).ToDictionary();

        Queue<Brick> active = [.. bricks];

        while (active.TryDequeue(out Brick brick))
        {
            if (brick.Update(index))
            {
                active.Add(brick);
            }
        }

        foreach (var brick in bricks)
        {
            foreach (var cube in brick.Cubes.Select(c => (c.X, c.Y, c.Z + 1)))
            {
                if (index.TryGetValue(cube, out var aboveBrick) && aboveBrick != brick)
                {
                    brick.Supporting.Add(aboveBrick);
                    aboveBrick.SupportedBy.Add(brick);
                }
            }
        }

        return bricks;
    }

    private static bool IsFreelyRemovable(Brick brick) => brick.Supporting.Count == 0 || !brick.Supporting.Any(s => s.SupportedBy.Count == 1);
    private static bool WillCauseReaction(Brick brick) => !IsFreelyRemovable(brick);

    static int CountWillFall(Brick brick)
    {
        HashSet<Brick> fallen = [];
        var active = new Queue<Brick>() { brick };

        while (active.TryDequeue(out var b))
        {
            fallen.Add(b);

            active.EnqueueRange(b.Supporting.Where(s => !s.SupportedBy.Except(fallen).Any()));
        }

        return fallen.Count - 1;
    }

    public static int Part1(string input) => SimulateBricks(input).Count(IsFreelyRemovable);
    public static int Part2(string input) => SimulateBricks(input).Where(WillCauseReaction).Sum(CountWillFall);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}