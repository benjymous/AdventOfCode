namespace AoC.Advent2024;
public class Day14 : IPuzzle
{
    [Regex(@"p=(.+) v=(.+)")]
    private record struct Guard([Regex(@"(\d+),(\d+)")] (int x, int y) Pos, [Regex(@"(-?\d+),(-?\d+)")] (int x, int y) Speed)
    {
        public (int, int) StepGuard(int steps, int width, int height)
        {
            var (x, y) = Pos.OffsetBy(Speed, steps);

            x.ModWrap(width);
            y.ModWrap(height);

            return Pos = (x, y);
        }
    }

    public static long Part1(string input, int width = 101, int height = 103)
    {
        var data = Parser.Parse<Guard>(input).ToArray();

        for (int i = 0; i < data.Length; ++i)
        {
            data[i].StepGuard(100, width, height);
        }

        int[] quadrants = new int[4];
        int hw = width / 2, hh = height / 2;
        foreach (var (x, y) in data.Select(g => g.Pos))
        {
            if (x < hw && y < hh) quadrants[0]++;
            else if (x < hw && y > hh) quadrants[1]++;
            else if (x > hw && y < hh) quadrants[2]++;
            else if (x > hw && y > hh) quadrants[3]++;
        }

        return quadrants.Product();
    }

    public static int Part2(string input)
    {
        int width = 101;
        int height = 103;

        var data = Parser.Parse<Guard>(input).ToArray();

        int iter = 1;
        while (true)
        {
            HashSet<(int, int)> dupes = [];
            bool unique = true;

            for (int i = 0; i < data.Length; ++i)
            {
                var pos = data[i].StepGuard(1, width, height);
                if (unique) unique &= dupes.Add(pos);
            }

            if (unique) return iter;

            iter++;
        }
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}