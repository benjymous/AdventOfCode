namespace AoC.Advent2021;
public class Day09 : IPuzzle
{
    class Map(string input)
    {
        public Dictionary<(int x, int y), int> Data { get; private set; } = Util.ParseSparseMatrix<int>(input);

        public int FloodFill((int x, int y) pos)
        {
            if (!Data.TryGetValue(pos, out var current) || current == 9) return 0;
            Data[pos] = 9;
            return 1 +
                FloodFill((pos.x, pos.y + 1)) +
                FloodFill((pos.x, pos.y - 1)) +
                FloodFill((pos.x + 1, pos.y)) +
                FloodFill((pos.x - 1, pos.y));
        }

        public Dictionary<(int x, int y), int>.KeyCollection Coordinates => Data.Keys;
    }

    static readonly (int x, int y)[] directions = [(-1, 0), (1, 0), (0, -1), (0, 1)];

    public static int Part1(string input)
    {
        var map = new Map(input);

        return map.Data.Where(kvp => directions.All(offset => !map.Data.TryGetValue((kvp.Key.x + offset.x, kvp.Key.y + offset.y), out var other) || other > kvp.Value)).Sum(kvp => kvp.Value + 1);
    }

    public static long Part2(string input)
    {
        var map = new Map(input);

        return map.Coordinates.Select(map.FloodFill)
                  .OrderDescending()
                  .Take(3)
                  .Product();
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}