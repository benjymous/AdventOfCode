using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVI
{
    public class Day20 : IPuzzle
    {
        public string Name { get { return "2016-20";} }

        class Rule
        {
            public Rule(string input)
            {
                var vals = Util.ParseU64(input, '-');
                min = vals[0];
                max = vals[1];
            }
            public UInt64 min;
            public UInt64 max;

            public override string ToString() => $"{min} - {max}";
        }
 
        public static UInt64 Part1(string input)
        {
            var rules = Util.Parse<Rule>(input).OrderBy(r => r.min);

            UInt64 current = 0;
            foreach (var rule in rules)
            {
                if (current >= rule.min && current <= rule.max)
                {
                    current = rule.max+1;
                }
            }

            return current;
        }

        public static UInt64 Part2(string input)
        {
            var rules = Util.Parse<Rule>(input).OrderBy(r => r.min);
            UInt64 max = uint.MaxValue;

            UInt64 current = 0;
            UInt64 ranges = 0;

            foreach (var rule in rules)
            {
                if (current >= rule.min && current <= rule.max)
                {
                    current = rule.max+1;
                }
                if (rule.min > current)
                {                   
                    var range = (rule.min) - current;
                    ranges += range;
                    current = rule.max+1;                  
                }            
            }

            if (current <= max)
            {
                ranges += (max-current+1);
            }

            return ranges;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}