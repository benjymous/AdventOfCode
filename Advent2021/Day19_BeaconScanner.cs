using AoC.Utils;
using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day19 : IPuzzle
    {
        public string Name => "2021-19";

        static readonly Func<(int x, int y, int z), (int x, int y, int z)>[] Transforms = Util.Values<Func<(int x, int y, int z), (int x, int y, int z)>>(v => (v.x, v.y, v.z), v => (-v.y, v.x, v.z), v => (-v.x, -v.y, v.z), v => (v.y, -v.x, v.z), v => (-v.z, v.y, v.x), v => (-v.y, -v.z, v.x), v => (v.z, -v.y, v.x), v => (v.y, v.z, v.x), v => (-v.x, v.y, -v.z), v => (-v.y, -v.x, -v.z), v => (v.x, -v.y, -v.z), v => (v.y, v.x, -v.z), v => (v.z, v.y, -v.x), v => (-v.y, v.z, -v.x), v => (-v.z, -v.y, -v.x), v => (v.y, -v.z, -v.x), v => (v.x, v.z, -v.y), v => (-v.z, v.x, -v.y), v => (-v.x, -v.z, -v.y), v => (v.z, -v.x, -v.y), v => (-v.x, v.z, v.y), v => (-v.z, -v.x, v.y), v => (v.x, -v.z, v.y), v => (v.z, v.x, v.y)).ToArray();

        public class Group
        {
            public Group(IEnumerable<(int x, int y, int z)> input, (int x, int y, int z) origin, int id)
            {
                Id = id;
                Origin = origin;
                Points = input.ToArray();
                Offsets = Points.Select(point => (point, Points.Select(other => point.Subtract(other)).ToHashSet())).ToDictionary();
                Fingerprint = Points.Select(p1 => Points.Where(p2 => p2 != p1).Select(p2 => p1.Distance(p2)).Order().Take(2).ToArray()).Select(a => a[0] + (a[1] << 16)).ToList();
            }

            public (bool isOverlap, int transformIdx, (int x, int y, int z) offset) TestOverlap(Group other)
            {
                if (Fingerprint.Intersect(other.Fingerprint).Count() >= 8)
                {
                    foreach (var theirs in other.Offsets)
                    {
                        for (int transformIdx = 0; transformIdx < Transforms.Length; ++transformIdx)
                        {
                            var test = Transform(transformIdx, theirs.Value);
                            foreach (var mine in Offsets)
                            {
                                if (mine.Value.Intersect(test).Count() >= 12) return (true, transformIdx, mine.Key.Subtract(Transforms[transformIdx](theirs.Key)));
                            }
                        }
                    }
                }
                return (false, -1, default);
            }

            public readonly int Id;
            public readonly (int x, int y, int z) Origin;
            public readonly IEnumerable<(int x, int y, int z)> Points;
            public readonly List<int> Fingerprint;
            readonly Dictionary<(int x, int y, int z), HashSet<(int x, int y, int z)>> Offsets;

            static HashSet<(int x, int y, int z)> Transform(int transformIdx, HashSet<(int x, int y, int z)> data) => data.Select(point => Transforms[transformIdx](point)).ToHashSet();

            public override int GetHashCode() => Id;
        }

        private static IEnumerable<Group> AlignGroups(string input)
        {
            var groups = input.Split("\n\n").Select(bit => Util.Parse<ManhattanVector3>(bit.Split("\n", StringSplitOptions.RemoveEmptyEntries).Skip(1)).Select(p => p.AsSimple()).ToArray()).ToArray().WithIndex().Select(item => new Group(item.Value, (0, 0, 0), item.Index)).ToArray();

            Dictionary<int, int> overlaps = new();
            for (int i = 0; i < groups.Length; ++i)
            {
                overlaps[i] = 0;
                for (int j = i + 1; j < groups.Length; ++j)
                {
                    overlaps[i] += groups[i].Fingerprint.Intersect(groups[j].Fingerprint).Count();
                }
            }

            var mostMatches = overlaps.OrderByDescending(v => v.Value).First().Key;

            HashSet<Group> aligned = new() { groups[mostMatches] };
            HashSet<Group> unaligned = new(groups.Where(g => g.Id != mostMatches));

            HashSet<int> tried = new();
            while (unaligned.Count != 0)
            {
                foreach (var group in unaligned)
                {
                    foreach (var fixedGroup in aligned)
                    {
                        if (tried.Add(group.Id + (fixedGroup.Id << 16)) == false) continue;
                        var (isOverlap, transformIdx, offset) = fixedGroup.TestOverlap(group);
                        if (isOverlap)
                        {
                            aligned.Add(new Group(group.Points.Select(p => Transforms[transformIdx](p).OffsetBy(offset)), offset, group.Id));
                            unaligned.Remove(group);
                            break;
                        }
                    }
                }
            }
            return aligned;
        }

        private static int Part1(IEnumerable<Group> aligned) => aligned.Select(g => g.Points.AsEnumerable()).Aggregate((lhs, rhs) => lhs.Union(rhs)).ToHashSet().Count;
        public static int Part2(IEnumerable<Group> aligned) => Util.Matrix(aligned, aligned).Max(pair => pair.item1.Origin.Distance(pair.item2.Origin));

        public static int Part1(string input)
        {
            return Part1(AlignGroups(input));
        }

        public static int Part2(string input)
        {
            return Part2(AlignGroups(input));
        }

        public void Run(string input, ILogger logger)
        {
            IEnumerable<Group> aligned = AlignGroups(input);

            logger.WriteLine("- Pt1 - " + Part1(aligned));
            logger.WriteLine("- Pt2 - " + Part2(aligned));
        }
    }
}