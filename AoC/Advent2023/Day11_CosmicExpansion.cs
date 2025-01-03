﻿namespace AoC.Advent2023;
public class Day11 : IPuzzle
{
    static int CountGaps(int v1, int v2, int[] gaps)
    {
        var (min, max) = Util.MinMax(v1, v2);
        int count = 0;
        foreach (var gap in gaps)
            if (gap > min)
                if (gap < max) count++;
                else break;

        return count;
    }

    public static long MeasureGalaxies(string input, long gapSize)
    {
        var galaxies = Util.ParseSparseMatrix<bool>(input);

        var (maxX, maxY) = (galaxies.Width, galaxies.Height);

        var gapsX = Enumerable.Range(0, maxX).Except(galaxies.Keys.Select(p => p.x)).ToArray();
        var gapsY = Enumerable.Range(0, maxY).Except(galaxies.Keys.Select(p => p.y)).ToArray();

        return galaxies.Keys.UniquePairs()
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