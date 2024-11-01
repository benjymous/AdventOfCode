namespace AoC.Advent2023;
public class Day05 : IPuzzle
{
    public class Mapper
    {
        readonly List<(long from, long delta, long count)> ranges = [];
        public void AddRange(long to, long from, long count) => ranges.Add((from, to - from, count));

        public long Remap(bool forwards, long source)
        {
            var remap = forwards
                ? ranges.FirstOrDefault(range => source >= range.from && source < range.from + range.count)
                : ranges.FirstOrDefault(range => source - range.delta >= range.from && source - range.delta < range.from + range.count);
            return remap == default ? source : forwards ? source + remap.delta : source - remap.delta;
        }
    }

    public class Factory
    {
        public long[] Seeds;

        public Dictionary<string, (string to, Mapper map)> maps = [], mapsReverse = [];
        private Mapper current = null;

        [Regex(@"seeds: (.+)")]
        public void ParseSeeds([Split(" ")] long[] seedNums) => Seeds = seedNums;

        [Regex(@"(.+)-to-(.+) map:")]
        public void NewMap(string from, string to)
        {
            current = new();
            maps[from] = (to, current);
            mapsReverse[to] = (from, current);
        }

        [Regex(@"(\d+) (\d+) (\d+)")]
        public void ParseMapEntry(long to, long from, long count) => current.AddRange(to, from, count);

        private long DoMapping(bool forwards, string from, string to, long val)
        {
            var (mapTo, mapper) = (forwards ? maps : mapsReverse)[from];
            var nextVal = mapper.Remap(forwards, val);
            return mapTo == to ? nextVal : DoMapping(forwards, mapTo, to, nextVal);
        }

        public long Remap(string from, string to, long val) => DoMapping(true, from, to, val);
        public long Reverse(string from, string to, long val) => DoMapping(false, from, to, val);
    }

    public static long Part1(string input)
    {
        var factory = Util.RegexFactory<Factory>(input);

        return factory.Seeds.Select(seed => factory.Remap("seed", "location", seed)).Min();
    }

    public static long Part2(string input)
    {
        var factory = Util.RegexFactory<Factory>(input);

        var chunks = factory.Seeds.Chunk(2).Select(c => (start: c[0], end: c[0] + c[1])).ToArray();

        int jumpSize = 10000;

        for (long location = 0; ; location++)
        {
            var seed = factory.Reverse("location", "seed", location);

            if (chunks.Any(c => seed >= c.start && seed < c.end)) return location;

            if ((factory.Reverse("location", "seed", location + jumpSize) - (location + jumpSize)) == seed - location) location += jumpSize;
        }
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}