﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXIX
{
    public class Day14 : IPuzzle
    {
        public string Name { get { return "2019-14";} }

        public class Component
        {
            public int quantity;
            public string type;

            public Component(string input)
            {
                if (input.Contains(",")) throw new Exception("comma in input!");

                var bits = input.Trim().Split(" ");
                quantity = int.Parse(bits[0]);
                type = bits[1];
            }

            public Component(string t, int q)
            {
                quantity = q;
                type = t;
            }

            public override string ToString()
            {
                return $"{quantity} {type}";
            }
        }

        public class ComponentSet
        {
            public List<Component> components = new List<Component>();
            public List<string> history = new List<string>();

            public ComponentSet(List<Component> c, List<string> h, Rule rule)
            {
                components.AddRange(c);
                history.AddRange(h);
                history.Add(rule.ToString());
            }

            public ComponentSet(List<Component> c)
            {
                history.Add(String.Join(", ", c));
                components.AddRange(c);
            }

            public override string ToString()
            {
                return string.Join(", ", components);
            }
        }

        public class Rule
        {
            public List<Component> inputs;
            public Component output;

            public Dictionary<string, int> index;

            public Rule(string input)
            {
                var halves = input.Split("=>");

                inputs = Util.Parse<Component>(halves[0], ',');
                output = new Component(halves[1]);    

                index = inputs.ToDictionary( i => i.type, i => i.quantity );          
            }

            public override string ToString()
            {
                return $"{string.Join(", ",inputs)} => {output}";
            }
        }

        public static IEnumerable<ComponentSet> Deconstruct(ComponentSet available, List<Rule> rules)
        {
            List<ComponentSet> result = new List<ComponentSet>();
            foreach (var input in available.components)
            {
                var rest = available.components.Where(i => i!=input);
                foreach (var rule in rules)
                {
                    if (rule.output.type == input.type)
                    {
                        //if (rule.output.quantity <= input.quantity)
                        {
                            var multiplier = input.quantity / rule.output.quantity;
                            var remainder = input.quantity % rule.output.quantity;

                            if (remainder > 0) multiplier++;

                            var requirements = new List<Component>();
                            requirements.AddRange(rule.inputs.Select(c => new Component(c.type, c.quantity * multiplier)));
                            requirements.AddRange(rest);
                            // if (remainder > 0)
                            // {
                            //     requirements.Add(new Component(input.type, remainder));
                            // }

                            var grouped = requirements.GroupBy(c => c.type);

                            if (grouped.Count() < requirements.Count())
                            {
                                var final = new List<Component>();
                                foreach (var group in grouped)
                                {
                                    var comp = new Component(group.Key, group.Select(x => x.quantity).Sum());
                                    final.Add(comp);
                                }

                                // if (input.quantity > rule.output.quantity)
                                // {
                                //     requirements.Add(new Component(input.type, input.quantity-rule.output.quantity));
                                // }

                                //Console.WriteLine($"({input}) {string.Join(", ", rest)} : [{rule}] == {string.Join(", ", final)}");
                                result.Add(new ComponentSet(final, available.history, rule));
                            }
                            else
                            {
                                //Console.WriteLine($"({input}) {string.Join(", ", rest)} : [{rule}] == {string.Join(", ", requirements)}");
                                result.Add(new ComponentSet(requirements, available.history, rule));
                            }
                        }
                    }
                }
            }
            return result;
        }

        const int SetSize = 5000;

        public static int Part1(string input)
        {
            var rules = Util.Parse<Rule>(input);

            seen.Clear();

            var available = new List<ComponentSet>()
            {
                new ComponentSet(new List<Component>()
                {
                    new Component("1 FUEL")
                })

                // new ComponentSet(new List<Component>()
                // {
                //     new Component("16 C"),
                //     new Component("10 A"),
                //     new Component("8 B"),
                //     new Component("3 BC"),
                // })

                

                // new List<Component>()
                // {
                //     new Component("85 ORE"),
                //     new Component("37 C"),
                //     new Component("8 B"),
                // }
            };

            int turns = 0;
            var best = int.MaxValue;

            while (/*turns < 500 &&*/ available.Any())
            {
                //var next = new List<ComponentSet>();
                // foreach (var set in available)
                // {
                //     next.AddRange(Deconstruct(set, rules));
                // }

                IEnumerable<ComponentSet> use;
                IEnumerable<ComponentSet> skip;

                if (available.Count < SetSize)
                {
                    use = available.ToList();
                    skip = null;
                }
                else
                {
                    use = available.Take(SetSize);
                    skip = available.Skip(SetSize);
                }

                IEnumerable<ComponentSet> next = use.AsParallel().Select(set => Deconstruct(set, rules)).SelectMany(a => a);

                // List<ComponentSet> next = new List<ComponentSet>();

                // foreach (var set in use)
                // {
                //     next.AddRange(Deconstruct(set, rules));
                // }


                List<ComponentSet> unique = MakeUnique(next).ToList();

                if (skip != null)
                {
                    unique.AddRange(skip);
                }

                //next = new List<List<Component>>(){next.First()};

                turns++;

                var sorted = unique.OrderBy(e => -e.components.Select(c => c.quantity).Sum());
                
                available = new List<ComponentSet>();
                foreach (var set in sorted)
                {
                    if (set.components.Count == 1 && set.components.First().type == "ORE")
                    {
                        var el = set.components.First();
                        //Console.WriteLine($"Ore completed at {el.quantity}");
                        
                        best = Math.Min(best, el.quantity);
                    }
                    else
                    {
                        // if (set.components.Where(el => el.type=="ORE").Any())
                        // {
                        //     Console.WriteLine($"Ore found in {set}");
                        // }
                        available.Add(set);
                    }
                }

                if (best < int.MaxValue)
                {
                    Console.WriteLine($"{turns} - {best} -  {available.Count}");
                }
                else
                {
                    Console.WriteLine($"{turns} - ??? - {available.Count}");
                }

            }

            //Dictionary<string, Rule> index = rules.ToDictionary(rule => rule.outputs, rule=>rule );#
            Console.WriteLine($"{seen.Count()} combinations tried");
            return best;
        }

        static HashSet<string> seen = new HashSet<string>();
        
        private static IEnumerable<ComponentSet> MakeUnique(IEnumerable<ComponentSet> next)
        {
            
            var result = new List<ComponentSet>();

            foreach (var set in next)
            {
                var hash = string.Join(": ", set.components.OrderBy(c => c.type)).GetMD5String();
                if (!seen.Contains(hash))
                {
                    result.Add(set);
                    seen.Add(hash);
                }
            }

            return result;
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, System.IO.TextWriter console)
        {
            // //165
            // Console.WriteLine(Part1("9 ORE => 2 A\n8 ORE => 3 B\n7 ORE => 5 C\n3 A, 4 B => 1 AB\n5 B, 7 C => 1 BC\n4 C, 1 A => 1 CA\n2 AB, 3 BC, 4 CA => 1 FUEL"));
            
            // // 13312
            // Console.WriteLine(Part1("157 ORE => 5 NZVS\n165 ORE => 6 DCFZ\n44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL\n12 HKGWZ, 1 GPVTF, 8 PSHF => 9 QDVJ\n179 ORE => 7 PSHF\n177 ORE => 5 HKGWZ\n7 DCFZ, 7 PSHF => 2 XJWVT\n165 ORE => 2 GPVTF\n3 DCFZ, 7 NZVS, 5 HKGWZ, 10 PSHF => 8 KHKGT"));
            
            // // 180697 
            // Console.WriteLine(Part1("2 VPVL, 7 FWMGM, 2 CXFTF, 11 MNCFX => 1 STKFG\n17 NVRVD, 3 JNWZP => 8 VPVL\n53 STKFG, 6 MNCFX, 46 VJHF, 81 HVMC, 68 CXFTF, 25 GNMV => 1 FUEL\n22 VJHF, 37 MNCFX => 5 FWMGM\n139 ORE => 4 NVRVD\n144 ORE => 7 JNWZP\n5 MNCFX, 7 RFSQX, 2 FWMGM, 2 VPVL, 19 CXFTF => 3 HVMC\n5 VJHF, 7 MNCFX, 9 VPVL, 37 CXFTF => 6 GNMV\n145 ORE => 6 MNCFX\n1 NVRVD => 8 CXFTF\n1 VJHF, 6 MNCFX => 4 RFSQX\n176 ORE => 6 VJHF"));
            
            // // 2210736  
            // Console.WriteLine(Part1("171 ORE => 8 CNZTR\n7 ZLQW, 3 BMBT, 9 XCVML, 26 XMNCP, 1 WPTQ, 2 MZWV, 1 RJRHP => 4 PLWSL\n114 ORE => 4 BHXH\n14 VRPVC => 6 BMBT\n6 BHXH, 18 KTJDG, 12 WPTQ, 7 PLWSL, 31 FHTLT, 37 ZDVW => 1 FUEL\n6 WPTQ, 2 BMBT, 8 ZLQW, 18 KTJDG, 1 XMNCP, 6 MZWV, 1 RJRHP => 6 FHTLT\n15 XDBXC, 2 LTCX, 1 VRPVC => 6 ZLQW\n13 WPTQ, 10 LTCX, 3 RJRHP, 14 XMNCP, 2 MZWV, 1 ZLQW => 1 ZDVW\n5 BMBT => 4 WPTQ\n189 ORE => 9 KTJDG\n1 MZWV, 17 XDBXC, 3 XCVML => 2 XMNCP\n12 VRPVC, 27 CNZTR => 2 XDBXC\n15 KTJDG, 12 BHXH => 5 XCVML\n3 BHXH, 2 VRPVC => 7 MZWV\n121 ORE => 7 VRPVC\n7 XCVML => 6 RJRHP\n5 BHXH, 4 VRPVC => 5 LTCX"));

            //console.WriteLine("- Pt1 - "+Part1(input));
            //console.WriteLine("- Pt2 - "+Part2(input));

            // int[] x = {1,2,3,4,5,6,7,8,9,10};

            // console.WriteLine(string.Join(",",x.Take(4)));
            // console.WriteLine(string.Join(",",x.Skip(4)));

            // ? 903538
            // ? 904144
            // ? 933733
        }
    }
}