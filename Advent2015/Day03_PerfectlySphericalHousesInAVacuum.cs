using AoC.Utils;
using AoC.Utils.Vectors;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day03 : IPuzzle
    {
        public string Name => "2015-03";

        public class SantaStepper
        {
            public ManhattanVector2 Position { get; set; } = new ManhattanVector2(0, 0);

            public ManhattanVector2 Step(Direction2 dir)
            {
                Position += dir;
                return Position;
            }
        }

        public static int Part1(string input)
        {
            HashSet<(int x, int y)> visited = new() { (0, 0) };

            var santa = new SantaStepper();

            foreach (var dir in input.Select(c => new Direction2(c)))
            {
                visited.Add(santa.Step(dir));
            }

            return visited.Count;
        }

        public static int Part2(string input)
        {
            HashSet<(int x, int y)> visited = new() { (0, 0) };

            Queue<SantaStepper> santas = new() { new SantaStepper(), new SantaStepper() };

            foreach (var dir in input.Select(c => new Direction2(c)))
            {
                var santa = santas.Dequeue();
                visited.Add(santa.Step(dir));
                santas.Enqueue(santa);
            }

            return visited.Count;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}