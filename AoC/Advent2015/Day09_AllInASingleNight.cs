namespace AoC.Advent2015;
public class Day09 : IPuzzle
{
    class Factory
    {
        [Regex(@"(.+) to (.+) = (.+)")]
        public void Parse(string from, string to, int distance) => Atlas[Remap(from) | Remap(to)] = distance;

        int Remap(string name) => MappingDict.GetIndexBit(name);

        public int[] LocationKeys => [.. MappingDict.Values];

        readonly Dictionary<string, int> MappingDict = [];
        public readonly Dictionary<int, int> Atlas = [];

        public static implicit operator Factory(string data) => Parser.Factory<Factory>(data);
    }

    static (int min, int max) MeasureRoutes(IEnumerable<int> remaining, Dictionary<int, int> atlas, int current = 0)
    {
        if (!remaining.Any()) return (0, 0);
        int min = int.MaxValue, max = int.MinValue;

        foreach (var node in remaining)
        {
            int distance = current == 0 ? 0 : atlas[current | node];

            var (minRemaining, maxRemaining) = MeasureRoutes(remaining.Where(i => i != node).ToArray(), atlas, node);

            (min, max) = (Math.Min(minRemaining + distance, min), Math.Max(maxRemaining + distance, max));
        }
        return (min, max);
    }

    static (int min, int max) Solve(Factory data) => MeasureRoutes(data.LocationKeys, data.Atlas);

    public static int Part1(string input) => Solve(input).min;

    public static int Part2(string input) => Solve(input).max;

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}