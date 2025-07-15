namespace AoC.Advent2024;
public class Day06 : IPuzzle
{
    private static (HashSet<(int, int)> seen, bool looped) SimulateGuard(string input, (int, int)? obstacle = null)
    {
        var data = Memoize(input, _ => Util.ParseSparseMatrix<char>(input, new Util.Convertomatic.SkipChars('.')));

        var map = data.Keys.ToHashSet();

        if (obstacle != null) map.Add(obstacle.Value);
        (int x, int y) pos = data.SingleWithValue('^');
        map.Remove(pos);

        var dir = new Direction2('^');
        HashSet<((int, int) pos, char)> seen = [];

        while (true)
        {
            if (!data.IsInside(pos)) return (seen.Select(e => e.pos).ToHashSet(), false);
            if (!seen.Add((pos, dir))) return (seen.Select(e => e.pos).ToHashSet(), true);

            var next = pos.OffsetBy(dir);
            if (map.Contains(next))
            {
                dir.TurnRight();
            }
            else
            {
                pos = next;
            }
        }
    }

    public static int Part1(string input) => SimulateGuard(input).seen.Count;

    public static int Part2(string input)
    {
        var (seen, looped) = SimulateGuard(input);
        return seen.AsParallel().Count(pos => SimulateGuard(input, pos).looped);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}