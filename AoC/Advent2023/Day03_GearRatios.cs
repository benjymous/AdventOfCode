﻿
namespace AoC.Advent2023;
public class Day03 : IPuzzle
{
    private static IEnumerable<(int partNumber, char type, (int x, int y) pos)> GetParts(string input)
    {
        var data = Util.ParseSparseMatrix<char>(input, new Util.Convertomatic.SkipChars('.'));

        foreach (var symbol in data.Where(kvp => !kvp.Value.IsDigit()))
        {
            for (int y = symbol.Key.y - 1; y <= symbol.Key.y + 1; y++)
            {
                for (int x = symbol.Key.x - 1; x <= symbol.Key.x + 1; x++)
                {
                    if (data.TryGetValue((x, y), out var v) && v.IsDigit())
                    {
                        yield return (FindNumber((x, y), data), symbol.Value, symbol.Key);
                    }
                }
            }
        }
    }

    static int Seek(Dictionary<(int x, int y), char> numbers, int x, int y, int direction) => Util.Sequence(x, direction).Where(x => !(numbers.TryGetValue((x, y), out var v) && v.IsDigit())).First() - direction;

    private static int FindNumber((int x, int y) value, Dictionary<(int x, int y), char> numbers)
    {
        int x1 = Seek(numbers, value.x, value.y, -1), x2 = Seek(numbers, value.x, value.y, +1);

        var str = "";
        for (int x = x1; x <= x2; ++x)
        {
            str += numbers[(x, value.y)];
            numbers.Remove((x, value.y));
        }
        return int.Parse(str);
    }

    public static int Part1(string input) => GetParts(input).Sum(v => v.partNumber);

    public static long Part2(string input) => GetParts(input).GroupBy(p => p.pos).Where(g => g.Count() == 2).Sum(g => g.Take(2).Product(g => g.partNumber));

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}