namespace AoC.Advent2017;
public class Day13 : IPuzzle
{
    [Regex(@"(\d+): (\d+)")]
    public record class Scanner(int Id, int Range)
    {
        public int Position { get; private set; } = 0;
        public int Frequency { get; private set; } = (2 * Range) - 2;

        public void Step() => Position = (Position + 1) % Frequency;
    }

    private static Dictionary<int, Scanner> GetScanners(string input) => Util.RegexParse<Scanner>(input).ToDictionary(s => s.Id, s => s);

    private static int CheckBlock((int Key, int Mod)[] scanners, int start, int size)
    {
        var collisions = new HashSet<int>(size);
        foreach (var (Key, Mod) in scanners)
        {
            for (int i = start + ((Mod + (Mod - (start + Key))) % Mod); i < (start + size); i += Mod)
                if (i >= start && i < start + size) collisions.Add(i);

            if (collisions.Count == size) return -1;
        }
        return collisions.Count == size ? -1 : Enumerable.Range(start, size).Except(collisions).First();
    }

    public static int Part1(string input)
    {
        var scanners = GetScanners(input);
        int maxDepth = scanners.Keys.Max() + 1;

        int severity = 0;
        for (int packetDepth = 0; packetDepth < maxDepth; ++packetDepth)
        {
            if (scanners.TryGetValue(packetDepth, out var scanner) && scanner.Position == 0)
            {
                severity += packetDepth * scanner.Range;
            }
            scanners.Values.ForEach(s => s.Step());
        }

        return severity;
    }

    public static int Part2(string input)
    {
        (int Key, int Mod)[] scanners = GetScanners(input).Select(s => (s.Key, Mod: s.Value.Frequency)).ToArray();
        int blockSize = 1000;
        for (int i = 0; true; i += blockSize)
        {
            int res = CheckBlock(scanners, i, blockSize);
            if (res != -1) return res;
        }
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}