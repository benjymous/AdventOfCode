using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2019
{
    public class Day14 : IPuzzle
    {
        public string Name => "2019-14";

        public class Component
        {
            public long quantity;
            public string type;

            public Component(string input)
            {
                if (input.Contains(',')) throw new Exception("comma in input!");

                var bits = input.Trim().Split(" ");
                quantity = long.Parse(bits[0]);
                type = bits[1];
            }

            public Component(string t, long q)
            {
                quantity = q;
                type = t;
            }
        }

        public class Rule
        {
            public List<Component> inputs;
            public Component output;

            public Rule(string input)
            {
                var halves = input.Split("=>");

                inputs = Util.Parse<Component>(halves[0], ",");
                output = new Component(halves[1]);
            }
        }


        public static long Decompose(Component input, Dictionary<string, Rule> rules)
        {
            long ore = 0;
            Queue<Component> currentSet = new();
            currentSet.Enqueue(input);

            Dictionary<string, int> wasteHeap = new();

            while (currentSet.Count > 0)
            {
                var component = currentSet.Dequeue();
                if (component.type == "ORE")
                {
                    ore += component.quantity;
                }
                else
                {
                    var rule = rules[component.type];

                    if (wasteHeap.ContainsKey(component.type) && wasteHeap[component.type] > 0)
                    {
                        var wasteUsed = Math.Min(component.quantity, wasteHeap[component.type]);
                        component.quantity -= wasteUsed;
                        wasteHeap[component.type] -= (int)wasteUsed;
                    }

                    if (component.quantity > 0)
                    {
                        var multiplier = (long)Math.Ceiling((double)component.quantity / rule.output.quantity);

                        var newElements = rule.inputs.Select(c => new Component(c.type, c.quantity * multiplier));

                        currentSet.EnqueueRange(newElements);

                        var waste = rule.output.quantity * multiplier - component.quantity;

                        if (waste > 0)
                        {
                            wasteHeap.IncrementAtIndex(component.type, (int)waste);
                        }
                    }
                }
            }
            return ore;
        }

        public static long Part1(string input)
        {
            var rules = Util.Parse<Rule>(input).ToDictionary(e => e.output.type, e => e);

            return Decompose(new Component("1 FUEL"), rules);
        }

        public static long Part2(string input)
        {
            var rules = Util.Parse<Rule>(input).ToDictionary(e => e.output.type, e => e);

            long ore = 1000000000000;

            long guess = (ore*1000) / Decompose(new Component("1000 FUEL"), rules);

            return Util.BinarySearch(guess, fuel =>
            {             
                long actual = Decompose(new Component($"{fuel} FUEL"), rules);
                return (actual > ore, actual);
            }).input-1;

        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}