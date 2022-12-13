using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day13 : IPuzzle
    {
        public string Name => "2022-13";

        class Element
        {
            public Element(string data)
            {
                Data = data;
            }
            public string Data;
        }

        class Pair
        {
            [Regex(@"(.+)\n(.+)")]
            public Pair(string left, string right)
            {
                Left = new(left);
                Right = new(right);
            }
            public Element Left, Right;
        }

        public static int Part1(string input)
        {
            var data = Util.RegexParse<Pair>(input.Split("\n\n")).ToArray();
            return 0;
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}