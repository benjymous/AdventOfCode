using AoC.Utils;
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
            var data = Util.Parse<ManhattanVector2>(input).Select(v => v.AsSimple()).ToArray();

            var width = data.Max(pos => pos.x);
            var height = data.Max(pos => pos.y);

            var grid = new int[height, width];

            Dictionary<int, int> counts = new();

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
                        grid[y, x] = smallestIdx;
                        counts.IncrementAtIndex(smallestIdx);
                    }
                    else
                    {
                        grid[y, x] = -1;
                    }
                }
            }

            for (var x = 0; x < width; ++x)
            {
                counts.Remove(grid[0, x]);
                counts.Remove(grid[height - 1, x]);
            }

            for (var y = 0; y < height; ++y)
            {
                counts.Remove(grid[y, 0]);
                counts.Remove(grid[y, width - 1]);
            }

            return counts.Max(kvp => kvp.Value);

        }

        public static int Part2(string input, int safeDistance)
        {
            var data = Util.Parse<ManhattanVector2>(input).Select(v => v.AsSimple()).ToArray();

            var width = data.Max(pos => pos.x);
            var height = data.Max(pos => pos.y);

            return ParallelEnumerable.Range(0, height).Sum(y => Enumerable.Range(0, width).Where(x => data.Select(e => e.Distance(x, y)).Sum() < safeDistance).Count());
        }

        public static int Part2(string input) => Part2(input, 10000);

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}