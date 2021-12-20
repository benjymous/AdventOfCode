using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day18 : IPuzzle
    {
        public string Name => "2021-18";

        public class Val
        {
            public bool isPair = false;
            public int Value = 0;
            public Val first, second, parent, left, right;

            public Val Clone(Val parent=null)
            {
                var v = new Val
                {
                    isPair = this.isPair,
                    Value = this.Value,
                    parent = parent
                };
                v.first = first != null ? first.Clone(v) : null;
                v.second = second != null ? second.Clone(v) : null;
                return v;
            }

            public int Depth => parent == null ? 0 : parent.Depth + 1;

            public void Reduce() { while (TryExplode() || TrySplit()); }

            public IEnumerable<Val> Flatten()
            {
                if (isPair)
                {
                    foreach (var val in first.Flatten()) yield return val;
                    foreach (var val in second.Flatten()) yield return val;
                }
                else
                {
                    yield return this;
                }
            }

            public bool TryExplode()
            {
                if (parent == null)
                {
                    var vals = Flatten().ToArray();
                    vals.First().left = null;
                    vals.Last().right = null;
                    foreach (var pair in vals.OverlappingPairs())
                    {
                        pair.second.left = pair.first;
                        pair.first.right = pair.second;
                    }
                }

                return Explode() || (isPair && (first.TryExplode() || second.TryExplode()));
            }

            public bool TrySplit()
            {
                return Split() || (isPair && (first.TrySplit() || second.TrySplit()));
            }

            public bool Split()
            {
                if (isPair || Value < 10) return false;

                int v1 = Value / 2;
                int v2 = v1;
                if (Value % 2 == 1) v2++;

                isPair = true;
                first = new Val { Value = v1, parent = this };
                second = new Val { Value = v2, parent = this };

                return true;
            }

            public bool Explode()
            {
                if (!isPair) return false;
                if (first.Explode() || second.Explode()) return true;
                if (Depth != 4) return false;

                if (first.left != null)  first.left.Value += first.Value;
                if (second.right != null) second.right.Value += second.Value;
         
                isPair = false;
                Value = 0;

                return true;
            }

            public long Magnitude => isPair ? 3 * first.Magnitude + 2 * second.Magnitude : Value;

            public override string ToString() => isPair ? $"[{first},{second}]" : Value.ToString();
        }

        public static Val Add(Val lhs, Val rhs)
        {
            var v = new Val
            {
                isPair = true,
                first = lhs,
                second = rhs,
            };
            lhs.parent = rhs.parent = v;

            v.Reduce();

            return v;
        }

        public static Val Parse(string data) => Parse(data.ToQueue());

        static Val Parse(Queue<char> data, Val parent = null)
        {
            var v = new Val { parent = parent };
            while (true)
            {
                var ch = data.Dequeue();
                switch (ch)
                {
                    case '[':
                        v.isPair = true;
                        v.first = Parse(data, v);
                        break;
                    case ',':
                        v.second = Parse(data, v);
                        break;
                    case ']':
                        return v;
                    case >= '0' and <= '9':
                        v.Value = ch - '0';
                        return v;
                }
            }
        }

        public static Val AddList(IEnumerable<Val> values) => values.Aggregate((lhs, rhs) => Add(lhs, rhs));

        public static long Part1(string input)
        {
            var numbers = Util.Split(input, '\n').Select(line => Parse(line));

            return AddList(numbers).Magnitude;
        }

        public static long Part2(string input)
        {
            var numbers = Util.Split(input, '\n').Select(line => Parse(line));

            return Util.Matrix(numbers, numbers).Max(pair => Add(pair.item1.Clone(), pair.item2.Clone()).Magnitude);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}