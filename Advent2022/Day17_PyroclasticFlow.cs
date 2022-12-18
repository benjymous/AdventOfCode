using AoC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day17 : IPuzzle
    {
        public string Name => "2022-17";

        readonly static string shapeData = "####|.#.,###,.#.|###,..#,..#|#,#,#,#|##,##";

        static IEnumerable<int> WindSequence(string input) => input.Trim().Forever().Select(c => c == '<' ? -1 : 1);

        class State
        {
            public int MaxY = 0;
            public int LastMax = 0;
            Dictionary<int, byte> map = new() { [0] = 255 };

            public bool Blocked((int x, int y) pos)
            {
                return pos.y <= 0 || pos.x <= 0 || pos.x >= 8 || map.TryGetValue(pos.y, out var row) && ((row & 1 << pos.x) != 0);
            }

            public bool CheckBlocked(IEnumerable<(int x, int y)> shape, int dx, int dy)
            {
                foreach (var (x, y) in shape)
                {
                    if (Blocked((x + dx, y + dy)))
                    {
                        return true;
                    }
                }
                return false;
            }

            public void FinishBlock(IEnumerable<(int x, int y)> shape, (int x, int y) pos)
            {
                foreach (var (x, y) in shape.Select(p => (p.x + pos.x, p.y + pos.y)))
                {
                    if (!map.ContainsKey(y)) map[y] = 0;
                    MaxY = Math.Max(y, MaxY);
                    map[y] |= (byte)(1 << x);
                }
            }

            public void Cull()
            {
                if (MaxY % 100 == 0)
                {
                    map = map.Where(kvp => kvp.Key >= MaxY - 20).ToDictionary();
                }
            }

            public IEnumerable<byte> Checksum(int currentShape)
            {
                yield return (byte)currentShape;
                //yield return (byte)(MaxY - LastMax);
                LastMax = MaxY;
                for (int i = MaxY; i>0 && i > MaxY - 7; i--) 
                    yield return map[i];
            }
        }

        private static ulong RunRocktris(string input, ulong rounds)
        {
            var wind = WindSequence(input).GetEnumerator();
            wind.MoveNext();

            var shapes = Util.Split(shapeData, '|').Select(part => Util.ParseSparseMatrix<bool>(part).Keys.ToArray()).ToArray();

            int currentShape = 0;

            var state = new State();
            Dictionary<ulong, ulong> seen = new();
            Dictionary<int, int> cycles = new();
            Dictionary<ulong, int> counts = new();
            ulong extra = 0;

            for (ulong i = 0; i < rounds; ++i)
            {
                (int x, int y) rockPos = (3, 4 + state.MaxY);
                while (true)
                {
                    var shape = shapes[currentShape];
                    var dx = wind.Pop();
                    if (!state.CheckBlocked(shape, rockPos.x + dx, rockPos.y))
                    {
                        rockPos.x += dx;
                    }

                    if (state.CheckBlocked(shape, rockPos.x, rockPos.y - 1))
                    {
                        state.FinishBlock(shape, rockPos);
                        break;
                    }
                    rockPos.y--;
                }
                currentShape = (currentShape + 1) % 5;

                state.Cull();

                if (rounds > 2022)// && extra==0)
                {
                    counts[i] = state.MaxY;
                    var blah = state.Checksum(currentShape).ToArray();
                    if (blah.Length == 8)
                    {
                        var check = BitConverter.ToUInt64(blah, 0);
                        if (seen.TryGetValue(check, out ulong value))
                        {
                            int cycle = (int)(i - value);
                            //if (cycle > 1500)
                            {
                                //Console.WriteLine($"{i} => {value} -- {cycle}");
                                cycles.IncrementAtIndex(cycle);
                                var maxValueKey = cycles.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
                                if (cycles[maxValueKey] > 1000)
                                {
                                    Console.WriteLine($"  {maxValueKey} : {cycles[maxValueKey]}");

                                    var deltaScore = state.MaxY - counts[i -(ulong)maxValueKey];
                                    Console.WriteLine($"  => {deltaScore}");

                                    var remaining = Math.DivRem((rounds - i), (ulong)maxValueKey);

                                    rounds = i + remaining.Remainder;
                                    extra = (ulong)deltaScore * remaining.Quotient;

                                }
                            }
                        }
                        seen[check] = i;
                    }
                }
            }

            return (ulong)state.MaxY + extra;
        }

        public static int Part1(string input)
        {
            return (int)RunRocktris(input, 2022);
        }

        public static ulong Part2(string input)
        {
            return RunRocktris(input, 1000000000000);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
// 1500874635626 << high
// 1500874635588 << high
// 1511111111112 << high
