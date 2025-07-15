namespace AoC.Advent2020;
public class Day10 : IPuzzle
{
    private static List<int> GetValues(string input)
    {
        var values = Util.ParseNumbers<int>(input).Order().ToList();
        return [0, .. values, values[^1] + 3];
    }

    public static int Part1(string input)
    {
        var values = GetValues(input);

        int diff1 = 0, diff3 = 0;
        for (var i = 0; i < values.Count - 1; ++i)
        {
            int v1 = values[i], v2 = values[i + 1], diff = v2 - v1;

            if (diff == 1) diff1++;
            else if (diff == 3) diff3++;
        }

        return diff1 * diff3;
    }

    private static Dictionary<int, long> GetCombinations(IEnumerable<int> values)
    {
        var results = new Dictionary<int, long> { { 0, 1 } };

        int[] offsets = [1, 2, 3];
        foreach (var val in values)
        {
            long combinations = 0;
            foreach (var offset in offsets)
            {
                var testVal = val - offset;
                if (results.TryGetValue(testVal, out long resVal))
                {
                    combinations += resVal;
                }
            }

            results[val] = combinations;
        }

        return results;
    }

    public static long Part2(string input)
    {
        var values = GetValues(input).Skip(1);

        var results = GetCombinations(values);

        var final = values.Last();

        return results[final];
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}