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

        class Group
        {
            public int Id;
            public ManhattanVector3 origin = null;

            public Group(IEnumerable<ManhattanVector3> input)
            {
                foreach (var point in input)
                {
                    HashSet<(int x, int y, int z)> entry = new();
                    foreach (var other in input)
                    {
                        var diff = point - other;
                        entry.Add(diff.AsSimple());
                    }
                    var p = point.AsSimple();
                    constellations[p]=entry;
                    points.Add(p);
                }
            }

            public (bool isOverlap, int func, ManhattanVector3 offset) TestOverlap(Group other, IEnumerable<Func<(int x, int y, int z), (int x, int y, int z)>> funcs)
            {

                foreach (var func in funcs.WithIndex())
                {
                    //var test = other.constellations.Select(c => Transform(func.Value, c));

                    foreach (var mine in constellations)
                    {
                        var pivot1 = mine.Key;
                        foreach (var theirs in other.constellations)
                        {
                            var test = Transform(func.Value, theirs.Value);
                            var pivot2 = func.Value(theirs.Key);
                            var intersect = mine.Value.Intersect(test);
                            if (intersect.Count() == 12)
                            {
                                ManhattanVector3 offset = new ManhattanVector3(pivot1) - new ManhattanVector3(pivot2);
                                Console.WriteLine($"{pivot1} == {pivot2} => {offset}");
                                return (true, func.Index, offset);
                            }
                        }
                    }
                }

                return (false,-1, ManhattanVector3.Zero);
            }

            HashSet<(int x, int y, int z)> Transform(Func<(int x, int y, int z), (int x, int y, int z)> f, HashSet<(int x, int y, int z)> data)
            {
                return data.Select(point => f(point)).ToHashSet();
            }

            public HashSet<(int x, int y, int z)> points = new();
            Dictionary<(int x, int y, int z), HashSet<(int x, int y, int z)>> constellations = new();
        }

        private static List<Group> AlignGroups(string input)
        {
            var bits = input.Split("\n\n");

            var data = bits.Select(bit => Util.Parse<ManhattanVector3>(bit.Split("\n", StringSplitOptions.RemoveEmptyEntries).Skip(1)).ToArray()).ToArray();

            var groups = data.WithIndex().Select(item => new Group(item.Value) { Id = item.Index }).ToArray();

            //List<Func<(int x, int y, int z), (int x, int y, int z)>> funcs = new();

            Func<(int x, int y, int z), (int x, int y, int z)>[] funcs = GetTransforms();

            //var test = funcs.Select(f => f((5, 6, -4))).ToHashSet();

            //var v1 = (test.Contains((5, 6, -4)));
            //var v2 = (test.Contains((-5, -4, -6)));
            //var v3 = (test.Contains((4, 6, 5)));
            //var v4 = (test.Contains((-4, -6, 5)));
            //var v5 = (test.Contains((-6, -4, -5)));

            HashSet<int> overlaps = new();


            var primeGroup = groups.First();
            primeGroup.origin = new ManhattanVector3(0, 0, 0);

            List<Group> aligned = new();
            aligned.Add(primeGroup);
            HashSet<Group> unaligned = groups.Skip(1).ToHashSet();

            HashSet<(int, int)> compared = new HashSet<(int, int)>();

            HashSet<int> usedFuncs = new();

            while (unaligned.Any())
            {
                Console.WriteLine($"{aligned.Count}/{groups.Count()}");

                foreach (var group in unaligned)
                {
                    foreach (var fixedGroup in aligned)
                    {
                        var key = (fixedGroup.Id, group.Id);
                        if (compared.Contains(key)) continue;

                        compared.Add(key);

                        var overlap = fixedGroup.TestOverlap(group, funcs);
                        if (overlap.isOverlap)
                        {
                            Console.WriteLine($"Group {group.Id} aligns with group {fixedGroup.Id} with rotation {overlap.func}");

                            usedFuncs.Add(overlap.func);

                            // rotate overlap into final place..

                            var newGroup = new Group(group.points.Select(p => (new ManhattanVector3(funcs[overlap.func](p))) + overlap.offset));
                            newGroup.Id = group.Id;
                            newGroup.origin = overlap.offset;


                            var intersect = fixedGroup.points.Intersect(newGroup.points).Count();
                            if (intersect < 12)
                            {
                                Console.WriteLine("Oh dear!");
                            }

                            aligned.Add(newGroup);
                            unaligned.Remove(group);
                            //reset = true;
                            break;
                        }
                        // if (reset) break;
                    }
                    //if (reset) break;
                }
            }

            Console.WriteLine($"{aligned.Count} / {groups.Count()}");
            return aligned;
        }

        private static Func<(int x, int y, int z), (int x, int y, int z)>[] GetTransforms()
        {

            return Util.Values<Func<(int x, int y, int z), (int x, int y, int z)>>(
    v => (v.x, v.y, v.z),   //  0 *
    v => (-v.y, v.x, v.z),  //  1 *
    v => (-v.x, -v.y, v.z), //  2 *
    v => (v.y, -v.x, v.z),  //  3 *

    v => (-v.z, v.y, v.x),  //  4 *
    v => (-v.y, -v.z, v.x), //  5 *
    v => (v.z, -v.y, v.x),  //  6 *
    v => (v.y, v.z, v.x),   //  7 *

    v => (-v.x, v.y, -v.z), //  8 *
    v => (-v.y, -v.x, -v.z),//  9 *
    v => (v.x, -v.y, -v.z), // 10 *
    v => (v.y, v.x, -v.z),  // 11 *

    v => (v.z, v.y, -v.x),  // 12 *
    v => (-v.y, v.z, -v.x),// 13
    v => (-v.z, -v.y, -v.x),// 14 *
    v => (v.y, -v.z, -v.x), // 15 *

    v => (v.x, v.z, -v.y),  // 16 *
    v => (-v.z, v.x, -v.y), // 17 *
    v => (-v.x, -v.z, -v.y),// 18 *
    v => (v.z, -v.x, -v.y), // 19 *

    v => (-v.x, v.z, v.y),  // 20 *
    v => (-v.z, -v.x, v.y), // 21 *
    v => (v.x, -v.z, v.y),  // 22 *
    v => (v.z, v.x, v.y)    // 23 *
).ToArray();

            //return Util.Values<Func<(int x, int y, int z), (int x, int y, int z)>>(
            //    v => (v.x, v.y, v.z),   //  0
            //    v => (v.y, v.x, v.z),   //  1
            //    v => (-v.x, -v.y, v.z), //  2 *
            //    v => (v.y, -v.x, v.z),  //  3 *

            //    v => (-v.z, v.y, v.x),  //  4 *
            //    v => (-v.y, -v.z, v.x), //  5 *
            //    v => (v.z, -v.y, v.x),  //  6 *
            //    v => (v.y, v.z, v.x),   //  7

            //    v => (-v.x, v.y, -v.z), //  8 *
            //    v => (-v.y, -v.x, -v.z),//  9 *
            //    v => (v.x, -v.y, -v.z), // 10 *
            //    v => (v.y, v.x, -v.z),  // 11

            //    v => (v.z, v.y, -v.x),  // 12 *
            //    v => (-v.y, -v.z, -v.x), // 13
            //    v => (-v.z, -v.y, -v.x),// 14 *
            //    v => (v.y, -v.z, -v.x), // 15 *

            //    v => (v.x, v.z, -v.y),   // 16 *
            //    v => (-v.z, v.x, -v.y),  // 17 *
            //    v => (-v.x, v.z, -v.y), // 18
            //    v => (v.z, -v.x, -v.y),  // 19 *

            //    v => (-v.x, v.z, v.y),  // 20 *
            //    v => (-v.z, -v.x, v.y), // 21
            //    v => (v.x, -v.z, v.y),  // 22 *
            //    v => (v.z, v.x, v.y)    // 23 *
            //).ToArray();
        }

        public static int Part1(string input)
        {
            List<Group> aligned = AlignGroups(input);

            HashSet<(int x, int y, int z)> allPoints = new HashSet<(int x, int y, int z)>();
            foreach (var group in aligned)
            {
                allPoints = allPoints.Union(group.points).ToHashSet();
            }

            return allPoints.Count();
        }

        public static int Part2(string input)
        {
            List<Group> aligned = AlignGroups(input);

            return Util.Matrix(aligned, aligned).Max(pair => pair.x.origin.Distance(pair.y.origin));
        }

        public void Run(string input, ILogger logger)
        {



            //Console.WriteLine(Part1(test.Replace("\r", "")));
            //Console.WriteLine(Part2(test.Replace("\r", "")));

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}