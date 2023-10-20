using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2020
{
    public class Day16 : IPuzzle
    {
        public string Name => "2020-16";

        [Regex(@"(\d+)\s?-\s?(\d+)")]
        public record class Range(int Min, int Max)
        {
            public bool InRange(int v) => (v >= Min && v <= Max);
        }

        public class TicketRule
        {
            public TicketRule(string input)
            {
                var bits = input.Split(": ");
                Class = bits[0];
                Ranges = Util.RegexParse<Range>(bits[1], " or ").ToList();
            }
            public string Class;
            public List<Range> Ranges;

            public bool Validate(int input) => Ranges.Any(r => r.InRange(input));
        }

        public class Ticket
        {
            public Ticket(string input) => Values = Util.ParseNumbers<int>(input);

            public bool Valid = true;
            public int[] Values;

            public IEnumerable<(string, int)> Decode(Dictionary<string, int> lookup) => lookup.Select(kvp => (kvp.Key, Values[kvp.Value]));
        }

        static List<Ticket> ParseTickets(string input) => Util.Parse<Ticket>(input.Trim().Split("\n").Skip(1));

        public class TicketScanner
        {
            public TicketScanner(string input)
            {
                var sections = input.Split("\n\n");
                Rules = Util.Parse<TicketRule>(sections[0]);
                YourTicket = ParseTickets(sections[1]).First();
                NearbyTickets = ParseTickets(sections[2]).ToList();
            }

            public int ValidateNearbyTickets()
            {
                int errorRate = 0;
                foreach (var (ticket, val) in NearbyTickets.SelectMany(ticket => ticket.Values.Where(val => !Rules.Any(r => r.Validate(val))).Select(val => (ticket, val))))
                {
                    ticket.Valid = false;
                    errorRate += val;
                }

                return errorRate;
            }

            public void DecodeTickets()
            {
                var validTickets = ValidNearbyTickets().ToArray();

                Dictionary<string, HashSet<int>> classMatrix = new();

                foreach (var rule in Rules)
                {
                    classMatrix[rule.Class] = new HashSet<int>();
                    for (var i = 0; i < Rules.Count; ++i)
                        if (!validTickets.Any(t => !rule.Validate(t.Values[i]))) classMatrix[rule.Class].Add(i);
                }

                Lookup = new Dictionary<string, int>();

                while (Lookup.Count < Rules.Count)
                {
                    foreach (var entry1 in classMatrix)
                    {
                        if (entry1.Value.Count == 1 && !Lookup.ContainsKey(entry1.Key))
                        {
                            var found = entry1.Value.First();
                            Lookup[entry1.Key] = found;
                            // Remove this value from all the other matrices
                            foreach (var entry2 in classMatrix)
                            {
                                if (entry2.Key != entry1.Key) entry2.Value.Remove(found);
                            }
                        }
                    }
                }
            }

            public IEnumerable<Ticket> ValidNearbyTickets()
            {
                ValidateNearbyTickets();
                return NearbyTickets.Where(t => t.Valid);
            }

            public IEnumerable<(string, int)> DecodedTicket() => YourTicket.Decode(Lookup);

            public List<TicketRule> Rules;
            public Ticket YourTicket;
            public List<Ticket> NearbyTickets;
            public Dictionary<string, int> Lookup;
        }

        public static int Part1(string input)
        {
            return new TicketScanner(input).ValidateNearbyTickets();
        }

        public static long Part2(string input)
        {
            var scanner = new TicketScanner(input);
            scanner.DecodeTickets();

            return scanner.DecodedTicket()
                .Where(kvp => kvp.Item1.StartsWith("departure"))
                .Product(kvp => kvp.Item2);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}