using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent.MMXX
{
    public class Day19 : IPuzzle
    {
        public string Name { get { return "2020-19";} }
 
        class Rule
        {
            public Rule(string input)
            {
                var bits = input.Split(": ");
                ID = bits[0];
                Values = bits[1].Replace("\"", "").Split(" ").ToList();
            }

            public string ID;
            public List<string> Values;
        }

        static string Resolve(string key, Dictionary<string, Rule> rules)
        {
            if (!rules.ContainsKey(key)) return key;
            //Console.WriteLine("?+'"+key+"'");
            var current = rules[key];
            string result="";

            foreach (var child in current.Values)
            {            
                result += Resolve(child, rules);
            }

            if (result.Contains('|')) return "("+result+")";

            return result;
        }

        public static int Solve(string input, bool part2)
        {
            var sections = input.Split("\n\n");
            var rules = Util.Parse<Rule>(sections[0]).ToDictionary(v => v.ID, v => v);
            var messages = sections[1].Split("\n");

            if (part2)
            {
                rules["8"] = new Rule("8: ( 42 )+");
                rules["11"] = new Rule("11: 42 ( 42 ( 42 ( 42 ( 42 ( 42 31 )* 31 )* 31 )* 31 )* 31 )* 31");
            }

            var r = new Regex("^"+Resolve("0", rules)+"$");

            return messages.Where(m => r.Match(m).Success).Count();
        }

        public static int Part1(string input)
        {
            return Solve(input, false);
        }

        public static int Part2(string input)
        {
            return Solve(input, true);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - "+Part1(input)); // 134
            logger.WriteLine("- Pt2 - "+Part2(input)); // 377
        }
    }
}