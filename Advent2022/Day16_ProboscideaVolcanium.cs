using AoC.Utils;
using AoC.Utils.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day16 : IPuzzle
    {
        public string Name => "2022-16";

        class Valve
        {
            [Regex("Valve (..) has flow rate=(.+); tunnels? leads? to valves? (.+)")]
            public Valve(string id, int rate, string[] neighbours)
            {
                Id = id;
                Rate = rate;
                Neighbours = neighbours.Select(n => n.Trim()).ToArray();
            }

            public string Id;
            public ulong IntId;
            public int Rate;
            public string[] Neighbours;
            public ulong[] IntNeighbours;
        }
        public static int Part1(string input)
        {
            var data = Util.RegexParse<Valve>(input);
            
            var map = data.ToDictionary(val => val.Id, val => val);            

            var queue = new Queue<(string location, int mins, int open, int released, HashSet<string> opened, string log)>();
            queue.Enqueue(("AA", 0, 0, 0, new HashSet<string>(), ""));

            Dictionary<string, int> visited = new();

            int best = int.MinValue;
            string bestLog = "";

            queue.Operate((val) =>
            {
                var key = $"{val.location}:{string.Join(",",val.opened.Order())}";
                if (visited.TryGetValue(key, out var prev) && prev >= val.released) return;

                var log = val.log + $"\n\n== Minute {val.mins+1} ==";
                
                visited[key] = val.released;

                var node = map[val.location];

                if (val.mins < 30)
                {
                    val.released += val.open;

                    var potential = val.released + (val.open * (29 - val.mins));
                    if (potential > best)
                    {
                        bestLog = log + $"\n=> {potential} ({val.open} * {(29 - val.mins)}) after waiting";
                        best = potential;
                    }

                    log += $"\n{val.open} pressure released";
                    if (node.Rate > 0 && !val.opened.Contains(val.location))
                    {
                        queue.Enqueue((val.location, val.mins + 1, val.open + node.Rate, val.released, val.opened.Append(val.location).ToHashSet(), log + $"\nYou open valve {val.location}"));
                    }

                    foreach (var neighbour in node.Neighbours)
                    {
                        queue.Enqueue((neighbour, val.mins + 1, val.open, val.released, val.opened, log + $"\nYou move to valve {neighbour}"));
                    }
                }
            });

            Console.WriteLine(bestLog);

            return best;
        }

        public static int Part2(string input, ILogger logger)
        {
            var data = Util.RegexParse<Valve>(input).ToArray();
            ulong id = 1;
            foreach (var elem in data)
            {
                elem.IntId = id;
                id <<= 1;
            }

            var lookup = data.ToDictionary(val => val.Id, val => val.IntId);

            foreach (var elem in data)
            {
                elem.IntNeighbours = elem.Neighbours.Select(id => lookup[id]).ToArray();
            }

            var map = data.ToDictionary(val => val.IntId, val => val);

            var start = data.Where(el => el.Id == "AA").First();
            var queue = new PriorityQueue<(ulong me, ulong elephant, int mins, int open, int released, ulong opened), int>();
            queue.Enqueue((start.IntId, start.IntId, 0, 0, 0, 0), 0);

            Dictionary<(ulong, ulong), int> visited = new();

            int best = int.MinValue;

            int step = 0;

            queue.Operate((val) =>
            {
                if (step++ % 200000 == 0)
                {
                    logger.WriteLine($"{step} : best: {best} : checked {visited.Count} : queue: {queue.Count}");
                }

                var key = (val.me+val.elephant, val.opened);
                if (visited.TryGetValue(key, out var prev) && prev < val.mins) return;

                visited[key] = val.mins;

                var node1 = map[val.me];
                var node2 = map[val.elephant];

                if (val.mins < 26)
                {
                    val.released += val.open;

                    var potential = val.released + (val.open * (25 - val.mins));
                    if (potential > best)
                    {
                        best = potential;
                    }

                    List<(ulong loc, int openRate)> myMoves = new(6), elephantMoves = new(6);

                    // me
                    if (node1.Rate > 0 && ((val.opened & val.me) == 0))
                    {
                        myMoves.Add((val.me, node1.Rate));
                    }

                    foreach (var neighbour in node1.IntNeighbours)
                    {
                        myMoves.Add((neighbour, 0));
                    }

                    // elephant
                    if (node2.Rate > 0 && ((val.opened & val.elephant) == 0))
                    {
                        elephantMoves.Add((val.elephant, node2.Rate));
                    }

                    foreach (var neighbour in node2.IntNeighbours)
                    { 
                        elephantMoves.Add((neighbour, 0));
                    }

                    // combine possible moves
                    var combos = Util.Combinations(myMoves, elephantMoves);
                    foreach (var combo in combos)
                    {
                        if (val.me == val.elephant && combo.Item1.openRate > 0 && combo.Item1.openRate == combo.Item2.openRate) continue;

                        var newOpened = val.opened;
                        if (combo.Item1.openRate > 0) newOpened += val.me;
                        if (combo.Item2.openRate > 0) newOpened += val.elephant;
                        var next = (me: combo.Item1.loc, elephant: combo.Item2.loc, mins: val.mins + 1, val.open + combo.Item1.openRate + combo.Item2.openRate, val.released, newOpened);
                        var nextKey = (next.me + next.elephant, newOpened);
                        if (!visited.TryGetValue(nextKey, out var prev2) || prev2 > next.mins)
                        {
                            queue.Enqueue(next, 0);
                            //visited[nextKey] = next.mins;
                        }
                    }
                }
            });
            logger.WriteLine($"{step} : best: {best} : checked {visited.Count} : queue: {queue.Count}");

            return best;
        }

        public void Run(string input, ILogger logger)
        {
            //logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input, logger));
        }
    }
}