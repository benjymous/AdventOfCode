using AoC.Utils;
using AoC.Utils.Pathfinding;
using AoC.Utils.Vectors;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2016
{
    public class Day13 : IPuzzle
    {
        public string Name => "2016-13";

        static bool IsOpen(int x, int y, int seed)
        {
            int v = (seed) + (x * x) + (3 * x) + (2 * x * y) + (y) + (y * y);
            var b = v.BitSequence();
            var c = b.Count();
            return (c % 2 == 0);
        }

        public class CubicleMap : IMap<ManhattanVector2>
        {
            public Dictionary<string, bool> data = new Dictionary<string, bool>();

            int Seed = 0;

            public CubicleMap(int seed)
            {
                Seed = seed;
            }

            public virtual IEnumerable<ManhattanVector2> GetNeighbours(ManhattanVector2 center)
            {
                ManhattanVector2 pt;
                pt = new ManhattanVector2(center.X - 1, center.Y);
                if (IsValidNeighbour(pt))
                    yield return pt;

                pt = new ManhattanVector2(center.X + 1, center.Y);
                if (IsValidNeighbour(pt))
                    yield return pt;

                pt = new ManhattanVector2(center.X, center.Y + 1);
                if (IsValidNeighbour(pt))
                    yield return pt;

                pt = new ManhattanVector2(center.X, center.Y - 1);
                if (IsValidNeighbour(pt))
                    yield return pt;

            }

            public bool IsValidNeighbour(ManhattanVector2 pt)
            {
                if (pt.X < 0 || pt.Y < 0)
                {
                    return false;
                }

                lock (data)
                {
                    if (!data.TryGetValue(pt.ToString(), out var isOpen))
                    {
                        isOpen = IsOpen(pt.X, pt.Y, Seed);
                        data[pt.ToString()] = isOpen;
                    }

                    return isOpen;
                }
            }

            public int Heuristic(ManhattanVector2 location1, ManhattanVector2 location2)
            {
                return location1.Distance(location2);
            }
        }

        public static int Part1(string input)
        {
            int seed = int.Parse(input);
            var map = new CubicleMap(seed);
            var route = AStar<ManhattanVector2>.FindPath(map, new ManhattanVector2(1, 1), new ManhattanVector2(31, 39));
            return route.Count();
        }

        public static int Part2(string input)
        {
            int seed = int.Parse(input);
            var map = new CubicleMap(seed);

            var start = new ManhattanVector2(1, 1);
            var dest = new ManhattanVector2(0, 0);

            const int MaxDistance = 50;

            int count = 0;
            for (int y = 0; y < MaxDistance; ++y)
            {
                for (int x = 0; x < MaxDistance - y; ++x)
                {
                    if (!map.IsValidNeighbour(new ManhattanVector2(x, y)))
                    {
                        //Console.Write('#');
                        continue;
                    }
                    if (x == 1 && y == 1)
                    {
                        count++;
                        //Console.Write('o');
                    }
                    else
                    {
                        dest.Set(x, y);
                        var route = AStar<ManhattanVector2>.FindPath(map, start, dest);
                        if (route.Any())
                        {
                            if (route.Count() <= MaxDistance)
                            {
                                count++;
                                //Console.Write(" ");
                            }
                            // else
                            // {
                            //     Console.Write("-");
                            // }    
                        }
                        // else
                        // {
                        //     Console.Write("X");
                        // }                
                    }
                }
                //Console.WriteLine();
            }

            return count;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}