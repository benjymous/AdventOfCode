namespace AoC.Advent2024;
public class Day04 : IPuzzle
{
    private static readonly (int, int)[] directions = [(0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1)];

    private static bool IsItXmas(char[,] data, (int, int) pos, (int, int) dir)
        => data.GetOrDefault(pos.OffsetBy(dir)) == 'M' &&
           data.GetOrDefault(pos.OffsetBy(dir, 2)) == 'A' &&
           data.GetOrDefault(pos.OffsetBy(dir, 3)) == 'S';

    private static bool IsItX_mas(char[,] data, (int, int) pos)
        => ((data.GetOrDefault(pos.OffsetBy((-1, -1))) == 'M' && data.GetOrDefault(pos.OffsetBy((1, 1))) == 'S') ||
            (data.GetOrDefault(pos.OffsetBy((-1, -1))) == 'S' && data.GetOrDefault(pos.OffsetBy((1, 1))) == 'M')) &&

           ((data.GetOrDefault(pos.OffsetBy((1, -1))) == 'M' && data.GetOrDefault(pos.OffsetBy((-1, 1))) == 'S') ||
            (data.GetOrDefault(pos.OffsetBy((1, -1))) == 'S' && data.GetOrDefault(pos.OffsetBy((-1, 1))) == 'M'));

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