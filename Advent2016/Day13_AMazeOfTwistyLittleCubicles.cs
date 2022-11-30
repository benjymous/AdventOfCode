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

        public class CubicleMap : IMap<(int x, int y)>
        {
            readonly Dictionary<(int x, int y), bool> data = new();
            readonly int Seed = 0;

            public CubicleMap(int seed) => Seed = seed;

            public virtual IEnumerable<(int x, int y)> GetNeighbours((int x, int y) center)
            {
                (int x, int y) pt = (center.x - 1, center.y);
                if (IsValidNeighbour(pt))
                    yield return pt;

                pt = (center.x + 1, center.y);
                if (IsValidNeighbour(pt))
                    yield return pt;

                pt = (center.x, center.y + 1);
                if (IsValidNeighbour(pt))
                    yield return pt;

                pt = (center.x, center.y - 1);
                if (IsValidNeighbour(pt))
                    yield return pt;
            }

            public bool IsValidNeighbour((int x, int y) pt)
            {
                if (pt.x < 0 || pt.y < 0)
                {
                    return false;
                }

                if (!data.TryGetValue(pt, out var isOpen))
                {
                    isOpen = IsOpen(pt.x, pt.y);
                    data[pt] = isOpen;
                }

                return isOpen;
            }

            bool IsOpen(int x, int y)
            {
                int c = (Seed + (x * x) + (3 * x) + (2 * x * y) + (y) + (y * y)).BitSequence().Count();
                return (c % 2 == 0);
            }

            public int Heuristic((int x, int y) location1, (int x, int y) location2) => location1.Distance(location2);
        }

        public static int Part1(string input)
        {
            int seed = int.Parse(input);
            var map = new CubicleMap(seed);
            var route = AStar<(int x, int y)>.FindPath(map, (1, 1), (31, 39));
            return route.Count();
        }

        public static int Part2(string input)
        {
            int seed = int.Parse(input);
            var map = new CubicleMap(seed);

            const int MaxDistance = 50;

            int count = 0;
            for (int y = 0; y < MaxDistance; ++y)
            {
                for (int x = 0; x < MaxDistance - y; ++x)
                {
                    if (!map.IsValidNeighbour((x, y)))
                    {
                        continue;
                    }
                    var route = AStar<(int x, int y)>.FindPath(map, (1,1), (x, y));
                    if (route.Any() && route.Count() <= MaxDistance)
                    { 
                        count++;
                    }
                }
            }

            return count+1;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}