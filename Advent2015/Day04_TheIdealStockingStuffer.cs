﻿namespace AoC.Advent2015
{
    public class Day04 : IPuzzle
    {
        public string Name => "2015-04";

        public static int Part1(string input)
        {
            return HashBreaker.FindHash(input.Trim(), 5).foundIndex;
        }

        public static int Part2(string input)
        {
            return HashBreaker.FindHash(input.Trim(), 6).foundIndex;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}