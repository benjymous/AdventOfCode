using AoC.Utils;
using System.Linq;

namespace AoC.Advent2017
{
    public class Day02 : IPuzzle
    {
        public string Name => "2017-02";

        public static int RowDifference(string line)
        {
            var data = Util.ParseNumbers<int>(line, '\t');
            return data.Max() - data.Min();
        }

        public static int RowMultiple(string line)
        {
            var data = Util.ParseNumbers<int>(line, '\t');
            return data.Pairs().Where(pair => pair.Item1 % pair.Item2 == 0).Select(pair => pair.Item1 / pair.Item2).First();
        }

        public static int Part1(string input)
        {
            var lines = Util.Split(input);
            return lines.Sum(RowDifference);
        }

        public static int Part2(string input)
        {
            var lines = Util.Split(input);
            return lines.Sum(RowMultiple);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}