namespace AoC.Advent2023;
public class Day22 : IPuzzle
{

    public class Brick
    {
        [Regex(@"(\d+),(\d+),(\d+)~(\d+),(\d+),(\d+)")]
        public Brick(int x1, int y1, int z1, int x2, int y2, int z2)
        {
            for (int z = z1; z <= z2; ++z)
            {
                for (int y = y1; y <= y2; ++y)
                {
                    for (int x = x1; x <= x2; ++x)
                    {
                        Cubes.Add((x, y, z));
                    }
                }
            }
            id = (x1, y1, z2, x2, y2, z2).GetHashCode();
        }

        public List<(int x, int y, int z)> Cubes = [];

        public HashSet<Brick> Supporting = [];
        public HashSet<Brick> SupportedBy = [];

        public bool Stable = false;

        public int id = 0;
        public string Name;

        public IEnumerable<(int x, int y, int z)> Bottom() => Cubes.Where(c => !Cubes.Contains((c.x, c.y, c.z - 1)));
        public IEnumerable<(int x, int y, int z)> Top() => Cubes.Where(c => !Cubes.Contains((c.x, c.y, c.z + 1)));

        public void Drop()
        {
            Cubes = Cubes.Select(p => (p.x, p.y, p.z - 1)).ToList();
        }

        public override string ToString() => Name;
    }



    public static Brick[] SimulateBricks(string input)
    {
        Brick[] bricks = Util.RegexParse<Brick>(input).ToArray();

        bricks.WithIndex(0).ForEach(elem => elem.Value.Name = ((char)('A' + elem.Index)).ToString());

        Dictionary<(int x, int y, int z), Brick> index = bricks.SelectMany(b => b.Cubes.Select(c => (c, b))).ToDictionary();


        bool dropped = false;
        do
        {
            dropped = false;

            var active = bricks.Where(b => !b.Stable).ToArray();

            Console.WriteLine($"{active.Length} bricks");

            foreach (var brick in active)
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
                    if (touching.Any(p => index[(p.x, p.y, p.z - 1)].Stable))
                    {
                        brick.Stable = true;
                    }

                    continue;
                }

                brick.Drop();
                dropped = true;
                index = bricks.SelectMany(b => b.Cubes.Select(c => (c, b))).ToDictionary();
            }
        } while (dropped);

        index = bricks.SelectMany(b => b.Cubes.Select(c => (c, b))).ToDictionary();



        foreach (var brick in bricks)
        {
            var top = brick.Top().ToArray();
            brick.Supporting = top.Select(p => (p.x, p.y, p.z + 1)).Where(p => index.ContainsKey(p)).Select(p => index[p]).ToHashSet();
            foreach (var s in brick.Supporting)
            {
                s.SupportedBy.Add(brick);
            }

        }

        return bricks;
    }

    public static int Part1(string input)
    {
        Brick[] bricks = SimulateBricks(input);

        int targets = 0;
        foreach (var brick in bricks)
        {
            if (brick.Supporting.Count == 0 || !brick.Supporting.Any(s => s.SupportedBy.Count == 1))
            {
                targets++;
            }
        }

        return targets;
    }

    public static bool IsSupportedBy(Brick brick, Brick support) => Memoize((brick.id, support.id), _ =>
    {
        if (brick.SupportedBy.Contains(support) && brick.SupportedBy.Count == 1)
        {
            return true;
        }

        if (brick.SupportedBy.Any(c => IsSupportedBy(c, support)))
        {
            return true;
        }

        return false;
    });

    public static int Part2(string input)
    {
        Brick[] bricks = SimulateBricks(input);
        Console.WriteLine();
        int count = 0;
        foreach (var b1 in bricks)
        {
            foreach (var b2 in bricks)
            {
                if (IsSupportedBy(b1, b2))
                {
                    count++;
                }
            }
        }
        return count;
    }

    public void Run(string input, ILogger logger)
    {
        //logger.WriteLine("- Pt1 - " + Part1(input)); 
        logger.WriteLine("- Pt2 - " + Part2(input)); // 87746 << too high
    }
}
