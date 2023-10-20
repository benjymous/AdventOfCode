using AoC.Utils;
using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;

namespace AoC.Advent2022
{
    public class Day09 : IPuzzle
    {
        public string Name => "2022-09";

        [Regex(@"(.) (.+)")] public readonly record struct Instruction(Direction2 Direction, int Steps) { }

        public static bool UpdateTail((int x, int y) head, ref (int x, int y) tail)
        {
            int dx = head.x - tail.x, dy = head.y - tail.y;
            if (Math.Abs(dx) > 1 || Math.Abs(dy) > 1)
            {
                tail = tail.OffsetBy((Math.Sign(dx), Math.Sign(dy)));
                return true;
            }
            return false;
        }

        private static int SimulateRope(string input, int numSegments)
        {
            (int x, int y)[] rope = new (int x, int y)[numSegments];
            HashSet<(int x, int y)> tailPositions = new();

            foreach (var instr in Util.RegexParse<Instruction>(input))
            {
                for (int i = 0; i < instr.Steps; i++)
                {
                    rope[0] = rope[0].OffsetBy(instr.Direction);

                    for (int j = 0; j < numSegments - 1; ++j)
                        if (!UpdateTail(rope[j], ref rope[j + 1])) break;

                    tailPositions.Add(rope[numSegments - 1]);
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