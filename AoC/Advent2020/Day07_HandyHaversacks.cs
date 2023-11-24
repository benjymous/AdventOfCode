namespace AoC.Advent2020;
public class Day07 : IPuzzle
{
    static readonly string ShinyGoldKey = "shiny gold";

    readonly struct BagRule
    {
        public readonly string BagType;
        public readonly Dictionary<string, uint> Children = [];

        [Regex(@"(.+) bags contain no other bags")]
        public BagRule(string bagType) => BagType = bagType;

        [Regex(@"(.+) bags contain (.+)\.")]
        public BagRule(string bagType, [Split(", ", @"(?<value>\d+) (?<key>.+) bags?")] Dictionary<string, uint> children) => (BagType, Children) = (bagType, children);
    }

    static long Count(string type, Dictionary<string, BagRule> rules, Dictionary<string, long> cache = null)
    {
        cache ??= [];
        return cache.GetOrCalculate(type, type => rules[type].Children.Sum(c => c.Value * Count(c.Key, rules, cache)) + 1);
    }

    public static int Part1(string input)
    {
        var rules = Util.RegexParse<BagRule>(input).ToArray();

        HashSet<string> goldholders = rules.Where(r => r.Children.ContainsKey(ShinyGoldKey)).Select(r => r.BagType).ToHashSet();

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