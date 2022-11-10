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
            public Group(IEnumerable<ManhattanVector3> input, (int x, int y, int z) origin, int id=0)
            {
                Id = id;
                Origin = new ManhattanVector3(origin);
                Points = input.Select(p => p.AsSimple());
                input.ForEach(point => Offsets[point.AsSimple()] = input.Select(other => (point - other).AsSimple()).ToHashSet());
                Fingerprint = input.Select(p1 => input.Where(p2 => p2 != p1).Select(p2 => p1.Distance(p2)).OrderBy(v => v).Take(2).ToArray()).Select(a => (a[0], a[1])).ToHashSet();
            }

            public (bool isOverlap, int transformIdx, ManhattanVector3 offset) TestOverlap(Group other)
            {
                if (Fingerprint.Intersect(other.Fingerprint).Take(8).Count() == 8)
                {
                    foreach (var theirs in other.Offsets)
                    {
                        for (int transformIdx = 0; transformIdx < Transforms.Length; ++transformIdx)
                        {
                            var test = Transform(transformIdx, theirs.Value);
                            foreach (var mine in Offsets)
                            {
                                if (mine.Value.Intersect(test).Count() == 12) return (true, transformIdx, new ManhattanVector3(mine.Key) - new ManhattanVector3(Transforms[transformIdx](theirs.Key)));
                            }
                        }
                    }
                }
                return (false, -1, ManhattanVector3.Zero);
            }

            public readonly int Id;
            public readonly ManhattanVector3 Origin;
            public readonly IEnumerable<(int x, int y, int z)> Points;
            public readonly HashSet<(int v1, int v2)> Fingerprint;
            readonly Dictionary<(int x, int y, int z), HashSet<(int x, int y, int z)>> Offsets = new();

            static HashSet<(int x, int y, int z)> Transform(int transformIdx, HashSet<(int x, int y, int z)> data) => data.Select(point => Transforms[transformIdx](point)).ToHashSet();
        }

        private static IEnumerable<Group> AlignGroups(string input)
        {
            var groups = input.Split("\n\n").Select(bit => Util.Parse<ManhattanVector3>(bit.Split("\n", StringSplitOptions.RemoveEmptyEntries).Skip(1)).ToArray()).ToArray().WithIndex().Select(item => new Group(item.Value, (0,0,0), item.Index)).ToArray();

            HashSet<Group> aligned = groups.Take(1).ToHashSet();
            HashSet<Group> unaligned = groups.Skip(1).ToHashSet();

            while (unaligned.Any())
            { 
                foreach (var group in unaligned)
                {
                    foreach (var fixedGroup in aligned)
                    {
                        var (isOverlap, transformIdx, offset) = fixedGroup.TestOverlap(group);
                        if (isOverlap)
                        {
                            aligned.Add(new Group(group.Points.Select(p => new ManhattanVector3(Transforms[transformIdx](p)) + offset), offset.AsSimple(), group.Id));
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