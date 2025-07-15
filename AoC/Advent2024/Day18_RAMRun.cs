namespace AoC.Advent2024;
public class Day18 : IPuzzle
{
    public static (int x, int y)[] ParseData(string input)
        => [.. Parser.Parse<ManhattanVector2>(input).Select(p => p.AsSimple())];

    public record class Map(HashSet<(int, int)> Data, (int x, int y) Target) : IMap<(int, int)>
    {
        private static readonly (int dx, int dy)[] Neighbours = [(1, 0), (0, 1), (0, -1), (-1, 0)];

        public bool IsInside((int x, int y) p)
            => p.x >= 0 && p.y >= 0 && p.x <= Target.x && p.y <= Target.y;

        public IEnumerable<(int, int)> GetNeighbours((int, int) location)
            => Neighbours.Select(n => location.OffsetBy(n))
                .Where(n => !Data.Contains(n) && IsInside(n));

        public int FindPath() => this.FindPath((0, 0), Target).Length;
    }

    public static int Part1(string input, int targetX = 70, int targetY = 70, int simulateAt = 1024)
        => new Map([.. ParseData(input).Take(simulateAt)], (targetX, targetY)).FindPath();

    public static string Part2(string input, int targetX = 70, int targetY = 70)
    {
        var data = ParseData(input);

        var (x, y) = Util.BinarySearch(0, data.Length, iter =>
        {
            var fallen = data.Take(iter).ToArray();
            var m = new Map([.. fallen], (targetX, targetY));
            return (m.FindPath() == 0, fallen.Last());
        }).result;

        return $"{x},{y}";
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}