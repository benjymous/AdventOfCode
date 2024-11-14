namespace AoC.Advent2015;
public class Day11 : IPuzzle
{
    public static bool IsBad(char c) => c is 'o' or 'i' or 'l';

    public static char[] Increment(char[] pwd)
    {
        var newPwd = pwd.ToArray();
        int i = pwd.Length - 1;
        while (true)
        {
            newPwd[i]++;
            if (newPwd[i] <= 'z')
            {
                if (!IsBad(newPwd[i])) return newPwd;
            }
            else newPwd[i--] = 'a';
        }
    }

    public static bool HasStraight(char[] line)
    {
        for (int i = 0; i < line.Length - 2; ++i)
        {
            if (line[i] == line[i + 1] - 1 && line[i] == line[i + 2] - 2) return true;
        }
        return false;
    }

    public static bool NoBads(char[] line) => !line.Any(IsBad);

    public static bool HasTwoNonOverlappingPairs(char[] line)
    {
        int pairs = 0;
        for (int i = 0; i < line.Length - 1; ++i)
        {
            if (line[i] == line[i + 1])
            {
                pairs++;
                i++;
            }
        }
        return pairs > 1;
    }

    public static bool IsValid(char[] line) => HasStraight(line) && HasTwoNonOverlappingPairs(line) && NoBads(line);

    public static char[] FindNextValid(char[] input)
    {
        do input = Increment(input); while (!IsValid(input));

        return input;
    }

    public static string Part1(string input) => FindNextValid([.. input.Trim()]).AsString();

    public static string Part2(string input) => FindNextValid(FindNextValid([.. input.Trim()])).AsString();

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}