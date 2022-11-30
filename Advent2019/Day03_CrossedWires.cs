using AoC.Utils.Vectors;
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

        public static int FindIntersection(string input, SearchMode mode)
        {
            var lines = input.Split("\n");

            var minDist = int.MaxValue;

            Dictionary<(int x, int y), int> seen = new();

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;
                var instructions = line.Split(",");

                var position = new ManhattanVector2(0, 0);
                int steps = 0;
                Dictionary<(int x, int y), int> current = new();

                foreach (var i in instructions)
                {
                    if (string.IsNullOrEmpty(i)) continue;
                    var distance = int.Parse(i[1..]);

                    for (var j = 0; j < distance; ++j)
                    {
                        switch (i[0])
                        {
                            case 'R':
                                position.X++;
                                break;

                            case 'L':
                                position.X--;
                                break;

                            case 'U':
                                position.Y--;
                                break;

                            case 'D':
                                position.Y++;
                                break;
                        }

                        steps++;

                        if (seen.ContainsKey(position))
                        {
                            int dist = int.MaxValue;
                            if (mode == SearchMode.Closest)
                            {
                                dist = position.Distance(ManhattanVector2.Zero);
                            }
                            else
                            {
                                dist = steps + seen[position];
                            }
                            minDist = Math.Min(minDist, dist);
                        }
                        else
                        {
                            current[position] = steps;
                        }
                    }
                }

                foreach (var s in current) seen[s.Key] = s.Value;
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
