using AoC.Utils;

namespace AoC.Advent2019
{
    public class Day08 : IPuzzle
    {
        public string Name => "2019-08";

        public static int Part1(string input)
        {
            var image = new NPSA.Image(input, 25, 6);
            return image.GetChecksum();
        }

        public static string Part2(string input, ILogger logger)
        {
            var image = new NPSA.Image(input, 25, 6);
            logger.WriteLine("\n" + image.ToString());
            return image.ToString().GetSHA256String();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input, logger));
        }
    }
}