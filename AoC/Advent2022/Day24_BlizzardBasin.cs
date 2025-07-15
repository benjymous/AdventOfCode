namespace AoC.Advent2022;
public class Day24 : IPuzzle
{
    private static PackedPos32 ToDirection(char dir) => dir switch { '>' => (1, 0), '<' => (-1, 0), '^' => (0, -1), 'v' => (0, 1), _ => 0 };

    private static readonly PackedPos32[] Directions = [(0, -1), (1, 0), (0, 1), (-1, 0), 0];

    private static int Solve(string input, QuestionPart part)
    {
        var map = Util.ParseSparseMatrix<PackedPos32, char>(input, new Util.Convertomatic.SkipChars('.'));

        (int w, int h) = (map.Width - 1, map.Height - 1);

        HashSet<PackedPos32> walls = [.. map.KeysWithValue('#'), (1, -1), (w, h + 2)];
        var blizzards = map.Where(kvp => kvp.Value != '#').Select(kvp => (pos: kvp.Key, dir: ToDirection(kvp.Value))).ToArray();

        var blizH = blizzards.Where(b => b.dir == -1 || b.dir == 1).ToArray();
        var blizV = blizzards.Except(blizH).ToArray();

        HashSet<PackedPos32>[] blizStepsH = new HashSet<PackedPos32>[w + 1], blizStepsV = new HashSet<PackedPos32>[h + 1];

        for (int i = 0; i <= Math.Max(w, h); ++i)
        {
            if (i <= w) blizStepsH[i] = StepBlizzards(blizH, w, h);
            if (i <= h) blizStepsV[i] = StepBlizzards(blizV, w, h);
        }

        var (start, end) = ((1, 0), (w, h + 1));

        Queue<PackedPos32> waypoints = part.One ? [end] : [end, start, end];
        HashSet<PackedPos32> generation = [start];

        for (int step = 0; ; ++step)
        {
            generation = [.. generation.SelectMany(p => Directions.Select(dir => p + dir))
                .Where(newPos => !blizStepsH[step % w].Contains(newPos) && !blizStepsV[step % h].Contains(newPos) && !walls.Contains(newPos))
                .OrderBy(p => Math.Abs(p.X - waypoints.Peek().X)).Take(50)];

            if (generation.Contains(waypoints.Peek()))
            {
                generation = [(waypoints.Dequeue())];
                if (waypoints.Count == 0) return step + 1;
            }
        }
    }

    private static HashSet<PackedPos32> StepBlizzards((PackedPos32 pos, PackedPos32 dir)[] blizzards, int w, int h)
    {
        for (int i = 0; i < blizzards.Length; ++i)
        {
            var pos = blizzards[i].pos + blizzards[i].dir;
            blizzards[i].pos = (((pos.X + w - 1) % w) + 1, ((pos.Y + h - 1) % h) + 1);
        }
        return [.. blizzards.Select(b => b.pos)];
    }

    public static int Part1(string input) => Solve(input, QuestionPart.Part1);

    public static int Part2(string input) => Solve(input, QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}