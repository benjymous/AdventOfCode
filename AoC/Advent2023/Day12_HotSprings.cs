namespace AoC.Advent2023;
public class Day12 : IPuzzle
{
    [method: Regex(@"(.+) (.+)")]
    public record class Row(string Input, int[] Lengths);

    static T[] Unfold<T>(IEnumerable<T> input, bool addJoiner = false, T join = default)
    {
        List<T> output = [];
        for (int i = 0; i < 5; ++i)
        {
            output.AddRange(input);
            if (addJoiner && i < 4) output.Add(join);
        }
        return [.. output];
    }

    public static long Solve(string input, int[] lengths, int currentCount = 0) =>
        Memoize((input, lengths.GetCombinedHashCode(), currentCount), _ =>
    {
        if (input.Length == 0) return lengths.Length == 0 ? 1 : 0;

        int i = 0, count = currentCount;

        do
        {
            while (i < input.Length && input[i] == '#')
            {
                i++; count++; // count the set blocks
            }

            if (count > 0 && (lengths.Length == 0 || count > lengths[0])) return 0;

            if (i == input.Length || (input[i] == '.' && count > 0))
            {
                // end of block, compare against lengths
                if (count != lengths[0]) return 0; // this block doesn't match the first length
                if (lengths.Length == 1 && i >= input.Length) return 1; // last length matches, so full match

                lengths = lengths[1..];
                count = 0;
            }

            while (i < input.Length && input[i] == '.') i++;
        } while (i < input.Length && input[i] != '?');

        if (i >= input.Length && lengths.Length == 0) return 1; // full match

        if (i < input.Length && input[i] == '?')
        {
            long res = Solve(string.Concat("#", input.AsSpan(i + 1)), lengths, count);

            if (count > 0 && (count == lengths[0])) // last block matches
            {
                lengths = lengths[1..];
                count = 0;
            }

            return count == 0 ? res + Solve(input[(i + 1)..], lengths, count) : res;
        }

        return 0;
    });

    public static long Part1(string input)
        => Util.RegexParse<Row>(input).ToArray().Sum(row => Solve(row.Input, row.Lengths));

    public static long Part2(string input)
        => Util.RegexParse<Row>(input).ToArray().Sum(row => Solve(Unfold(row.Input, true, '?').AsString(), Unfold(row.Lengths)));

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}