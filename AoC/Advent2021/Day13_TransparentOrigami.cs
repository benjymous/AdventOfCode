namespace AoC.Advent2021;
public class Day13 : IPuzzle
{
    [method: Regex(@"fold along ([x|y])=(\d+)")]
    class Fold(char dir, int foldLine)
    {
        public IEnumerable<(int x, int y)> Perform(IEnumerable<(int x, int y)> dots)
        {
            return from dot in dots
                   select dir == 'x'
                       ? dot.x < foldLine ? dot : (foldLine - (dot.x - foldLine), dot.y)
                       : dot.y < foldLine ? dot : (dot.x, foldLine - (dot.y - foldLine));
        }
    }

    static string Display(HashSet<(int x, int y)> dots)
    {
        StringBuilder sb = new();
        sb.AppendLine();

        var maxX = dots.Max(v => v.x);
        var maxY = dots.Max(v => v.y);

        for (int y = 0; y <= maxY; ++y)
        {
            for (int x = 0; x <= maxX; ++x)
            {
                sb.Append(dots.Contains((x, y)) ? "▊▊" : "  ");
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

    static (IEnumerable<(int x, int y)>, IEnumerable<Fold>) Parse(string input)
    {
        var (coordinates, folds) = input.Split("\n\n").Decompose2();
        return
        (
            Util.Parse<ManhattanVector2>(coordinates).Select(v => v.AsSimple()),
            Util.RegexParse<Fold>(folds).ToArray()
        );
    }

    public static int Part1(string input)
    {
        (var dots, var folds) = Parse(input);

        var data = folds.First().Perform(dots).ToHashSet();

        return data.Count;
    }

    public static string Part2(string input, ILogger logger = null)
    {
        (var dots, var folds) = Parse(input);

        var data = folds.Aggregate(dots, (folded, fold) => fold.Perform(folded)).ToHashSet();

        var display = Display(data);
        logger?.WriteLine(display);

        return display.GetMD5String(false);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input, logger));
    }
}