using Advent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVII
{
    public class Day06 : IPuzzle
    {
        public string Name { get { return "2017-06"; } }

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

            HashSet<string> seen = new HashSet<string>();

            while (true)
            {
                string key = string.Join(" ", banks);

                if (seen.Contains(key)) return seen.Count;

                seen.Add(key);

                Redistribute(ref banks);
            }
        }

        public static int Part2(string input)
        {
            var banks = Util.Parse32(input, '\t');

            Dictionary<string, int> seen = new Dictionary<string, int>();

            int iteration = 0;
            while (true)
            {
                string key = string.Join(" ", banks);

                if (seen.ContainsKey(key)) return iteration - seen[key];

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