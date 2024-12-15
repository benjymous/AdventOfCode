namespace AoC.Advent2024;
public class Day14 : IPuzzle
{
    [Regex(@"p=(.+) v=(.+)")]
    record struct Guard([Regex(@"(\d+),(\d+)")] (int x, int y) pos, [Regex(@"(-?\d+),(-?\d+)")] (int x, int y) speed)
    {
        public (int,int) StepGuard(int steps, int width, int height)
        {
            var finalPos = pos.OffsetBy(speed, steps);
            while (finalPos.x < width) finalPos.x += width;
            while (finalPos.y < height) finalPos.y += height;
            finalPos.x %= width;
            finalPos.y %= height;
            pos = finalPos;
            return finalPos;
        }
    }

    public static long SolvePt1(string input, int width, int height)
    {
        var data = Parser.Parse<Guard>(input).ToArray();

        for (int i = 0; i < data.Length; ++i)
        {
            data[i].StepGuard(100, width, height);
        }

        int[] quadrants = new int[4];
        var hw = width / 2;
        var hh = height / 2;
        foreach (var (x, y) in data.Select(g => g.pos))
        {
            if (x < hw && y < hh) quadrants[0]++;
            else if (x < hw && y > hh) quadrants[1]++;
            else if (x > hw && y < hh) quadrants[2]++;
            else if (x > hw && y > hh) quadrants[3]++;
        }

        return quadrants.Product();
    }

    public static long Part1(string input) => SolvePt1(input, 101, 103);

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
                unique &= dupes.Add(data[i].StepGuard(1, width, height));
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