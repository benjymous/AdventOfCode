using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2020
{
    public class Day07 : IPuzzle
    {
        public string Name => "2020-07";

        static readonly uint ShinyGoldKey = MakeKey("shiny gold");

        static uint MakeKey(string input) => input.GetCRC32();

        readonly struct SubBag
        {
            [Regex(@"(.+) (.+ .+) bags?")] public SubBag(int count, string bagType) => (Count, BagType) = (count, MakeKey(bagType));
            [Regex("no other bags")] public SubBag() { }

            public readonly int Count = 0;
            public readonly uint BagType;
        }

        readonly struct BagRule
        {
            [Regex(@"(.+) bags contain (.+)")]
            public BagRule(string bagType, string[] children)
            {
                BagType = MakeKey(bagType);
                Children = Util.RegexParse<SubBag>(children).Where(bag => bag.Count > 0).ToDictionary(bag => bag.BagType, bag => bag.Count);
            }

            public readonly uint BagType;
            public readonly Dictionary<uint, int> Children = new();
        }

        static long Count(uint type, Dictionary<uint, BagRule> rules, Dictionary<uint, long> cache = null)
        {
            cache ??= new();
            return cache.GetOrCalculate(type, type => rules[type].Children.Sum(c => c.Value * Count(c.Key, rules, cache)) + 1);
        }

        public static int Part1(string input)
        {
            var rules = Util.RegexParse<BagRule>(input).ToArray();

            HashSet<uint> goldholders = rules.Where(r => r.Children.ContainsKey(ShinyGoldKey)).Select(r => r.BagType).ToHashSet();

            while (true)
            {
                var found = rules.Where(rule => !goldholders.Contains(rule.BagType)).Where(rule => rule.Children.Keys.Any(goldholders.Contains)).Select(rule => rule.BagType);
                if (!found.Any()) return goldholders.Count;

                goldholders.UnionWith(found);
            }
        }

        public static long Part2(string input)
        {
            var rules = Util.RegexParse<BagRule>(input).ToDictionary(r => r.BagType);

            return Count(ShinyGoldKey, rules) - 1;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}