using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent.Utils.Vectors;

namespace Advent.MMXV
{
    public class Day06 : IPuzzle
    {
        public string Name { get { return "2015-06";} }
 
        public enum Mode
        {
            on,
            off,
            toggle
        }

        public class Instruction
        {
            public Mode mode;
            public ManhattanVector2 bl;
            public ManhattanVector2 tr;

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

                bl = new ManhattanVector2(bits[1]);
                tr = new ManhattanVector2(bits[3]);

                if (bits[2] != "through") throw new Exception("Malformed instruction");                       
            }

            bool Apply(bool val)
            {
                switch(mode)
                {
                    case Mode.on: return true;
                    case Mode.off: return false;
                    case Mode.toggle: return !val;
                }

                throw new Exception("Unexpected light mode");
            }

            int Apply(int val)
            {
                switch(mode)
                {
                    case Mode.on: return val+1;
                    case Mode.off: return Math.Max(0, val-1);
                    case Mode.toggle: return val+2;
                }

                throw new Exception("Unexpected light mode");
            }

            public void Apply(bool[,] grid)
            {
                for (var y=bl.Y; y<=tr.Y; ++y)
                {
                    for (var x=bl.X; x<=tr.X; ++x)
                    {
                        grid[x,y] = Apply(grid[x,y]);
                    }
                }
            }

            public void Apply(int[,] grid)
            {
                for (var y=bl.Y; y<=tr.Y; ++y)
                {
                    for (var x=bl.X; x<=tr.X; ++x)
                    {
                        grid[x,y] = Apply(grid[x,y]);
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
            bool[,] grid = new bool[1000,1000];
            for (var y=0; y<1000; ++y)
            {
                for (var x=0; x<1000; ++x)
                {
                    grid[x,y] = false;
                }
            }

            var instructions = Util.Parse<Instruction>(input);

            foreach (var instr in instructions)
            {
                instr.Apply(grid);
            }

            var query = from bool item in grid
                where item
                select item;

            return query.Count();
        }

        public static int Part2(string input)
        {
            int[,] grid = new int[1000,1000];
            for (var y=0; y<1000; ++y)
            {
                for (var x=0; x<1000; ++x)
                {
                    grid[x,y] = 0;
                }
            }

            var instructions = Util.Parse<Instruction>(input);

            foreach (var instr in instructions)
            {
                instr.Apply(grid);
            }

            var query = from int item in grid
                select item;

            return query.Sum();
        }

        public void Run(string input, ILogger logger)
        {
            // var bit1 = new Instruction("toggle 275,796 through 493,971");
            // var bit2 = new Instruction("turn off 70,873 through 798,923");
            // var bit3 = new Instruction("turn on 258,985 through 663,998");

            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}