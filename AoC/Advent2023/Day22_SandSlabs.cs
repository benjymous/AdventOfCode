namespace AoC.Advent2023;
public class Day22 : IPuzzle
{
    public class Brick
    {
        [Regex(@"(\d+),(\d+),(\d+)~(\d+),(\d+),(\d+)")]
        public Brick(int x1, int y1, int z1, int x2, int y2, int z2)
        {
            for (int z = z1; z <= z2; ++z)
                for (int y = y1; y <= y2; ++y)
                    for (int x = x1; x <= x2; ++x)
                        Cubes.Add((x, y, z));
        }

        public List<(int x, int y, int z)> Cubes = [];
        public HashSet<Brick> Supporting = [], SupportedBy = [];

        public bool Stable = false;

        public IEnumerable<(int x, int y, int z)> Bottom() => Cubes.Where(c => !Cubes.Contains((c.x, c.y, c.z - 1)));

        public void Drop() => Cubes = Cubes.Select(p => (p.x, p.y, p.z - 1)).ToList();
    }

    public static Brick[] SimulateBricks(string input)
    {
        Brick[] bricks = Util.RegexParse<Brick>(input).OrderBy(b => b.Cubes.Min(c => c.z)).ToArray();

        Dictionary<(int x, int y, int z), Brick> index = bricks.SelectMany(b => b.Cubes.Select(c => (c, b))).ToDictionary();

        Queue<Brick> active = [.. bricks];

        while (active.TryDequeue(out Brick brick))
        {
            var bottom = brick.Bottom().ToArray();
            if (bottom.Any(p => p.z == 1))
            {
                brick.Stable = true;
                continue;
            }

            var touching = bottom.Where(p => index.ContainsKey((p.x, p.y, p.z - 1))).ToArray();

            if (touching.Length != 0)
            {
                brick.Stable = touching.Any(p => index[(p.x, p.y, p.z - 1)].Stable);
                continue;
            }

            index.RemoveRange(brick.Cubes);
            brick.Drop();
            index.AddRange(brick.Cubes.Select(c => (c, brick)));

            active.Add(brick);
        }

        foreach (var brick in bricks)
        {
            foreach (var cube in brick.Cubes.Select(c => (c.x, c.y, c.z + 1)))
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