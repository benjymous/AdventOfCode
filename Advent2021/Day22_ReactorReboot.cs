using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day22 : IPuzzle
    {
        public string Name => "2021-22";

        public enum Toggle
        {
            on,
            off,
        }

        public class Instruction
        {
            [Regex(@"(.+) x=(-?\d+)..(-?\d+),y=(-?\d+)..(-?\d+),z=(-?\d+)..(-?\d+)")]
            public Instruction(Toggle action, int x1, int x2, int y1, int y2, int z1, int z2) => (Action, Box) = (action, (x1, x2 + 1, y1, y2 + 1, z1, z2 + 1));

            public Toggle Action;
            public (int minX, int maxX, int minY, int maxY, int minZ, int maxZ) Box;
        }

        static IEnumerable<int> GetDivisions(int a1, int a2, int b1, int b2) => new HashSet<int> { a1, a2, b1, b2 }.OrderBy(v => v);

        static IEnumerable<(int minX, int maxX, int minY, int maxY, int minZ, int maxZ)> Subtract((int minX, int maxX, int minY, int maxY, int minZ, int maxZ) b1, (int minX, int maxX, int minY, int maxY, int minZ, int maxZ) b2)
        {
            var divX = GetDivisions(b1.minX, b1.maxX, b2.minX, b2.maxX).ToArray();
            var divY = GetDivisions(b1.minY, b1.maxY, b2.minY, b2.maxY).ToArray();
            var divZ = GetDivisions(b1.minZ, b1.maxZ, b2.minZ, b2.maxZ).ToArray();
            foreach (var (zpair, ypair, xpair) in divZ.OverlappingPairs().SelectMany(zpair => divY.OverlappingPairs().SelectMany(ypair => divX.OverlappingPairs().Select(xpair => (zpair, ypair, xpair)))))
            {
                var newBox = (xpair.first, xpair.second, ypair.first, ypair.second, zpair.first, zpair.second);
                // keep segments in b1 but not b2
                if (Overlaps(newBox, b1) && !Overlaps(newBox, b2))
                {
                    yield return newBox;
                }
            }
        }

        static bool Overlaps((int minX, int maxX, int minY, int maxY, int minZ, int maxZ) box1, (int minX, int maxX, int minY, int maxY, int minZ, int maxZ) box2) => (box1.minX < box2.maxX) && (box1.maxX > box2.minX) && (box1.minY < box2.maxY) && (box1.maxY > box2.minY) && (box1.minZ < box2.maxZ) && (box1.maxZ > box2.minZ);

        static long Volume((int minX, int maxX, int minY, int maxY, int minZ, int maxZ) box) => (box.maxX - (long)box.minX) * (box.maxY - (long)box.minY) * (box.maxZ - (long)box.minZ);

        public static long RunOperation(IEnumerable<Instruction> instructions)
        {
            var data = Enumerable.Empty<(int minX, int maxX, int minY, int maxY, int minZ, int maxZ)>();

            foreach (var instr in instructions)
            {
                var newBoxes = new List<(int minX, int maxX, int minY, int maxY, int minZ, int maxZ)>();
                if (instr.Action == Toggle.on)
                {
                    newBoxes.Add(instr.Box);
                }
                foreach (var box in data)
                {
                    if (Overlaps(box, instr.Box))
                    {
                        newBoxes.AddRange(Subtract(box, instr.Box));
                    }
                    else
                    {
                        newBoxes.Add(box);
                    }
                }
                data = newBoxes;
            }

            return data.Sum(Volume);
        }

        public static IEnumerable<Instruction> ParseData(string input) => Util.RegexParse<Instruction>(input);

        public static long Part1(string input)
        {
            IEnumerable<Instruction> lines = ParseData(input);

            return RunOperation(lines.Take(20));
        }

        public static long Part2(string input)
        {
            IEnumerable<Instruction> lines = ParseData(input);

            return RunOperation(lines);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}