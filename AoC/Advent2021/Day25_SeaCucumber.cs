namespace AoC.Advent2021;
public class Day25 : IPuzzle
{
    public static int Part1(string input)
    {
        var state = Util.ParseSparseMatrix<PackedPos32, char>(input, new Util.Convertomatic.SkipChars('.'));

        bool moved = false;
        int moves = 0;

        int maxx = state.Max(k => k.Key.X) + 1, maxy = state.Max(k => k.Key.Y) + 1;

        Dictionary<PackedPos32, char> next;

        do
        {
            moved = false;

            for (int phase = 0; phase < 2; ++phase)
            {
                var (moving, waiting) = phase == 0 ? ('>', 'v') : ('v', '>');

                next = state.Where(kvp => kvp.Value == waiting).ToDictionary();

                foreach (var cell in state.Where(kvp => kvp.Value == moving).Select(kvp => kvp.Key))
                {
                    PackedPos32 dest = phase == 0 ? ((cell.X + 1) % maxx, cell.Y) : (cell.X, (cell.Y + 1) % maxy);
                    if (state.ContainsKey(dest)) dest = cell;
                    else moved = true;
                    next[dest] = moving;
                }

                state = next;
            }
            moves++;

        } while (moved);

        return moves;
    }

    public void Run(string input, ILogger logger) => logger.WriteLine("- Pt1 - " + Part1(input));
}