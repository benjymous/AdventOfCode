using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day09 : IPuzzle
    {
        public string Name => "2015-09";

        class Distance
        {
            [Regex(@"(.+) to (.+) = (.+)")]
            public Distance(string from, string to, int distance)
            {
                (From, To, Dist) = (from.GetHashCode(), to.GetHashCode(), distance);
            }

            public int From;
            public int To;
            public int Dist;

            public override string ToString()
            {
                return $"{From} -> {To} = {Dist}";
            }
        }

        static int GetDistance(IEnumerable<int> route, Dictionary<int, int> atlas)
        {
            var r = route.ToArray();

            var distance = 0;
            for (int i = 0; i < r.Length - 1; ++i)
            {
                distance += atlas[r[i] * r[i + 1]];
            }

            return distance;
        }

        public static IEnumerable<int> GetRoutes(string input)
        {
            var distances = Util.RegexParse<Distance>(input);

            var lookup = distances.ToDictionary(d => d.From * d.To, d => d.Dist);

            var names = distances.Select(d => d.From).Union(distances.Select(d => d.To)).Distinct().ToArray();

            var permutations = names.Permutations().ToArray();

            return permutations.Select(r => GetDistance(r, lookup));
        }

        public static int Part1(string input)
        {
            return GetRoutes(input).Min();
        }

        public static int Part2(string input)
        {
            return GetRoutes(input).Max();
        }

        public void Run(string input, ILogger logger)
        {
            //Console.WriteLine(Part1("London to Dublin = 464\nLondon to Belfast = 518\nDublin to Belfast = 141"));
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}