using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2017
{
    public class Day13 : IPuzzle
    {
        public string Name { get { return "2017-13"; } }

        public class Scanner
        {
            public int Id { get; private set; }
            public int Range { get; private set; }
            public int Position { get; private set; } = 0;

            public int Direction { get; private set; } = 1;

            public Scanner(string line)
            {
                var bits = line.Split(":");
                Id = int.Parse(bits[0]);
                Range = int.Parse(bits[1]);
            }

            public void Step()
            {
                Position += Direction;
                if (Position == 0) Direction = 1;
                if (Position == Range - 1) Direction = -1;
            }

        }

        private static Dictionary<int, Scanner> GetScanners(string input)
        {
            return Util.Parse<Scanner>(input).ToDictionary(s => s.Id, s => s);
        }

        private static (int severity, bool hit) RunScanners(string input, int delay = 0, bool earlyExit = false)
        {
            var scanners = GetScanners(input);
            int maxDepth = scanners.Keys.Max() + 1;

            bool hit = false;
            int severity = 0;
            int packetDepth = -1;
            while (packetDepth < maxDepth)
            {
                if (delay-- <= 0)
                {
                    packetDepth++;
                }
                if (scanners.TryGetValue(packetDepth, out var scanner))
                {
                    // packet inside scanner
                    if (scanner.Position == 0)
                    {
                        // detected!
                        severity += packetDepth * scanner.Range;
                        hit = true;

                        if (earlyExit) return (packetDepth, true);
                    }
                }
                scanners.ForEach(s => s.Value.Step());
            }

            return (severity, hit);
        }

        public static int Part1(string input)
        {
            return RunScanners(input, 0).severity;
        }

        public static int Part2(string input)
        {
            var scanners = GetScanners(input).ToDictionary(s => s.Key, s => s.Value.Range);

            return Util.Forever(0).First(delay => scanners.All(s => (delay + s.Key) % (2 * s.Value - 2) != 0));
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}