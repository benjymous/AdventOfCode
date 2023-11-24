namespace AoC.Advent2021;
public class Day14 : IPuzzle
{
    [method: Regex("(..) -> (.)")]
    public readonly struct Rule(string input, char insert)
    {
        public readonly string Input = input;
        public readonly List<string> Sub = [$"{input[0]}{insert}", $"{insert}{input[1]}"];
    }

    private static long Solve(string input, int steps)
    {
        var (initial, ruleLines) = input.Split("\n\n").Decompose2();
        var rules = Util.RegexParse<Rule>(ruleLines).ToDictionary(rule => rule.Input, rule => rule);

        var pairs = initial.Windows(2)
                           .Select(p => p.AsString())
                           .GroupBy(p => p)
                           .ToDictionary(g => g.Key, g => (long)g.Count());

        for (int i = 0; i < steps; ++i)
        {
            pairs = Step(rules, pairs);
        }

        Dictionary<char, long> groups = [];
        foreach (var pair in pairs)
        {
            groups.IncrementAtIndex(pair.Key[0], pair.Value);
        }

        groups[initial.Last()]++; // include the last character

        return groups.Values.Max() - groups.Values.Min();
    }

    private static Dictionary<string, long> Step(Dictionary<string, Rule> rules, Dictionary<string, long> pairs)
    {
        Dictionary<string, long> next = [];
        foreach (var pair in pairs)
        {
            if (rules.TryGetValue(pair.Key, out var rule))
            {
                foreach (var newpair in rule.Sub)
                {
                    next.IncrementAtIndex(newpair, pair.Value);
                }
            }
            else
            {
                next.Add(pair.Key, pair.Value);
            }
        }

        return next;
    }

    public static long Part1(string input) => Solve(input, 10);

    public static long Part2(string input) => Solve(input, 40);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}