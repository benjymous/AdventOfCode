using AoC.Utils.Vectors;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2016
{
    public class Day01 : IPuzzle
    {
        public string Name => "2016-01";

        private static int FollowPath(string input, QuestionPart part)
        {
            var lines = Util.Split(input).Select(x => x.Trim());

            var position = new ManhattanVector2(0, 0);
            var direction = new Direction2(0, -1);

            var seen = new HashSet<(int x, int y)>();

            foreach (var instruction in lines)
            {
                switch (instruction[0])
                {
                    case 'L': direction.TurnLeft(); break;
                    case 'R': direction.TurnRight(); break;
                }

                var distance = int.Parse(instruction[1..]);

                if (part.One())
                {
                    position.Offset(direction, distance);
                }
                else
                {
                    for (int i = 0; i < distance; ++i)
                    {
                        position.Offset(direction);

                        if (seen.Contains(position))
                        {
                            return position.Length;
                        }
                        seen.Add(position);
                    }
                }
            }

            return position.Length;
        }

        public static int Part1(string input)
        {
            return FollowPath(input, QuestionPart.Part1);
        }

        public static int Part2(string input)
        {
            return FollowPath(input, QuestionPart.Part2);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}