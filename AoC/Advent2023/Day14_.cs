namespace AoC.Advent2023;
public class Day14 : IPuzzle
{

    static List<(List<(int x, int y)> positions, int rockCount)> FindZones(HashSet<(int x, int y)> rocks, HashSet<(int x, int y)> obstacles, int maxx, int maxy)
    {
        List<(List<(int x, int y)> positions, int rockCount)> res = [];

        for (int x = 0; x <= maxx; ++x)
        {
            (List<(int x, int y)> positions, int rockCount) entry = ([], 0);
            for (int y = 0; y <= maxy; ++y)
            {
                if (obstacles.Contains((x, y)))
                {
                    if (entry.rockCount > 0) res.Add(entry);

                    entry = ([], 0);
                    continue;
                }

                if (rocks.Contains((x, y))) entry.rockCount++;

                entry.positions.Add((x, y));
            }
            if (entry.rockCount > 0) res.Add(entry);
        }

        return res;
    }

    public static int Part1(string input)
    {
        var grid = Util.ParseSparseMatrix<char>(input);

        HashSet<(int x, int y)> rocks = grid.Where(kvp => kvp.Value == 'O').Select(kvp => kvp.Key).ToHashSet();

        HashSet<(int x, int y)> obstacles = grid.Where(kvp => kvp.Value == '#').Select(kvp => kvp.Key).ToHashSet();

        var maxx = grid.Max(kvp => kvp.Key.x);
        var maxy = grid.Max(kvp => kvp.Key.y);

        var zones = FindZones(rocks, obstacles, maxx, maxy);

        return zones.Sum(z => z.positions.Take(z.rockCount).Sum(el => maxy + 1 - el.y));

    }

    public static int Part2(string input)
    {

        return 0;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
