using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day20 : IPuzzle
    {
        public string Name => "2015-20";

        public static IEnumerable<int> getFactors(int x)
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
            int score = 0;
            var factors = getFactors(doorNumber);
            foreach (var factor in factors)
            {
                score += factor * 10;
            }
            return score;
        }

        static Dictionary<int, int> elfCount = new Dictionary<int, int>();
        static int NumPresents2(int doorNumber)
        {
            int score = 0;
            var factors = getFactors(doorNumber);
            foreach (var factor in factors)
            {
                if (!elfCount.ContainsKey(factor)) elfCount[factor] = 0;
                if (elfCount[factor] < 50)
                {
                    elfCount[factor]++;
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
            return Util.Forever(1).Where(i => NumPresents2(i) > target).First();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}