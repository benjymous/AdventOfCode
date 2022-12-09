using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day08 : IPuzzle
    {
        public string Name => "2022-08";

        static bool VisibleFromEdge(char[,] grid, (int x, int y) pos) 
            => VisibleFromEdge(grid.Row(pos.y), pos.x) || VisibleFromEdge(grid.Column(pos.x), pos.y);

        static bool VisibleFromEdge(IEnumerable<char> values, int index)
        {
            var row = values.ToArray();
            if (index == 0 || index == row.Length - 1) return true;
            var tree = row[index];

            var visibleLeft = row.Take(index).All(v => v < tree);
            var visibleRight = row.Skip(index + 1).All(v => v < tree);
            return visibleLeft || visibleRight;
        }

        static int CountVisible(IEnumerable<char> values, int compare)
        {
            int seen = 0;
            foreach (var v in values)
            {
                seen++;
                if (v >= compare) break;
            }
            return seen;
        }

        static int ScenicScore(char[,] grid, (int x, int y) pos)
        {
            var tree = grid[pos.x, pos.y];

            var row = grid.Row(pos.y);
            var col = grid.Column(pos.x);

            return CountVisible(row.Take(pos.x).Reverse(), tree)
                 * CountVisible(row.Skip(pos.x + 1), tree)
                 * CountVisible(col.Take(pos.y).Reverse(), tree)
                 * CountVisible(col.Skip(pos.y + 1), tree);
        }

        public static int Part1(string input)
        {
            var grid = Util.ParseMatrix<char>(input);

            return grid.Keys().Count(pos => VisibleFromEdge(grid, pos));
        }

        public static int Part2(string input)
        {
            var grid = Util.ParseMatrix<char>(input);

            return grid.Keys().Max(pos => ScenicScore(grid, pos));
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}