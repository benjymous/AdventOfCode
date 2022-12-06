using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day06 : IPuzzle
    {
        public string Name => "2018-06";

        public static int Part1(string input)
        {
            var data = Util.Parse<ManhattanVector2>(input).ToArray();

            var width = data.Max(pos => pos.X);
            var height = data.Max(pos => pos.Y);

            var grid = new int[height,width];

            for (var y = 0; y < height; ++y)
            {
                for (var x = 0; x < width; ++x)
                {
                    var distances = new List<int>();
                    var smallest = width * height;
                    var smallestIdx = -1;

                    for (var i = 0; i < data.Length; ++i)
                    {
                        var entry = data[i];

                        distances.Add(entry.Distance(x, y));
                        if (distances[i] < smallest)
                        {
                            smallest = distances[i];
                            smallestIdx = i;
                        }
                    }

                    var smallestCount = 0;
                    for (var i = 0; i < data.Length; ++i)
                    {
                        if (distances[i] == smallest)
                        {
                            smallestCount++;
                        }
                    }

                    if (smallestCount == 1)
                    {
                        grid[y,x] = smallestIdx;
                    }
                    else
                    {
                        grid[y,x] = -1;
                    }
                }
            }

            var infinite = new HashSet<int>();

            for (var x = 0; x < width; ++x)
            {
                infinite.Add(grid[0, x]);
                infinite.Add(grid[height - 1,x]);
            }

            for (var y = 0; y < height; ++y)
            {
                infinite.Add(grid[y, 0]);
                infinite.Add(grid[y, width - 1]);
            }

            var maxArea = 0;
            for (var i = 0; i < data.Length; ++i)
            {
                var count = 0;

                if (!infinite.Contains(i))
                {
                    for (var y = 0; y < height; ++y)
                    {
                        for (var x = 0; x < width; ++x)
                        {
                            if (grid[y,x] == i)
                            {
                                count++;
                            }
                        }
                    }
                }

                maxArea = Math.Max(maxArea, count);
            }

            return maxArea;
        }

        public static int Part2(string input, int safeDistance)
        {
            var data = Util.Parse<ManhattanVector2>(input);

            var width = data.Max(pos => pos.X);
            var height = data.Max(pos => pos.Y);

            return ParallelEnumerable.Range(0, height).Sum(y => ParallelEnumerable.Range(0, width).Where(x => data.Select(e => e.Distance(x, y)).Sum() < safeDistance).Count());
        }

        public static int Part2(string input) => Part2(input, 10000);

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}