namespace AoC.Advent2022;
public class Day15 : IPuzzle
{
    [Regex(@"Sensor at (.+): closest beacon is at (.+)")]
    public record class Sensor([Regex("x=(.+), y=(.+)")] (int x, int y) Pos, [Regex("x=(.+), y=(.+)")] (int x, int y) Beacon)
    {
        public readonly int Range = Pos.Distance(Beacon);

        public bool InRange((int x, int y) p) => Pos.Distance(p) <= Range;

        public int Overlap(Sensor other)
        {
            int dist = Pos.Distance(other.Pos);
            return other != this && dist <= Range + other.Range ? dist : int.MaxValue;
        }

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

        public (int min, int max) RowMinMax(int row)
        {
            int halfWidth = Range - Math.Abs(row - Pos.y);
            return (min: Pos.x - halfWidth, max: Pos.x + halfWidth + 1);
        }
    }

    public static int Part1(Parser.AutoArray<Sensor> input, int line = 2000000)
    {
        var sensors = input.Where(s => s.WithinRow(line)).ToArray();
        var beacons = sensors.Select(s => s.Beacon).Where(p => p.y == line).Distinct();

        var minMax = sensors.Select(s => s.RowMinMax(line)).ToArray();

        return minMax.Max(v => v.max) - minMax.Min(v => v.min) - beacons.Count();
    }

    public static long Part2(Parser.AutoArray<Sensor> sensors, int max = 4000000)
    {
        foreach (var sensor in sensors)
        {
            var boundary = sensor.OuterBoundary().WithinBounds(0, max, 0, max);
            var nearestNeighbours = sensors.Select(s2 => (s2, sensor.Overlap(s2))).OrderBy(pair => pair.Item2).Take(8).Select(pair => pair.s2).ToArray();
            var res = boundary.FirstOrDefault(pos => !nearestNeighbours.Any(s => s.InRange(pos)));
            if (res != default)
            {
                return (4000000L * res.x) + res.y;
            }
        }

        return 0;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}