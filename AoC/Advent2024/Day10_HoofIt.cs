namespace AoC.Advent2024;
public class Day10 : IPuzzle
{
    static readonly (int, int)[] neighbours = [(-1, 0), (1, 0), (0, -1), (0, 1)];
    const char START = '0', END = '9';

    static IEnumerable<(int, int)> FindRoutes(char[,] map, (int, int) position, int current = START)
        => current == END
            ? ([position])
            : neighbours.Select(dir => position.OffsetBy(dir))
                        .Where(nextPos => map.TryGetValue(nextPos, out var nextCell) && nextCell == current + 1)
                        .SelectMany(nextPos => FindRoutes(map, nextPos, current + 1));

    static int FindRoutes(string input, QuestionPart part)
    {
        var map = Util.ParseMatrix<char>(input);

        return map.KeysWithValue(START)
                  .Sum(startPosition => FindRoutes(map, startPosition)
                                       .If(part.One(), vals => vals.Distinct())
                                       .Count());
    }

    public static int Part1(string input) => FindRoutes(input, QuestionPart.Part1);
    public static int Part2(string input) => FindRoutes(input, QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}