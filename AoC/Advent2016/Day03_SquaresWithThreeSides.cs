namespace AoC.Advent2016;
public class Day03 : IPuzzle
{
    public static bool TriangleValid(int[] row) => row[0] + row[1] > row[2] && row[0] + row[2] > row[1] && row[1] + row[2] > row[0];

    public static int Part1(string input)
    {
        var lines = Util.Split(input);
        var data = lines.Select(line => Util.ParseNumbers<int>(line, " "));

        return data.Count(TriangleValid);
    }

    public static int Part2(string input)
    {
        var numbers = Util.ParseNumbers<int>(input.Replace("\n", " "), " ");

        var triangles = new Queue<List<int>>();

        var count = 0;

        for (var i = 0; i < 3; ++i)
        {
            triangles.Enqueue([]);
        }

        foreach (var num in numbers)
        {
            var current = triangles.Dequeue();
            current.Add(num);

            if (current.Count == 3)
            {
                count += TriangleValid([.. current]) ? 1 : 0;
                current.Clear();
            }
            triangles.Enqueue(current);
        }
        return count;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}