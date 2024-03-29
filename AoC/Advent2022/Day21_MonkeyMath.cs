﻿namespace AoC.Advent2022;
public class Day21 : IPuzzle
{
    static readonly string HumanKey = "humn";
    static readonly string RootKey = "root";

    public class Monkey
    {
        [Regex("(....): (....) (.) (....)")]
        public Monkey(string name, string left, char op, string right) => (Name, _Left, _Right, Op) = (name, left, right, op);

        [Regex(@"(....): (\d+)")]
        public Monkey(string name, long value) => (Name, _Value) = (name, value);

        public readonly string Name;

        readonly string _Left = null, _Right = null;
        readonly char Op;

        Monkey Left, Right;

        public void ResolveChildren(Dictionary<string, Monkey> index) => (Left, Right) = (_Left != null) ? (index[_Left], index[_Right]) : (null, null);

        public static implicit operator long(Monkey m) => m._Value ??= m.Op switch
        {
            '+' => m.Left + m.Right,
            '*' => m.Left * m.Right,
            '-' => m.Left - m.Right,
            '/' => m.Left / m.Right,
            _ => throw new Exception("unexpected operator")
        };
        long? _Value = null;

        bool ContainsHuman => _ContainsHuman ??= (Name == HumanKey || (Left != null && (Left.ContainsHuman || Right.ContainsHuman)));
        bool? _ContainsHuman = null;

        public long CalculateHumanValue(long targetResult = 0)
        {
            if (Name == HumanKey) return targetResult;

            (Monkey humanSide, long resolvedBranch) = Left.ContainsHuman ? (Left, Right) : (Right, Left);

            return humanSide.CalculateHumanValue(((Name == RootKey) ? '=' : Op) switch
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

    private static Monkey GetRootMonkey(string input) => Memoize(input, _ =>
        Util.RegexParse<Monkey>(input)
            .ToDictionary(m => m.Name).Resolve(entry => entry.Value.ResolveChildren(entry.Collection))[RootKey]);

    public static long Part1(string input) => GetRootMonkey(input);

    public static long Part2(string input) => GetRootMonkey(input).CalculateHumanValue();

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}