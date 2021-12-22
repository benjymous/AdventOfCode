using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day18 : IPuzzle
    {
        public string Name => "2021-18";

        public class Val
        {
            public int Value = 0;
            public Val first, second, parent, left, right;

            public static Val Parse(Queue<char> data, Val parent = null)
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

            public Val Clone(Val parent = null)
            {
                var v = new Val { Value = Value, parent = parent };
                (v.first, v.second) = (first?.Clone(v), second?.Clone(v));
                return v;
            }

            public bool IsPair => first != null;
            public int Depth => parent == null ? 0 : parent.Depth + 1;
            public long Magnitude => IsPair ? 3 * first.Magnitude + 2 * second.Magnitude : Value;

            public Val Reduce() { while (TryExplode() || TrySplit()) ; return this; }

            IEnumerable<Val> Flatten() => IsPair ? first.Flatten().Concat(second.Flatten()) : (new[] { this });

            public bool TrySplit() => Split() || (IsPair && (first.TrySplit() || second.TrySplit()));
            bool Split()
            {
                if (IsPair || Value < 10) return false;
                (first, second) = (new Val { Value = Value / 2, parent = this }, new Val { Value = Value / 2 + Value % 2, parent = this });
                return true;
            }

            public bool TryExplode() => Explode() || (IsPair && (first.TryExplode() || second.TryExplode()));
            bool Explode()
            {
                if (parent == null) Flatten().ToArray().OverlappingPairs().ForEach(pair => (pair.second.left, pair.first.right) = pair);
                if (IsPair)
                {
                    if (first.Explode() || second.Explode()) return true;
                    if (Depth == 4)
                    {
                        if (first.left != null) first.left.Value += first.Value;
                        if (second.right != null) second.right.Value += second.Value;
                        (first, second, Value) = (null, null, 0);
                        return true;
                    }
                }
                return false;
            }

            public static Val Add(Val lhs, Val rhs)
            {
                var v = lhs.parent = rhs.parent = new Val { first = lhs, second = rhs };
                return v.Reduce();
            }
        }

        public static long Part1(string input)
        {
            return Util.Split(input, '\n').Select(line => Val.Parse(line.ToQueue())).Aggregate((lhs, rhs) => Val.Add(lhs, rhs)).Magnitude;
        }

        public static long Part2(string input)
        {
            var numbers = Util.Split(input, '\n').Select(line => Val.Parse(line.ToQueue()));
            return Util.Matrix(numbers, numbers).Max(pair => Val.Add(pair.item1.Clone(), pair.item2.Clone()).Magnitude);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}