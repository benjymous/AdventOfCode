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
            public Distance(string line)
            {
                var bits = line.Split(" ");
                from = bits[0];
                to = bits[2];
                distance = int.Parse(bits[4]);
            }

            public string from;
            public string to;
            public int distance;

            public override string ToString()
            {
                return $"{from} -> {to} = {distance}";
            }
        }

        static int GetDistance(IEnumerable<string> route, Dictionary<string, int> atlas)
        {
            var r = route.ToArray();

            var distance = 0;
            for (int i = 0; i < r.Length - 1; ++i)
            {
                var key = $"{r[i]}:{r[i + 1]}";
                if (!atlas.ContainsKey(key))
                {
                    key = $"{r[i + 1]}:{r[i]}";
                }
                distance += atlas[key];
            }

            //Console.WriteLine($"{String.Join(" -> ", route)} = {distance}");

            return distance;
        }

        public static IEnumerable<int> GetRoutes(string input)
        {
            var distances = Util.Parse<Distance>(input);

            var lookup = distances.ToDictionary(d => $"{d.from}:{d.to}", d => d.distance);

            var names = distances.Select(d => d.from).Union(distances.Select(d => d.to)).Distinct();

            var permutations = names.Permutations();

            return permutations.AsParallel().Select(r => GetDistance(r, lookup));
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