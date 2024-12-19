namespace AoC.Advent2024;
public class Day19 : IPuzzle
{
    const int MaxLen = 8;
    public static IEnumerable<int> GetPrefixes(HashSet<string> parts, string line)
        => Enumerable.Range(1, Math.Min(MaxLen, line.Length)).Where(i => parts.Contains(line[..i]));

    public static bool IsMatch(HashSet<string> parts, string line) => Memoize((line, parts.Count), _ =>
        line == "" || GetPrefixes(parts, line).Any(part => IsMatch(parts, line[part..])));

    public static long CountMatches(HashSet<string> parts, string line) => Memoize((line, parts.Count), _ =>
        line == "" ? 1 : GetPrefixes(parts, line).Sum(part => CountMatches(parts, line[part..])));

    public static (HashSet<string>, string[]) SplitData(string input)
        => input.ParseSections(p1 => p1.Split(", ").ToHashSet(), p2 => Util.Split(p2));

    public static int Part1(string input)
    {
        var (pattern, lines) = SplitData(input);
        return lines.Count(l => IsMatch(pattern, l));
    }

    public static long Part2(string input)
    {
        var (pattern, lines) = SplitData(input);
        return lines.Sum(l => CountMatches(pattern, l));
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}