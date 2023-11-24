namespace AoC.Advent2017;
public class Day09 : IPuzzle
{
    public static (IEnumerable<char> stripped, int garbageCount) StripGarbage(string input)
    {
        List<char> result = [];
        bool inGarbage = false;
        int garbageCount = 0;
        for (int i = 0; i < input.Length; i++)
        {
            char ch = input[i];
            if (inGarbage)
            {
                if (ch == '!') i++;
                else if (ch == '>') inGarbage = false;
                else garbageCount++;
            }
            else
            {
                if (ch == '<') inGarbage = true;
                else result.Add(ch);
            }
        }
        return (result.AsString(), garbageCount);
    }

    public static IEnumerable<int> GetGroups(string input)
    {
        int currentScore = 0;
        foreach (var ch in StripGarbage(input).stripped)
        {
            if (ch == '{') currentScore++;
            else if (ch == '}') yield return currentScore--;
        }
    }

    public static int GetScore(string input) => GetGroups(input).Sum();

    public static int CountGarbage(string input) => StripGarbage(input).garbageCount;

    public static int Part1(string input) => GetScore(input);

    public static int Part2(string input) => CountGarbage(input);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}