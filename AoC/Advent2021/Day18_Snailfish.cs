namespace AoC.Advent2021;
public class Day18 : IPuzzle
{
    public class Val : ISummable<Val>
    {
        public int Value = 0;
        public Val first, second, parent, left, right;

        public static Val Parse(string data) => Parse(data.ToQueue());

        static Val Parse(Queue<char> data, Val parent = null)
        {
            var v = new Val { parent = parent };
            while (true)
            {
                var ch = data.Dequeue();
                switch (ch)
                {
                    case '[': v.first = Parse(data, v); continue;
                    case ',': v.second = Parse(data, v); continue;
                    case >= '0' and <= '9': v.Value = ch - '0'; break;
                }
                return v;
            }
        }

        public bool IsPair => first != null;
        public int Depth => parent == null ? 0 : parent.Depth + 1;
        public long Magnitude => IsPair ? (3 * first.Magnitude) + (2 * second.Magnitude) : Value;

        public Val Reduce() { while (TryExplode() || TrySplit()) ; return this; }

        IEnumerable<Val> Flatten() => IsPair ? first.Flatten().Concat(second.Flatten()) : [this];

        public bool TrySplit() => Split() || (IsPair && (first.TrySplit() || second.TrySplit()));
        bool Split()
        {
            if (IsPair || Value < 10) return false;
            (first, second) = (new Val { Value = Value / 2, parent = this }, new Val { Value = (Value / 2) + (Value % 2), parent = this });
            return true;
        }

        public bool TryExplode() => Explode() || (IsPair && (first.TryExplode() || second.TryExplode()));
        bool Explode()
        {
            if (parent == null)
            {
                var flat = Flatten().ToArray();
                for (int i = 0; i < flat.Length - 1; ++i)
                {
                    flat[i].right = flat[i + 1];
                    flat[i + 1].left = flat[i];
                }
            }
            if (IsPair)
            {
                if (first.Explode() || second.Explode()) return true;
                else if (Depth == 4)
                {
                    if (first.left != null) first.left.Value += first.Value;
                    if (second.right != null) second.right.Value += second.Value;
                    (first, second, Value) = (null, null, 0);
                    return true;
                }
            }
            return false;
        }

        public static Val operator +(Val lhs, Val rhs) => (lhs.parent = rhs.parent = new Val { first = lhs, second = rhs }).Reduce();
    }

    public static long Part1(string input) => Util.Split(input, "\n").Select(Val.Parse).Sum().Magnitude;

    public static long Part2(string input)
    {
        var lines = Util.Split(input, "\n");
        return Util.Matrix2(lines).AsParallel().Max(pair => (Val.Parse(pair.item1) + Val.Parse(pair.item2)).Magnitude);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}