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

            public void Step(char c)
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
            }
        }

        public static int Part1(string input)
        {
            Dictionary<string, int> visited = new();

            var santa = new SantaStepper();

            visited.IncrementAtIndex(santa.Position.ToString());
            foreach (var c in input)
            {
                santa.Step(c);
                visited.IncrementAtIndex(santa.Position.ToString());
            }

            return visited.Count;
        }

        public static int Part2(string input)
        {
            Dictionary<string, int> visited = new();

            var santas = new Queue<SantaStepper>();
            santas.Enqueue(new SantaStepper());
            santas.Enqueue(new SantaStepper());

            foreach (var santa in santas)
            {
                visited.IncrementAtIndex(santa.Position.ToString());
            }

            foreach (var c in input)
            {
                var santa = santas.Dequeue();
                santa.Step(c);
                visited.IncrementAtIndex(santa.Position.ToString());
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