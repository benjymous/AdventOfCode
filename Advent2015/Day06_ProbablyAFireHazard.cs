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

        public readonly struct Instruction
        {
            readonly Mode mode;
            readonly (int x, int y) bl, tr;

            public Instruction(string line)
            {
                line = line.Replace("toggle", "T")
                           .Replace("turn on", "N")
                           .Replace("turn off", "F");

                var bits = line.Split(" ");
                mode = bits[0] switch
                {
                    "T" => Mode.toggle,
                    "N" => Mode.on,
                    "F" => Mode.off,
                    _ => throw new Exception("Unexpected light mode"),
                };

                bl = new ManhattanVector2(bits[1]);
                tr = new ManhattanVector2(bits[3]);

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
                    for (var x = bl.x; x <= tr.x; ++x)
                        Apply(ref grid[x, y]);
            }

            public void Apply(int[,] grid)
            {
                for (var y = bl.y; y <= tr.y; ++y)
                    for (var x = bl.x; x <= tr.x; ++x)
                        Apply(ref grid[x, y]);
            }
        }


        public static int Part1(string input)
        {
            bool[,] grid = new bool[1000, 1000];

            Util.Parse<Instruction>(input).ForEach(i => i.Apply(grid));
            return grid.Values().Count(i => i);
        }

        public static int Part2(string input)
        {
            int[,] grid = new int[1000, 1000];

            Util.Parse<Instruction>(input).ForEach(i => i.Apply(grid));
            return grid.Values().Sum();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}