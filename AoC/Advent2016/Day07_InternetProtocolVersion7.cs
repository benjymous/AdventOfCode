namespace AoC.Advent2016;
public class Day07 : IPuzzle
{
    private static bool HasAbba(string address)
    {
        bool hasAbba = false;
        int bracketCount = 0;
        for (int i = 0; i < address.Length - 3; ++i)
        {
            if (address[i] == '[') bracketCount++;
            else if (address[i] == ']') bracketCount--;
            else if (address[i] == address[i + 3] && address[i + 1] == address[i + 2] && address[i] != address[i + 1])
            {
                if (bracketCount > 0)
                {
                    return false;
                }
                else
                {
                    hasAbba = true;
                }
            }
        }

        return hasAbba;
    }

    private static bool HasAbaBab(string address)
    {
        int bracketCount = 0;

        HashSet<string> abas = [];
        HashSet<string> babs = [];

        for (int i = 0; i < address.Length - 2; ++i)
        {
            if (address[i] == '[') bracketCount++;
            else if (address[i] == ']') bracketCount--;
            else if (address[i] == address[i + 2] && address[i] != address[i + 1])
            {
                if (bracketCount > 0)
                {
                    babs.Add(address.Substring(i, 2));
                }
                else
                {
                    abas.Add(address.Substring(i + 1, 2));
                }
            }
        }

        return babs.Intersect(abas).Any();
    }

    public static int Part1(string input) => Util.Split(input).Count(HasAbba);

    public static int Part2(string input) => Util.Split(input).Count(HasAbaBab);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}