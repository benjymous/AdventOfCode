using AoC.Utils.Vectors;
using System;

namespace AoC.Advent2020
{
    public class Day12 : IPuzzle
    {
        public string Name => "2020-12";

        class Instruction
        {
            public Instruction(string line)
            {
                cmd = line[0];
                val = Int32.Parse(line[1..]);
            }

            readonly char cmd;
            readonly int val;

            public override string ToString()
            {
                return $"{cmd} {val}";
            }

            public void Apply(ManhattanVector2 v, Direction2 d)
            {
                // Action N means to move north by the given value.
                // Action S means to move south by the given value.
                // Action E means to move east by the given value.
                // Action W means to move west by the given value.
                // Action L means to turn left the given number of degrees.
                // Action R means to turn right the given number of degrees.
                // Action F means to move forward by the given value in the direction the ship is currently facing.
                switch (cmd)
                {
                    case 'N':
                        v.Offset(Direction2.North, val);
                        break;
                    case 'S':
                        v.Offset(Direction2.South, val);
                        break;
                    case 'E':
                        v.Offset(Direction2.East, val);
                        break;
                    case 'W':
                        v.Offset(Direction2.West, val);
                        break;

                    case 'L':
                        d.TurnLeftByDegrees(val);
                        break;

                    case 'R':
                        d.TurnRightByDegrees(val);
                        break;

                    case 'F':
                        v.Offset(d, val);
                        break;

                }
            }

            public void Apply2(ManhattanVector2 ship, ManhattanVector2 wp)
            {
                switch (cmd)
                {
                    case 'N':
                        wp.Offset(Direction2.North, val);
                        break;
                    case 'S':
                        wp.Offset(Direction2.South, val);
                        break;
                    case 'E':
                        wp.Offset(Direction2.East, val);
                        break;
                    case 'W':
                        wp.Offset(Direction2.West, val);
                        break;

                    case 'L':
                        wp.TurnLeftBy(val);
                        break;

                    case 'R':
                        wp.TurnRightBy(val);
                        break;

                    case 'F':
                        ship.Offset(wp, val);
                        break;
                }
            }
        }

        public static int Part1(string input)
        {
            var instructions = Util.Parse<Instruction>(input);

            var position = new ManhattanVector2(0, 0);
            var direction = new Direction2(1, 0);

            foreach (var i in instructions)
            {
                //Console.WriteLine(i);
                i.Apply(position, direction);
                //Console.WriteLine($"{position} {direction.AsChar()}");

            }

            return position.Distance(ManhattanVector2.Zero);
        }

        public static int Part2(string input)
        {
            var instructions = Util.Parse<Instruction>(input);

            var position = new ManhattanVector2(0, 0);
            var waypoint = new ManhattanVector2(10, -1);

            foreach (var i in instructions)
            {
                //Console.WriteLine(i);
                i.Apply2(position, waypoint);
                //Console.WriteLine($"{position} {waypoint}");

            }

            return position.Distance(ManhattanVector2.Zero);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}