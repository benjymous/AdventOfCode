namespace AoC.Advent2024;
public class Day19 : IPuzzle
{
    public static bool IsMatch(WordTree parts, string line) => Memoize((line, parts.Count), _
        => line == "" || parts.GetPrefixes(line).Any(part => IsMatch(parts, line[part..])));

    public static long CountMatches(WordTree parts, string line) => Memoize((line, parts.Count), _
        => line == "" ? 1 : parts.GetPrefixes(line).Sum(part => CountMatches(parts, line[part..])));

    public static (WordTree, string[]) SplitData(string input)
        => input.ParseSections(p1 => new WordTree(p1.Split(", ")), p2 => Util.Split(p2));

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