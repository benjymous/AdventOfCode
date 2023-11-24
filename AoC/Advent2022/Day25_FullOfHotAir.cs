namespace AoC.Advent2022;
public class Day25 : IPuzzle
{
    public static Snafu Part1(string input) => Util.Parse<Snafu>(input).Sum();

    public void Run(string input, ILogger logger) => logger.WriteLine("- Pt1 - " + Part1(input));
}

public class Snafu : ISummable<Snafu>
{
    public Snafu(long value = 0) => components = value.While(value => value > 0, value => { var res = DivRemBalance(value, out value); return (value, res); }).ToArray();
    public Snafu(string value) => components = value.Reverse().Select(ToDecimal).ToArray();
    Snafu(IEnumerable<sbyte> comp) => (components, balanced) = (comp.ToArray(), false);

    readonly sbyte[] components;
    bool balanced = true;

    public static Snafu operator +(Snafu a, Snafu b) => new(a.components.ZipLongest(b.components).Select(pair => (sbyte)(pair.Item1 + pair.Item2)));

    Snafu Balance()
    {
        if (!balanced)
        {
            long rem = 0;
            for (int i = 0; i < components.Length; ++i)
            {
                components[i] = DivRemBalance((sbyte)(rem + components[i]), out rem);
            }
            balanced = true;
        }

        return this;
    }

    static sbyte DivRemBalance(long input, out long next)
    {
        next = input / 5;
        input -= next * 5;

        if (input > 2) { input -= 5; next++; }
        if (input < -2) { input += 5; next--; }
        return (sbyte)input;
    }

    public long ToDecimal() => components.Reverse().Aggregate(0L, (current, val) => (current * 5) + val);
    public override string ToString() => Balance().components.Reverse().Select(ToChar).AsString();

    static sbyte ToDecimal(char c) => c switch { '=' => -2, '-' => -1, _ => (sbyte)(c - '0') };
    static char ToChar(sbyte v) => v switch { -2 => '=', -1 => '-', _ => (char)('0' + v) };
}