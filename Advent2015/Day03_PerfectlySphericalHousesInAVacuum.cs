using AoC.Utils;
using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;

namespace AoC.Advent2015
{
    public class Day03 : IPuzzle
    {
        public string Name => "2015-03";

        public class SantaStepper
        {
            public ManhattanVector2 Position { get; set; } = new ManhattanVector2(0, 0);

            public ManhattanVector2 Step(char c)
            {
                switch (c)
                {
                    case '^':
                        Position.Offset(0, -1);
                        break;
                    case 'v':
                        Position.Offset(0, 1);
                        break;
                    case '>':
                        Position.Offset(1, 0);
                        break;
                    case '<':
                        Position.Offset(-1, 0);
                        break;
                    default:
                        throw new Exception("unexpected char!");
                }
                return Position;
            }
        }

        public static int Part1(string input)
        {
            HashSet<(int x, int y)> visited = new() { (0, 0) };

            var santa = new SantaStepper();

            foreach (var c in input)
            {                
                visited.Add(santa.Step(c));
            }

            return visited.Count;
        }

        public static int Part2(string input)
        {
            HashSet<(int x, int y)> visited = new() { (0, 0) };

            Queue<SantaStepper> santas = new() { new SantaStepper(), new SantaStepper() };

            foreach (var c in input)
            {
                var santa = santas.Dequeue();
                visited.Add(santa.Step(c));
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