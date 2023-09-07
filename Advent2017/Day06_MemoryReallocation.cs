using AoC.Utils;
using System.Collections.Generic;

namespace AoC.Advent2017
{
    public class Day06 : IPuzzle
    {
        public string Name => "2017-06";

        static void Redistribute(ref int[] banks)
        {
            int i = banks.MaxIndex();

            int redistribute = banks[i];
            banks[i] = 0;

            while (redistribute-- > 0)
            {
                i = (i + 1) % banks.Length;
                banks[i]++; 
            }
        }

        public static int Part1(string input)
        {
            var banks = Util.ParseNumbers<int>(input, '\t');

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
            var banks = Util.ParseNumbers<int>(input, '\t');

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