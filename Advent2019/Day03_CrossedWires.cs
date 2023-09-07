using System;
using System.Collections.Generic;

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

        static IEnumerable<List<(int direction, int count)>> ParseData(string input)
        {
            var lines = Util.Split(input, '\n');
            foreach (var line in lines)
            {
                List<(int, int)> lineRes = new();
                var instructions = Util.Split(line);
                foreach (var i in instructions)
                {
                    lineRes.Add((i[0] switch
                    {
                        'R' => 1,
                        'L' => -1,
                        'U' => -1 << 16,
                        'D' => 1 << 16,
                        _ => 0
                    }, int.Parse(i[1..])));
                }
                yield return lineRes;
            }
        }

        public static int FindIntersection(string input, SearchMode mode)
        {
            var data = ParseData(input);

            var minDist = int.MaxValue;

            Dictionary<int, int> seen = null;

            foreach (var line in data)
            {
                int position = 0x1000 + (0x1000 << 16);
                int steps = 0;
                Dictionary<int, int> current = new();

                foreach (var (direction, count) in line)
                {
                    for (var j = 0; j < count; ++j)
                    {
                        position += direction;
                        steps++;

                        if (seen != null && seen.ContainsKey(position))
                        {
                            minDist = Math.Min(minDist, mode == SearchMode.Closest
                                ? Math.Abs((position & 0xffff) - 0x1000) + Math.Abs((position >> 16) - 0x1000)
                                : steps + seen[position]);
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
