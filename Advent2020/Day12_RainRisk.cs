using AoC.Utils.Vectors;
using System.Linq;

namespace AoC.Advent2020
{
    public class Day12 : IPuzzle
    {
        public string Name => "2020-12";

        [Regex(@"(.)(\d+)")]
        record class Instruction(char Cmd, int Val)
        {
            public void Apply(ManhattanVector2 v, Direction2 d)
            {
                switch (Cmd)
                {
                    case 'N':
                        v.Offset(Direction2.North, Val);
                        break;
                    case 'S':
                        v.Offset(Direction2.South, Val);
                        break;
                    case 'E':
                        v.Offset(Direction2.East, Val);
                        break;
                    case 'W':
                        v.Offset(Direction2.West, Val);
                        break;

                    case 'L':
                        d.TurnLeftByDegrees(Val);
                        break;
                    case 'R':
                        d.TurnRightByDegrees(Val);
                        break;
                    case 'F':
                        v.Offset(d, Val);
                        break;
                }
            }

            public void Apply2(ManhattanVector2 ship, ManhattanVector2 wp)
            {
                switch (Cmd)
                {
                    case 'N':
                        wp.Offset(Direction2.North, Val);
                        break;
                    case 'S':
                        wp.Offset(Direction2.South, Val);
                        break;
                    case 'E':
                        wp.Offset(Direction2.East, Val);
                        break;
                    case 'W':
                        wp.Offset(Direction2.West, Val);
                        break;

                    case 'L':
                        wp.TurnLeftBy(Val);
                        break;
                    case 'R':
                        wp.TurnRightBy(Val);
                        break;
                    case 'F':
                        ship.Offset(wp, Val);
                        break;
                }
            }
        }

        public static int Part1(string input)
        {
            var position = new ManhattanVector2(0, 0);
            var direction = new Direction2(1, 0);

            Util.RegexParse<Instruction>(input).ToList().ForEach(i => i.Apply(position, direction));

            return position.Distance(ManhattanVector2.Zero);
        }

        public static int Part2(string input)
        {
            var position = new ManhattanVector2(0, 0);
            var waypoint = new ManhattanVector2(10, -1);

            Util.RegexParse<Instruction>(input).ToList().ForEach(i => i.Apply2(position, waypoint));

            return position.Distance(ManhattanVector2.Zero);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}