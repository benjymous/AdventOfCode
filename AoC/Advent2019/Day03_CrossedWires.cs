namespace AoC.Advent2019;
public class Day03 : IPuzzle
{
    public enum SearchMode
    {
        Closest,
        Shortest
    }

    [Regex(@"([LRUD])(\d+)")] public record class Command(Direction2 Dir, int Count)
    {
        public readonly PackedPos32 IntDir = new(Dir.DX, Dir.DY);
    }

    public static int FindIntersection(Parser.AutoArray<Command[]> data, SearchMode mode)
    {
        var minDist = int.MaxValue;

        Dictionary<int, int> seen = [];

        foreach (var line in data)
        {
            PackedPos32 position = new(1000, 1000);
            int steps = 0;
            Dictionary<int, int> current = [];

            foreach (var cmd in line)
            {
                for (var j = 0; j < cmd.Count; ++j)
                {
                    position += cmd.IntDir;
                    steps++;

                    if (seen.TryGetValue(position, out int value))
                    {
                        minDist = Math.Min(minDist, mode == SearchMode.Closest
                            ? Math.Abs(position.X - 1000) + Math.Abs(position.Y - 1000)
                            : steps + value);
                    }
                    else
                    {
                        current[position] = steps;
                    }
                }
            }

            seen = current;
        }

        return minDist;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + FindIntersection(input, SearchMode.Closest));
        logger.WriteLine("- Pt2 - " + FindIntersection(input, SearchMode.Shortest));
    }
}
