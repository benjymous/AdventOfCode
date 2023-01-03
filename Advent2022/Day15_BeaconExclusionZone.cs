using AoC.Utils;
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
            public Sensor(int x, int y, int bx, int by)
            {
                Pos = (x, y);
                Beacon = (bx, by);
                Range = Pos.Distance(Beacon);
            }
            public readonly (int x, int y) Pos, Beacon;
            public readonly int Range;

            public bool InRange((int x, int y) test) => test.Distance(Pos) <= Range;

            public bool WithinRow(int row) => row >= Pos.y - Range && row <= Pos.y + Range;

            public IEnumerable<(int x, int y)> OuterBoundary()
            {
                for (int y = 0; y <= Range + 1; ++y)
                {
                    int size = Range - y + 1;
                    yield return (Pos.x - size, Pos.y + y);
                    if (size > 0) yield return (Pos.x + size, Pos.y + y);
                    if (y != 0)
                    {
                        yield return (Pos.x - size, Pos.y - y);
                        if (size > 0) yield return (Pos.x + size, Pos.y - y);
                    }
                }
            }
        }

        public static int Part1(string input, int line = 2000000)
        {
            var sensors = Util.RegexParse<Sensor>(input).Where(s => s.WithinRow(line)).ToArray();
            var beacons = sensors.Select(s => s.Beacon).Where(p => p.y == line).Distinct();

            var minX = sensors.Min(s => s.Pos.x - s.Range);
            var maxX = sensors.Max(s => s.Pos.x + s.Range);

            return Enumerable.Range(minX, maxX - minX)
                             .Where(x => sensors.Any(s => s.InRange((x, line))))
                             .Count() - beacons.Count();
        }

        public static long Part2(string input, int max = 4000000)
        {
            var sensors = Util.RegexParse<Sensor>(input).ToArray();

            var (x, y) = sensors.SelectMany(s => s.OuterBoundary())
                               .Where(p => p.x >= 0 && p.x <= max && p.y >= 0 && p.y <= max)
                               .Where(p => !sensors.Any(s => s.InRange(p)))
                               .First();

            return (4000000L * x) + y;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}