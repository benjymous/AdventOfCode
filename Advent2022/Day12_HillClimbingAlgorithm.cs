using AoC.Utils;
using AoC.Utils.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day12 : IPuzzle
    {
        public string Name => "2022-12";

        public class MapData : IMap<(int x, int y)>
        {
            public MapData(string input)
            {
                Grid = Util.ParseSparseMatrix<char>(input);
                Index = Grid.Invert();

                Start = Index['S'].First();
                End = Index['E'].First();

                Grid[Start] = 'a';
                Grid[End] = 'z';
            }

            public readonly Dictionary<(int x, int y), char> Grid;
            public readonly Dictionary<char, IEnumerable<(int x, int y)>> Index;

            public readonly (int x, int y) Start, End;

            static readonly (int dx, int dy)[] neighbours = new[] { (1, 0), (-1, 0), (0, 1), (0, -1) };
            public virtual IEnumerable<(int x, int y)> GetNeighbours((int x, int y) center)
            {
                var maxClimb = Grid[center]+1;

                return neighbours.Select(delta => (center.x + delta.dx, center.y + delta.dy))
                                 .Where(pt => Grid.TryGetValue(pt, out var height) && height <= maxClimb);
            }

            public int Heuristic((int x, int y) location1, (int x, int y) location2) => location1.Distance(location2);
        }
    
        public static int Part1(string input)
        {
            var map = new MapData(input);

            return AStar<(int x, int y)>.FindPath(map, map.Start, map.End).Length;
        }

        public static int Part2(string input)
        {
            var map = new MapData(input);
            var allStarts = map.Index['a'];
            var goodStarts = allStarts.Where(pos => map.GetNeighbours(pos).Any(pos => map.Grid[pos] == 'b')).ToHashSet();
            allStarts.Except(goodStarts).ForEach(pos => map.Grid.Remove(pos));

            int min = int.MaxValue;
            goodStarts.Operate(pos =>
            {
                var route = AStar<(int x, int y)>.FindPath(map, pos, map.End);
                if (route.Any())
                {
                    int routeLength = route.Length;

                    min = Math.Min(min, routeLength);

                    foreach (var step in route)
                    {
                        routeLength--;
                        if (goodStarts.Contains(step))
                        {
                            map.Grid.Remove(step);
                            min = Math.Min(min, routeLength);
                            goodStarts.Remove(step);
                        }
                    }
                }
            });

            return min;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}