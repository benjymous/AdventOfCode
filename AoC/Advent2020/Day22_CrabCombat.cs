namespace AoC.Advent2020;
public class Day22 : IPuzzle
{
    static Queue<int>[] ParseData(string input)
    {
        var groups = input.Split("\n\n");

        return groups.Select(group => Util.ParseNumbers<int>(group.Split("\n").Skip(1)).ToQueue()).ToArray();
    }

    static int GetKey(Queue<int>[] decks) => decks[0].Take(4).GetCombinedHashCode();

    static int PlayRound(Queue<int>[] decks, bool recursive = false, bool subgame = false)
    {
        var seen = new HashSet<int>();

        while (decks.All(d => d.Count > 0))
        {
            if (subgame)
            {
                var key = GetKey(decks);
                if (seen.Contains(key)) return 0;
                seen.Add(key);
            }

            var taken = decks.Select(d => d.Dequeue()).ToArray();

            if (recursive && decks[0].Count >= taken[0] && decks[1].Count >= taken[1])
            {
                var result = PlayRound([
                    decks[0].Take(taken[0]).ToQueue(),
                    decks[1].Take(taken[1]).ToQueue()
                ], true, true);

                decks[result].EnqueueRange(result == 0 ? taken : taken.Reverse());
            }
            else
            {
                decks[taken[0] > taken[1] ? 0 : 1].EnqueueRange(taken.OrderDescending());
            }
        }

        if (subgame)
        {
            return decks[0].Count == 0 ? 1 : 0;
        }
        else
        {
            var idx = 1;
            return decks.Where(d => d.Count > 0)
                .First()
                .Reverse()
                .Select(card => card * idx++)
                .Sum();
        }
    }

    public static int Part1(string input) => PlayRound(ParseData(input));

    public static int Part2(string input) => PlayRound(ParseData(input), true, false);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}