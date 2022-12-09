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

        struct Instruction
        {
            [Regex(@"(.) (.+)")]
            public Instruction(char d, int s)
            {
                Direction = Decode(d), Steps = s;
            }
            public readonly (int x, int y) Direction;
            public readonly int Steps;

            (int x, int y) Decode(char dir)
            {
                switch (dir)
                {
                    case 'U': return (0, -1);
                    case 'D': return (0, 1);
                    case 'R': return (1, 0);
                    case 'L': return (-1, 0);
                    default: throw new Exception("Unexpected direction");
                }
            }
        }

        public static int Part1(string input)
        {
            var instructions = Util.Parse<Instruction>(input);

            ManhattanVector2 head = (0, 0);
            ManhattanVector2 tail = (0, 0);

            foreach (var instr in instructions)
            {
                for (int i=0; i<instr.Steps; i++)
                {
                    head += instr.Direction;

                    
                }
            }

            return 0;
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            Console.WriteLine("- Pt1 - " + Part1(input));
            Console.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}