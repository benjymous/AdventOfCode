namespace AoC.Advent2018;
public class Day11 : IPuzzle
{
    public static int Power(int serial, int x, int y) => ((((x + 10) * y) + serial) * (x + 10) / 100 % 10) - 5;

    private static int[,] InitGrid(string input)
    {
        int serial = int.Parse(input);

        var grid = new int[301, 301];

        for (var y = 1; y <= 300; ++y)
            for (var x = 1; x <= 300; ++x)
                grid[y, x] = Power(serial, x, y);

        return grid;
    }

    private static int CalcScore(int[,] grid, int size, int x, int y)
    {
        var score = 0;

        for (var ya = 0; ya < size; ++ya)
            for (var xa = 0; xa < size; ++xa)
                score += grid[y + ya, x + xa];

        return score;
    }

    public static string Part1(string input)
    {
        int[,] grid = InitGrid(input);

        var max = 0;
        (int x, int y) pos = (0, 0);

        for (var y = 1; y < 298; ++y)
        {
            for (var x = 1; x < 298; ++x)
            {
                int score = CalcScore(grid, 3, x, y);

                if (score > max)
                {
                    max = score;
                    pos = (x, y);
                }
            }
        }

        return $"{pos.x},{pos.y}";
    }

    public static string Part2(string input)
    {
        int[,] grid = InitGrid(input);

        var max = 0;
        var lastBest = 0;
        ManhattanVector3 pos = null;

        for (var size = 1; size < 300; ++size)
        {
            int sizeBest = 0;

            for (var y = 1; y < 300 - size; ++y)
            {
                for (var x = 1; x < 300 - size; ++x)
                {
                    int score = CalcScore(grid, size, x, y);

                    if (score > max)
                    {
                        max = score;
                        pos = new ManhattanVector3(x, y, size);
                    }
                    if (score > sizeBest) sizeBest = score;
                }
            }

            if (sizeBest + 10 < lastBest) return pos.ToString();
            lastBest = sizeBest;
        }

        return pos.ToString();
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}