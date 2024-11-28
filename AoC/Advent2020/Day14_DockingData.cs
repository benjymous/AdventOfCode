namespace AoC.Advent2020;
public class Day14 : IPuzzle
{
    public struct Statement
    {
        [Regex("mask = (.+)")]
        public Statement(string mask) => Mask = (Convert.ToInt64(mask.Replace('X', '0'), 2), Convert.ToInt64(mask.Replace('1', '0').Replace('X', '1'), 2));

        [Regex(@"mem\[(\d+)\] = (\d+)")]
        public Statement(long address, long value) => (Address, Value) = (address, value);

        public readonly long Address = 0, Value = 0;
        public (long Value, long QuantumBits) Mask;

        public Statement SetMask((long Value, long QuantumBits) mask) { Mask = mask; return this; }
    }

    public static long ApplyMaskV1(long value, (long Value, long QuantumBits) mask)
    {
        for (var b = 1L; b <= 1L << 35; b <<= 1)
        {
            if ((mask.QuantumBits & b) == 0)
            {
                if ((mask.Value & b) == 0) value &= ~b;
                else value |= b;
            }
        }

        return value;
    }

    public static (long Value, long QuantumBits) ApplyMaskV2(long value, (long Value, long QuantumBits) mask) => (value | mask.Value, mask.QuantumBits & ~mask.Value);

    class MemoryContainer<T>
    {
        readonly Queue<T> items = new(255);
        readonly HashSet<T> seen = new(255);
        public void Push(T v)
        {
            if (seen.IsUnseen(v)) items.Enqueue(v);
        }

        public T Take() => items.Dequeue();
        public bool Any() => items.Count > 0;
    }

    public static IEnumerable<long> Combinations((long Value, long QuantumBits) input) => input.QuantumBits.BitSequence().Combinations().Select(combo => (input.Value & ~input.QuantumBits) | combo.Sum());

    static IEnumerable<Statement> Flatten(IEnumerable<Statement> inputs)
    {
        var mask = (0L, 0L);

        foreach (var statement in inputs)
        {
            if (statement.Address == 0) mask = statement.Mask;
            else yield return statement.SetMask(mask);
        }
    }

    private static IEnumerable<Statement> ParseData(string input) => Flatten(Parser.Parse<Statement>(input)).Reverse();

    public static long Part1(string input)
    {
        var statements = ParseData(input);

        HashSet<long> seen = [];
        return statements.Where(s => seen.IsUnseen(s.Address)).Sum(s => ApplyMaskV1(s.Value, s.Mask));
    }

    public static long Part2(string input)
    {
        var statements = ParseData(input);

        HashSet<long> seen = [];
        return statements.Sum(s => s.Value * Combinations(ApplyMaskV2(s.Address, s.Mask)).Count(a => seen.IsUnseen(a)));
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}