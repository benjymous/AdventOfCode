using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Advent2019
{
    public class Day14 : IPuzzle
    {
        public string Name => "2019-14";

        public class Component
        {
            public long Quantity;
            public string Type;

            [Regex(@"(\d+) (\S+)")]
            public Component(long q, string t) => (Quantity, Type) = (q, t);
        }

        public class Rule
        {
            public List<Component> inputs;
            public Component output;

            [Regex(@"(.+) => (.+)")]
            public Rule(string inp, Component outp)
            {
                inputs = Util.RegexParse<Component>(inp, ",").ToList();
                output = outp;
            }
        }

        public static long Decompose(Component input, Dictionary<string, Rule> rules)
        {
            Queue<Component> currentSet = new(new[] { input });
            Dictionary<string, long> wasteHeap = new();

            long ore = 0;
            while (currentSet.Count > 0)
            {
                var component = currentSet.Dequeue();
                if (component.Type == "ORE") ore += component.Quantity;
                else
                {
                    var rule = rules[component.Type];

                    if (wasteHeap.TryGetValue(component.Type, out long wasteAvailable) && wasteAvailable > 0)
                    {
                        var wasteUsed = Math.Min(component.Quantity, wasteAvailable);
                        component.Quantity -= wasteUsed;
                        wasteHeap[component.Type] -= wasteUsed;
                    }

                    if (component.Quantity > 0)
                    {
                        var multiplier = (long)Math.Ceiling((double)component.Quantity / rule.output.Quantity);

                        currentSet.EnqueueRange(rule.inputs.Select(c => new Component(c.Quantity * multiplier, c.Type)));

                        var waste = rule.output.Quantity * multiplier - component.Quantity;
                        if (waste > 0) wasteHeap.IncrementAtIndex(component.Type, waste);
                    }
                }
            }
            return ore;
        }

        public static long Part1(string input)
        {
            var rules = Util.RegexParse<Rule>(input).ToDictionary(e => e.output.Type, e => e);

            return Decompose(new Component(1, "FUEL"), rules);
        }

        public static long Part2(string input)
        {
            var rules = Util.RegexParse<Rule>(input).ToDictionary(e => e.output.Type, e => e);

            long ore = 1000000000000;
            long guess = ore * 1000 / Decompose(new Component(1000, "FUEL"), rules);

            return Util.BinarySearch(guess, fuel =>
            {
                long actual = Decompose(new Component(fuel, "FUEL"), rules);
                return (actual > ore, actual);
            }).input - 1;

        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}