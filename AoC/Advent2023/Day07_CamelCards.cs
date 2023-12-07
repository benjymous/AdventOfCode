namespace AoC.Advent2023;
public class Day07 : IPuzzle
{
    public static int CardIndex(char c) => "AKQJT98765432*".IndexOf(c);

    public enum HandType
    {
        FiveOfAKind = 6,
        FourOfAKind = 5,
        FullHouse = 4,
        ThreeOfAKind = 3,
        TwoPair = 2,
        OnePair = 1,
        HighCard = 0,
    }

    [Regex(@"(.....) (\d+)")]
    public readonly record struct Hand(string Cards, int Bid = 0)
    {
        public readonly HandType HandType = GetHandType(Cards);
        public readonly int Ordering = GetSortOrder(Cards);
    }

    public static int GetSortOrder(string cards)
    {
        int num = 0;
        foreach (var c in cards) num = (num << 4) + CardIndex(c);
        return -num;
    }

    public static string ApplyJokers(string cards)
    {
        if (!cards.Contains('*')) return cards;
        if (cards == "*****") return "AAAAA";

        var groups = GroupCards(cards.Replace("*", "")).ToArray();

        if (groups.Length == 2 && groups[0].Count == 2 && groups[1].Count == 2)
        {
            return cards.Replace('*', (CardIndex(groups[0].Key) <= CardIndex(groups[1].Key)) ? groups[0].Key : groups[1].Key);
        }

        return cards.Replace('*', groups[0].Key);
    }

    public static HandType GetHandType(string hand) => GroupCards(ApplyJokers(hand)).Select(g => g.Count).Decompose2() switch
    {
        (5, 0) => HandType.FiveOfAKind,
        (4, 1) => HandType.FourOfAKind,
        (3, 2) => HandType.FullHouse,
        (3, 1) => HandType.ThreeOfAKind,
        (2, 2) => HandType.TwoPair,
        (2, 1) => HandType.OnePair,
        _ => HandType.HighCard
    };

    static IEnumerable<(char Key, int Count)> GroupCards(string hand) => hand.GroupBy(c => c).Select(g => (g.Key, Count: g.Count())).OrderByDescending(g => g.Count);

    public static IEnumerable<Hand> SortCards(IEnumerable<Hand> cards) => cards.OrderBy(h => (h.HandType, h.Ordering));

    static int Solve(string input) => SortCards(Util.RegexParse<Hand>(input).ToArray()).WithIndex(1).ToArray().Sum(h => h.Index * h.Value.Bid);

    public static int Part1(string input) => Solve(input);

    public static int Part2(string input) => Solve(input.Replace('J', '*'));

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}