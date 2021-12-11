using System.Linq;
using System.Text;

namespace AoC.Advent2016
{
    public class Day18 : IPuzzle
    {
        public string Name => "2016-18";

        private static string Step(string current)
        {
            current = "." + current + ".";
            var next = new StringBuilder();
            for (var i = 0; i < current.Length - 2; ++i)
            {
                if (current[i] == current[i + 2])
                {
                    next.Append('.');
                }
                else
                {
                    next.Append('^');
                }
            }
            return next.ToString();
        }

        static int Count(string line) => line.Where(x => x == '.').Count();

        public static int BuildMap(string input, int numLines)
        {
            var currentState = input.Trim();

            int count = Count(currentState);

            for (int i = 1; i < numLines; ++i)
            {
                currentState = Step(currentState);
                int score = Count(currentState);
                count += score;
            }

            return count;
        }

        public static int Part1(string input)
        {
            return BuildMap(input, 40);
        }

        public static int Part2(string input)
        {
            return BuildMap(input, 400000);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}