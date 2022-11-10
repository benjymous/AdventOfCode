using AoC.Utils.Vectors;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2016
{
    public class Day01 : IPuzzle
    {
        public string Name => "2016-01";

        public static int Part1(string input)
        {
            var lines = Util.Split(input).Select(x => x.Trim());

            var position = new ManhattanVector2(0, 0);
            var direction = new Direction2(0, -1);

            foreach (var instruction in lines)
            {
                switch (instruction[0])
                {
                    case 'L': direction.TurnLeft(); break;
                    case 'R': direction.TurnRight(); break;
                }

                var distance = int.Parse(instruction[1..]);
                position.Offset(direction, distance);

            }

            return position.Distance(ManhattanVector2.Zero);
        }

        public static int Part2(string input)
        {
            var lines = Util.Split(input).Select(x => x.Trim());

            var position = new ManhattanVector2(0, 0);
            var direction = new Direction2(0, -1);

            var seen = new HashSet<string>();

            foreach (var instruction in lines)
            {
                switch (instruction[0])
                {
                    case 'L': direction.TurnLeft(); break;
                    case 'R': direction.TurnRight(); break;
                }

                var distance = int.Parse(instruction[1..]);

                for (int i = 0; i < distance; ++i)
                {
                    position.Offset(direction);

                    if (seen.Contains(position.ToString()))
                    {
                        return position.Distance(ManhattanVector2.Zero);
                    }
                    seen.Add(position.ToString());
                }
            }

            return -1;
        }

        public void Run(string input, ILogger logger)
        {
            //logger.WriteLine(Part2("R8, R4, R4, R8"));

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}