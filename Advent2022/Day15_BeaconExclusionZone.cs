using AoC.Utils;
using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;

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

            public bool InRange((int x, int y) test)
            {
                return test.Distance(Pos) <= Range;
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

        public static int Part1(string input)
        {
            
            return SolvePart1(input, 2000000);
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            string test = @"Sensor at x=2, y=18: closest beacon is at x=-2, y=15
Sensor at x=9, y=16: closest beacon is at x=10, y=16
Sensor at x=13, y=2: closest beacon is at x=15, y=3
Sensor at x=12, y=14: closest beacon is at x=10, y=16
Sensor at x=10, y=20: closest beacon is at x=10, y=16
Sensor at x=14, y=17: closest beacon is at x=10, y=16
Sensor at x=8, y=7: closest beacon is at x=2, y=10
Sensor at x=2, y=0: closest beacon is at x=2, y=10
Sensor at x=0, y=11: closest beacon is at x=2, y=10
Sensor at x=20, y=14: closest beacon is at x=25, y=17
Sensor at x=17, y=20: closest beacon is at x=21, y=22
Sensor at x=16, y=7: closest beacon is at x=15, y=3
Sensor at x=14, y=3: closest beacon is at x=15, y=3
Sensor at x=20, y=1: closest beacon is at x=15, y=3".Replace("\r", "");

            Console.WriteLine(SolvePart1(test, 10));

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}