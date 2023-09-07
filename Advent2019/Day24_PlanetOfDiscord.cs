using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2019
{
    public class Day24 : IPuzzle
    {
        public string Name => "2019-24";

        public class State
        {
            public State()
            {
                cells[0] = 0;
            }

            public bool Infinite = false;

            public IEnumerable<int> Keys() => cells.Keys.ToList();

            public State(string input)
            {
                cells[0] = 0;

                var lines = Util.Split(input);

                for (var y = 0; y < lines.Length; ++y)
                {
                    for (var x = 0; x < lines[y].Length; ++x)
                    {
                        if (lines[y][x] == '#')
                        {
                            Set(x, y);
                        }
                    }
                }
            }

            public void Clear() => cells.Clear();

            public int Count() => cells.Sum(level => level.Value.CountBits());

            public Dictionary<int, uint> cells = new();

            public static uint Bit(int x, int y) => 1U << ((y * 5) + x);

            public void Set(int x, int y, int level = 0)
            {
                if (cells.ContainsKey(level)) cells[level] |= Bit(x, y);
                else cells[level] = Bit(x, y);
            }

            public bool Get(int x, int y, int level = 0) => cells.ContainsKey(level) && x >= 0 && y >= 0 && x < 5 && y < 5 && (cells[level] & Bit(x, y)) > 0;

            static IEnumerable<(int x, int y, int level)> GetNeighboursFlat(int x, int y, int level)
            {
                yield return (x - 1, y, level);
                yield return (x + 1, y, level);
                yield return (x, y - 1, level);
                yield return (x, y + 1, level);
            }

            readonly Dictionary<long, (int, int, int)[]> neighbourCache = new();
            public IEnumerable<(int x, int y, int level)> GetNeighbours(int x, int y, int level) => neighbourCache.GetOrCalculate(x + (y << 3) + (level << 6), _ => GetNeighbours_(x, y, level).ToArray());

            private IEnumerable<(int x, int y, int level)> GetNeighbours_(int x, int y, int level)
            {
                if (Infinite)
                {
                    var values = GetNeighboursFlat(x, y, level);

                    foreach (var n in values)
                    {
                        if (n.x == 2 && n.y == 2)
                        {
                            // Centre cell, need to recurse in
                            if (x == 1)
                            {
                                // ->
                                yield return (0, 0, level + 1);
                                yield return (0, 1, level + 1);
                                yield return (0, 2, level + 1);
                                yield return (0, 3, level + 1);
                                yield return (0, 4, level + 1);
                            }
                            else if (x == 3)
                            {
                                // <-
                                yield return (4, 0, level + 1);
                                yield return (4, 1, level + 1);
                                yield return (4, 2, level + 1);
                                yield return (4, 3, level + 1);
                                yield return (4, 4, level + 1);
                            }
                            else if (y == 1)
                            {
                                // V
                                yield return (0, 0, level + 1);
                                yield return (1, 0, level + 1);
                                yield return (2, 0, level + 1);
                                yield return (3, 0, level + 1);
                                yield return (4, 0, level + 1);
                            }
                            else if (y == 3)
                            {
                                // ^
                                yield return (0, 4, level + 1);
                                yield return (1, 4, level + 1);
                                yield return (2, 4, level + 1);
                                yield return (3, 4, level + 1);
                                yield return (4, 4, level + 1);
                            }
                        }
                        else if (n.x == -1)
                        {
                            // left edge
                            yield return (1, 2, level - 1);
                        }
                        else if (n.x == 5)
                        {
                            // right edge
                            yield return (3, 2, level - 1);
                        }
                        else if (n.y == -1)
                        {
                            // top edge
                            yield return (2, 1, level - 1);
                        }
                        else if (n.y == 5)
                        {
                            // bottom edge
                            yield return (2, 3, level - 1);
                        }
                        else
                        {
                            yield return n;
                        }
                    }
                }
                else
                {
                    foreach (var n in GetNeighboursFlat(x, y, level))
                    {
                        yield return n;
                    }
                }
            }

            public int Neighbours(int x, int y, int level = 0) => GetNeighbours(x, y, level).Count(n => Get(n.x, n.y, n.level));

            public void Tick(State oldState, int x, int y, int level = 0)
            {
                int neighbours = oldState.Neighbours(x, y, level);

                if (neighbours == 1 || neighbours == 2 && !oldState.Get(x, y, level))
                {
                    Set(x, y, level);
                }
            }
        }

        public static void Tick1(State oldState, State newState)
        {
            newState.Clear();
            for (var y = 0; y < 5; ++y)
            {
                for (var x = 0; x < 5; ++x)
                {
                    newState.Tick(oldState, x, y);
                }
            }
        }

        public static void Tick2(State oldState, State newState)
        {
            newState.Clear();

            var keys = oldState.Keys();

            int min = keys.Min() - 1;
            int max = keys.Max() + 1;

            for (var level = min; level <= max; ++level)
            {
                for (var y = 0; y < 5; ++y)
                {
                    for (var x = 0; x < 5; ++x)
                    {
                        if (x != 2 || y != 2) newState.Tick(oldState, x, y, level);
                    }
                }
            }
        }

        public static uint Part1(string input)
        {
            HashSet<uint> seen = new();

            State s1 = new (input), s2 = new ();

            while (true)
            {
                if (seen.Contains(s1.cells[0])) return s1.cells[0];
                seen.Add(s1.cells[0]);
                Tick1(s1, s2);

                (s1, s2) = (s2, s1);
            }
        }

        public static int Part2(string input, int runs = 200)
        {
            State s1 = new(input) { Infinite = true }, s2 = new() { Infinite = true };

            for (var i = 0; i < runs; ++i)
            {
                Tick2(s1, s2);

                (s1, s2) = (s2, s1);
            }

            return s1.Count();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input, 200));
        }
    }
}