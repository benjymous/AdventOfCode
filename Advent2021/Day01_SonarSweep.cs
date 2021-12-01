using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2021
{
    public class Day01 : IPuzzle
    {
        public string Name { get { return "2021-01"; } }

        static IEnumerable<int> Sliding3s(int[] input)
        {
            for (int i = 0; i < input.Length - 2; ++i)
            {
                yield return input[i] + input[i + 1] + input[i + 2];
            }
        }

        private static int AscendingDepths(IEnumerable<int> depths) =>
            depths.Skip(1)
             .Zip(depths, (curr, prev) => curr > prev ? 1 : 0)
             .Sum(); 

        public static int Part1(string input)
        {
            var depths = Util.Parse32(input);
            return AscendingDepths(depths);
        }

        public static int Part2(string input)
        {
            var depths = Sliding3s( Util.Parse32(input)).ToArray();
            return AscendingDepths(depths);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}