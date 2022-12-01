using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2020
{
    public class Day10 : IPuzzle
    {
        public string Name => "2020-10";

        static List<int> GetValues(string input)
        {
            var values = Util.ParseNumbers<int>(input).ToList();
            values.Add(0);
            values = values.Order().ToList();
            values.Add(values.Last() + 3);
            return values;
        }

        public static int Part1(string input)
        {
            var values = GetValues(input);

            int diff1 = 0;
            int diff3 = 0;
            for (var i = 0; i < values.Count - 1; ++i)
            {
                var v1 = values[i];
                var v2 = values[i + 1];

                int diff = v2 - v1;

                if (diff == 1) diff1++;
                if (diff == 3) diff3++;
            }

            return diff1 * diff3;
        }

        static Dictionary<int, Int64> GetCombinations(IEnumerable<int> values)
        {
            var results = new Dictionary<int, Int64> { { 0, 1 } };

            var offsets = new int[] { 1, 2, 3 };
            foreach (var value in values)
            {
                Int64 combinations = 0;
                foreach (var offset in offsets)
                {
                    var testVal = value - offset;
                    if (results.ContainsKey(testVal))
                    {
                        combinations += results[testVal];
                    }
                }

                results[value] = combinations;
            }

            return results;
        }

        public static Int64 Part2(string input)
        {
            var values = GetValues(input).Skip(1);

            var results = GetCombinations(values);

            var final = values.Last();

            return results[final];
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}