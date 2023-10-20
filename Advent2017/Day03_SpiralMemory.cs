using AoC.Utils;
using AoC.Utils.Vectors;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2017
{
    public class Day03 : IPuzzle
    {
        public string Name => "2017-03";

        public static ManhattanVector2 Spiralize(int steps) => Spiralize().Skip(steps - 1).First();

        public static IEnumerable<(int x, int y)> Spiralize()
        {
            ManhattanVector2 position = new(0, 0);
            Direction2 dir = new('>');

            int step = 1;
            int repeat = 2;
            int currentStraight = step;
            while (true)
            {
                yield return position;

                position.Offset(dir);
                if (--currentStraight == 0)
                {
                    dir.TurnLeft();
                    if (--repeat == 0)
                    {
                        repeat = 2;
                        step++;
                    }
                    currentStraight = step;
                }
            }
        }

        public static int Part1(string input)
        {
            return Spiralize(int.Parse(input)).Distance(ManhattanVector2.Zero);
        }

        public static int Part2(string input)
        {
            int target = int.Parse(input);
            var cells = new Dictionary<int, int> { { 0, 1 } };

            var spiral = Spiralize();

            foreach (var pos in spiral)
            {
                int sum = 0;
                for (int y = -1; y <= 1; ++y)
                    for (int x = -1; x <= 1; ++x)
                        sum += cells.GetOrDefault(pos.x + x + ((pos.y + y) << 16));

                cells[pos.x + (pos.y << 16)] = sum;

                if (sum > target) return sum;
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