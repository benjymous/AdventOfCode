namespace AoC.Advent2022;
public class Day21 : IPuzzle
{
    private static readonly string HumanKey = "humn", RootKey = "root";

    public class Tree
    {
        [Regex("(....): (....) (.) (....)")]
        public void MonkeyLink(string name, string left, char op, string right) => Get(name).Link(Get(left), op, Get(right));

        [Regex(@"(....): (\d+)")]
        public void MonkeyValue(string name, long value) => Get(name).Value = value;

        public static Monkey GetRootMonkey(string input) => Memoize(input, _ => Parser.Factory<Tree>(input).index[RootKey]);

        private readonly Dictionary<string, Monkey> index = [];
        private Monkey Get(string monkey) => index.GetOrCalculate(monkey, n => new(n));
    }

    public class Monkey(string name)
    {
        public char Op;
        public Monkey Left, Right;
        public long? Value = null;

        public void Link(Monkey left, char op, Monkey right) => (Left, Right, Op) = (left, right, op);

        public static implicit operator long(Monkey m)
        {
            return m.Value ??= m.Op switch
            {
                '+' => m.Left + m.Right,
                '*' => m.Left * m.Right,
                '-' => m.Left - m.Right,
                '/' => m.Left / m.Right,
                _ => throw new Exception("unexpected operator")
            };
        }

        private bool ContainsHuman => _ContainsHuman ??= (name == HumanKey || (Left != null && (Left.ContainsHuman || Right.ContainsHuman)));
        private bool? _ContainsHuman = null;

        public long CalculateHumanValue(long targetResult = 0)
        {
            if (name == HumanKey) return targetResult;

            (Monkey humanSide, long resolvedBranch) = Left.ContainsHuman ? (Left, Right) : (Right, Left);

            return humanSide.CalculateHumanValue(((name == RootKey) ? '=' : Op) switch
            {
                '=' => resolvedBranch,
                '+' => targetResult - resolvedBranch,
                '*' => targetResult / resolvedBranch,
                '-' => humanSide == Left ? targetResult + resolvedBranch : resolvedBranch - targetResult,
                '/' => humanSide == Left ? targetResult * resolvedBranch : resolvedBranch / targetResult,
                _ => throw new Exception("unexpected operator")
            });
        }
    }

    public static long Part1(string input) => Tree.GetRootMonkey(input);

    public static long Part2(string input) => Tree.GetRootMonkey(input).CalculateHumanValue();

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}