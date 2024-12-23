﻿namespace AoC.Advent2016;
public class Day13 : IPuzzle
{
    public class CubicleMap(string seed) : IMap<(int x, int y)>
    {
        readonly int Seed = int.Parse(seed);

        readonly (int dx, int dy)[] Neighbours = [(-1, 0), (1, 0), (0, 1), (0, -1)];

        public virtual IEnumerable<(int x, int y)> GetNeighbours((int x, int y) center) => Neighbours.Select(n => center.OffsetBy(n)).Where(IsValidNeighbour);

        bool IsValidNeighbour((int x, int y) pt) => pt.x >= 0 && pt.y >= 0 && Memoize(pt, _ => IsOpen(pt.x, pt.y));

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

        Queue<(int x, int y)> toTry = [(1, 1)];
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