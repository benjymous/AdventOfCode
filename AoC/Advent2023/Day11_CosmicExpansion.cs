namespace AoC.Advent2023;
public class Day11 : IPuzzle
{
    static int CountGaps(int v1, int v2, int[] gaps)
    {
        var (min, max) = v1 < v2 ? (v1, v2) : (v2, v1);
        int count = 0;
        foreach (var gap in gaps)
            if (gap > min)
                if (gap < max) count++;
                else break;

        return count;
    }

    public static long MeasureGalaxies(string input, long gapSize)
    {
        var galaxies = Util.ParseSparseMatrix<bool>(input).Keys;

        var (maxX, maxY) = (galaxies.Max(v => v.x), galaxies.Max(x => x.y));

        var gapsX = Enumerable.Range(0, maxX).Where(x => !galaxies.Any(p => p.x == x)).ToArray();
        var gapsY = Enumerable.Range(0, maxY).Where(y => !galaxies.Any(p => p.y == y)).ToArray();

        return galaxies.UniquePairs()
                .Sum(pair => pair.Item1.Distance(pair.Item2) +
                   ((CountGaps(pair.Item1.x, pair.Item2.x, gapsX) +
                     CountGaps(pair.Item1.y, pair.Item2.y, gapsY)) * (gapSize - 1)));
    }

    public static long Part1(string input) => MeasureGalaxies(input, 2);

    public static long Part2(string input) => MeasureGalaxies(input, 1000000);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}