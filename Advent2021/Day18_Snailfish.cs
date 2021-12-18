using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2021
{
    public class Day18 : IPuzzle
    {
        public string Name => "2021-18";

        public class Val
        {
            public bool isPair = false;
            public int Value = 0;
            public Val first = null;
            public Val second = null;

            public Val parent = null;
            public Val left = null;
            public Val right = null;

            public int Depth
            {
                get
                {
                    if (parent == null) return 0;
                    return parent.Depth + 1;
                }
            }

            public void Reduce()
            {
                while (true)
                {
                    //Console.WriteLine($".. {this}");
                    bool exploded = TryExplode();
                    if (exploded) continue;

                    bool split = TrySplit();
                    if (split) continue;

                    return;
                }
            }

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
                    var vals = Flatten();
                    vals.First().left = null;
                    vals.Last().right = null;
                    foreach (var pair in vals.Windows(2))
                    {
                        var v1 = pair.First();
                        var v2 = pair.Last();

                        v2.left = v1;
                        v1.right = v2;
                    }
                }

                if (Explode())
                {
                    return true;
                }

                if (isPair)
                {
                    if (first.TryExplode()) return true;
                    if (second.TryExplode()) return true;
                }
                return false;
            }

            public bool TrySplit()
            {
                if (Split())
                {
                    return true;
                }

                if (isPair)
                {
                    if (first.TrySplit()) return true;
                    if (second.TrySplit()) return true;
                }
                return false;
            }

            public bool Split()
            {
                if (isPair) return false;
                if (Value < 10) return false;

                //Console.WriteLine($"Splitting {this}");

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

                if (first.isPair && first.Explode()) return true;
                if (second.isPair && second.Explode()) return true;

                if (Depth != 4) return false;

                var left = first.left;
                if (left!=null)
                {
                    left.Value += first.Value;
                }

                var right = second.right;
                if (right!=null)
                {
                    right.Value += second.Value;
                }

                isPair = false;
                Value = 0;

                return true;
            }

            public long Magnitude => isPair ? 3 * first.Magnitude + 2 * second.Magnitude : Value;


            public override string ToString()
            {
                if (isPair)
                {
                    return $"[{first},{second}]";
                }
                else
                {
                    return Value.ToString();
                }
            }
        }

        public static Val Add(Val lhs, Val rhs)
        {
            var v = new Val
            {
                isPair = true,
                first = lhs,
                second = rhs,
            };
            lhs.parent = v;
            rhs.parent = v;

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
                        {
                            v.Value = ch - '0';
                            return v;
                        }
                }
            }
        }

        public static Val AddList(IEnumerable<string> values)
        {
            var first = values.First();
            var number = Parse(first);

            foreach (var next in values.Skip(1))
            {
                var v = Parse(next);
                number = Add(number, v);
            }

            return number;
        }

        public static long Part1(string input)
        {
            var numbers = Util.Split(input, '\n');

            return AddList(numbers).Magnitude;
        }

        public static long Part2(string input)
        {
            var numbers = Util.Split(input, '\n');

            long max = 0;

            foreach (var first in numbers)
            {
                foreach (var second in numbers)
                {
                    if (first != second)
                    {
                        var sum = AddList(new string[] { first, second });
                        var mag = sum.Magnitude;
                        max = Math.Max(mag, max);
                    }
                }
            }
            return max;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}