using AoC.Utils;
using System.Linq;

namespace AoC.Advent2016
{
    public class Day20 : IPuzzle
    {
        public string Name => "2016-20";

        class Rule
        {
            public Rule(string input) => (min, max) = Util.ParseNumbers<uint>(input, '-').Decompose2();
            public uint min;
            public uint max;

            public override string ToString() => $"{min} - {max}";
        }

        public static uint Part1(string input)
        {
            var rules = Util.Parse<Rule>(input).OrderBy(r => r.min).ToArray();

            uint current = 0;
            foreach (var rule in rules)
            {
                if (current >= rule.min && current <= rule.max)
                {
                    current = rule.max + 1;
                }
                if (current < rule.min) return current;
            }

            return current;
        }

        public static uint Part2(string input)
        {
            var rules = Util.Parse<Rule>(input).OrderBy(r => r.min).ToArray();
            uint max = uint.MaxValue;

            uint current = 0;
            uint ranges = 0;

            foreach (var rule in rules)
            {
                if (current >= rule.min && current <= rule.max)
                {
                    current = rule.max + 1;
                }
                if (rule.min > current)
                {
                    var range = rule.min - current;
                    ranges += range;
                    if (rule.max == max)
                    {
                        current = max;
                        break;
                    }
                    current = rule.max + 1;
                }
            }

            if (current < max)
            {
                ranges += (max - current);
            }

            return ranges;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}