using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2019
{
    public class Day24 : IPuzzle
    {
        public string Name { get { return "2019-24"; } }

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

            public void Clear()
            {
                foreach (var level in cells.Keys.ToList())
                {
                    cells[level] = 0;
                }
            }

            public int Count()
            {
                int count = 0;
                foreach (var cell in cells)
                {
                    count += cell.Value.BitSequence().Count();
                }
                return count;
            }

            public Dictionary<int, int> cells = new Dictionary<int, int>();

            public static int Bit(int x, int y) => 1 << ((y * 5) + x);

            public void Set(int x, int y, int level = 0)
            {
                if (!cells.ContainsKey(level)) cells[level] = 0;
                cells[level] |= Bit(x, y);
            }

            public int Get(int x, int y, int level = 0)
            {
                if (!cells.ContainsKey(level)) return 0;
                if (x < 0 || y < 0 || x >= 5 || y >= 5)
                {
                    return 0;
                }
                else return ((cells[level] & Bit(x, y)) > 0) ? 1 : 0;
            }

            IEnumerable<(int x, int y, int level)> GetNeighboursFlat(int x, int y, int level)
            {
                yield return (x - 1, y, level);
                yield return (x + 1, y, level);
                yield return (x, y - 1, level);
                yield return (x, y + 1, level);
            }

            public IEnumerable<(int x, int y, int level)> GetNeighbours(int x, int y, int level)
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

            public int Neighbours(int x, int y, int level = 0) => GetNeighbours(x, y, level).Select(n => Get(n.x, n.y, n.level)).Sum();

            public void Tick(State oldState, int x, int y, int level)
            {
                int neighbours = oldState.Neighbours(x, y, level);
                if (oldState.Get(x, y, level) == 1)
                {
                    //Console.Write($"#{neighbours} ");
                    // bug at cell
                    if (neighbours == 1)
                    {
                        Set(x, y, level);
                    }
                }
                else
                {
                    // no bug at cell
                    //Console.Write($".{neighbours} ");
                    if (neighbours == 1 || neighbours == 2)
                    {
                        Set(x, y, level);
                    }
                }
            }

            public void Display()
            {
                var sb = new StringBuilder();
                foreach (var kvp in cells.OrderBy(kvp => kvp.Key))
                {
                    Console.WriteLine($"{kvp.Key} - {kvp.Value}");
                    for (var y = 0; y < 5; ++y)
                    {
                        for (var x = 0; x < 5; ++x)
                        {
                            Console.Write((Get(x, y, kvp.Key) == 1) ? '#' : '.');
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("--------");
            }
        }


        public static void Tick1(State oldState, State newState)
        {
            newState.Clear();
            for (var y = 0; y < 5; ++y)
            {
                for (var x = 0; x < 5; ++x)
                {
                    newState.Tick(oldState, x, y, 0);
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


        public static int Part1(string input)
        {
            HashSet<int> seen = new HashSet<int>();

            Queue<State> states = new Queue<State>();
            states.Enqueue(new State(input));
            states.Enqueue(new State());

            while (true)
            {
                var oldState = states.Dequeue();
                var newState = states.Dequeue();

                if (seen.Contains(oldState.cells[0]))
                {
                    return oldState.cells[0];
                }
                seen.Add(oldState.cells[0]);
                Tick1(oldState, newState);

                states.Enqueue(newState);
                states.Enqueue(oldState);
            }
        }



        public static int Part2(string input, int runs = 200)
        {
            Queue<State> states = new Queue<State>();
            states.Enqueue(new State(input) { Infinite = true });
            states.Enqueue(new State() { Infinite = true });

            for (var i = 0; i < runs; ++i)
            {
                var oldState = states.Dequeue();
                var newState = states.Dequeue();

                //Console.WriteLine($"-------[{i}]--------");

                //oldState.Display();

                Tick2(oldState, newState);

                //Console.WriteLine(newState.Count());

                states.Enqueue(newState);
                states.Enqueue(oldState);
            }

            //var s = states.Dequeue();
            //s.Display();
            //return s.Count();
            return states.Dequeue().Count();
        }

        public void Run(string input, ILogger logger)
        {

            //logger.WriteLine(Part2("....#\n#..#.\n#..##\n..#..\n#....", 10));

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input, 200));
        }
    }
}