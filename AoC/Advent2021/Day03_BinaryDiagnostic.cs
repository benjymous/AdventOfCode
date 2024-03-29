﻿namespace AoC.Advent2021;
public class Day03 : IPuzzle
{
    private static int FindValue(string[] lines, bool greater)
    {
        string[] current = [.. lines];
        (char c1, char c2) = greater ? ('1', '0') : ('0', '1');

        for (int i = 0; i < lines[0].Length; ++i)
        {
            var count1 = current.Select(line => line[i]).Count(i => i == '1');

            var filter = (count1 >= current.Length - count1) ? c1 : c2;

            current = current.Where(l => l[i] == filter).ToArray();

            if (current.Length == 1) break;
        }

        return Convert.ToInt32(current.Single(), 2);
    }

    public static int Part1(string input)
    {
        var lines = Util.Split(input);

        var gamma = Convert.ToInt32(
            Enumerable.Range(0, lines[0].Length)
                .Select(i => lines
                    .Select(line => line[i])
                    .Count(i => i == '1') > lines.Length / 2 ? '1' : '0')
                .AsString()
            , 2);
        int epsilon = (int)Math.Pow(2, lines[0].Length) - 1 - gamma;

        return gamma * epsilon;
    }

    public static int Part2(string input)
    {
        var lines = Util.Split(input);

        int res1 = FindValue(lines, true);
        int res0 = FindValue(lines, false);

        return res1 * res0;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}