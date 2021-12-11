using System.Linq;

namespace AoC.Advent2018
{
    public class Day05 : IPuzzle
    {
        public string Name => "2018-05";

        static int Reduce(string inp)
        {
            var input = inp.Trim().ToCharArray();
            bool replaced = true;
            do
            {
                replaced = false;

                for (var i = 0; i < input.Length - 1; ++i)
                {
                    if (input[i] != input[i + 1] && char.ToLower(input[i]) == char.ToLower(input[i + 1]))
                    {
                        input[i] = ' ';
                        input[i + 1] = ' ';
                        replaced = true;
                    }
                }
                input = input.Where(i => i != ' ').ToArray();

            } while (replaced);

            return input.Length;
        }

        public static int Part1(string input)
        {
            return Reduce(input);
        }

        public static int ShrinkReduce(char c, string input)
        {
            var shrunk = input.Replace(c.ToString(), "").Replace(c.ToString().ToUpper(), "");
            return Reduce(shrunk);
        }

        public static int Part2(string input)
        {
            return ParallelEnumerable.Range('a', 26).Select(alpha => ShrinkReduce((char)alpha, input)).Min();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}