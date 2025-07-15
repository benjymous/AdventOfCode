namespace AoC.Advent2024;
public class Day02 : IPuzzle
{
    public static bool StableIncline(IEnumerable<int> vals)
    {
        var incline = Math.Sign(vals.First() - vals.Skip(1).First());
        return vals.OverlappingPairs().Select(v => v.first - v.second).All(diff => Math.Sign(diff) == incline && Math.Abs(diff) is >= 1 and <= 3);
    }

    public static IEnumerable<IEnumerable<int>> DampenedCombos(int[] vals)
        => Enumerable.Range(0, vals.Length).Select(idx => DampenAt(vals, idx));
    public static IEnumerable<int> DampenAt(int[] vals, int idx)
        => vals.Where((v, i) => i != idx);

    public static bool Safe(IEnumerable<int> vals) => StableIncline(vals);
    public static bool SafeIfDampened(int[] vals) => Safe(vals) || DampenedCombos(vals).Any(Safe);

    public static int Part1(string input) => Util.ParseNumberList<int>(input).Count(Safe);

    public static int Part2(string input) => Util.ParseNumberList<int>(input).Count(SafeIfDampened);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}