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

        public readonly struct Instruction
        {
            [Regex(@"(.) (.+)")]
            public Instruction(char d, int s) => (Direction, Steps) = (Decode(d), s);

            public readonly (int x, int y) Direction;
            public readonly int Steps;

            static (int x, int y) Decode(char dir) => dir switch
            {
                'U' => (0, 1),
                'D' => (0, -1),
                'R' => (1, 0),
                'L' => (-1, 0),
                _ => throw new Exception("Unexpected direction"),
            };
        }

        public static bool UpdateTail(ManhattanVector2 head, ManhattanVector2 tail)
        {
            var distance = tail.Distance(head);
            if (distance > 1 && (distance != 2 || tail.X == head.X || tail.Y == head.Y))
            {
                tail.Offset((Math.Sign(head.X - tail.X), Math.Sign(head.Y - tail.Y)));
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