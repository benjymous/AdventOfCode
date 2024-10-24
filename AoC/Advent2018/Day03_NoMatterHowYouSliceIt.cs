namespace AoC.Advent2018;
public class Day03 : IPuzzle
{
    [Regex(@"#(\d+) @ (\d+),(\d+): (\d+)x(\d+)")]
    record struct Shape(string ID, int Left, int Top, int Width, int Height)
    {
        public readonly IEnumerable<PackedPos32> Squares() => Util.Range2DExclusive((Top, Top + Height, Left, Left + Width)).Select(k => (PackedPos32)k);
    }

    static HashSet<PackedPos32> FindOverlaps(IEnumerable<Shape> shapes)
    {
        Dictionary<PackedPos32, int> square = [];
        foreach (var squareId in shapes.SelectMany(s => s.Squares()))
        {
            square.IncrementAtIndex(squareId);
        }

        return square.Where(kvp => kvp.Value > 1).Select(kvp => kvp.Key).ToHashSet();
    }

    public static int Part1(string input)
    {
        var shapes = Util.RegexParse<Shape>(input);
        return FindOverlaps(shapes).Count;
    }

    public static string Part2(string input)
    {
        var shapes = Util.RegexParse<Shape>(input);
        var overlaps = FindOverlaps(shapes);

        return shapes.First(shape => !overlaps.Overlaps(shape.Squares())).ID;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
