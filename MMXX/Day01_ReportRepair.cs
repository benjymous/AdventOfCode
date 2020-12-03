using Advent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXX
{
    public class Day01 : IPuzzle
    {
        public string Name { get { return "2020-01"; } }

        public static int Part1(string input)
        {
            var numbers = Util.Parse32(input).OrderByDescending(x => x).ToArray();
           
            for (var i=0; i<numbers.Length; ++i)
            {
                for (var j=numbers.Length-1; j>=0; --j)
                {
                    if (numbers[i] + numbers[j] == 2020)
                    {
                        return numbers[i] * numbers[j];
                    }
                }
            }

            return 0;
        }

        public static int Part2(string input)
        {
            var numbers = Util.Parse32(input).OrderByDescending(x => x).ToArray();

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