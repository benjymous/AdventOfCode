namespace AoC.Advent2018;
public class Day23 : IPuzzle
{
    [Regex(@"pos=<(.+,.+,.+)>, r=(.+)")]
    public record struct Entry(ManhattanVector3 Position, int Radius)
    {
        public readonly bool Overlaps(Entry other) => Position.Distance(other.Position) <= Math.Max(Radius, other.Radius);
    }

    public static int Part1(Util.AutoParse<Entry> data)
    {
        var strongest = data.MaxBy(e => e.Radius);

        return data.Count(e => e.Position.Distance(strongest.Position) <= strongest.Radius);
    }

    public static int Part2(Util.AutoParse<Entry> data)
    {
        var (minZ, maxZ, minY, maxY, minX, maxX) = data.Select(e => e.Position).GetRange();

        var weakest = data.MinBy(e => e.Radius);

        int step = Math.Max(1, weakest.Radius / 2);

        (int x, int y, int z) bestPos = (0, 0, 0);
        int bestDistance = maxX + maxY + maxZ;
        int bestScore = 0;

        while (step >= 1)
        {
            int samples = 0;
            for (var x = minX; x <= maxX; x += step)
            {
                for (var y = minY; y <= maxY; y += step)
                {
                    for (var z = minZ; z <= maxZ; z += step)
                    {
                        var pos = (x, y, z);
                        var length = pos.Length();

                        if (length > (bestDistance * 1.2f)) continue;

                        samples++;

                        var count = data.Count(e => e.Position.Distance(pos) <= e.Radius);

                        if (count > bestScore)
                        {
                            bestScore = count;
                            bestDistance = length;
                            bestPos = pos;
                        }
                        else if (count == bestScore)
                        {
                            var distance = length;
                            if (distance < bestDistance)
                            {
                                bestDistance = distance;
                                bestPos = pos;
                            }
                        }
                    }
                }
            }

            step /= 3;

            (minX, minY, minZ) = (bestPos.x - (step * 2), bestPos.y - (step * 2), bestPos.z - (step * 2));
            (maxX, maxY, maxZ) = (bestPos.x + (step * 2), bestPos.y + (step * 2), bestPos.z + (step * 2));
        }

        return bestDistance;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
