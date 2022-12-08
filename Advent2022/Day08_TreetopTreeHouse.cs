using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day08 : IPuzzle
    {
        public string Name => "2022-08";

        public static bool TreeVisible(IEnumerable<int> values, int index)
        {
            var row = values.ToArray();
            if (index == 0 || index == row.Length - 1) return true;
            var tree = row[index];

            var left = values.Take(index).ToArray();
            var right = values.Skip(index + 1).ToArray();

            var visibleLeft = values.Take(index).All(v => v < tree);
            var visibleRight = values.Skip(index + 1).All(v => v < tree);
            return visibleLeft || visibleRight;
        }

        public static int Part1(string input)
        {
            var grid = Util.ParseMatrix<int>(input);

            int count = 0;
            for (int y = 0; y < grid.Height(); ++y)
            {
                for (int x = 0; x < grid.Width(); ++x)
                {
                    if (TreeVisible(grid.Row(y), y) || TreeVisible(grid.Column(x), x))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, ILogger logger)
        {

            string test = @"30373
25512
65332
33549
35390".Replace("\r", "");

            Console.WriteLine(Part1(test));

            Console.WriteLine("- Pt1 - " + Part1(input));
            Console.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}