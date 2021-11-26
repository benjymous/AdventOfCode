using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoC.Utils;

namespace AoC.Advent2017
{
    public class Day14 : IPuzzle
    {
        public string Name { get { return "2017-14"; } }

        public static IEnumerable<IEnumerable<bool>> BitMatrix(string input)
        {
            for(int i=0; i<128; ++i)
            {
                var hash = Day10.KnotHash($"{input}-{i}");
                yield return hash.BitSequenceFromHex();
            }
        }

        public static int Part1(string input)
        {
            return BitMatrix(input).Sum(row => row.Count(i => i));
        }

        static void FloodFill(int x, int y, bool[][] matrix)
        {
            if ((x<0) || (y<0) || x>127 || y>127) return;
            if (matrix[y][x] == false) return;
            matrix[y][x] = false;
            FloodFill(x, y + 1, matrix);
            FloodFill(x, y - 1, matrix);
            FloodFill(x + 1, y, matrix);
            FloodFill(x - 1, y, matrix);
        }

        public static int Part2(string input)
        {
            var matrix = BitMatrix(input).Select(row => row.ToArray()).ToArray();

            int groups = 0;

            for (int y=0; y<128; ++y)
            {
                for (int x=0; x<128; ++x)
                {
                    if (matrix[y][x])
                    {
                        FloodFill(x, y, matrix);
                        groups++;
                    }
                }
            }

            return groups;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}