using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day21 : IPuzzle
    {
        public string Name => "2022-21";

        public class Monkey
        {
            [Regex("(....): (....) (.) (....)")]
            public Monkey(string name, string left, char op, string right)
            {
                Name = name;
                Left = left;
                Right = right;
                Op = op;
            }

            [Regex(@"(....): (\d+)")]
            public Monkey(string name, long value)
            {
                Name = name;
                Value = value;
            }

            public readonly string Name, Left, Right;
            char Op;
            long? Value;
            public long GetValue(Dictionary<string, Monkey> index)
            {
                if (Value.HasValue) return Value.Value;

                var left = index[Left].GetValue(index);
                var right = index[Right].GetValue(index);

                switch (Op)
                {
                    case '+': Value = left + right; return Value.Value;
                    case '-': Value = left - right; return Value.Value;
                    case '*': Value = left * right; return Value.Value;
                    case '/': Value = left / right; return Value.Value;
                }

                throw new Exception("unknown op");
            }
        }

        public static long Part1(string input)
        {
            var index = Util.RegexParse<Monkey>(input).ToDictionary(m => m.Name, m => m);
            return index["root"].GetValue(index);
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