namespace AoC.Advent2024;

public class Day21 : IPuzzle
{
    static readonly string numericKeypad = "789\n456\n123\n 0A";
    static readonly string directionPad = " ^A\n<v>";

    public static long GetScore(string line, int level = 2)
    {
        var numPad = Util.ParseSparseMatrix<char>(numericKeypad, new Util.Convertomatic.SkipChars(' '));
        var dirPad = Util.ParseSparseMatrix<char>(directionPad, new Util.Convertomatic.SkipChars(' '));

        return CalculateScore(line, level + 1, numPad, dirPad) * int.Parse(line.Replace("A", ""));
    }

    private static long CalculateScore(string line, int level, Dictionary<(int, int), char> numPad, Dictionary<(int, int), char> dirPad)
    {
        if (level == 0 || line=="A")
        {
            return line.Length;
        }

        var parts = line.Split('A', StringSplitOptions.RemoveEmptyEntries).Select(p => p + "A").ToArray();

        if (parts.Length == 1)
        {
            return ("A" + line).OverlappingPairs().Sum(pair => GetKeyCombos(pair.first, pair.second, level, numPad, dirPad));
        }
        else
        {
            return parts.Sum(subpart => CalculateScore(subpart, level - 1, numPad, dirPad));
        }
    }

    public static long GetKeyCombos(char from, char to, int level, Dictionary<(int, int), char> numPad, Dictionary<(int, int), char> dirPad)
    {
        return Memoize((from, to, level), _ =>
        {
            Dictionary<(int x, int y), char> pad = from.IsDigit() || to.IsDigit() ? numPad : dirPad;

            Queue<(string current, (int x, int y) position)> queue = [];
            queue.Enqueue(("", pad.KeysWithValue(from).Single()));

            long smallest = long.MaxValue;
            var desired = pad.KeysWithValue(to).Single();

            while (queue.TryDequeue(out var state))
            {
                if (!pad.ContainsKey(state.position)) continue;

                if (state.position == desired)
                {
                    smallest = Math.Min(smallest, CalculateScore(state.current + 'A', level - 1, numPad, dirPad));
                }
                else
                {
                    var dx = Math.Sign(desired.x - state.position.x);
                    var dy = Math.Sign(desired.y - state.position.y);

                    if (dx != 0)
                    {
                        queue.Enqueue((state.current + (dx == -1 ? '<' : '>'), (state.position.x + dx, state.position.y)));
                    }
                    if (dy != 0)
                    {
                        queue.Enqueue((state.current + (dy == -1 ? '^' : 'v'), (state.position.x, state.position.y + dy)));
                    }
                }
            }

            return smallest;
        });
    }

    public static long Part1(string input) => Util.Split(input).Sum(line => GetScore(line));

    public static long Part2(string input) => Util.Split(input).Sum(line => GetScore(line, 25));

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}