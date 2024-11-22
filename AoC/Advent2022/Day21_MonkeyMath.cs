namespace AoC.Advent2022;
public class Day21 : IPuzzle
{
    static readonly string HumanKey = "humn", RootKey = "root";

    public class Factory(string input)
    {
        public Dictionary<string, Monkey> index = [];
        Monkey Get(string monkey) => index.GetOrCalculate(monkey, n => new(n));

        [Regex("(....): (....) (.) (....)")]
        public Monkey MonkeyLink(string name, string left, char op, string right)
        {
            var monkey = Get(name);
            monkey.Left = Get(left);
            monkey.Right = Get(right);
            monkey.Op = op;

            return monkey;
        }

        [Regex(@"(....): (\d+)")]
        public Monkey MonkeyValue(string name, long value)
        {
            var monkey = Get(name);
            monkey.Value = value;
            return monkey;
        }

        public Monkey GetRootMonkey()
        {
            Util.RegexFactory(input, this);
            return index[RootKey];
        }
    }

    public class Monkey(string name)
    {
        public char Op;
        public Monkey Left, Right;
        public long? Value = null;

        public static implicit operator long(Monkey m) => m.Value ??= m.Op switch
        {
            '+' => m.Left + m.Right,
            '*' => m.Left * m.Right,
            '-' => m.Left - m.Right,
            '/' => m.Left / m.Right,
            _ => throw new Exception("unexpected operator")
        };

        bool ContainsHuman => _ContainsHuman ??= (name == HumanKey || (Left != null && (Left.ContainsHuman || Right.ContainsHuman)));
        bool? _ContainsHuman = null;

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

    private static Monkey GetRootMonkey(string input) => Memoize(input, _ => new Factory(input).GetRootMonkey());

    public static long Part1(string input) => GetRootMonkey(input);

    public static long Part2(string input) => GetRootMonkey(input).CalculateHumanValue();

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}