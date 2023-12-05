using System.Diagnostics;

namespace AoC.Advent2023;
public class Day05 : IPuzzle
{
    public class Mapper
    {
        List<(long from, long delta, long count)> ranges = new();
        public void AddRange(long to, long from, long count) => ranges.Add((from, to - from, count));

        public long Remap(long source)
        {
            foreach (var (from, delta, count) in ranges.Where(range => source >= range.from && source < range.from + range.count))
            {
                return source + delta;
            }

            return source;
        }
    }

    public class Factory
    {
        public long[] Seeds;

        public Dictionary<string, (string to, Mapper map)> maps = [];
        private Mapper current = null;

        [Regex(@"seeds: (.+)")]
        public void ParseSeeds([Split(" ")] long[] seedNums) => Seeds = seedNums;

        [Regex(@"(.+)-to-(.+) map:")]
        public void NewMap(string from, string to)
        {
            current = new();
            maps[from] = (to, current);
        }

        [Regex(@"(\d+) (\d+) (\d+)")]
        public void ParseMapEntry(long to, long from, long count) => current.AddRange(to, from, count);

        public long Remap(string from, string to, long val)
        {
            if (maps.TryGetValue(from, out var map))
            {
                var nextVal = map.map.Remap(val);
                if (map.to == to)
                {
                    return nextVal;
                }
                else
                {
                    return Remap(map.to, to, nextVal);
                }
            }

            return val;
        }
    }

    public static long Part1(string input)
    {
        var factory = Util.RegexFactory<Factory>(input);

        var vals = factory.Seeds.Select(seed => factory.Remap("seed", "location", seed));

        return vals.Min();
    }

    public static long ProcessChunk(Factory factory, long rangeStart, long rangeCount)
    {
        long minLoc = long.MaxValue;
        Console.WriteLine($"{rangeStart},{rangeCount}");
        for (int i = 0; i < rangeCount; ++i)
        {
            var loc = factory.Remap("seed", "location", rangeStart + i);
            if (loc < minLoc)
            {
                minLoc = loc;
            }
        }
        return minLoc;
    }

    public static long Part2(string input)
    {
        var factory = Util.RegexFactory<Factory>(input);

        long minLoc = long.MaxValue;

        object lockObj = new();

        var sw = new Stopwatch();
        sw.Start();
        long count = 0;

        var chunks = factory.Seeds.Chunk(2).ToArray();
        var totalSeeds = chunks.Sum(pair => pair[1]);

        return chunks.AsParallel().Select(pair =>
        {
            var val = ProcessChunk(factory, pair[0], pair[1]);
            lock (lockObj)
            {
                count += pair[1];
                minLoc = Math.Min(val, minLoc);
                var remaining = sw.ElapsedMilliseconds / (float)count * (totalSeeds - count);
                Console.WriteLine($"{sw.Elapsed} {TimeSpan.FromMilliseconds(remaining)} [ {count}/{totalSeeds} ] == {minLoc}");
            }
            return minLoc;
        }).Min();
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));  // 289863851
    }
}
