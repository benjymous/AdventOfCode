using System.Linq;

namespace AoC.Advent2020
{
    public class Day01 : IPuzzle
    {
        public string Name => "2020-01";

        public static int Part1(string input)
        {
            var allNumbers = Util.ParseNumbers<int>(input).ToHashSet();

            return (from n1 in allNumbers
                    where allNumbers.Contains(2020 - n1)
                    select n1 * (2020 - n1)).First();
        }

        public static int Part2(string input)
        {
            var allNumbers = Util.ParseNumbers<int>(input).ToHashSet();

            return (from n1 in allNumbers
                    from n2 in allNumbers
                    where allNumbers.Contains(2020 - (n1 + n2))
                    select n1 * n2 * (2020-(n1+n2))).First();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}