namespace AoC.Advent2025;

public class Day12 : IPuzzle
{
    [Regex(@"\d:\s(.+\s.+\s.+)")]
    record class Shape(string Data)
    {
        public int Count = Data.Count(v => v == '#');
    }

    [Regex(@"(\d+)x(\d+): (.+)")]
    record class Grid(int W, int H, [Split(" ")] int[] Contents)
    {
        public bool CanSolve(Shape[] shapes) => Contents.Index().Sum(v => shapes[v.Index].Count * Contents[v.Index]) <= W * H;
    }

    public static int Part1(string input)
    {
        var sections = input.SplitSections();

        int numShapes = sections.Length - 1;

        var shapes = Parser.Parse<Shape>(sections.Take(numShapes)).ToArray();

        var grids = Parser.Parse<Grid>(sections.Last());

        return grids.Count(g => g.CanSolve(shapes));
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
    }
}