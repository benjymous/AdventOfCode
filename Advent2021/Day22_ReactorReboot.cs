using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2021
{
    public class Day22 : IPuzzle
    {
        public string Name => "2021-22";

        public enum OnOff
        {
            on,
            off,
        }

        public static IEnumerable<int> GetDivisions(int a1, int a2, int b1, int b2)
        {
            return new HashSet<int> { a1, a2, b1, b2 }.OrderBy(v => v);
        }

        public static List<Box> Subtract(Box b1, Box b2)
        {
            var res = new List<Box>();

            var divX = GetDivisions(b1.min[0], b1.max[0], b2.min[0], b2.max[0]).ToArray();
            var divY = GetDivisions(b1.min[1], b1.max[1], b2.min[1], b2.max[1]).ToArray();
            var divZ = GetDivisions(b1.min[2], b1.max[2], b2.min[2], b2.max[2]).ToArray();

            foreach (var zpair in divZ.OverlappingPairs())
            {
                foreach (var ypair in divY.OverlappingPairs())
                {
                    foreach (var xpair in divX.OverlappingPairs())
                    {
                        var newBox = new Box(xpair.first, xpair.second-1, ypair.first, ypair.second-1, zpair.first, zpair.second-1);

                        // keep segments in b1 but not b2
                        if (newBox.Overlaps(b1) && !newBox.Overlaps(b2))
                        {
                            res.Add(newBox);
                        }           
                    }
                }
            }

            return res;
        }

        public class Box
        {
            public Box(int x1, int x2, int y1, int y2, int z1, int z2)
            {
                min[0] = x1; max[0] = x2+1;
                min[1] = y1; max[1] = y2+1;
                min[2] = z1; max[2] = z2+1;
            }
            public int[] min = new int[3];
            public int[] max = new int[3];

            public override string ToString()
            {
                return $"[{min[0]}..{max[0]-1}, {min[1]}..{max[1]-1}, {min[2]}..{max[2]-1}]";
            }

            public ulong Volume => (ulong)((max[0] - min[0]) * (max[1] - min[1]) * (max[2] - min[2]));

            public bool Overlaps(Box other)
            {
                var x = (min[0] < other.max[0]) && (max[0] > other.min[0]);
                var y = (min[1] < other.max[1]) && (max[1] > other.min[1]);
                var z = (min[2] < other.max[2]) && (max[2] > other.min[2]);

                return x && y && z;
            }

            public bool OtherIsInside(Box other)
            {
                var x = (min[0]) <= other.min[0] && (max[0] >= other.max[0]);
                var y = (min[1]) <= other.min[1] && (max[1] >= other.max[1]);
                var z = (min[2]) <= other.min[2] && (max[2] >= other.max[2]);

                return x && y && z;
            }

            //public IEnumerable<Box> Overlay(Box other)
            //{
            //    return Subtract(other, this);
            //}


            //public IEnumerable<Box> Split(Instruction instruction)
            //{
            //    var other = instruction.Box;
            //    if (instruction.Action == OnOff.on)
            //    {
            //        if (OtherIsInside(other))
            //        {
            //            return new Box[] { this };
            //        }
            //        else if (other.OtherIsInside(this))
            //        {
            //            return new Box[] { instruction.Box };
            //        }
            //        else if (Overlaps(instruction.Box))
            //        {
            //            Box bigger, smaller;

            //            if (this.Volume > other.Volume)
            //            {
            //                bigger = this; smaller = other;
            //            }
            //            else
            //            {
            //                smaller = this; bigger = other;
            //            }

            //            var v1 = bigger.Volume;

            //            var res = Subtract(smaller, bigger);

            //            if (!res.Any())
            //            {
            //                Console.WriteLine("Hmm?");
            //            }

            //            res.Add(bigger);
            //            var v2 = CellCount(res);

            //            if (v2 < v1)
            //            {
            //                Console.WriteLine("Oops");
            //            }

            //            return res;
            //        }
            //        else
            //        {
            //            return new Box[] { this, other };
            //        }
            //    }
            //    else
            //    {
            //        if (Overlaps(instruction.Box))
            //        {
            //            var v1 = this.Volume - instruction.Box.Volume;
            //            var res = Subtract(this, instruction.Box);
            //            var v2 = CellCount(res);
            //            return res;
            //        }
            //        else
            //        {
            //            // subtraction has no effect
            //            return new Box[] { this };
            //        }
            //    }
            //}
        }

        public class Instruction
        {
            [Regex(@"(.+) x=(-?\d+)..(-?\d+),y=(-?\d+)..(-?\d+),z=(-?\d+)..(-?\d+)")]
            public Instruction(OnOff action, int x1, int x2, int y1, int y2, int z1, int z2)
            {
                Action = action;
                Box = new Box(x1, x2, y1, y2, z1, z2);
            }

            public OnOff Action;
            public Box Box;
        }

        static ulong CellCount(IEnumerable<Box> boxes)
        {
            ulong count = 0;
            foreach (var box in boxes) count += box.Volume;
            return count;
        }

        public static ulong RunOperation(IEnumerable<Instruction> instructions)
        {
            IEnumerable<Box> data = Enumerable.Empty<Box>();

            foreach (var instr in instructions)
            {
                var startCount = CellCount(data);
                Console.WriteLine($"{data.Count()} boxes - {startCount} volume");
                Console.WriteLine($" >> turning {instr.Action} {instr.Box} {instr.Box.Volume}");
                IEnumerable<Box> newBoxes = Enumerable.Empty<Box>();

                if (!data.Any())
                {
                    newBoxes = new List<Box> { instr.Box };               
                }
                else
                {
                    if (instr.Action == OnOff.on)
                    {
                        newBoxes = new List<Box> { instr.Box };
                    }
                    foreach (var box in data)
                    {
                        newBoxes = newBoxes.Union(Subtract(box, instr.Box));
                    }
                }

                data = newBoxes.ToHashSet();
                var endCount = CellCount(data);
                Console.WriteLine($"Ending step with {data.Count()} boxes and {endCount} volume");
                if (instr.Action == OnOff.on)
                {
                    if (endCount < startCount)
                    {
                        Console.WriteLine("--? Which is odd!");
                    }
                }
                else
                {
                    if (endCount > startCount)
                    {
                        Console.WriteLine("++? Which is odd!");
                    }
                }
                Console.WriteLine();

            }

            return CellCount(data);
        }

        public static IEnumerable<Instruction> ParseData(string input)
        {
            return Util.RegexParse<Instruction>(input);
        }

        public static ulong Part1(string input)
        {
            IEnumerable<Instruction> lines = ParseData(input);

            return RunOperation(lines.Take(20));
        }

        public static ulong Part2(string input)
        {
            IEnumerable<Instruction> lines = ParseData(input);

            return RunOperation(lines);
        }

        public void Run(string input, ILogger logger)
        {
            string test0 = @"on x=10..12,y=10..12,z=10..12
on x=11..13,y=11..13,z=11..13
off x=9..11,y=9..11,z=9..11
on x=10..10,y=10..10,z=10..10".Replace("\r","");

            const string test1 = @"on x=-20..26,y=-36..17,z=-47..7
on x=-20..33,y=-21..23,z=-26..28
on x=-22..28,y=-29..23,z=-38..16
on x=-46..7,y=-6..46,z=-50..-1
on x=-49..1,y=-3..46,z=-24..28
on x=2..47,y=-22..22,z=-23..27
on x=-27..23,y=-28..26,z=-21..29
on x=-39..5,y=-6..47,z=-3..44
on x=-30..21,y=-8..43,z=-13..34
on x=-22..26,y=-27..20,z=-29..19
off x=-48..-32,y=26..41,z=-47..-37
on x=-12..35,y=6..50,z=-50..-2
off x=-48..-32,y=-32..-16,z=-15..-5
on x=-18..26,y=-33..15,z=-7..46
off x=-40..-22,y=-38..-28,z=23..41
on x=-16..35,y=-41..10,z=-47..6
off x=-32..-23,y=11..30,z=-14..3
on x=-49..-5,y=-3..45,z=-29..18
off x=18..30,y=-20..-8,z=-3..13
on x=-41..9,y=-7..43,z=-33..15";

            Console.WriteLine(Part1(test1));

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}