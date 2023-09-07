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

                Cells = Util.ParseSparseMatrix<char>(input).Where(kvp => kvp.Value == '#').Select(kvp => (kvp.Key.x, kvp.Key.y, 0, 0)).ToHashSet();
            }

            public void Reset() => Cells.Clear();

            readonly int Dimensions;
            public HashSet<(int x, int y, int z, int w)> Cells { get; private set; } = new();

            public (int minx, int miny, int minz, int minw, int maxx, int maxy, int maxz, int maxw) Range()
            {
                var range = Cells.Aggregate((minx: 0, miny: 0, minz: 0, minw: 0, maxx: 0, maxy: 0, maxz: 0, maxw: 0), (curr, next) =>
                    (Math.Min(curr.minx, next.x), Math.Min(curr.miny, next.y), Math.Min(curr.minz, next.z), Math.Min(curr.minw, next.w),
                     Math.Max(curr.maxx, next.x), Math.Max(curr.maxy, next.y), Math.Max(curr.maxz, next.z), Math.Max(curr.maxw, next.w)));

                if (Dimensions == 4)
                {
                    range.minw--; range.maxw++;
                }
                range.minz--; range.maxz++;
                range.miny--; range.maxy++;
                range.minx--; range.maxx++;

               return range;
            }

            readonly IEnumerable<(int x, int y, int z, int w)> directions;

            public static IEnumerable<(int x, int y, int z, int w)> Directions(int dimensions)
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

            public bool CheckDirection((int x, int y, int z, int w) pos, (int x, int y, int z, int w) dir) =>
                Cells.Contains((pos.x + dir.x, pos.y + dir.y, pos.z + dir.z, pos.w + dir.w));

            public int Neighbours((int x, int y, int z, int w) pos) =>
                directions.Count(d => CheckDirection(pos, d));

            public void Tick(State oldState, (int x, int y, int z, int w) pos)
            {
                int neighbours = oldState.Neighbours(pos);
                if (oldState.Cells.Contains(pos) ? (neighbours == 2 || neighbours == 3) : (neighbours == 3)) Cells.Add(pos);
            }
        }


        static void Tick(State oldState, State newState)
        {
            newState.Reset();

            var (minx, miny, minz, minw, maxx, maxy, maxz, maxw) = oldState.Range();

            for (int w = minw; w <= maxw; ++w)
            {
                for (int z = minz; z <= maxz; ++z)
                {
                    for (int y = miny; y <= maxy; ++y)
                    {
                        for (int x = minx; x <= maxx; ++x)
                        {
                            newState.Tick(oldState, (x, y, z, w));
                        }
                    }
                }
            }
        }

        public static int Run(string input, int cycles, int dimensions)
        {
            State s1 = new (input, dimensions), s2 = new (dimensions);

            while (cycles-- > 0)
            {
                Tick(s1, s2);

                (s1, s2) = (s2, s1);
            }

            return s1.Cells.Count;
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