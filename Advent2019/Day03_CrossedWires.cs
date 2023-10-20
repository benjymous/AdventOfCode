using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2019
{
    public class Day03 : IPuzzle
    {
        public string Name => "2019-03";

        public enum SearchMode
        {
            Closest,
            Shortest
        }

        static IEnumerable<IEnumerable<(int direction, int count)>> ParseData(string input)
        {
            var lines = Util.Split(input, '\n');
            foreach (var line in lines)
            {
                yield return Util.Split(line).Select(i => (new Direction2(i[0]), int.Parse(i[1..]))).Select(v => (v.Item1.DX + (v.Item1.DY << 16), v.Item2));
            }
        }

        public static int FindIntersection(string input, SearchMode mode)
        {
            var data = ParseData(input);

            var minDist = int.MaxValue;

            Dictionary<int, int> seen = new();

            foreach (var line in data)
            {
                int position = 0x1000 + (0x1000 << 16), steps = 0;
                Dictionary<int, int> current = new();

                foreach (var (direction, count) in line)
                {
                    for (var j = 0; j < count; ++j)
                    {
                        position += direction;
                        steps++;

                        if (seen.TryGetValue(position, out int value))
                        {
                            minDist = Math.Min(minDist, mode == SearchMode.Closest
                                ? Math.Abs((position & 0xffff) - 0x1000) + Math.Abs((position >> 16) - 0x1000)
                                : steps + value);
                        }
                        else
                        {
                            current[position] = steps;
                        }
                    }
                }

                seen = current;
            }

            return minDist;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + FindIntersection(input, SearchMode.Closest));
            logger.WriteLine("- Pt2 - " + FindIntersection(input, SearchMode.Shortest));
        }
    }
}
