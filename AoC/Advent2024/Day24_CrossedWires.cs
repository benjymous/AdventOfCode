namespace AoC.Advent2024;
public class Day24 : IPuzzle
{
    static Dictionary<string, int> GetInputs(string section)
    {
        Dictionary<string, int> values = [];

        foreach (var line in Util.Split(section).WithoutNullOrWhiteSpace())
        {
            var parts = line.Split(": ");
            values[parts[0]] = parts[1] == "1" ? 1 : 0;
        }

        return values;
    }

    record class Op(string in1, string in2, string op, string res);

    static string Swap(string from, string to, string current)
    {
        if (current == from) return to;
        else if (current == to) return from;
        else return current;
    }

    static Op DoSwaps(HashSet<(string from, string to)> subs, Op op)
    {
        foreach (var (from, to) in subs)
        {
            op = new(op.in1, op.in2, op.op, Swap(from, to, op.res));
        }
        return op;
    }

    static IEnumerable<Op> GetActions(string section)
    {
        foreach (var line in Util.Split(section).WithoutNullOrWhiteSpace())
        {
            var parts1 = line.Split(" -> ");
            var parts2 = parts1[0].Split(" ");

            string[] toSort = [(parts2[0]), (parts2[2])];
            var sorted = toSort.Order().ToArray();

            yield return new Op(sorted[0], sorted[1], parts2[1], parts1[1]);
        }
    }

    static long GetZResult(Dictionary<string, int> values)
    {
        var zValues = values.Where(kvp => kvp.Key.StartsWith('z'));

        long bitResult = 0;
        foreach (var b in zValues)
        {
            var idx = int.Parse(b.Key.Substring(1));
            long bit = (long)b.Value << idx;
            bitResult |= bit;
        }

        return bitResult;
    }

    static long RunAdder(Dictionary<string, int> values, IEnumerable<Op> actions)
    {
        var queue = actions.ToQueue();

        while (queue.TryDequeue(out var entry))
        {
            if (values.TryGetValue(entry.in1, out var in1) && values.TryGetValue(entry.in2, out var in2))
            {
                var result = entry.op switch
                {
                    "AND" => in1 & in2,
                    "OR" => in1 | in2,
                    "XOR" => in1 ^ in2,
                    _ => throw new InvalidOperationException(),
                };

                values[entry.res] = result;
            }
            else
            {
                queue.Enqueue(entry);
            }
        }

        return GetZResult(values);
    }

    static (string, string) Sort((string a, string b) v) => v.a.CompareTo(v.b) < 0 ? (v.a, v.b) : (v.b, v.a);

    public static long Part1(string input)
    {
        var sections = input.SplitSections();
        return RunAdder(GetInputs(sections[0]), GetActions(sections[1]));
    }

    public static string Part2(string input)
    {
        var sections = input.SplitSections();

        var testValues = GetInputs(sections[0]);
        var actions = GetActions(sections[1]);

        HashSet<(string from, string to)> fullSubs = [];

        List<Op[]> validated = [];
    
        var remaining = actions.ToHashSet();

        HashSet<string> keys = testValues.Keys.Where(k => k.StartsWith('x')).ToHashSet();
        keys.Remove("x00"); // this one is a half adder, and seems to be ok

        while (true)
        {
            List<Op[]> invalid = [];

            HashSet<(string from, string to)> allSubs = [];

            foreach (var key in keys)
            {
                var op1 = FindOperation(actions, key, key.Replace("x", "y"), "XOR");
                var op2 = FindOperation(actions, key, key.Replace("x", "y"), "AND");

                var mid1 = op1.res;
                var mid2 = op2.res;

                var op3 = FindOperation(actions, mid1, "", "XOR", key.Replace("x", "z"));
                var op4 = FindOperation(actions, op3.in1, op3.in2, "AND");

                var mid3 = op4?.res;

                var op5 = FindOperation(actions, mid2, mid3, "OR");

                Op[] set = [op1, op2, op3, op4, op5];

                var (isValid, subs) = Validate(op1, op2, op3, op4, op5);

                if (isValid)
                {
                    validated.Add(set);
                    remaining.RemoveRange(set);
                    keys.Remove(key);
                }
                else
                {
                    invalid.Add(set);
                    allSubs.UnionWith(subs.Select(Sort));
                }
            }

            if (invalid.Count == 0 || allSubs.Count == 0) break;

            fullSubs.UnionWith(allSubs);
            actions = actions.Select(a => DoSwaps(allSubs, a)).ToArray();
        }

        return string.Join(",", fullSubs.SelectMany(v => new string[] { v.from, v.to }).Order());
    }

    static bool HasInput(Op op, string input) => op.in1 == input || op.in2 == input;

    static (bool, List<(string from, string to)>) Validate(Op op1, Op op2, Op op3, Op op4, Op op5)
    {
        var mid1 = op1.res;
        var sum = op3.res;

        var sumExpected = string.Concat("z", op1.in2.AsSpan(1));
        return sum != sumExpected ? (false, [(sumExpected, sum)]) : !HasInput(op3, mid1) ? (false, [(mid1, op3.in2)]) : (true, []);
    }

    private static Op FindOperation(IEnumerable<Op> actions, string a, string b, string op, string res = null)
    {
        if (string.IsNullOrEmpty(b))
        {
            var found = actions.SingleOrDefault(v => (v.in1 == a || v.in2 == a) && v.op == op);

            if (res != null && found == default)
            {
                found = actions.SingleOrDefault(v => (v.in1 == a || v.in2 == a) && v.op == op && v.res == res);
                if (found == default)
                {
                    found = actions.SingleOrDefault(v => v.op == op && v.res == res);
                }
            }

            return found;
        }
        else
        {
            string[] vals = [a, b];
            var sorted = vals.Order().ToArray();

            return actions.SingleOrDefault(v => v.in1 == sorted[0] && v.in2 == sorted[1] && v.op == op);
        }
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}