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

        public int WalkUntil(string location, Func<string, bool> test)
        {
            int steps = 0;
            while (!test(location))
                location = Paths[location][Route[steps++ % Route.Length]];
            return steps;
        }
    }

    public static int Part1(string input)
        => Util.RegexFactory<GhostMap>(input).WalkUntil("AAA", current => current == "ZZZ");

    public static long Part2(string input)
    {
        var map = Util.RegexFactory<GhostMap>(input);
        return map.Paths.Keys.Where(k => k.EndsWith('A'))
            .Aggregate(1L, (cycle, start)
                => Util.LCM(cycle, map.WalkUntil(start, current => current.EndsWith('Z'))));
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}