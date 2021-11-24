using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day25 : IPuzzle
    {
        public string Name { get { return "2015-25"; } }

        static IEnumerable<(int x, int y, UInt64 code)> NumberSequence()
        {
            int x = 1;
            int y = 1;
            UInt64 code = 20151125;
            yield return (x, y, code);

            while (true)
            {
                y--;
                if (y == 0)
                {
                    y = x + 1;
                    x = 0;
                }
                x++;
                code *= 252533;
                code %= 33554393;
                yield return (x, y, code);
            }
        }

        public static int Part1(string input)
        {
            var numbers = Util.ExtractNumbers(input);

            var row = numbers[0];
            var col = numbers[1];

            var code = NumberSequence().Where(val => val.x == col && val.y == row).First().code;

            return (int)code;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
        }
    }
}