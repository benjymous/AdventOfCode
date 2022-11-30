using System.Linq;

namespace AoC.Advent2017
{
    public class Day01 : IPuzzle
    {
        public string Name => "2017-01";

        public static int Captcha(string input, int offset)
        {
            var data = input.Trim().Select(c => c - '0').ToArray();

            int count = 0;
            for (int i = 0; i < data.Length; ++i)
            {
                if (data[i] == data[(i + offset) % data.Length])
                {
                    count += data[i];
                }
            }

            return count;
        }

        public static int Part1(string input)
        {
            return Captcha(input, 1);
        }

        public static int Part2(string input)
        {
            return Captcha(input, input.Length / 2);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}