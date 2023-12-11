namespace AoC.Advent2023;
public class Day11 : IPuzzle
{
    static bool IsBetween(int val, int v1, int v2) => (val > v1 && val < v2) || (val > v2 && val < v1);

    public static long MeasureGalaxies(string input, long gapSize)
    {
        var galaxies = Util.ParseSparseMatrix<bool>(input).Keys.ToArray();

        var (maxX, maxY) = (galaxies.Max(v => v.x), galaxies.Max(x => x.y));

        var gapRows = Enumerable.Range(0, maxX).Where(x => !galaxies.Any(p => p.x == x)).ToList();
        var gapCols = Enumerable.Range(0, maxY).Where(y => !galaxies.Any(p => p.y == y)).ToList();

        return galaxies.UniquePairs()
                .Sum(pair => pair.Item1.Distance(pair.Item2) +
                   ((gapRows.Where(x => IsBetween(x, pair.Item1.x, pair.Item2.x)).Count() +
                     gapCols.Where(y => IsBetween(y, pair.Item1.y, pair.Item2.y)).Count()) * (gapSize - 1)));
    }

    public static long Part1(string input) => MeasureGalaxies(input, 2);

    public static long Part2(string input) => MeasureGalaxies(input, 1000000);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}