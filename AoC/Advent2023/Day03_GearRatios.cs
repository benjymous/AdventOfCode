
namespace AoC.Advent2023;
public class Day03 : IPuzzle
{
    private static IEnumerable<(int partNumber, char type, (int x, int y) pos)> GetParts(string input)
    {
        var data = Util.ParseSparseMatrix<char>(input, new Util.Convertomatic.SkipChars('.'));
        var symbols = data.Where(kvp => !kvp.Value.IsDigit()).ToDictionary();
        var numbers = data.Where(kvp => kvp.Value.IsDigit()).ToDictionary();

        foreach (var symbol in symbols)
        {
            for (int y = symbol.Key.y - 1; y <= symbol.Key.y + 1; y++)
            {
                for (int x = symbol.Key.x - 1; x <= symbol.Key.x + 1; x++)
                {
                    if (numbers.ContainsKey((x, y)))
                    {
                        var num = FindNumber((x, y), numbers);
                        yield return (num, symbol.Value, symbol.Key);
                    }
                }
            }
        }
    }

    private static int FindNumber((int x, int y) value, Dictionary<(int x, int y), char> numbers)
    {
        int x1, x2;

        for (x1 = value.x; numbers.TryGetValue((x1, value.y), out var v) && v.IsDigit(); --x1) ;
        for (x2 = value.x; numbers.TryGetValue((x2, value.y), out var v) && v.IsDigit(); ++x2) ;

        var str = "";
        for (int x = x1 + 1; x < x2; ++x)
        {
            str += numbers[(x, value.y)];
            numbers.Remove((x, value.y));
        }
        return int.Parse(str);
    }

    public static int Part1(string input) => GetParts(input).Sum(v => v.partNumber);

    public static int Part2(string input) => GetParts(input).Where(p => p.type == '*').GroupBy(p => p.pos).Where(g => g.Count() == 2).Sum(g => g.First().partNumber * g.Skip(1).First().partNumber);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
