using AoC.Utils;
using AoC.Utils.Pathfinding;
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

            public CubicleMap(string seed) => Seed = int.Parse(seed);

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

            public bool IsValidNeighbour((int x, int y) pt) => pt.x >= 0 && pt.y >= 0 && data.GetOrCalculate(pt, _ => IsOpen(pt.x, pt.y));

            bool IsOpen(int x, int y) => (Seed + (x * x) + (3 * x) + (2 * x * y) + y + (y * y)).CountBits() % 2 == 0;

            public int Heuristic((int x, int y) location1, (int x, int y) location2) => location1.Distance(location2);
        }

        public static int Part1(string input)
        {
            var map = new CubicleMap(input);
            var route = map.FindPath((1, 1), (31, 39));
            return route.Length;
        }

        public static int Part2(string input)
        {
            var map = new CubicleMap(input);

            const int MaxDistance = 50;

            Queue<(int x, int y)> toTry = new() { (1, 1) };
            Dictionary<(int x, int y), int> distances = new() { { (1, 1), 0 } };
            toTry.Operate(pos =>
            {
                var dist = distances[pos];

                foreach (var n in map.GetNeighbours(pos))
                {
                    if (!distances.TryGetValue(n, out var nDist))
                    {
                        nDist = dist + 1;
                        if (nDist < MaxDistance)
                        {
                            distances[n] = nDist;
                            toTry.Add(n);
                        }
                    }
                    else
                    {
                        if (nDist > dist + 1)
                        {
                            distances[pos] = nDist - 1;
                            toTry.Add(n);
                        }
                    }

                }
            });

            return distances.Values.Count(v => v < MaxDistance) + 1;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}