namespace AoC.Advent2015;
public class Day13 : IPuzzle
{
    public class Factory
    {
        [Regex(@"(.).+ would (.).+ (.+) happiness units by sitting next to (.).+")]
        public void Parse(char initial1, char factor, int units, char initial2)
        {
            Names.Add(initial1);
            Atlas.IncrementAtIndex(GetKey(initial1, initial2), units * ((factor == 'l') ? -1 : 1));
        }

        public readonly Dictionary<int, int> Atlas = [];
        public readonly HashSet<char> Names = [];

        public static implicit operator Factory(string data) => Util.RegexFactory<Factory>(data);
    }

    public static int GetKey(char p1, char p2) => p1 * p2;

    static int TryPermutations(char first, char prev, IEnumerable<char> remaining, Dictionary<int, int> scores)
    {
        return !remaining.Any()
            ? scores[GetKey(first, prev)]
            : remaining.Max(next => scores[GetKey(prev, next)] + TryPermutations(first, next, remaining.Where(c => c != next).ToArray(), scores));
    }

    public static int Solve(Factory data, bool includeYou = false)
    {
        char starter = includeYou ? '\0' : data.Names.First();
        data.Atlas[0] = 0;
        data.Names.Remove(starter);

        return TryPermutations(starter, starter, data.Names, data.Atlas);
    }

    public static int Part1(string input) => Solve(input);

    public static int Part2(string input) => Solve(input, true);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}