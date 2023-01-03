using AoC.Utils;
using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day09 : IPuzzle
    {
        public string Name => "2022-09";

        [Regex(@"(.) (.+)")]
        public readonly record struct Instruction(Direction2 Direction, int Steps) {}

        public static bool UpdateTail(ManhattanVector2 head, ManhattanVector2 tail)
        {
            var (dx, dy) = (head.X - tail.X, head.Y - tail.Y);
            if (Math.Abs(dx) > 1 || Math.Abs(dy) > 1)
            {
                tail.Offset(Math.Sign(dx), Math.Sign(dy));
                return true;
            }
            return false;
        }

        private static int SimulateRope(string input, int numSegments)
        {
            var instructions = Util.RegexParse<Instruction>(input);

            ManhattanVector2[] rope = new ManhattanVector2[numSegments];
            for (int i = 0; i < numSegments; ++i) rope[i] = new(0, 0);

            var head = rope.First();
            var tail = rope.Last();

            HashSet<(int x, int y)> tailPositions = new();

            foreach (var instr in instructions)
            {
                for (int i = 0; i < instr.Steps; i++)
                {
                    head.Offset(instr.Direction);

                    for (int j = 0; j < numSegments - 1; ++j)
                        if (!UpdateTail(rope[j], rope[j + 1])) break;

                    tailPositions.Add(tail);
                }
            }

            return tailPositions.Count;
        }

        public static int Part1(string input)
        {
            return SimulateRope(input, 2);
        }

        public static int Part2(string input)
        {
            return SimulateRope(input, 10);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}