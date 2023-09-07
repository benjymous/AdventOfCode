using System.Linq;

namespace AoC.Advent2020
{
    public class Day01 : IPuzzle
    {
        public string Name => "2020-01";

        public static int Part1(string input)
        {
            var allNumbers = Util.ParseNumbers<int>(input);

            var bigNumbers = allNumbers.Where(x => x >= 800).ToArray();
            var smallNumbers = allNumbers.Where(x => x < 800).ToArray();

            foreach (var big in bigNumbers)
            {
                foreach (var small in smallNumbers)
                {
                    if (big + small == 2020) return big * small;
                }
            }

            return 0;
        }

        public static int Part2(string input)
        {
            var allNumbers = Util.ParseNumbers<int>(input).Order();

            var bigNumbers = allNumbers.ToArray();
            var smallNumbers = allNumbers.Reverse().ToArray();

            foreach (var big1 in bigNumbers)
            {
                foreach (var big2 in bigNumbers)
                {
                    if (big1 + big2 >= 2020) break;
                    foreach (var small in smallNumbers)
                    {
                        var res = big1 + big2 + small;
                        if (res == 2020) return big1 * big2 * small;
                        else if (res < 2020) break;
                        
                    }
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