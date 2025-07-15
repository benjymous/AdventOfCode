using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AoC.Advent2015;
public class Day12 : IPuzzle
{
    public static IEnumerable<int> FindNumbers(string input)
    {
        StringBuilder current = new();
        for (int i = 0; i < input.Length; ++i)
        {
            if (input[i] is '-' or (>= '0' and <= '9'))
            {
                current.Append(input[i]);
            }
            else
            {
                if (current.Length > 0)
                {
                    yield return int.Parse(current.ToString());
                    current = new StringBuilder();
                }
            }
        }
        if (current.Length > 0)
        {
            yield return int.Parse(current.ToString());
        }
    }

    private static bool HasRed(dynamic jsonObj)
    {
        if (jsonObj.red != null) return true;

        foreach (var child in jsonObj)
        {
            if (child.Value.Type == JTokenType.String && child.Value == "red")
                return true;
        }

        return false;
    }

    private static int GetSum(dynamic jsonObj)
    {
        if (jsonObj.Type == JTokenType.String) return 0;
        if (jsonObj.Type == JTokenType.Integer) return (int)jsonObj;

        if (jsonObj.Type == JTokenType.Array)
        {
            int sum = 0;

            foreach (var child in jsonObj)
            {
                sum += GetSum(child);
            }
            return sum;
        }

        if (HasRed(jsonObj))
        {
            return 0;
        }
        else
        {
            int sum = 0;
            foreach (var child in jsonObj)
            {
                sum += GetSum(child.Value);
            }
            return sum;
        }
    }

    public static int Part1(string input) => FindNumbers(input).Sum();

    public static int Part2(string input) => GetSum((dynamic)JsonConvert.DeserializeObject(input));

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}