using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2020
{
    public static class MapRowExtension
    {
        const char Tree = '#';
        public static bool IsTree(this string row, int x) => row[x % row.Length] == Tree;
    }

    public class Day03 : IPuzzle
    {
        public string Name { get { return "2020-03"; } }

        private static int CountTrees(string[] map, int dx, int dy)
        {
            int treeCount = 0;
            for (int x = 0, y = 0; y < map.Count(); y += dy, x += dx)
            {
                if (map[y].IsTree(x)) treeCount++;
            }
            return treeCount;
        }

        public static Int64 Part1(string input)
        {
            var map = Util.Split(input).ToArray();

            return CountTrees(map, 3, 1);
        }

        public static Int64 Part2(string input)
        {
            var map = Util.Split(input).ToArray();

            return new List<(int dx, int dy)> {
                ( 1, 1 ),   //Right 1, down 1.
                ( 3, 1 ),   //Right 3, down 1. (This is the slope you already checked.)
                ( 5, 1 ),   //Right 5, down 1.
                ( 7, 1 ),   //Right 7, down 1.
                ( 1, 2 )    //Right 1, down 2.
            }.Select(dir => CountTrees(map, dir.dx, dir.dy))
             .Product();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}