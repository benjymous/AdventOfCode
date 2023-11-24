namespace AoC.Advent2020;
public class Day19 : IPuzzle
{
    [Regex("(.+): \\\"?([^\\\"]+)\\\"?")]
    record class Rule(string ID, [Split(" ")] string[] Values);
    static string Resolve(string key, Dictionary<string, Rule> rules)
    {
        if (!rules.TryGetValue(key, out Rule rule)) return key;

        string result = string.Concat(rule.Values.Select(child => Resolve(child, rules)));

        return result.Contains('|') ? "(" + result + ")" : result;
    }

    public static int Solve(string input, QuestionPart part)
    {
        var sections = input.Split("\n\n");
        var rules = Util.RegexParse<Rule>(sections[0]).ToDictionary(v => v.ID, v => v);
        var messages = sections[1].Split("\n");

        if (part.Two())
        {
            rules["8"] = Util.FromString<Rule>("8: ( 42 )+");
            rules["11"] = Util.FromString<Rule>("11: 42 ( 42 ( 42 ( 42 ( 42 ( 42 31 )* 31 )* 31 )* 31 )* 31 )* 31");
        }

        var r = new Regex("^" + Resolve("0", rules) + "$");

        return messages.Count(m => r.Match(m).Success);
    }

    public static int Part1(string input) => Solve(input, QuestionPart.Part1);

    public static int Part2(string input) => Solve(input, QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input)); // 134
        logger.WriteLine("- Pt2 - " + Part2(input)); // 377
    }
}