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

        }

        private static int RunRocktris(string input, ulong rounds)
        {
            var wind = WindSequence(input).GetEnumerator();
            wind.MoveNext();

            var shapes = Util.Split(shapeData, '|').Select(part => Util.ParseSparseMatrix<bool>(part).Keys.ToArray()).ToArray();

            int currentShape = 0;

            var state = new State();

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

                //if (i % 100000 == 0)
                //{
                //    Console.WriteLine($"{i} / {rounds}");
                //}
            }

            return state.MaxY;
        }

        public static int Part1(string input)
        {
            return RunRocktris(input, 2022);
        }

        public static long Part2(string input)
        {
            return RunRocktris(input, 1000000000000);
        }

        public void Run(string input, ILogger logger)
        {
            //Console.WriteLine(Part1(test));

            logger.WriteLine("- Pt1 - " + Part1(input));
            //logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}