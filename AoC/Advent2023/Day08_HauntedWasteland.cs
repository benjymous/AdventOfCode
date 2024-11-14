namespace AoC.Advent2023;
public class Day08 : IPuzzle
{
    public class GhostMap
    {
        public Dictionary<string, Dictionary<char, string>> Paths = [];
        string Route;

        [Regex(@"(...) = \((...), (...)\)")]
        public void AddNode(string location, string left, string right)
            => Paths[location] = new Dictionary<char, string> { { 'L', left }, { 'R', right } };

        [Regex("(.+)")]
        public void AddRoute(string route) => Route = route;

        public int Walk(string location, Func<string, bool> test)
        {
            int steps = 0;
            while (!test(location))
                location = Paths[location][Route[steps++ % Route.Length]];
            return steps;
        }

        public static implicit operator GhostMap(string data) => Util.RegexFactory<GhostMap>(data);
    }

    public static int Part1(GhostMap map) => map.Walk("AAA", current => current == "ZZZ");

    public static long Part2(GhostMap map)
    {
        return map.Paths.Keys.Where(k => k.EndsWith('A'))
            .Aggregate(1L, (cycle, start)
                => Util.LCM(cycle, map.Walk(start, current => current.EndsWith('Z'))));
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}