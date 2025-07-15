namespace AoC.Advent2021;
public class Day18 : IPuzzle
{
    public class Val : ISummable<Val>
    {
        public int Value = 0;
        public Val first, second, parent, left, right;

        [Regex(".+")]
        public Val(string s) : this([.. s]) { }

        protected Val(Queue<char> data, Val parent = null)
        {
            this.parent = parent;
            while (true)
            {
                var ch = data.Dequeue();
                switch (ch)
                {
                    case '[': first = new Val(data, this); continue;
                    case ',': second = new Val(data, this); continue;
                    case >= '0' and <= '9': this.Value = ch.AsDigit(); break;
                }
                break;
            }
        }

        public Val() { }

        public bool IsPair => first != null;
        public int Depth => parent == null ? 0 : parent.Depth + 1;
        public long Magnitude => IsPair ? (3 * first.Magnitude) + (2 * second.Magnitude) : Value;

        public Val Reduce() { while (TryExplode() || TrySplit()) ; return this; }

        private IEnumerable<Val> Flatten() => IsPair ? first.Flatten().Concat(second.Flatten()) : [this];

        public bool TrySplit() => Split() || (IsPair && (first.TrySplit() || second.TrySplit()));
        private bool Split()
        {
            if (IsPair || Value < 10) return false;
            (first, second) = (new Val { Value = Value / 2, parent = this }, new Val { Value = (Value / 2) + (Value % 2), parent = this });
            return true;
        }

        public bool TryExplode() => Explode() || (IsPair && (first.TryExplode() || second.TryExplode()));
        private bool Explode()
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

    public static long Part1(Parser.AutoArray<Val> input) => input.Sum().Magnitude;

    public static long Part2(string input) => Util.Matrix2(Util.Split(input, "\n")).AsParallel().Max(pair => (new Val(pair.item1) + new Val(pair.item2)).Magnitude);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}