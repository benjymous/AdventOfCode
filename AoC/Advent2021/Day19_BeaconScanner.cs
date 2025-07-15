namespace AoC.Advent2021;
public class Day19 : IPuzzle
{
    private static readonly Func<(int x, int y, int z), (int x, int y, int z)>[] Transforms = [v => (v.x, v.y, v.z), v => (-v.y, v.x, v.z), v => (-v.x, -v.y, v.z), v => (v.y, -v.x, v.z), v => (-v.z, v.y, v.x), v => (-v.y, -v.z, v.x), v => (v.z, -v.y, v.x), v => (v.y, v.z, v.x), v => (-v.x, v.y, -v.z), v => (-v.y, -v.x, -v.z), v => (v.x, -v.y, -v.z), v => (v.y, v.x, -v.z), v => (v.z, v.y, -v.x), v => (-v.y, v.z, -v.x), v => (-v.z, -v.y, -v.x), v => (v.y, -v.z, -v.x), v => (v.x, v.z, -v.y), v => (-v.z, v.x, -v.y), v => (-v.x, -v.z, -v.y), v => (v.z, -v.x, -v.y), v => (-v.x, v.z, v.y), v => (-v.z, -v.x, v.y), v => (v.x, -v.z, v.y), v => (v.z, v.x, v.y)];

    public readonly struct Group(int id, IEnumerable<(int x, int y, int z)> input, (int x, int y, int z) origin = default)
    {
        public (bool isOverlap, int transformIdx, (int x, int y, int z) offset) TestOverlap(Group other)
        {
            if (Fingerprint.Intersect(other.Fingerprint).Count() >= 8)
            {
                foreach (var theirs in other.Offsets)
                {
                    for (int transformIdx = 0; transformIdx < Transforms.Length; ++transformIdx)
                    {
                        var test = Transform(transformIdx, theirs.Value);
                        foreach (var mine in Offsets.Where(mine => mine.Value.Intersect(test).Count() >= 12))
                        {
                            return (true, transformIdx, mine.Key.Subtract(Transforms[transformIdx](theirs.Key)));
                        }
                    }
                }
            }
            return default;
        }

        public readonly int Id = id;
        public readonly (int x, int y, int z) Origin = origin;
        public readonly (int x, int y, int z)[] Points = [.. input];
        public readonly HashSet<int> Fingerprint = [.. input.Select(p1 => input.Where(p2 => p2 != p1).Select(p2 => p1.Distance(p2)).Order().Take(2).ToArray()).Select(a => a[0] + (a[1] << 16))];
        private readonly Dictionary<(int x, int y, int z), HashSet<(int x, int y, int z)>> Offsets = input.Select(point => (point, input.Select(other => point.Subtract(other)).ToHashSet())).ToDictionary();

        private static HashSet<(int x, int y, int z)> Transform(int transformIdx, HashSet<(int x, int y, int z)> data) => [.. data.Select(point => Transforms[transformIdx](point))];
    }

    private static List<Group> AlignGroups(string input) => Memoize(input, _ =>
    {
        var groups = input.SplitSections().Select(bit => Parser.Parse<ManhattanVector3>(bit.Split("\n", StringSplitOptions.RemoveEmptyEntries).Skip(1)).Select(p => p.AsSimple()).ToArray()).ToArray().Index().Select(item => new Group(item.Index, item.Item)).ToArray();

        Dictionary<int, int> overlaps = [];
        for (int i = 0; i < groups.Length; ++i)
            for (int j = i + 1; j < groups.Length; ++j)
                overlaps.IncrementAtIndex(i, groups[i].Fingerprint.Intersect(groups[j].Fingerprint).Count());

        var mostMatches = overlaps.OrderByDescending(v => v.Value).First().Key;

        List<Group> aligned = [groups[mostMatches]];
        Queue<Group> unaligned = [.. groups.Where(g => g.Id != mostMatches)];

        unaligned.Operate((group) =>
        {
            foreach (var (isOverlap, transformIdx, offset) in aligned.Select(a => a.TestOverlap(group)).Where(v => v.isOverlap))
            {
                aligned.Add(new Group(group.Id, group.Points.Select(p => Transforms[transformIdx](p).OffsetBy(offset)), offset));
                return;
            }
            unaligned.Enqueue(group);
        });

        return aligned;
    });

    public static int Part1(string input) => AlignGroups(input).Select(g => g.Points.AsEnumerable()).Aggregate((lhs, rhs) => lhs.Union(rhs)).ToHashSet().Count;

    public static int Part2(string input)
    {
        var aligned = AlignGroups(input);
        return Util.Matrix(aligned, aligned).Max(pair => pair.item1.Origin.Distance(pair.item2.Origin));
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}