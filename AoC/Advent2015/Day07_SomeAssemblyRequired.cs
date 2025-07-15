namespace AoC.Advent2015;
public class Day07 : IPuzzle
{
    public enum Operator
    {
        NULL,
        AND,
        OR,
        LSHIFT,
        RSHIFT,
        NOT
    }

    public readonly struct Variant
    {
        public Variant(string val)
        {
            if (int.TryParse(val, out var v)) intValue = v;
            else strValue = val;
        }

        private readonly string strValue = null;
        private readonly int? intValue = null;

        public bool IsNumeric => intValue.HasValue;
        public int IntValue => intValue.Value;
        public string StringValue => strValue;

        public static implicit operator Variant(string str) => new(str);

        public override string ToString() => IsNumeric ? $"'{IntValue}' [int]" : $"'{StringValue}' [str]";
    }

    public class Component
    {
        [Regex(@"(\S+) (\S+) (\S+) -> (\S+)")] public Component(string in1, Operator op, string in2, string outName) => (Input1, Operator, Input2, OutName) = (in1, op, in2, outName);
        [Regex(@"(\S+) (\S+) -> (\S+)")] public Component(Operator op, string in1, string outName) => (Input1, Operator, OutName) = (in1, op, outName);
        [Regex(@"(\S+) -> (\S+)")] public Component(string in1, string outName) => (Input1, OutName) = (in1, outName);

        public int? Resolved { get; set; } = null;
        public readonly Variant OutName = null, Input1 = null, Input2 = null;
        public readonly Operator Operator = Operator.NULL;
    }

    public class Circuit(string input)
    {
        public Circuit Override(string wire, int value) { index[wire] = new($"{value}", wire); return this; }

        public int Solve(Variant v)
        {
            if (v.IsNumeric) return v.IntValue;

            if (!index.TryGetValue(v.StringValue, out var comp)) throw new Exception("Unexpected wire");

            if (!comp.Resolved.HasValue) comp.Resolved = comp.Operator switch
            {
                Operator.NULL => Solve(comp.Input1),
                Operator.AND => Solve(comp.Input1) & Solve(comp.Input2),
                Operator.OR => Solve(comp.Input1) | Solve(comp.Input2),
                Operator.LSHIFT => Solve(comp.Input1) << Solve(comp.Input2),
                Operator.RSHIFT => Solve(comp.Input1) >> Solve(comp.Input2),
                Operator.NOT => 65535 - Solve(comp.Input1),
                _ => throw new NotImplementedException(),
            };

            return comp.Resolved.Value;
        }

        private readonly Dictionary<string, Component> index = Parser.Parse<Component>(input).ToDictionary(c => c.OutName.StringValue);
    }

    public static int Part1(string input) => new Circuit(input).Solve("a");

    public static int Part2(string input) => new Circuit(input).Override("b", 16076).Solve("a");

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}