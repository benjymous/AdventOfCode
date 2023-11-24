namespace AoC.Advent2016;
public class Day20 : IPuzzle
{
    [Regex(@"(\d+)-(\d+)")]
    record class Rule(uint Min, uint Max);

    public static uint Part1(string input)
    {
        var rules = Util.RegexParse<Rule>(input).OrderBy(r => r.Min).ToArray();

        uint current = 0;
        foreach (var rule in rules)
        {
            if (current >= rule.Min && current <= rule.Max)
                current = rule.Max + 1;

            if (current < rule.Min)
                return current;
        }

        return current;
    }

    public static uint Part2(string input)
    {
        var rules = Util.RegexParse<Rule>(input).OrderBy(r => r.Min).ToArray();
        uint max = uint.MaxValue;

        uint current = 0;
        uint ranges = 0;

        foreach (var rule in rules)
        {
            if (current >= rule.Min && current <= rule.Max)
                current = rule.Max + 1;

            if (rule.Min > current)
            {
                var range = rule.Min - current;
                ranges += range;
                if (rule.Max == max)
                {
                    current = max;
                    break;
                }
                current = rule.Max + 1;
            }
        }

        if (current < max)
        {
            ranges += max - current;
        }

        return ranges;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}