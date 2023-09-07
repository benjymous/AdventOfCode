using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace AoC.Advent2015
{
    public class Day20 : IPuzzle
    {
        public string Name => "2015-20";

        private static int Solve(string input, QuestionPart part)
        {
            int multiplier = part.One() ? 10 : 11;
            int target = int.Parse(input);
            int[] numPresents = new int[1000000];
            for (int i = 1; i < numPresents.Length; i++)
            {
                for (int j = i, count = part.Two() ? 50 : int.MaxValue; j < numPresents.Length && count > 0; j += i, count--)
                {
                    numPresents[j] += i * multiplier;
                }
                if (numPresents[i] >= target) return i;
            }
            return 0;
        }

        public static int Part1(string input)
        {
            return Solve(input, QuestionPart.Part1);
        }

        public static int Part2(string input)
        {
            return Solve(input, QuestionPart.Part2);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}