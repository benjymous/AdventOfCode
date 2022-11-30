﻿using AoC.Utils;
using System.Collections.Generic;

namespace AoC.Advent2017
{
    public class Day06 : IPuzzle
    {
        public string Name => "2017-06";

        static void Redistribute(ref int[] banks)
        {
            int maxIndex = banks.MaxIndex();

            int redistribute = banks[maxIndex];
            banks[maxIndex] = 0;

            int i = Util.WrapIndex(maxIndex + 1, banks.Length);
            while (redistribute > 0)
            {
                banks[i]++;
                redistribute--;
                i = Util.WrapIndex(i + 1, banks.Length);
            }
        }

        public static int Part1(string input)
        {
            var banks = Util.Parse32(input, '\t');

            HashSet<int> seen = new();

            while (true)
            {
                var key = banks.GetCombinedHashCode();

                if (seen.Contains(key)) return seen.Count;

                seen.Add(key);

                Redistribute(ref banks);
            }
        }

        public static int Part2(string input)
        {
            var banks = Util.Parse32(input, '\t');

            Dictionary<int, int> seen = new();

            int iteration = 0;
            while (true)
            {
                var key = banks.GetCombinedHashCode();

                if (seen.TryGetValue(key, out int value)) return iteration - value;

                seen[key] = iteration++;

                Redistribute(ref banks);
            }
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}