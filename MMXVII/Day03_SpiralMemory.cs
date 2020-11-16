using Advent.Utils;
using Advent.Utils.Vectors;
using System.Collections.Generic;
using System.Linq;

namespace Advent.MMXVII
{
    public class Day03 : IPuzzle
    {
        public string Name { get { return "2017-03"; } }

        enum Direction
        {
            Right = 0,
            Up = 1,
            Left = 2,
            Down = 3,
        }

        static void ApplyDirection(ManhattanVector2 v, Direction d)
        {
            switch (d)
            {
                case Direction.Right:
                    v.Offset(1, 0);
                    break;

                case Direction.Up:
                    v.Offset(0, -1);
                    break;

                case Direction.Left:
                    v.Offset(-1, 0);
                    break;

                case Direction.Down:
                    v.Offset(0, 1);
                    break;
            }
        }

        public static ManhattanVector2 Spiralize(int steps)
        {
            return Spiralize().Skip(steps - 1).First();
        }

        public static IEnumerable<ManhattanVector2> Spiralize()
        {
            ManhattanVector2 position = new ManhattanVector2(0, 0);

            Direction dir = Direction.Right;
            int step = 1;
            int repeat = 2;
            int currentStraight = step;
            while (true)
            {
                yield return position;
                //Console.WriteLine($"{i+1} - {position} - {dir}");
                ApplyDirection(position, dir);
                currentStraight--;
                if (currentStraight == 0)
                {
                    dir = (Direction)(((int)dir + 1) % 4);
                    repeat--;
                    if (repeat == 0)
                    {
                        repeat = 2;
                        step++;
                    }
                    currentStraight = step;

                }
            }
        }

        public static int Part1(string input) => Part1(int.Parse(input));
        public static int Part1(int steps) => Spiralize(steps).Distance(ManhattanVector2.Zero);

        public static int Part2(string input) => Part2(int.Parse(input));
        public static int Part2(int input)
        {
            var cells = new Dictionary<string, int>
            {
                {"0,0", 1}
            };

            var spiral = Spiralize();

            foreach (var pos in spiral)
            {
                int sum = 0;
                for (int y = -1; y <= 1; ++y)
                {
                    for (int x = -1; x <= 1; ++x)
                    {
                        sum += cells.GetStrKey($"{pos.X + x},{pos.Y + y}");
                    }
                }
                cells[pos.ToString()] = sum;

                if (sum > input) return sum;
            }
            return -1;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}