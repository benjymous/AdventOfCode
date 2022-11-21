using AoC.Utils;
using AoC.Utils.Vectors;
using System;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day06 : IPuzzle
    {
        public string Name => "2015-06";

        public enum Mode
        {
            on,
            off,
            toggle
        }

        public struct Instruction
        {
            public Mode mode;
            public (int x, int y) bl;
            public (int x, int y) tr;

            public Accumulator XRange;
            public Accumulator YRange;

            public Instruction(string line)
            {
                line = line.Replace("toggle", "T")
                           .Replace("turn on", "N")
                           .Replace("turn off", "F");

                var bits = line.Split(" ");
                switch (bits[0])
                {
                    case "T": mode = Mode.toggle; break;
                    case "N": mode = Mode.on; break;
                    case "F": mode = Mode.off; break;
                }

                bl = new ManhattanVector2(bits[1]).AsSimple();
                tr = new ManhattanVector2(bits[3]).AsSimple();

                if (bits[2] != "through") throw new Exception("Malformed instruction");
            }

            void Apply(ref bool val) => val = mode switch
            {
                Mode.on => true,
                Mode.off => false,
                Mode.toggle => !val,
                _ => throw new Exception("Unexpected light mode"),
            };

            void Apply(ref int val) => val = mode switch
            {
                Mode.on => val + 1,
                Mode.off => Math.Max(0, val - 1),
                Mode.toggle => val + 2,
                _ => throw new Exception("Unexpected light mode"),
            };

            public void Apply(bool[,] grid)
            {
                for (var y = bl.y; y <= tr.y; ++y)
                {
                    for (var x = bl.x; x <= tr.x; ++x)
                    {
                        Apply(ref grid[x, y]);
                    }
                }
            }

            public void Apply(int[,] grid)
            {
                for (var y = bl.y; y <= tr.y; ++y)
                {
                    for (var x = bl.x; x <= tr.x; ++x)
                    {
                        Apply(ref grid[x, y]);
                    }
                }
            }

            public override string ToString()
            {
                return $"{mode} {bl} through {tr}";
            }
        }


        public static int Part1(string input)
        {
            bool[,] grid = new bool[1000, 1000];
            for (var y = 0; y < 1000; ++y)
            {
                for (var x = 0; x < 1000; ++x)
                {
                    grid[x, y] = false;
                }
            }

            var instructions = Util.Parse<Instruction>(input);

            foreach (var instr in instructions)
            {
                instr.Apply(grid);
            }

            return grid.Values().Count(i => i);
        }

        public static int Part2(string input)
        {
            int[,] grid = new int[1000, 1000];
            for (var y = 0; y < 1000; ++y)
            {
                for (var x = 0; x < 1000; ++x)
                {
                    grid[x, y] = 0;
                }
            }

            var instructions = Util.Parse<Instruction>(input);

            foreach (var instr in instructions)
            {
                instr.Apply(grid);
            }

            return grid.Values().Sum();
        }

        public void Run(string input, ILogger logger)
        {
            // var bit1 = new Instruction("toggle 275,796 through 493,971");
            // var bit2 = new Instruction("turn off 70,873 through 798,923");
            // var bit3 = new Instruction("turn on 258,985 through 663,998");

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}