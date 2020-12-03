using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXX
{
    public class Day03 : IPuzzle
    {
        public string Name { get { return "2020-03"; } }

        class MapRow
        {
            public MapRow(string row)
            {
                Row = row;
            }

            string Row;

            public char Get(int x)
            {
                return Row[x % Row.Length];
            }
        }

        private static int CountTrees(MapRow[] map, int dx, int dy)
        {
            int x = 0;
            int y = 0;
            int treeCount = 0;
            while (y < map.Count())
            {
                var cell = map[y].Get(x);
                if (cell == '#') treeCount++;
                x += dx;
                y += dy;
            }
            return treeCount;
        }

        public static int Part1(string input)
        {
            var map = Util.Parse<MapRow>(input).ToArray();
            return CountTrees(map, 3, 1);
        }

        public static Int64 Part2(string input)
        {
            var map = Util.Parse<MapRow>(input).ToArray();
            var results = new List<Int64>();

            //Right 1, down 1.
            results.Add(CountTrees(map, 1, 1));
            //Right 3, down 1. (This is the slope you already checked.)
            results.Add(CountTrees(map, 3, 1));
            //Right 5, down 1.
            results.Add(CountTrees(map, 5, 1));
            //Right 7, down 1.
            results.Add(CountTrees(map, 7, 1));
            //Right 1, down 2.
            results.Add(CountTrees(map, 1, 2));

            return results.Aggregate((total, next) => total * next);
            

            Int64 prod = 1;
            foreach (int value in results)
            {
                prod *= value;
            }
            return prod;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}