namespace AoC.Advent2023;
public class Day04 : IPuzzle
{
    [method: Regex(@"Card\s+(\d+):\s(.+)\s+\|\s+(.+)")]
    public readonly struct Scratchcard(int id, [Split(" ")] int[] winning, [Split(" ")] int[] numbers)
    {
        public readonly int Id = id, Matches = winning.Intersect(numbers).Count();
        public int Value => Matches != 0 ? (int)Math.Pow(2, Matches - 1) : 0;
    }

    public static int CountWinnings(Dictionary<int, int> cards, int cardId, int matches) => cards.Memoize(cardId, _
        => matches == 0 ? 0 : matches + Enumerable.Range(cardId + 1, matches).Sum(c => CountWinnings(cards, c, cards[c])));

    public static int Part1(string input) => Util.RegexParse<Scratchcard>(input).Sum(t => t.Value);

    public static int Part2(string input)
    {
        var cards = Util.RegexParse<Scratchcard>(input).ToDictionary(c => c.Id, c => c.Matches);

        return cards.Count + cards.Keys.Sum(cardId => CountWinnings(cards, cardId, cards[cardId]));
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}