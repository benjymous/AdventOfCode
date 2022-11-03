using System.Linq;

namespace AoC.Advent2021
{
    public class Day06 : IPuzzle
    {
        public string Name => "2021-06";

        private static long Simulate(string input, int days)
        {
            var data = Util.Parse32(input);
            var v = Enumerable.Range(0, 9).Select(i => (long)data.Count(v => v == i)).ToArray();

            while ((days--) > 0)
                (v[0], v[1], v[2], v[3], v[4], v[5], v[6], v[7], v[8]) =
                (v[1], v[2], v[3], v[4], v[5], v[6], v[7] + v[0], v[8], v[0]);

            return v.Sum();
        }

        public static long Part1(string input)
        {
            return Simulate(input, 80);
        }

        public static long Part2(string input)
        {
            return Simulate(input, 256);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}