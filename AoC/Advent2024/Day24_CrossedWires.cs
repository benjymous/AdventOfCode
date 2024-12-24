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

    static Op DoSwap(string from, string to, Op op) => new(op.in1, op.in2, op.op, Swap(from, to, op.res));

    static Op DoSwaps(HashSet<(string from, string to)> subs, Op op)
    {
        foreach (var sub in subs)
        {
            op = DoSwap(sub.from, sub.to, op);
        }
        return op;
    }

    static IEnumerable<Op> GetActions(string section)
    {
        foreach (var line in Util.Split(section).WithoutNullOrWhiteSpace())
        {
            var parts1 = line.Split(" -> ");
            var parts2 = parts1[0].Split(" ");

            var in1 = parts2[0];
            var op = parts2[1];
            var in2 = parts2[2];
            var res = parts1[1];

            string[] toSort = [in1, in2];
            var sorted = toSort.Order().ToArray();

            yield return new Op(sorted[0], sorted[1], op, res);
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

    static IEnumerable<(int i, int v)> GetBits(long input)
    {
        long num = 1;
        int i = 0;
        while (num < input)
        {
            if ((input & num) != 0)
                yield return (i, 1);
            else
                yield return (i, 0);

            i++;
            num <<= 1;
        }
    }

    static Dictionary<string, int> SetInputs(long x, long y)
    {
        Dictionary<string, int> values = [];

        var xbits = GetBits(x).ToDictionary();
        var ybits = GetBits(y).ToDictionary();

        for (int i = 0; i < 64; ++i)
        {
            values.Add($"x{i:D2}", (xbits.TryGetValue(i, out int valx)) ? valx : 0);
            values.Add($"y{i:D2}", (ybits.TryGetValue(i, out int valy)) ? valy : 0);
        }

        return values;
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

    static (string,string) Sort((string a, string b) v)
    {
        if (v.a.CompareTo(v.b)<0) return (v.a, v.b);
        else return (v.b, v.a);
    }

    public static long Part1(string input)
    {
        var sections = input.SplitSections();

        Dictionary<string, int> values = GetInputs(sections[0]);
        var actions = GetActions(sections[1]);

        return RunAdder(values, actions);
    }


    public static string Part2(string input)
    {
        var sections = input.SplitSections();

        var testValues = GetInputs(sections[0]);
        var actions = GetActions(sections[1]);

        var x = 123456781;
        var y = 876543211;
        Dictionary<string, int> values = SetInputs(x, y);

        var z1 = RunAdder(values, actions);

        Console.WriteLine($"{actions.Count()} ops");
        Console.WriteLine($"{actions.Count(a => a.op == "AND")} ANDs");
        Console.WriteLine($"{actions.Count(a => a.op == "XOR")} XORs");
        Console.WriteLine($"{actions.Count(a => a.op == "OR")} ORs");


        HashSet<(string from, string to)> fullSubs = [];


        List<Op[]> validated = [];
    
        var remaining = actions.ToHashSet();

        HashSet<string> keys = testValues.Keys.Where(k => k.StartsWith('x')).ToHashSet();
        keys.Remove("x00"); // this one is a half adder, and seems to be ok

        while (true)
        {

            List<Op[]> invalid = [];

            /*
            *  op1   x XOR y  =>  mid1
            *  op2   x AND y  =>  mid2
            * 
            *  op3   mid1 XOR cin => sum
            *  op4   mid1 AND cin => mid3
            * 
            *  op5   mid2 OR mid3 => cout
            */

            HashSet<(string from, string to)> allSubs = [];

            foreach (var key in keys)
            {
                if (key == "x31")
                {
                    Console.WriteLine("x31");
                }

                var op1 = FindOperation(actions, key, key.Replace("x", "y"), "XOR");
                var op2 = FindOperation(actions, key, key.Replace("x", "y"), "AND");

                Console.WriteLine(op1);
                Console.WriteLine(op2);

                var mid1 = op1.res;
                var mid2 = op2.res;

                var op3 = FindOperation(actions, mid1, "", "XOR", key.Replace("x", "z"));
                var op4 = FindOperation(actions, op3.in1, op3.in2, "AND");

                if (op3 == default && op4 == default)
                {
                    Console.WriteLine("Need to find cin");
                }

                Console.WriteLine(op3);
                Console.WriteLine(op4);

                var mid3 = op4 != null ? op4.res : null;

                var op5 = FindOperation(actions, mid2, mid3, "OR");


                if (op5 == default && !string.IsNullOrWhiteSpace(mid2))
                {
                    op5 = FindOperation(actions, mid2, null, "OR");
                }
                if (op5 == default && !string.IsNullOrWhiteSpace(mid3))
                {
                    op5 = FindOperation(actions, mid3, null, "OR");
                }

                Console.WriteLine(op5);

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

                Console.WriteLine();
            }

            Console.WriteLine($"valid: {validated.Count} Invalid: {invalid.Count}  Unaccounted: {remaining.Count}  Subs: {allSubs.Count} pairs");

            foreach (var inv in invalid)
            {
                Console.WriteLine();
                foreach (var op in inv)
                {
                    Console.WriteLine(op);
                }
            }

            if (!invalid.Any() || !allSubs.Any()) break;

            Console.WriteLine("subs:");

            foreach (var s in allSubs)
            {
                Console.WriteLine(s);
            }

            fullSubs.UnionWith(allSubs);

            Console.WriteLine("---");
            foreach (var s in fullSubs)
            {
                Console.WriteLine(s);
            }

            actions = actions.Select(a => DoSwaps(allSubs, a)).ToArray();

            //Console.ReadKey();

        }

        Console.WriteLine("-----");
        foreach (var s in fullSubs)
        {
            Console.WriteLine(s);
        }


        var z2 = RunAdder(values, actions);
        Console.WriteLine($"{x} + {y} = {z1}?");
        Console.WriteLine($"{x} + {y} = {z2}?");
                    Console.WriteLine($"{x} + {y} = {x+y}!");

        return string.Join(",", fullSubs.SelectMany(v => new string[] { v.from, v.to }).Order());
    }

    static bool HasInput(Op op, string input) => op.in1 == input || op.in2 == input;
    static string GetOther(Op op, string input) => op.in1 == input ? op.in2 : op.in1;

    static (bool, List<(string from, string to)>) Validate(Op op1, Op op2, Op op3, Op op4, Op op5)
    {
        if (op1 == default || op2 == default || op3 == default || op4 == default || op5 == default)
        {
            Console.WriteLine("has gaps");

            return (false, []);
        }

        var mid1 = op1.res;
        var mid2 = op2.res;
        var sum = op3.res;
        var mid3 = op4.res;

        var sumExpected = string.Concat("z", op1.in2.AsSpan(1));
        if (sum != sumExpected)
        {
            Console.WriteLine($"Output should be '{sumExpected}' not '{sum}'");
            return (false, [(sumExpected, sum)]);
        }

        if (!HasInput(op3, mid1))
        {
            Console.WriteLine("mid1 mismatch on op3");
            return (false, [(mid1, op3.in2)]);
        }

        if (!HasInput(op4, mid1))
        {
            Console.WriteLine("mid1 mismatch on op4");
            return (false, []);
        }

        if (!HasInput(op5, mid2))
        {
            Console.WriteLine($"mid2 mismatch on op5 - '{mid2}'");

            if (HasInput(op5, mid3))
            {
                var other = GetOther(op5, mid3);
                Console.WriteLine($"Should swap with '{other}'");
                return (false, [(mid2, other)]);
            }

            return (false, []);
        }

        if (!HasInput(op5, mid3))
        {
            Console.WriteLine("mid3 mismatch on op5");
            return (false, []);
        }

/*
*  op1   x XOR y  =>  mid1
*  op2   x AND y  =>  mid2
* 
*  op3   mid1 XOR cin => sum
*  op4   mid1 AND cin => mid3
* 
*  op5   mid2 OR mid3 => cout
*/

        Console.WriteLine("maybe?");
        return (true, []);
    }

    private static Op FindOperation(IEnumerable<Op> actions, string a, string b, string op, string res = null)
    {
        if (string.IsNullOrEmpty(b))
        {
            var found = actions.SingleOrDefault(v => (v.in1 == a || v.in2 == a) && v.op == op);

            if (res != null && found == default)
            {
                found = actions.SingleOrDefault(v => (v.in1 == a || v.in2 == a) && v.op == op && v.res == res);
                if (found != default)
                {
                    Console.WriteLine("found via a, result");
                }
                else
                {
                    found = actions.SingleOrDefault(v => v.op == op && v.res == res);
                }
                

                return found;
            }

            return found;
        }
        else
        {
            string[] vals = [a, b];
            var sorted = vals.Order().ToArray();

            var found = actions.SingleOrDefault(v => v.in1 == sorted[0] && v.in2 == sorted[1] && v.op == op);



            return found;
        }
    }



    public void Run(string input, ILogger logger)
    {


        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}