using AoC.Advent2019.NPSA;
namespace AoC.Advent2019;
internal class Hoovamatic(string input) : ASCIITerminal(input)
{
    public long Run(bool activate = false) { if (activate) Poke(0, 2); base.Run(); return finalOutput; }

    private static int CountNeighbours(HashSet<PackedPos32> items, PackedPos32 pos) => Util.CountTrue(items.Contains(pos + (-1, 0)), items.Contains(pos + (+1, 0)), items.Contains(pos + (0, -1)), items.Contains(pos + (0, +1)));

    public IEnumerable<PackedPos32> FindIntersections()
    {
        var items = buffer.FindAll('#');
        return items.Where(pos => CountNeighbours(items, pos) == 4);
    }

    private static IEnumerable<string> PatternsToSearch(string value)
    {
        for (int i = 0; i < value.Length; i++)
            for (int j = 12; j <= 21; j++)
                if (i + j <= value.Length)
                    yield return value.Substring(i, j);
    }

    private static bool NeedsToShrink(string s) => s.Contains('L') || s.Contains('R');

    public static string Filter(string input) => input.Replace("-", "").Trim(',');

    public static IEnumerable<string> TestResult(string shrunk, List<string> used) => NeedsToShrink(shrunk) ? null : [Filter(shrunk), .. used];

    public static IEnumerable<string> FindSubstitutions(string commands, List<string> used)
    {
        if (!NeedsToShrink(commands)) return TestResult(commands, used);
        var patterns = PatternsToSearch(commands).Distinct().Where(p => p[^1] == ',' && p[0] is 'L' or 'R' && p[^2] is >= '0' and <= '9' && !p.Contains('-')).Select(pattern => (pattern, Regex.Matches(commands, pattern, RegexOptions.IgnoreCase).Count)).OrderByDescending(x => x.pattern.Length + x.Count).Select(kvp => kvp.pattern);
        foreach (var pattern in patterns)
        {
            var result = commands.Replace(pattern, $"-{(char)('A' + used.Count)}-,");
            var newUsed = new List<string>(used) { Filter(pattern) };

            var res = TestResult(result, newUsed);
            if (res != null) return res;

            if (newUsed.Count < 3)
            {
                var recurse = FindSubstitutions(result, newUsed);
                if (recurse != null) return recurse;
            }
        }

        return patterns.Any() ? null : TestResult(commands, used);
    }

    public override IEnumerable<string> AutomaticInput() => [.. FindSubstitutions(string.Concat(BuildPath()), []), buffer.DisplayLive ? "y" : "n"];

    private IEnumerable<string> BuildPath()
    {
        var scaffolds = buffer.FindAll('#');
        var position = buffer.FindCharacter('^');
        var direction = new Direction2(0, -1);
        while (true)
        {
            int spins = Util.CountTrue((i) => i == 2 || !scaffolds.Contains(position + direction.AsSimple()), () => direction.TurnRight()), dir = (PackedPos32)(direction.DX, direction.DY), distance = Util.CountTrue((_) => scaffolds.Contains(position + dir), () => position += dir);
            yield return $"{((spins == 1) ? 'R' : 'L')},{distance},";
            if (CountNeighbours(scaffolds, position) == 1) break;
        }
    }
}

public class Day17 : IPuzzle
{
    public static int Part1(string input)
    {
        var robot = new Hoovamatic(input);
        robot.Run();

        return robot.FindIntersections().Sum(v => v.X * v.Y);
    }

    public static long Part2(string input)
    {
        var robot = new Hoovamatic(input);
        return robot.Run(true);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}