using AoC.Utils;
using System;
using System.Linq;

namespace AoC.Advent2017
{
    public class Day04 : IPuzzle
    {
        public string Name => "2017-04";

        public static bool ValidationRule1(string passphrase)
        {
            var words = passphrase.Split(" ");
            return !words.GroupBy(w => w)
                         .Any(group => group.Count() > 1);
        }

        public static bool ValidationRule2(string passphrase)
        {
            var words = passphrase.Split(" ").Select(x => x.Order().AsString());
            return !words.GroupBy(w => w)
                         .Any(group => group.Count() > 1);
        }

        public static int Part1(string input)
        {
            var lines = Util.Split(input, '\n');

            return lines.Count(ValidationRule1);
        }

        public static int Part2(string input)
        {
            var lines = Util.Split(input, '\n');

            return lines.Where(ValidationRule1)
                        .Count(ValidationRule2);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}