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

        static int ToKey((int x, int y) v) => v.x + (v.y << 16);

        public class MapData : IMap<int>
        {
            public MapData(string input)
            {
                Grid = Util.ParseSparseMatrix<char>(input).ToDictionary(kvp => ToKey(kvp.Key), kvp => kvp.Value);
                Index = Grid.Invert();

                Start = Index['S'].First();
                End = Index['E'].First();

                Grid[Start] = 'a';
                Grid[End] = 'z';
            }

            public readonly Dictionary<int, char> Grid;
            public readonly Dictionary<char, IEnumerable<int>> Index;

            public readonly int Start, End;

            static readonly int[] neighbours = new[] { 1, -1, 1<<16, -1<<16 };
            public virtual IEnumerable<int> GetNeighbours(int center)
            {
                var maxClimb = Grid[center]+1;

                return neighbours.Select(delta => center + delta)
                                 .Where(pt => Grid.TryGetValue(pt, out var height) && height <= maxClimb);
            }
        }
    
        public static int Part1(string input)
        {
            var map = new MapData(input);

            return map.FindPath(map.Start, map.End).Length;
        }

        public static int Part2(string input)
        {
            var map = new MapData(input);
            var allStarts = map.Index['a'];
            var goodStarts = allStarts.Where(pos => map.GetNeighbours(pos).Any(pos => map.Grid[pos] == 'b')).ToHashSet();
            allStarts.Except(goodStarts).ForEach(pos => map.Grid.Remove(pos));
            HashSet<int> seen = new();

            int min = int.MaxValue;
            goodStarts.Operate(pos =>
            {
                var route = map.FindPath(pos, map.End);
                if (!route.Any()) return;

                int routeLength = route.Length;
                min = Math.Min(min, routeLength);

                var intersect = route.Reverse().Intersect(goodStarts);
                if (intersect.Any())
                {
                    min = Math.Min(min, routeLength - route.IndexOf(intersect.First()));
                    goodStarts.ExceptWith(intersect);
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