namespace AoC.Advent2018;
public class Day06 : IPuzzle
{
    public static int Part1(string input)
    {
        var data = Parser.Parse<ManhattanVector2>(input).Select(v => v.AsSimple()).ToArray();

        var width = data.Max(pos => pos.x);
        var height = data.Max(pos => pos.y);

        var edges = new HashSet<int>();

        Dictionary<int, int> counts = [];

        for (var y = 0; y < height; ++y)
        {
            for (var x = 0; x < width; ++x)
            {
                int smallestTally = 0;
                var smallest = int.MaxValue;
                var smallestIdx = -1;

                for (var i = 0; i < data.Length; ++i)
                {
                    var d = data[i].Distance(x, y);
                    if (d < smallest)
                    {
                        smallest = d;
                        smallestIdx = i;
                        smallestTally = 1;
                    }
                    else if (d == smallest) smallestTally++;
                }

                if (smallestTally == 1)
                {
                    if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                    {
                        edges.Add(smallestIdx);
                    }
                    else counts.IncrementAtIndex(smallestIdx);
                }
            }
        }

        counts.RemoveRange(edges);
        return counts.Max(kvp => kvp.Value);
    }

    public static int Part2(string input, int safeDistance)
    {
        var data = Parser.Parse<ManhattanVector2>(input).Select(v => v.AsSimple()).ToArray();

        var width = data.Max(pos => pos.x);
        var height = data.Max(pos => pos.y);

        return ParallelEnumerable.Range(0, height).Sum(y => Enumerable.Range(0, width).Count(x => data.Select(e => e.Distance(x, y)).Sum() < safeDistance));
    }

    public static int Part2(string input) => Part2(input, 10000);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}