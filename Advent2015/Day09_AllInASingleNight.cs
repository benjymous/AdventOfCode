using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day09 : IPuzzle
    {
        public string Name => "2015-09";

        class Factory
        {
            [Regex(@"(.+) to (.+) = (.+)")]
            public void Parse(string from, string to, int distance) => Atlas[Remap(from) | Remap(to)] = distance;

            int Remap(string name) => MappingDict.GetIndexBit(name);

            public int[] LocationKeys => MappingDict.Values.ToArray();

            readonly Dictionary<string, int> MappingDict = new();
            public readonly Dictionary<int, int> Atlas = new();
        }

        static (int min, int max) MeasureRoutes(IEnumerable<int> remaining, Dictionary<int, int> atlas, int current = 0)
        {
            if (!remaining.Any()) return (0, 0);
            int min = int.MaxValue, max = int.MinValue;

            foreach (var node in remaining)
            {
                int distance = current == 0 ? 0 : atlas[current | node];

                var (minRemaining, maxRemaining) = MeasureRoutes(remaining.Where(i => i != node).ToArray(), atlas, node);

                (min, max) = (Math.Min(minRemaining + distance, min), Math.Max(maxRemaining + distance, max));
            }
            return (min, max);
        }

        static (int min, int max) Solve(string input)
        {
            var data = Util.RegexFactory<Factory>(input);

            return MeasureRoutes(data.LocationKeys, data.Atlas);
        }

        public static int Part1(string input)
        {
            return Solve(input).min;
        }

        public static int Part2(string input)
        {
            return Solve(input).max;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}