namespace AoC.Advent2018;
public class Day12 : IPuzzle
{
    struct State
    {
        public State(HashSet<int> rules, int[] initial)
        {
            Rules = rules;
            for (int i = 0; i < initial.Length; i++) if (initial[i] == 1) Set(i);
        }
        public State(HashSet<int> rules) => Rules = rules;

        public readonly HashSet<int> Rules;
        readonly HashSet<int> data = [];
        public int Left { get; private set; } = 0;
        public int Right { get; private set; } = 0;

        public void Set(int index)
        {
            if (data.Count == 0) Left = index;
            Right = index;
            data.Add(index - Left);
        }

        public readonly int Slice(int pos)
        {
            int val = 0;
            for (int j = 0; j < 5; ++j) val = (val << 1) + (data.Contains(pos + j - Left) ? 1 : 0);
            return val;
        }

        public readonly void Step(ref State next)
        {
            next.data.Clear();

            for (int i = Left - 2; i < Right + 2; ++i) if (Rules.Contains(Slice(i - 2))) next.Set(i);
        }

        public readonly bool Equivalent(State other) => data.SequenceEqual(other.data);

        public readonly long Score(long offset = 0) => data.Sum() + ((Left + offset) * data.Count);
    }

    static int DecodeRule(string rule) => rule.Aggregate(0, (val, ch) => (val << 1) + (ch == '#' ? 1 : 0));

    private static void ParseInput(string input, out State initialState)
    {
        var lines = Util.Split(input);
        var rules = lines.Skip(1).Select(line => line.Split(" => ")).Where(bits => bits[1][0] == '#').Select(bits => DecodeRule(bits[0])).ToHashSet();
        initialState = new(rules, lines[0].Split(": ")[1].Select(ch => ch == '#' ? 1 : 0).ToArray());
    }

    public static long Solve(string input, long generations)
    {
        ParseInput(input, out var s1);
        State s2 = new(s1.Rules);

        long gen;
        for (gen = 0; gen < generations; ++gen)
        {
            if (s1.Equivalent(s2)) break; // We've got a stable pattern
            s1.Step(ref s2);
            (s1, s2) = (s2, s1);
        }

        long additionalLeftShift = (generations - gen) * (s1.Left - s2.Left); // we'd progress this many cells over the remaining generations
        return s1.Score(additionalLeftShift);
    }

    public static int Part1(string input) => (int)Solve(input, 20);

    public static long Part2(string input) => Solve(input, 50000000000);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}