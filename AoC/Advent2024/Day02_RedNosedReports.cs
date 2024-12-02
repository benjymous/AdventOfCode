namespace AoC.Advent2024;
public class Day02 : IPuzzle
{
    public static bool AllDecrease(IEnumerable<int> vals)
        => vals.OverlappingPairs().All(v => (v.first - v.second) is >= 1 and <= 3);
    public static bool AllIncrease(IEnumerable<int> vals)
        => vals.OverlappingPairs().All(v => (v.second - v.first) is >= 1 and <= 3);

    public static IEnumerable<IEnumerable<int>> DampenedCombos(int[] vals)
        => Enumerable.Range(0, vals.Length).Select(idx => DampenAt(vals, idx));
    public static IEnumerable<int> DampenAt(IEnumerable<int> vals, int idx)
        => vals.Index().Where(v => v.Index != idx).Select(v => v.Item);

    public static bool Safe(IEnumerable<int> vals) => AllIncrease(vals) || AllDecrease(vals);
    public static bool SafeIfDampened(int[] vals) => DampenedCombos(vals).Any(Safe);

    public static int Part1(string input) => Util.ParseNumberList<int>(input).Count(Safe);

    public static int Part2(string input) => Util.ParseNumberList<int>(input).Count(SafeIfDampened);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}