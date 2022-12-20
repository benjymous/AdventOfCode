using AoC.Utils;
using AoC.Utils.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day20 : IPuzzle
    {
        public string Name => "2022-20";

        public static int Part1(string input)
        {
            var numbers = Util.ParseNumbers<int>(input);

            var circle = Circle<int>.Create(numbers);
            var elements = circle.Elements().ToArray();

            foreach (var el in elements)
            {
                el.Move(el.Value);
            }

            var v = circle.Find(1);

            Console.WriteLine(string.Join(", ", v.Values()));


            return 0;
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            string test = "1\n2\n-3\n3\n-2\n0\n4";

            Console.WriteLine(Part1(test));

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}