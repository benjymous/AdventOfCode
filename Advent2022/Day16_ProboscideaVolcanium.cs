using AoC.Utils;
using AoC.Utils.Collections;
using System;
using System.Collections.Generic;
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
                Neighbours = neighbours.Select(n => n.Trim()).ToHashSet();
            }

            public string Id;
            public int Rate;
            public HashSet<string> Neighbours;
        }
        public static int Part1(string input)
        {
            var data = Util.RegexParse<Valve>(input);
            
            var map = data.ToDictionary(val => val.Id, val => val);

            //var tree = new Tree<string>();

            //foreach (var valve in data)
            //{
            //    foreach (var neighbour in valve.Neighbours)
            //    {
            //        tree.AddPair(valve.Id, neighbour);
            //    }
            //}

            var root = data.First().Id;

            var queue = new Queue<(string location, int mins, int open, int released, HashSet<string> visited)>();
            queue.Enqueue((root, 0, 0, 0, new HashSet<string>(Util.Values(root))));

            int best = int.MaxValue;

            queue.Operate((val) =>
            {
                var node = map[val.location];

                val.released += val.open;
                val.open += node.Rate;

                if (val.mins == 30)
                {
                    best = Math.Min(best, val.released);
                }
                else
                {
                    foreach (var neighbour in node.Neighbours.Except(val.visited))
                    {
                        queue.Enqueue((neighbour, val.mins + 1, val.open, val.released, val.visited.Append(neighbour).ToHashSet()));
                    }
                }


            });

            return best;
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            string test = @"Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
Valve BB has flow rate=13; tunnels lead to valves CC, AA
Valve CC has flow rate=2; tunnels lead to valves DD, BB
Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
Valve EE has flow rate=3; tunnels lead to valves FF, DD
Valve FF has flow rate=0; tunnels lead to valves EE, GG
Valve GG has flow rate=0; tunnels lead to valves FF, HH
Valve HH has flow rate=22; tunnel leads to valve GG
Valve II has flow rate=0; tunnels lead to valves AA, JJ
Valve JJ has flow rate=21; tunnel leads to valve II".Replace("\r", "");

            Console.WriteLine(Part1(test));


            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}