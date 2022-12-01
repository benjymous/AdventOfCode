using AoC.Utils;
using System.Linq;

namespace AoC.Advent2020
{
    public class Day01 : IPuzzle
    {
        public string Name => "2020-01";

        public static int Part1(string input)
        {
            return Util.ParseNumbers<int>(input)
                       .Pairs()
                       .Where(x => x.Item1 + x.Item2 == 2020)
                       .Select(x => x.Item1 * x.Item2)
                       .First();
        }

        public static int Part2(string input)
        {
            var numbers = Util.ParseNumbers<int>(input).OrderDescending().ToArray();

            for (var i = 0; i < numbers.Length; ++i)
            {
                for (var j = numbers.Length - 1; j >= 0; --j)
                {
                    for (var k = numbers.Length - 1; k >= 0; --k)
                    {
                        if (numbers[i] + numbers[j] + numbers[k] == 2020)
                        {
                            return numbers[i] * numbers[j] * numbers[k];
                        }
                        if (numbers[i] + numbers[j] + numbers[k] > 2020) break;
                    }
                    if (numbers[i] + numbers[j] > 2020) break;
                }
            }
            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}