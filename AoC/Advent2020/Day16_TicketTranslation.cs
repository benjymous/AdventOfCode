namespace AoC.Advent2020;
public class Day16 : IPuzzle
{
    [Regex(@"(\d+)\s?-\s?(\d+)")]
    public record struct Range(int Min, int Max)
    {
        public readonly bool InRange(int v) => v >= Min && v <= Max;
    }

    [method: Regex("(.+): (.+)")]
    public struct TicketRule(string className, [Split(" or ")] Range[] ranges)
    {
        public string Class = className;

        public readonly bool Validate(int input) => ranges.Any(r => r.InRange(input));
    }

    public class Ticket(string input)
    {
        public int[] Values = Util.ParseNumbers<int>(input);

        public IEnumerable<(string key, int value)> Decode(Dictionary<string, int> lookup) => lookup.Select(kvp => (kvp.Key, Values[kvp.Value]));
    }

    static List<Ticket> ParseTickets(string input) => [.. Util.Parse<Ticket>(input.Trim().Split("\n").Skip(1))];

    public class TicketScanner
    {
        public TicketScanner(string input)
        {
            var sections = input.Split("\n\n");
            Rules = Util.RegexParse<TicketRule>(sections[0]).ToList();
            YourTicket = ParseTickets(sections[1]).First();
            NearbyTickets = ParseTickets(sections[2]);
        }

        public HashSet<(Ticket ticket, int rate)> InvalidTickets() => NearbyTickets.SelectMany(ticket => ticket.Values.Where(val => !Rules.Any(r => r.Validate(val))).Select(val => (ticket, val))).ToHashSet();
        public IEnumerable<Ticket> ValidTickets() => NearbyTickets.Except(InvalidTickets().Select(v => v.ticket));

        public IEnumerable<(string key, int value)> DecodeTickets()
        {
            var validTickets = ValidTickets().ToArray();

            Dictionary<string, HashSet<int>> classMatrix = [];

            foreach (var rule in Rules)
                for (var i = 0; i < Rules.Count; ++i)
                    if (!validTickets.Any(t => !rule.Validate(t.Values[i]))) classMatrix.GetOrCreate(rule.Class).Add(i);

            while (Lookup.Count < Rules.Count)
            {
                foreach (var (found, key) in classMatrix.Where(e => e.Value.Count == 1 && !Lookup.ContainsKey(e.Key)).Select(e => (e.Value.First(), e.Key)))
                {
                    Lookup[key] = found;
                    // Remove this value from all the other matrices
                    foreach (var entry2 in classMatrix.Where(e => e.Key != key)) entry2.Value.Remove(found);
                }
            }
            return YourTicket.Decode(Lookup);
        }

        readonly List<TicketRule> Rules;
        readonly Ticket YourTicket;
        readonly List<Ticket> NearbyTickets;
        readonly Dictionary<string, int> Lookup = [];
    }

    public static int Part1(string input) => new TicketScanner(input).InvalidTickets().Sum(v => v.rate);

    public static long Part2(string input)
    {
        return new TicketScanner(input).DecodeTickets()
            .Where(kvp => kvp.key.StartsWith("departure"))
            .Product(kvp => kvp.value);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}