namespace AoC.Advent2017;
public class Day14 : IPuzzle
{
    public static IEnumerable<PackedPos32> BuildBits(string input) =>
        ParallelEnumerable.Range(0, 128).SelectMany(y =>
             Day10.KnotHash($"{input}-{y}").SelectMany(h => h.BinarySequence(0xff).Reverse()).Select((v, x) => (v, x)).Where(d => d.v == 1).Select(d => (PackedPos32)(d.x, y))
        ).ToArray();

    static void FloodFill(PackedPos32 pos, HashSet<PackedPos32> matrix)
    {
        if (!matrix.Contains(pos)) return;
        matrix.Remove(pos);
        FloodFill(pos + (0, 1), matrix);
        FloodFill(pos - (0, 1), matrix);
        FloodFill(pos + (1, 0), matrix);
        FloodFill(pos - (1, 0), matrix);
    }

    public static int Part1(IEnumerable<PackedPos32> data) => data.Count();

    public static int Part2(IEnumerable<PackedPos32> data)
    {
        var matrix = data.ToHashSet();

        int groups = 0;

        while (matrix.Count != 0)
        {
            var pos = matrix.First();
            FloodFill(pos, matrix);
            groups++;
        }

        return groups;
    }

    public static int Part1(string input) => Part1(BuildBits(input.Trim()));

    public static int Part2(string input) => Part2(BuildBits(input.Trim()));

    public void Run(string input, ILogger logger)
    {
        var data = BuildBits(input);
        logger.WriteLine("- Pt1 - " + Part1(data));
        logger.WriteLine("- Pt2 - " + Part2(data));
    }
}