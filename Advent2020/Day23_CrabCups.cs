using AoC.Utils;
using AoC.Utils.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2020
{
    public class Day23 : IPuzzle
    {
        public string Name => "2020-23";

        static Circle<int> CreateCircle(IEnumerable<int> vals, bool part2)
        {
            var start = new Circle<int>(vals.First());
            var node = start.InsertRange(vals.Skip(1));

            if (part2)
            {
                node.InsertRange(Enumerable.Range(10, 1000000 - start.Count));
            }

            return start;
        }

        static Circle<int>[] Take(Circle<int> circle)
            => new Circle<int>[] { circle.PopNext(), circle.PopNext(), circle.PopNext() };


        static Circle<int> FindDestination(Circle<int> current, Circle<int>[] vals, bool part2)
        {
            var (v1, v2, v3) = (vals[0].Value, vals[1].Value, vals[2].Value);
            int max = part2 ? 1000000 : 9;

            var destinationVal = current.Value;
            do
            {
                destinationVal = destinationVal == 1 ? max : destinationVal - 1;
            }
            while (destinationVal == v1 || destinationVal == v2 || destinationVal == v3);

            return current.Find(destinationVal);
        }

        public static IEnumerable<int> Play(string input, int rounds, bool part2 = false)
        {
            var current = CreateCircle(input.Trim().Select(ch => ch - '0'), part2);

            while (rounds-- > 0)
            {
                var taken = Take(current);

                FindDestination(current, taken, part2).InsertRange(taken);

                current = current.Next();
            }

            return current.Find(1).Values().Skip(1);
        }

        public static string Part1(string input, int rounds = 100)
        {
            return string.Join("", Play(input, rounds).Select(i => i.ToString()));
        }

        public static Int64 Part2(string input)
        {
            return Play(input, 10000000, true).Take(2).Product();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}