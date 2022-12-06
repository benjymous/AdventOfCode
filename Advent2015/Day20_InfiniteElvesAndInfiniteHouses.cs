using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day20 : IPuzzle
    {
        public string Name => "2015-20";

        public static IEnumerable<int> GetFactors(int x)
        {
            for (int i = 1; i * i <= x; i++)
            {
                if (0 == (x % i))
                {
                    yield return i;
                    if (i != (x / i))
                    {
                        yield return x / i;
                    }
                }
            }
        }

        static int NumPresents(int doorNumber)
        {
            return GetFactors(doorNumber).Sum(f => f * 10);
        }

        static int NumPresents2(int doorNumber, Dictionary<int, int> elfCount)
        {
            int score = 0;
            var factors = GetFactors(doorNumber);
            foreach (var factor in factors)
            {
                elfCount.IncrementAtIndex(factor);
                if (elfCount[factor] <= 50)
                {
                    score += factor * 11;
                }
            }
            return score;
        }

        public static int Part1(string input)
        {
            int target = int.Parse(input);
            return Util.Forever(1).Where(i => NumPresents(i) > target).First();
        }

        public static int Part2(string input)
        {
            int target = int.Parse(input);
            Dictionary<int, int> elfCount = new();
            return Util.Forever(1).Where(i => NumPresents2(i, elfCount) > target).First();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}