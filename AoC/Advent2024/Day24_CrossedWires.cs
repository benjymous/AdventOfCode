namespace AoC.Advent2024;
public class Day24 : IPuzzle
{
    private static Dictionary<string, bool> GetInputs(string section)
    {
        Dictionary<string, bool> values = [];

        foreach (var line in Util.Split(section).WithoutNullOrWhiteSpace())
        {
            var parts = line.Split(": ");
            values[parts[0]] = parts[1] == "1";
        }

        return values;
    }

    [Regex(@"(...) (.+) (...) -> (...)")]
    private record class Action(string In1, string Op, string In2, string Res)
    {
        public bool HasInput(string a) => In1 == a || In2 == a;
        public bool HasInputs(string a, string b) => (In1 == a && In2 == b) || (In2 == a && In1 == b);
    }

    private static long GetZResult(Dictionary<string, bool> values)
        => values.Where(kvp => kvp.Key.StartsWith('z')).Aggregate(0L, (a, b) => a |= (b.Value ? 1L : 0) << int.Parse(b.Key[1..]));

    private static long RunAdder(Dictionary<string, bool> values, IEnumerable<Action> actions)
    {
        var queue = actions.ToQueue();

        while (queue.TryDequeue(out var entry))
        {
            if (values.TryGetValue(entry.In1, out var in1) && values.TryGetValue(entry.In2, out var in2))
            {
                values[entry.Res] = entry.Op switch
                {
                    "AND" => in1 & in2,
                    "OR" => in1 | in2,
                    "XOR" => in1 ^ in2,
                    _ => throw new InvalidOperationException(),
                };
            }
            else
            {
                queue.Enqueue(entry);
            }
        }

        return GetZResult(values);
    }

    public static long Part1(string input)
    {
        var sections = input.SplitSections();
        return RunAdder(GetInputs(sections[0]), Parser.Parse<Action>(sections[1]));
    }

    public static string Part2(string input)
    {
        var sections = input.SplitSections();

        var testValues = GetInputs(sections[0]);

        var actions = Parser.Parse<Action>(sections[1]).Where(a => a.Op == "XOR").ToArray();

        List<string> subs = [];

        HashSet<string> keys = [.. testValues.Keys.Where(k => k.StartsWith('x'))];
        keys.Remove("x00"); // this one is a half adder, and seems to be ok

        foreach (var (x, y, z) in keys.Select(x => (x, x.Replace("x", "y"), x.Replace("x", "z"))))
        {
            var xor1 = actions.Single(a => a.HasInputs(x, y));
            var xor2 = actions.Single(v => v.HasInput(xor1.Res) || v.Res == z);

            if (xor2.Res != z)
            {
                subs.AddRange(z, xor2.Res);
            }
            else if (!xor2.HasInput(xor1.Res))
            {
                subs.AddRange(xor1.Res, xor2.In2);
            }
        }

        return string.Join(",", subs.Order());
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}