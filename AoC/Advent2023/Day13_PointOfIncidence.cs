namespace AoC.Advent2023;
public class Day13 : IPuzzle
{
    private static (bool valid, int value, bool isBlotchy) ValidatePairs(string[] vals, Dictionary<int, HashSet<int>> dict, int centre)
    {
        int blotches = 0;
        for (int i = centre, j = centre + 1; i >= 1 && j <= vals.Length; i--, j++)
        {
            if (!dict.TryGetValue(i, out HashSet<int> value) || !value.Contains(j) || (vals[i - 1] != vals[j - 1] && blotches++ == 2))
                return default;
        }
        return (true, centre, blotches > 0);
    }

    private static (bool found, int position, bool isBlotchy) ValidateGroup(HashSet<int> group, string[] vals, Dictionary<int, HashSet<int>> dict)
        => group.Where(v => group.Contains(v + 1)).Select(v => ValidatePairs(vals, dict, v)).FirstOrDefault(r => r.valid);

    static Dictionary<int, HashSet<int>> DoGrouping((int index, string value)[] data, QuestionPart part)
    {
        Dictionary<int, HashSet<int>> dict = [];

        for (int i = 0; i < data.Length; i++)
        {
            var v1 = data[i];
            for (int j = i + 1; j < data.Length; j++)
            {
                var v2 = data[j];

                if (part.Two() ? StringsDifferByNoMoreThanOne(v1.value, v2.value) : v1.value == v2.value)
                {
                    if (dict.TryGetValue(v1.index, out HashSet<int> value))
                    {
                        value.Add(v2.index);
                    }
                    else
                    {
                        dict[v1.index] = [v1.index, v2.index];
                    }
                }
            }
        }

        return dict;
    }

    public static int FindMirror(IEnumerable<IEnumerable<char>> input, QuestionPart part)
    {
        string[] vals = input.Select(c => c.AsString()).ToArray();
        var groups = DoGrouping(vals.WithIndex(1).Select(c => (c.Index, value: c.Value)).ToArray(), part);

        return groups.Select(g => ValidateGroup(g.Value, vals, groups))
                     .FirstOrDefault(v => v.found && v.isBlotchy == part.Two()).position;
    }

    public static int GetMirrorScore(string input, QuestionPart part)
    {
        var g = Util.ParseMatrix<char>(input);

        var col = FindMirror(g.Columns(), part);
        return col > 0 ? col : FindMirror(g.Rows(), part) * 100;
    }

    public static bool StringsDifferByNoMoreThanOne(string in1, string in2)
    {
        if (in1 == in2) return true;
        int diff = 0;

        for (int i = 0; i < in1.Length; ++i)
        {
            if (in1[i] != in2[i] && (++diff > 1)) return false;
        }

        return true;
    }

    public static int Part1(string input) => input.SplitSections().Sum(grid => GetMirrorScore(grid, QuestionPart.Part1));

    public static int Part2(string input) => input.SplitSections().Sum(grid => GetMirrorScore(grid, QuestionPart.Part2));

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}