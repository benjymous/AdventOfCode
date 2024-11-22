namespace AoC.Advent2018;
public class Day14 : IPuzzle
{
    static IEnumerable<int> Combine(int sum)
    {
        if (sum > 9) yield return sum / 10;
        yield return sum % 10;
    }

    private static void StepRecipe(List<int> recipe, ref int[] current, int required)
    {
        while (recipe.Count < required)
        {
            recipe.AddRange(Combine(recipe[current[0]] + recipe[current[1]]));

            for (int j = 0; j < 2; ++j)
            {
                current[j] = (current[j] + 1 + recipe[current[j]]) % recipe.Count;
            }
        }
    }

    public static string Part1(int start, int keep)
    {
        var recipe = new List<int> { 3, 7 };

        int[] current = [0, 1];

        StepRecipe(recipe, ref current, start + keep);

        return string.Concat(recipe).Substring(start, keep);
    }

    public static int Part2(string input)
    {
        var recipe = new List<int> { 3, 7 };
        var toFind = input.Trim().Select(c => c.AsDigit()).ToArray();

        int[] current = [0, 1];

        int searchPos = 0;

        while (true)
        {
            StepRecipe(recipe, ref current, searchPos + toFind.Length);

            bool found = true;
            for (int i = 0; i < toFind.Length; ++i)
            {
                if (recipe[i + searchPos] != toFind[i]) { found = false; break; }
            }
            if (found) return searchPos;

            searchPos++;
        }
    }

    public void Run(string input, ILogger logger)
    {
        input = input.Trim();

        logger.WriteLine("- Pt1 - " + Part1(int.Parse(input), 10));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
