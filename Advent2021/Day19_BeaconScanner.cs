using AoC.Utils;
using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AoC.Advent2021
{
    public class Day19 : IPuzzle
    {
        public string Name => "2021-19";

        static Func<(int x, int y, int z), (int x, int y, int z)>[] transforms = Util.Values<Func<(int x, int y, int z), (int x, int y, int z)>>(v => (v.x, v.y, v.z), v => (-v.y, v.x, v.z), v => (-v.x, -v.y, v.z), v => (v.y, -v.x, v.z), v => (-v.z, v.y, v.x), v => (-v.y, -v.z, v.x), v => (v.z, -v.y, v.x), v => (v.y, v.z, v.x), v => (-v.x, v.y, -v.z), v => (-v.y, -v.x, -v.z), v => (v.x, -v.y, -v.z), v => (v.y, v.x, -v.z), v => (v.z, v.y, -v.x), v => (-v.y, v.z, -v.x), v => (-v.z, -v.y, -v.x), v => (v.y, -v.z, -v.x), v => (v.x, v.z, -v.y), v => (-v.z, v.x, -v.y), v => (-v.x, -v.z, -v.y), v => (v.z, -v.x, -v.y), v => (-v.x, v.z, v.y), v => (-v.z, -v.x, v.y), v => (v.x, -v.z, v.y), v => (v.z, v.x, v.y)).ToArray();

        public class Group
        {
            public Group(IEnumerable<ManhattanVector3> input)
            {
                input.ForEach(point => constellations[point.AsSimple()] = input.Select(other => (point - other).AsSimple()).ToHashSet());
                points = input.Select(p => p.AsSimple());
            }

            public (bool isOverlap, int func, ManhattanVector3 offset) TestOverlap(Group other)
            {
                foreach (var theirs in other.constellations)
                {
                    for (int f=0; f<transforms.Length; ++f)
                    {
                        var test = other.Transform((theirs.Key, f), theirs.Value);
                        foreach (var mine in constellations)
                        {
                            if (mine.Value.Intersect(test).Count() == 12) return (true, f, new ManhattanVector3(mine.Key) - new ManhattanVector3(transforms[f](theirs.Key)));                                   
                        }
                    }
                }          
                return (false,-1, ManhattanVector3.Zero);
            }

            public int Id;
            public ManhattanVector3 origin = new ManhattanVector3(0, 0, 0);
            public IEnumerable<(int x, int y, int z)> points;
            Dictionary<(int x, int y, int z), HashSet<(int x, int y, int z)>> constellations = new();
            Dictionary<((int x, int y, int z) pos, int funcIdx), HashSet<(int x, int y, int z)>> transformedCache = new();
            HashSet<(int x, int y, int z)> Transform(((int x, int y, int z) pos, int funcIdx) key, HashSet<(int x, int y, int z)> data) => transformedCache.GetOrCalculate(key, _ => data.Select(point => transforms[key.funcIdx](point)).ToHashSet());
        }

        private static IEnumerable<Group> AlignGroups(string input)
        {
            var groups = input.Split("\n\n").Select(bit => Util.Parse<ManhattanVector3>(bit.Split("\n", StringSplitOptions.RemoveEmptyEntries).Skip(1)).ToArray()).ToArray().WithIndex().Select(item => new Group(item.Value) { Id = item.Index }).ToArray();

            HashSet<Group> aligned = groups.Take(1).ToHashSet();
            HashSet<Group> unaligned = groups.Skip(1).ToHashSet();
            HashSet<(int, int)> compared = new HashSet<(int, int)>();

            while (unaligned.Any())
            { 
                foreach (var group in unaligned)
                {
                    foreach (var fixedGroup in aligned)
                    {
                        if (!compared.Add((fixedGroup.Id, group.Id))) continue;                     

                        var overlap = fixedGroup.TestOverlap(group);
                        if (overlap.isOverlap)
                        {
                            aligned.Add(new Group(group.points.Select(p => (new ManhattanVector3(transforms[overlap.func](p))) + overlap.offset)) { Id = group.Id, origin = overlap.offset });
                            unaligned.Remove(group);
                            break;
                        }
                    }
                }
            }
            return aligned;
        }

        private static int Part1(IEnumerable<Group> aligned) => aligned.Select(g => g.points.AsEnumerable()).Aggregate((lhs, rhs) => lhs.Union(rhs)).ToHashSet().Count();
        public static int Part2(IEnumerable<Group> aligned) => Util.Matrix(aligned, aligned).Max(pair => pair.item1.origin.Distance(pair.item2.origin));

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