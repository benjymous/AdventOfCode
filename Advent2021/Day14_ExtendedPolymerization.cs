using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2021
{
    public class Day14 : IPuzzle
    {
        public string Name => "2021-14";

        public class Rule
        {
            [Regex("(..) -> (.)")]
            public Rule(string input, char insert)
            {
                Input = input;
                Sub = new List<string> { $"{input[0]}{insert}", $"{insert}{input[1]}" };
            }

            public string Input { get; private set; }
            public List<string> Sub { get; private set; }
        }

        private static long Solve(string input, int steps)
        {
            var bits = input.Split("\n\n");
            var initial = bits[0];
            var rules = Util.RegexParse<Rule>(bits[1]).ToDictionary(rule => rule.Input, rule => rule);

            var pairs = initial.Windows(2)
                               .Select(p => p.AsString())
                               .GroupBy(p => p)
                               .ToDictionary(g => g.Key, g => (long)g.Count());
    
            for (int i = 0; i < steps; ++i)
            {
                pairs = Step(rules, pairs);
            }

            Dictionary<char, long> groups = new();
            foreach (var pair in pairs)
            {
                groups.IncrementAtIndex(pair.Key[0], pair.Value);
            }

            groups[initial.Last()]++; // include the last character

            return groups.Values.Max() - groups.Values.Min();
        }

        private static Dictionary<string, long> Step(Dictionary<string, Rule> rules, Dictionary<string, long> pairs)
        {
            Dictionary<string, long> next = new();
            foreach (var pair in pairs)
            {
                if (rules.TryGetValue(pair.Key, out var rule))
                {
                    foreach (var newpair in rule.Sub)
                    {
                        next.IncrementAtIndex(newpair, pair.Value);
                    }
                }
                else
                {
                    next.Add(pair.Key, pair.Value);
                }
            }

            return next;
        }

        public static long Part1(string input)
        {
            return Solve(input, 10);
        }

        public static long Part2(string input)
        {
            return Solve(input, 40);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}