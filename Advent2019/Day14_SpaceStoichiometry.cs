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
            public Int64 quantity;
            public string type;

            public Component(string input)
            {
                if (input.Contains(',')) throw new Exception("comma in input!");

                var bits = input.Trim().Split(" ");
                quantity = Int64.Parse(bits[0]);
                type = bits[1];
            }

            public Component(string t, Int64 q)
            {
                quantity = q;
                type = t;
            }

            public override string ToString()
            {
                return $"{quantity} {type}";
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

            public override string ToString()
            {
                return $"{string.Join(", ", inputs)} => {output}";
            }
        }


        public static Int64 Decompose(Component input, Dictionary<string, Rule> rules)
        {
            Int64 ore = 0;
            Queue<Component> currentSet = new();
            currentSet.Enqueue(input);

            Dictionary<string, int> wasteHeap = new();

            while (currentSet.Count > 0)
            {
                // if (ore > 0)
                // {
                //     Console.Write($"[{ore} ORE] ");
                // }
                // Console.WriteLine(string.Join(", ", currentSet));

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

                        var multiplier = (int)(Math.Ceiling((double)component.quantity / rule.output.quantity));

                        var newElements = rule.inputs.Select(c => new Component(c.type, c.quantity * multiplier));

                        foreach (var el in newElements)
                        {
                            currentSet.Enqueue(el);
                        }

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

        public static Int64 Part1(string input)
        {
            var rules = Util.Parse<Rule>(input).ToDictionary(e => e.output.type, e => e);

            return Decompose(new Component("1 FUEL"), rules);
        }

        public static Int64 Part2(string input)
        {
            var rules = Util.Parse<Rule>(input).ToDictionary(e => e.output.type, e => e);

            Int64 ore = 1000000000000;

            Int64 oreFor1Fuel = Decompose(new Component("1 FUEL"), rules);

            HashSet<Int64> seen = new();

            Int64 guess = ore / oreFor1Fuel;
            Int64 bestGuess = guess;
            Int64 min = 1;
            Int64 max = 50000000;
            while (min <= max)
            {
                Int64 actual = Decompose(new Component($"{guess} FUEL"), rules);
                //Console.WriteLine($"{guess} fuel from {actual} ore");
                if (actual < ore) bestGuess = Math.Max(bestGuess, guess);
                //Console.WriteLine($"{(double)actual/(double)ore}");

                //Console.WriteLine($"{min} - {guess} - {max}");

                if (actual > ore)
                {
                    max = guess - 1;
                }
                else
                {
                    min = guess + 1;
                }

                guess = (min + max) / 2;
            }
            return bestGuess;
        }

        public void Run(string input, ILogger logger)
        {
            // //165
            // Console.WriteLine("165 ? "+Part1("9 ORE => 2 A\n8 ORE => 3 B\n7 ORE => 5 C\n3 A, 4 B => 1 AB\n5 B, 7 C => 1 BC\n4 C, 1 A => 1 CA\n2 AB, 3 BC, 4 CA => 1 FUEL"));

            // // 13312
            // Console.WriteLine("13312 ? "+Part1("157 ORE => 5 NZVS\n165 ORE => 6 DCFZ\n44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL\n12 HKGWZ, 1 GPVTF, 8 PSHF => 9 QDVJ\n179 ORE => 7 PSHF\n177 ORE => 5 HKGWZ\n7 DCFZ, 7 PSHF => 2 XJWVT\n165 ORE => 2 GPVTF\n3 DCFZ, 7 NZVS, 5 HKGWZ, 10 PSHF => 8 KHKGT"));

            // // 180697 
            // Console.WriteLine("180697 ? "+Part1("2 VPVL, 7 FWMGM, 2 CXFTF, 11 MNCFX => 1 STKFG\n17 NVRVD, 3 JNWZP => 8 VPVL\n53 STKFG, 6 MNCFX, 46 VJHF, 81 HVMC, 68 CXFTF, 25 GNMV => 1 FUEL\n22 VJHF, 37 MNCFX => 5 FWMGM\n139 ORE => 4 NVRVD\n144 ORE => 7 JNWZP\n5 MNCFX, 7 RFSQX, 2 FWMGM, 2 VPVL, 19 CXFTF => 3 HVMC\n5 VJHF, 7 MNCFX, 9 VPVL, 37 CXFTF => 6 GNMV\n145 ORE => 6 MNCFX\n1 NVRVD => 8 CXFTF\n1 VJHF, 6 MNCFX => 4 RFSQX\n176 ORE => 6 VJHF"));

            // // 2210736  
            // Console.WriteLine("2210736 ? "+Part1("171 ORE => 8 CNZTR\n7 ZLQW, 3 BMBT, 9 XCVML, 26 XMNCP, 1 WPTQ, 2 MZWV, 1 RJRHP => 4 PLWSL\n114 ORE => 4 BHXH\n14 VRPVC => 6 BMBT\n6 BHXH, 18 KTJDG, 12 WPTQ, 7 PLWSL, 31 FHTLT, 37 ZDVW => 1 FUEL\n6 WPTQ, 2 BMBT, 8 ZLQW, 18 KTJDG, 1 XMNCP, 6 MZWV, 1 RJRHP => 6 FHTLT\n15 XDBXC, 2 LTCX, 1 VRPVC => 6 ZLQW\n13 WPTQ, 10 LTCX, 3 RJRHP, 14 XMNCP, 2 MZWV, 1 ZLQW => 1 ZDVW\n5 BMBT => 4 WPTQ\n189 ORE => 9 KTJDG\n1 MZWV, 17 XDBXC, 3 XCVML => 2 XMNCP\n12 VRPVC, 27 CNZTR => 2 XDBXC\n15 KTJDG, 12 BHXH => 5 XCVML\n3 BHXH, 2 VRPVC => 7 MZWV\n121 ORE => 7 VRPVC\n7 XCVML => 6 RJRHP\n5 BHXH, 4 VRPVC => 5 LTCX"));

            // 

            // logger.WriteLine(ore / 13312);

            //Console.WriteLine(Part2("9 ORE => 2 A\n8 ORE => 3 B\n7 ORE => 5 C\n3 A, 4 B => 1 AB\n5 B, 7 C => 1 BC\n4 C, 1 A => 1 CA\n2 AB, 3 BC, 4 CA => 1 FUEL"));
            //Console.WriteLine(Part2("157 ORE => 5 NZVS\n165 ORE => 6 DCFZ\n44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL\n12 HKGWZ, 1 GPVTF, 8 PSHF => 9 QDVJ\n179 ORE => 7 PSHF\n177 ORE => 5 HKGWZ\n7 DCFZ, 7 PSHF => 2 XJWVT\n165 ORE => 2 GPVTF\n3 DCFZ, 7 NZVS, 5 HKGWZ, 10 PSHF => 8 KHKGT"));
            //Console.WriteLine(Part2("2 VPVL, 7 FWMGM, 2 CXFTF, 11 MNCFX => 1 STKFG\n17 NVRVD, 3 JNWZP => 8 VPVL\n53 STKFG, 6 MNCFX, 46 VJHF, 81 HVMC, 68 CXFTF, 25 GNMV => 1 FUEL\n22 VJHF, 37 MNCFX => 5 FWMGM\n139 ORE => 4 NVRVD\n144 ORE => 7 JNWZP\n5 MNCFX, 7 RFSQX, 2 FWMGM, 2 VPVL, 19 CXFTF => 3 HVMC\n5 VJHF, 7 MNCFX, 9 VPVL, 37 CXFTF => 6 GNMV\n145 ORE => 6 MNCFX\n1 NVRVD => 8 CXFTF\n1 VJHF, 6 MNCFX => 4 RFSQX\n176 ORE => 6 VJHF"));
            //Console.WriteLine(Part2("171 ORE => 8 CNZTR\n7 ZLQW, 3 BMBT, 9 XCVML, 26 XMNCP, 1 WPTQ, 2 MZWV, 1 RJRHP => 4 PLWSL\n114 ORE => 4 BHXH\n14 VRPVC => 6 BMBT\n6 BHXH, 18 KTJDG, 12 WPTQ, 7 PLWSL, 31 FHTLT, 37 ZDVW => 1 FUEL\n6 WPTQ, 2 BMBT, 8 ZLQW, 18 KTJDG, 1 XMNCP, 6 MZWV, 1 RJRHP => 6 FHTLT\n15 XDBXC, 2 LTCX, 1 VRPVC => 6 ZLQW\n13 WPTQ, 10 LTCX, 3 RJRHP, 14 XMNCP, 2 MZWV, 1 ZLQW => 1 ZDVW\n5 BMBT => 4 WPTQ\n189 ORE => 9 KTJDG\n1 MZWV, 17 XDBXC, 3 XCVML => 2 XMNCP\n12 VRPVC, 27 CNZTR => 2 XDBXC\n15 KTJDG, 12 BHXH => 5 XCVML\n3 BHXH, 2 VRPVC => 7 MZWV\n121 ORE => 7 VRPVC\n7 XCVML => 6 RJRHP\n5 BHXH, 4 VRPVC => 5 LTCX"));


            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}