namespace AoC.Advent2024;
public class Day16 : IPuzzle
{
    private static IEnumerable<((int, int) newPos, char newDir, int cost)> GetMoves(HashSet<(int, int)> walkable, (int, int) pos, Direction2 d)
    {
        var forward = pos.OffsetBy(d);
        if (walkable.Contains(forward)) yield return (forward, d, 1);
        var d2 = d - 1;
        if (walkable.Contains(pos.OffsetBy(d2))) yield return (pos, d2, 1000);
        var d3 = d + 1;
        if (walkable.Contains(pos.OffsetBy(d3))) yield return (pos, d3, 1000);
    }

    private static (int best, int visited) Solve(string input)
    {
        return Memoize(input, _ =>
        {
            var grid = Util.ParseSparseMatrix<char>(input, new Util.Convertomatic.SkipChars('#'));
            var walkable = grid.Keys.ToHashSet();
            var start = grid.SingleWithValue('S');
            var end = grid.SingleWithValue('E');

            PriorityQueue<((int x, int y) pos, char dir, PackedPos32[] history), int> queue = new();
            queue.Enqueue((start, '>', [start]), 0);

            Dictionary<((int, int), char), int> seen = [];

            int best = int.MaxValue;
            HashSet<PackedPos32> bestVisited = [];

            while (queue.TryDequeue(out var step, out int score))
            {
                if (step.pos == end)
                {
                    if (score <= best)
                    {
                        if (best > score)
                            bestVisited = [.. step.history];
                        else
                            bestVisited.UnionWith(step.history);
                        best = score;
                    }

                    continue;
                }

                if (score >= best || (seen.TryGetValue((step.pos, step.dir), out var prev) && prev < score)) continue;
                seen[(step.pos, step.dir)] = score;

                queue.EnqueueRange(GetMoves(walkable, step.pos, step.dir)
                     .Select(m => ((m.newPos, m.newDir, (PackedPos32[])[m.newPos, .. step.history]), score + m.cost)));
            }
            return (best, bestVisited.Count);
        });
    }

    public static int Part1(string input) => Solve(input).best;
    public static int Part2(string input) => Solve(input).visited;

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}