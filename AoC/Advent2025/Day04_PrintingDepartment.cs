using System.Linq;

namespace AoC.Advent2025;
public class Day04 : IPuzzle
{
    private static List<((int x, int y) Pos, char)> PerformStep(char[,] grid) 
        => grid.Entries().Where(e => e.Value == '@' && 
                             grid.Neighbours(e.Key).Count(v => v.Value == '@') < 4).ToList();

    public static int Part1(string input) 
        => PerformStep(Util.ParseMatrix<char>(input)).Count;

    public static int Part2(string input)
    {
        var grid = Util.ParseMatrix<char>(input);

        int total = 0;

        while (true)
        {
            var remove = PerformStep(grid);
            if (remove.Count == 0) break;
            total += remove.Count;

            foreach (var v in remove)
            {
                grid[v.Pos.x, v.Pos.y] = '.';
            }
        }

        return total;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}