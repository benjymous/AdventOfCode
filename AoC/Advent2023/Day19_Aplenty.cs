namespace AoC.Advent2023;
public class Day19 : IPuzzle
{
    [method: Regex(@"(.)(.)(\d+):(.+)")]
    record class Rule(char Key, char Op, int Value, string Dest)
    {
        [Regex(@"(.+)")] public Rule(string dest) : this('.', '!', 0, dest) { }

        public readonly int KeyIdx = "xmas".IndexOf(Key);

        public bool Passes(int[] qualities) => Op switch
        {
            '<' => qualities[KeyIdx] < Value,
            '>' => qualities[KeyIdx] > Value,
            _ => true
        };
    }

    [Regex(@"(.+){(.+)}")] record class Workflow(string WorkflowId, Rule[] Rules);

    [Regex(@"{(.+)}")]
    record class PartQualities([Split(",", "(?<key>.)=(?<value>.+)")] Dictionary<char, int> Qualities)
    {
        public readonly int[] AsArray = [Qualities['x'], Qualities['m'], Qualities['a'], Qualities['s']];
    }

    record class RangeSet((int min, int max)[] Ranges)
    {
        public RangeSet() : this([(1, 4000), (1, 4000), (1, 4000), (1, 4000)]) { }

        public (RangeSet pass, RangeSet keep) Split(Rule rule)
        {
            if (rule.Op == '!') return (this, default);

            var (min, max) = Ranges[rule.KeyIdx];
            RangeSet pass;

            if (rule.Op == '<')
            {
                pass = new RangeSet(Ranges.WithReplacement(rule.KeyIdx, (min, rule.Value - 1)));
                Ranges[rule.KeyIdx] = (rule.Value, max);
            }
            else
            {
                pass = new RangeSet(Ranges.WithReplacement(rule.KeyIdx, (rule.Value + 1, max)));
                Ranges[rule.KeyIdx] = (min, rule.Value);
            }
            return (pass, this);
        }
    }

    static IEnumerable<int> RunRules(string[] sections)
    {
        var workflows = Util.RegexParse<Workflow>(sections[0]).ToDictionary(w => w.WorkflowId, w => w.Rules);
        foreach (var part in Util.RegexParse<PartQualities>(sections[1]).Select(pq => pq.AsArray))
        {
            string current = "in";
            while (current is not "A" and not "R")
                current = workflows[current].First(r => r.Passes(part)).Dest;

            if (current == "A") yield return part.Sum();
        }
    }

    static IEnumerable<long> CountCombinations(Dictionary<string, Rule[]> workflows, string targetFlow = "in", RangeSet current = null)
    {
        if (targetFlow == "A") yield return current.Ranges.Product(v => v.max - v.min + 1);
        else if (targetFlow != "R")
        {
            foreach (var rule in workflows[targetFlow])
            {
                (var pass, current) = (current ?? new()).Split(rule);

                yield return CountCombinations(workflows, rule.Dest, pass).Sum();
                if (current == default) break;
            }
        }
    }

    public static int Part1(string input) => RunRules(input.Split("\n\n")).Sum();

    public static long Part2(string input) => CountCombinations(Util.RegexParse<Workflow>(input.Split("\n\n")[0]).ToDictionary(w => w.WorkflowId, w => w.Rules)).Sum();

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}