using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2020
{
    public class Day16 : IPuzzle
    {
        public string Name { get { return "2020-16"; } }

        class Range
        {
            public Range(string input)
            {
                var values = Util.Parse32(input, '-');
                Min = values[0];
                Max = values[1];
            }

            readonly int Min;
            readonly int Max;

            public bool InRange(int v) => (v >= Min && v <= Max);
        }

        class TicketRule
        {
            public TicketRule(string input)
            {
                var bits1 = input.Split(": ");
                Class = bits1[0];
                Ranges = Util.Parse<Range>(bits1[1], " or ");
            }
            public string Class;
            public List<Range> Ranges;

            public bool Validate(int input) => Ranges.Where(r => r.InRange(input)).Any();
        }

        class Ticket
        {
            public Ticket(string input)
            {
                Values = Util.Parse32(input);
            }

            public bool Valid = true;
            public int[] Values;

            public IEnumerable<(string, int)> Decode(Dictionary<string, int> lookup) => lookup.Select(kvp => (kvp.Key, Values[kvp.Value]));
        }

        static IEnumerable<Ticket> ParseTickets(string input) => Util.Parse<Ticket>(input.Trim().Split("\n").Skip(1));

        class TicketScanner
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
                foreach (var ticket in NearbyTickets)
                {
                    foreach (var val in ticket.Values)
                    {
                        if (!Rules.Where(r => r.Validate(val)).Any())
                        {
                            ticket.Valid = false;
                            errorRate += val;
                        }
                    }
                }
                return errorRate;
            }

            public void DecodeTickets()
            {
                var validTickets = ValidNearbyTickets().ToArray();

                Dictionary<string, HashSet<int>> classMatrix = new Dictionary<string, HashSet<int>>();

                foreach (var rule in Rules)
                {
                    classMatrix[rule.Class] = new HashSet<int>();
                    for (var i = 0; i < Rules.Count; ++i)
                    {
                        bool anyInvalid = validTickets.Where(t => rule.Validate(t.Values[i]) == false).Any();
                        if (!anyInvalid)
                        {
                            classMatrix[rule.Class].Add(i);
                        }
                    }
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
                                if (entry2.Key != entry1.Key)
                                {
                                    if (entry2.Value.Contains(found))
                                    {
                                        entry2.Value.Remove(found);
                                    }
                                }
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

        public static string TestDecode(string input)
        {
            var scanner = new TicketScanner(input);
            scanner.DecodeTickets();
            return string.Join(", ", scanner.DecodedTicket().OrderBy(kvp => kvp.Item1).Select(kvp => $"{kvp.Item1}: {kvp.Item2}"));
        }

        public static int Part1(string input)
        {
            var scanner = new TicketScanner(input);
            return scanner.ValidateNearbyTickets();
        }

        public static Int64 Part2(string input)
        {
            var scanner = new TicketScanner(input);
            scanner.DecodeTickets();

            return scanner.DecodedTicket()
                .Where(kvp => kvp.Item1.StartsWith("departure"))
                .Select(kvp => kvp.Item2)
                .Product();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}