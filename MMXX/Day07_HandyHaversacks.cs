using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXX
{
    public class Day07 : IPuzzle
    {
        public string Name { get { return "2020-07";} }

        class BagRule
        {
            public BagRule(string input)
            {
                var bits = input.Split("s contain ");
                BagType=bits[0].Replace(" bag", "");
                //Console.WriteLine($"-{BagType}-  '{bits[1]}'");
                var children = bits[1].Split(", ");
                foreach(var child in children)
                {
                    //Console.WriteLine(child);
                    var c = child.Trim().Split(" ");
                    if (c[0]=="no") break;
                    int count = Int32.Parse(c[0]);
                    string name = $"{c[1]} {c[2]}";
                    //Console.WriteLine($"  '{name}' : '{count}'");

                    Children[name]=count;
                }
            }

            public string BagType;
            public Dictionary<string, int> Children = new Dictionary<string, int>();
        }
 
        public static int Part1(string input)
        {
            var rules = Util.Parse<BagRule>(input);

            HashSet<string> goldholders = new HashSet<string> {"shiny gold"};

            var running = true;

            while(running)
            {
                //Console.WriteLine(string.Join(", ", goldholders));
                running=false;

                foreach(var rule in rules)
                {
                    if (!goldholders.Contains(rule.BagType))
                    {
                        if (rule.Children.Keys.Intersect(goldholders).Any()){
                            goldholders.Add(rule.BagType);
                            running=true;
                        }
                    }
                }
            }

            return goldholders.Count()-1;
        }


        static Int64 Count(string type, Dictionary<string, BagRule> rules)
        {
            var rule = rules[type];

            Int64 count = 1;

            foreach(var c in rule.Children)
            {
                count += c.Value * Count(c.Key, rules);
            }

            return count;

        }

        public static Int64 Part2(string input)
        {
            var rules = Util.Parse<BagRule>(input).ToDictionary(r => r.BagType, r => r);



            return Count("shiny gold", rules)-1;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}