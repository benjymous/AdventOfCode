namespace AoC.Advent2018;
public class Day17 : IPuzzle
{
    public class Wall
    {
        [Regex(@"(.)=(\d+), .=(\d+)..(\d+)")]
        public Wall(char xOrY, int v1, int v2, int v3)
        {
            for (int v = v2; v <= v3; ++v)
            {
                Vals.Add(xOrY == 'x' ? (v1, v) : (v, v1));
            }
        }

        public HashSet<(int x, int y)> Vals = [];
    }

    static bool IsWall(int x, int y, Dictionary<(int x, int y), char> grid) => grid.TryGetValue((x, y), out var c) && c == '#';

    static bool IsFilled(int x, int y, Dictionary<(int x, int y), char> grid) => (grid.TryGetValue((x, y), out var c) && c == '#') || c == '~';

    static void Set((int x, int y) pos, Dictionary<(int x, int y), char> grid, int maxY)
    {
        if (grid.ContainsKey(pos)) return;

        grid[pos] = '|';

        if (pos.y < maxY) Set((pos.x, pos.y + 1), grid, maxY);

        if (IsFilled(pos.x, pos.y + 1, grid))
        {
            int x1 = pos.x;
            int x2 = pos.x;

            while (!IsWall(x1 - 1, pos.y, grid) && IsFilled(x1 - 1, pos.y + 1, grid))
            {
                x1--;
            }
            bool openLeft = !IsWall(x1 - 1, pos.y, grid);

            while (!IsWall(x2 + 1, pos.y, grid) && IsFilled(x2 + 1, pos.y + 1, grid))
            {
                x2++;
            }
            bool openRight = !IsWall(x2 + 1, pos.y, grid);

            if (!openLeft && !openRight)
            {
                for (int x = x1; x <= x2; ++x)
                    grid[(x, pos.y)] = '~';
            }
            else
            {
                if (openLeft || x1 < pos.x)
                    Set((pos.x - 1, pos.y), grid, maxY);

                if (openRight || x2 > pos.x)
                    Set((pos.x + 1, pos.y), grid, maxY);
            }
        }
    }

    private static Dictionary<(int x, int y), char> RunWater(Parser.AutoArray<Wall> walls)
    {
        var data = walls.SelectMany(w => w.Vals).Distinct().ToDictionary(p => p, p => '#');
        var (minY, maxY) = data.Keys.MinMax(p => p.y);

        Set((500, minY), data, maxY);

        return data;
    }

    public static int Part1(string input)
    {
        Dictionary<(int x, int y), char> data = RunWater(input);

        return data.Values.Count(v => v is '|' or '~');
    }

    public static int Part2(string input)
    {
        Dictionary<(int x, int y), char> data = RunWater(input);

        return data.Values.Count(v => v == '~');
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
