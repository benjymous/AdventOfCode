using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day05 : IPuzzle
    {
        public string Name => "2018-05";

        static int Reduce(IEnumerable<char> inp)
        {
            var input = inp.ToArray();
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
            return Reduce(input.Trim());
        }

        public static int ShrinkReduce(char c, IEnumerable<char> input)
        {
            var shrunk = input.Where(ch => ch != c && ch != char.ToUpper(c));
            return Reduce(shrunk);
        }

        public static int Part2(string input)
        {
            return ParallelEnumerable.Range('a', 26).Select(alpha => ShrinkReduce((char)alpha, input.Trim())).Min();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}