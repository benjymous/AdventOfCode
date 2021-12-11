using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2020
{
    public class Day06 : IPuzzle
    {
        public string Name => "2020-06";

        public static int Part1(string input)
        {
            var groups = input.Split("\n\n");
            int total = 0;
            foreach (var g in groups)
            {
                var merged = g.Replace("\n", "");
                var set = new HashSet<char>(merged);
                total += set.Count();
            }
            return total;
        }

        public static int Part2(string input)
        {
            var groups = input.Split("\n\n");
            int total = 0;
            foreach (var g in groups)
            {
                var split = g.Split("\n");
                IEnumerable<char> set = split[0];
                foreach (var row in split)
                {
                    set = set.Intersect(row);
                }
                total += set.Count();
            }
            return total;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}