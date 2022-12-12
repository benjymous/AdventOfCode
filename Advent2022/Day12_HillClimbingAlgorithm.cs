using AoC.Utils;
using AoC.Utils.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

                Start = Grid.Where(kvp => kvp.Value == 'S').First().Key;
                End = Grid.Where(kvp => kvp.Value == 'E').First().Key;

                Grid[Start] = 'a';
                Grid[End] = 'z';
            }

            Dictionary<(int x, int y), char> Grid;

            public (int x, int y) Start;
            public (int x, int y) End;

            public virtual IEnumerable<(int x, int y)> GetNeighbours((int x, int y) center)
            {
                var current = Grid[center];
                (int x, int y) pt = (center.x - 1, center.y);
                if (IsValidNeighbour(current, pt))
                    yield return pt;

                pt = (center.x + 1, center.y);
                if (IsValidNeighbour(current, pt))
                    yield return pt;

                pt = (center.x, center.y + 1);
                if (IsValidNeighbour(current, pt))
                    yield return pt;

                pt = (center.x, center.y - 1);
                if (IsValidNeighbour(current, pt))
                    yield return pt;
            }

            public bool IsValidNeighbour(char current, (int x, int y) pt)
            {
                if (Grid.TryGetValue(pt, out var height))
                {
                    return height <= current + 1;
                }

                return false;
            }

            public int Heuristic((int x, int y) location1, (int x, int y) location2) => location1.Distance(location2);
        }
    



        public static int Part1(string input)
        {
            var map = new MapData(input);

            var route = AStar<(int x, int y)>.FindPath(map, map.Start, map.End);
            return route.Count();

        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}