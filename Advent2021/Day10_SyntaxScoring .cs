﻿using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day10 : IPuzzle
    {
        public string Name => "2021-10";

        static char ExpectedClose(char opener) => opener switch
        { '(' => ')', '[' => ']', '{' => '}', '<' => '>', _ => '!' };

        static (int part1, int part2) Score(char ch) => ch switch
        { ')' => (3, 1), ']' => (57, 2), '}' => (1197, 3), '>' => (25137, 4), _ => (0, 0) };

        static (char found, Stack<char> stack) CheckLine(string line)
        {
            var stack = new Stack<char>();
            foreach (var c in line)
            {
                switch (c)
                {
                    case '(':
                    case '[':
                    case '{':
                    case '<':
                        stack.Push(c);
                        break;

                    case ')':
                    case ']':
                    case '}':
                    case '>':
                        var expected = ExpectedClose(stack.Pop());
                        if (c != expected) return (c, stack);
                        break;
                }
            }
            return ('\0', stack);
        }

        public static int Part1(string input)
        {
            var lines = Util.Split(input);

            return lines.Select(line => Score(CheckLine(line).found).part1)
                        .Sum();
        }

        public static long Part2(string input)
        {
            var lines = Util.Split(input)
                            .Select(CheckLine)
                            .Where(res => res.found == 0);

            return lines.Select(line => line.stack.Select(line => Score(ExpectedClose(line)).part2)
                                                  .Aggregate(0L, (total, val) => (total * 5) + val))
                        .OrderBy(v => v)
                        .ElementAt(lines.Count() / 2);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}