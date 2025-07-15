namespace AoC.Advent2020;
public class Day07 : IPuzzle
{
    private static readonly string ShinyGoldKey = "shiny gold";

    public class Factory
    {
        [Regex(@"(.+) bags contain no other bags")]
        public static (string, Dictionary<string, uint>) EmptyBag(string bagType) => (bagType, []);

        [Regex(@"(.+) bags contain (.+)\.")]
        public static (string, Dictionary<string, uint>) Bag(string bagType, [Split(", ", @"(?<value>\d+) (?<key>.+) bags?")] Dictionary<string, uint> children) => (bagType, children);
    }

    private static long Count(string type, Dictionary<string, Dictionary<string, uint>> rules, Dictionary<string, long> cache = null) => cache.GetOrCalculate(type, type => rules[type].Sum(c => c.Value * Count(c.Key, rules, cache)) + 1);

    public static int Part1(Parser.AutoArray<(string BagType, Dictionary<string, uint> Children), Factory> rules)
    {
        HashSet<string> goldholders = [.. rules.Where(r => r.Children.ContainsKey(ShinyGoldKey)).Select(r => r.BagType)];

        while (true)
        {
            var found = rules.Where(rule => !goldholders.Contains(rule.BagType)).Where(rule => rule.Children.Keys.Any(goldholders.Contains)).Select(rule => rule.BagType);
            if (!found.Any()) return goldholders.Count;

            goldholders.UnionWith(found);
        }
    }

    public static long Part2(Parser.AutoArray<(string, Dictionary<string, uint>), Factory> input)
        => Count(ShinyGoldKey, input.ToDictionary(), []) - 1;

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}