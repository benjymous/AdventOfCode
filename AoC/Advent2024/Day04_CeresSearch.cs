namespace AoC.Advent2024;
public class Day04 : IPuzzle
{
    static readonly (int, int)[] directions = [(0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1)];

    static bool Check(char[,] data, (int, int) pos, char expected)
        => data.TryGetValue(pos, out char v) && v == expected;

    static bool IsItXmas(char[,] data, (int, int) pos, (int, int) dir)
        => Check(data, pos.OffsetBy(dir), 'M') &&
           Check(data, pos.OffsetBy(dir, 2), 'A') &&
           Check(data, pos.OffsetBy(dir, 3), 'S');

    static bool IsItX_mas(char[,] data, (int, int) pos)
        => ((Check(data, pos.OffsetBy((-1, -1)), 'M') && Check(data, pos.OffsetBy((1, 1)), 'S')) ||
            (Check(data, pos.OffsetBy((-1, -1)), 'S') && Check(data, pos.OffsetBy((1, 1)), 'M'))) &&

           ((Check(data, pos.OffsetBy((1, -1)), 'M') && Check(data, pos.OffsetBy((-1, 1)), 'S')) ||
            (Check(data, pos.OffsetBy((1, -1)), 'S') && Check(data, pos.OffsetBy((-1, 1)), 'M')));

    public static int Part1(string input)
    {
        var data = Util.ParseMatrix<char>(input);
        return data.KeysWithValue('X').Sum(pos => directions.Count(dir => IsItXmas(data, pos, dir)));
    }

    public static int Part2(string input)
    {
        var data = Util.ParseMatrix<char>(input);
        return data.KeysWithValue('A').Count(pos => IsItX_mas(data, pos));
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}