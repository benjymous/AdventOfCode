using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2020
{
    public class Day17 : IPuzzle
    {
        public string Name => "2020-17";

        public class State
        {
            public State(int dimensions)
            {
                directions = Directions(dimensions).ToList();
                Dimensions = dimensions;
            }

            public State(string input, int dimensions)
            {
                directions = Directions(dimensions).ToList();
                Dimensions = dimensions;

                var lines = Util.Split(input);

                for (var y = 0; y < lines.Length; ++y)
                {
                    for (var x = 0; x < lines[y].Length; ++x)
                    {
                        Set((x, y, 0, 0), lines[y][x] == '#');
                    }
                }
            }

            public void Reset()
            {
                Cells.Clear();
                XRange.Reset();
                YRange.Reset();
                ZRange.Reset();
                WRange.Reset();
            }

            readonly int Dimensions;
            readonly Accumulator<long> XRange = new();
            readonly Accumulator<long> YRange = new();
            readonly Accumulator<long> ZRange = new();
            readonly Accumulator<long> WRange = new();
            public HashSet<(Int64 x, Int64 y, Int64 z, Int64 w)> Cells { get; private set; } = new HashSet<(Int64 x, Int64 y, Int64 z, Int64 w)>();

            readonly Int64[] BlankDim = new Int64[] { 0 };

            public IEnumerable<(Int64 x, Int64 y, Int64 z, Int64 w)> Range()
            {
                IEnumerable<Int64> wrange = Dimensions == 4 ? WRange.RangeBuffered(1) : BlankDim;
                foreach (var w in wrange)
                {
                    foreach (var z in ZRange.RangeBuffered(1))
                    {
                        foreach (var y in YRange.RangeBuffered(1))
                        {
                            foreach (var x in XRange.RangeBuffered(1))
                            {
                                yield return (x, y, z, w);
                            }
                        }
                    }
                }
            }

            public void Set((Int64 x, Int64 y, Int64 z, Int64 w) pos, bool v)
            {
                if (v)
                {
                    XRange.Add(pos.x);
                    YRange.Add(pos.y);
                    ZRange.Add(pos.z);
                    WRange.Add(pos.w);
                    Cells.Add(pos);
                }
            }

            public bool Get((Int64 x, Int64 y, Int64 z, Int64 w) pos)
                => Cells.Contains(pos);

            readonly IEnumerable<(Int64 x, Int64 y, Int64 z, Int64 w)> directions;

            public static IEnumerable<(Int64 x, Int64 y, Int64 z, Int64 w)> Directions(int dimensions)
            {
                int minw = dimensions == 4 ? -1 : 0;
                int maxw = dimensions == 4 ? 1 : 0;
                for (var w = minw; w <= maxw; ++w)
                {
                    for (var z = -1; z <= 1; ++z)
                    {
                        for (var y = -1; y <= 1; ++y)
                        {
                            for (var x = -1; x <= 1; ++x)
                            {
                                if (x == 0 && y == 0 && z == 0 && w == 0) continue;
                                yield return (x, y, z, w);
                            }
                        }
                    }
                }
            }

            public bool CheckDirection((Int64 x, Int64 y, Int64 z, Int64 w) pos, (Int64 x, Int64 y, Int64 z, Int64 w) dir) =>
                Get((pos.x + dir.x, pos.y + dir.y, pos.z + dir.z, pos.w + dir.w));

            public int Neighbours((Int64 x, Int64 y, Int64 z, Int64 w) pos) =>
                directions.Where(d => CheckDirection(pos, d)).Count();

            public void Tick(State oldState, (Int64 x, Int64 y, Int64 z, Int64 w) pos)
            {
                int neighbours = oldState.Neighbours(pos);
                Set(pos, oldState.Get(pos) ? (neighbours == 2 || neighbours == 3) : (neighbours == 3));
            }

            public void Display()
            {
                foreach (var z in ZRange.RangeInclusive())
                {
                    Console.WriteLine();
                    Console.WriteLine($"z={z}");
                    foreach (var y in YRange.RangeInclusive())
                    {
                        foreach (var x in XRange.RangeInclusive())
                        {
                            var v = Get((x, y, z, 0));
                            Console.Write(v ? '#' : '.');
                        }
                        Console.WriteLine();
                    }
                }
                Console.WriteLine();
            }
        }


        public static void Tick(State oldState, State newState)
        {
            newState.Reset();
            foreach (var pos in oldState.Range())
            {
                newState.Tick(oldState, pos);
            }
        }

        public static int Run(string input, int cycles, int dimensions)
        {
            State[] states = new State[2];
            states[0] = new State(input, dimensions);
            states[1] = new State(dimensions);

            //states[0].Display();

            int current = 0;
            while (cycles-- > 0)
            {
                var oldState = states[current];
                var newState = states[(current + 1) % 2];

                Tick(oldState, newState);
                //newState.Display();

                current = 1 - current;
            }

            return states[current].Cells.Count;
        }

        public static int Part1(string input)
        {
            return Run(input, 6, 3);
        }

        public static int Part2(string input)
        {
            return Run(input, 6, 4);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}