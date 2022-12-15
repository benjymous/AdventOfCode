using AoC.Utils;
using System;
using System.Linq;
using System.Numerics;

namespace AoC.Advent2022
{
    public class Day15 : IPuzzle
    {
        public string Name => "2022-15";

        public class Sensor
        {
            [Regex(@"Sensor at x=(.+), y=(.+): closest beacon is at x=(.+), y=(.+)")]
            public Sensor (int x, int y, int bx, int by)
            {
                Pos = (x, y);
                Beacon = (bx, by);
                Range = Pos.Distance(Beacon);
            }
            public readonly (int x, int y) Pos, Beacon;
            public readonly int Range;

            public bool InRange((int x, int y) test) => test.Distance(Pos) <= Range;

            public (int minX, int maxX) GetRowRange(int row)
            {
                var dy = Math.Abs(Pos.y - row);
                if (dy > Range) return (-1, -1);
                return (Pos.x - (Range-dy), Pos.x + (Range-dy));
            }
        }

        public static int SolvePart1(string input, int line)
        {
            var sensors = Util.RegexParse<Sensor>(input).ToArray();
            var beacons = sensors.Select(s => s.Beacon).ToHashSet();

            var minX = sensors.Min(s => s.Pos.x);
            var maxX = sensors.Max(s => s.Pos.x);
            var maxRange = sensors.Max(s => s.Range);

            int count = 0;
            for (int x = minX-maxRange; x <= maxX+maxRange; ++x)
            {
                var test = (x, line);
                if (!beacons.Contains(test) && sensors.Any(s => s.InRange(test))) count++;
            }

            return count;
        }

        public static BigInteger SolvePart2(string input, int max)
        {
            var sensors = Util.RegexParse<Sensor>(input).ToArray();

            for (int y = 0; y <= max; ++y)
            {
                var ranges = sensors.Select(s => s.GetRowRange(y)).OrderBy(v => v.maxX);
                for (int x = 0; x <= max; ++x)
                {
                    foreach (var limit in ranges)
                    {
                        if (x >= limit.minX && x <= limit.maxX) x = limit.maxX+1;
                        if (x > max) break;
                    }
                    if (x <= max)
                    {
                        if (!sensors.Any(s => s.InRange((x, y))))
                        {
                            return ((BigInteger)x * 4000000) + y;
                        }
                    }
                }
            }

            return 0;
        }

        public static int Part1(string input)
        {         
            return SolvePart1(input, 2000000);
        }

        public static BigInteger Part2(string input)
        {
            return SolvePart2(input, 4000000);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}