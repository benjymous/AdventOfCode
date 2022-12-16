﻿using AoC.Utils;
using AoC.Utils.Collections;
using AoC.Utils.Pathfinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static AoC.Advent2021.Day18;

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
            public uint IntId;
            public int Rate;
            public string[] Neighbours;
        }

        class MapData : IMap<string>
        {
            public MapData(Dictionary<string, Valve> data)
            {
                Data = data;
            }

            Dictionary<string, Valve> Data;

            public IEnumerable<string> GetNeighbours(string location)
            {
                return Data[location].Neighbours;
            }
        }

        public static int Part1(string input)
        {
            var data = Util.RegexParse<Valve>(input);

            var valves = data.Where(val => val.Rate > 0).ToArray();

            uint id = 1 << 1;
            foreach (var v in valves)
            {
                v.IntId = id <<= 1;
            }

            var map = data.ToDictionary(val => val.Id, val => val);
            map["AA"] = data.Where(val => val.Id == "AA").First();

            Dictionary<uint, int> routes = new();
            var mapAdaptor = new MapData(map);

            var lookup = valves.ToDictionary(val => val.IntId, val => val.Rate);
            
            foreach (var v1 in valves)
            {
                routes[1 | v1.IntId] = AStar<string>.FindPath(mapAdaptor, "AA", v1.Id).Length;
                foreach (var v2 in valves)
                {
                    if (v1.IntId < v2.IntId)
                    {
                        routes[v1.IntId | v2.IntId] = AStar<string>.FindPath(mapAdaptor, v1.Id, v2.Id).Length;
                    }
                }
            }

            var queue = new Queue<(uint location, int mins, int open, int released, uint toVisit)>();
            queue.Enqueue((1, 0, 0, 0, (uint)valves.Sum(v => v.IntId)));

            Dictionary<uint, int> seen = new();

            int best = int.MinValue;

            queue.Operate(entry =>
            {               
                if (entry.mins < 30)
                {
                    entry.released += entry.open;

                    var potential = entry.released + (entry.open * (29 - entry.mins));                  
                    best = Math.Max(best, potential);

                    var key = entry.location + entry.toVisit << 16;
                    if (seen.TryGetValue(key, out var val) && val > potential) return;
                    seen[key] = potential;

                    foreach (var next in entry.toVisit.BitSequence())
                    {
                        var distance = routes[entry.location | next];

                        var nextState = (location: next, mins: entry.mins + distance + 1, open: entry.open + lookup[next], released: entry.released + (entry.open * distance), toVisit: entry.toVisit - next);

                        queue.Enqueue(nextState);
                    }
                }
            });

            return best;
        }

        public static int Part2(string input, ILogger logger)
        {
            //var data = Util.RegexParse<Valve>(input).ToArray();
            //uint id = 1;
            //foreach (var elem in data)
            //{
            //    elem.IntId = id;
            //    id <<= 1;
            //}

            //var lookup = data.ToDictionary(val => val.Id, val => val.IntId);

            //foreach (var elem in data)
            //{
            //    elem.IntNeighbours = elem.Neighbours.Select(id => lookup[id]).ToArray();
            //}

            //var map = data.ToDictionary(val => val.IntId, val => val);

            //var start = data.Where(el => el.Id == "AA").First();
            //var queue = new PriorityQueue<(uint me, uint elephant, int mins, int open, int released, uint opened), int>();
            //queue.Enqueue((start.IntId, start.IntId, 0, 0, 0, 0), 0);

            //Dictionary<(uint, uint), int> visited = new();

            //int best = int.MinValue;

            //int step = 0;

            //queue.Operate((val) =>
            //{
            //    if (step++ % 200000 == 0)
            //    {
            //        logger.WriteLine($"{step} : best: {best} : checked {visited.Count} : queue: {queue.Count}");
            //    }

            //    var key = (val.me+val.elephant, val.opened);
            //    if (visited.TryGetValue(key, out var prev) && prev < val.mins) return;

            //    visited[key] = val.mins;

            //    var node1 = map[val.me];
            //    var node2 = map[val.elephant];

            //    if (val.mins < 26)
            //    {
            //        val.released += val.open;

            //        var potential = val.released + (val.open * (25 - val.mins));
            //        if (potential > best)
            //        {
            //            best = potential;
            //        }

            //        List<(uint loc, int openRate)> myMoves = new(6), elephantMoves = new(6);

            //        // me
            //        if (node1.Rate > 0 && ((val.opened & val.me) == 0))
            //        {
            //            myMoves.Add((val.me, node1.Rate));
            //        }

            //        foreach (var neighbour in node1.IntNeighbours)
            //        {
            //            myMoves.Add((neighbour, 0));
            //        }

            //        // elephant
            //        if (node2.Rate > 0 && ((val.opened & val.elephant) == 0))
            //        {
            //            elephantMoves.Add((val.elephant, node2.Rate));
            //        }

            //        foreach (var neighbour in node2.IntNeighbours)
            //        { 
            //            elephantMoves.Add((neighbour, 0));
            //        }

            //        // combine possible moves
            //        var combos = Util.Combinations(myMoves, elephantMoves);
            //        foreach (var combo in combos)
            //        {
            //            if (val.me == val.elephant && combo.Item1.openRate > 0 && combo.Item1.openRate == combo.Item2.openRate) continue;

            //            var newOpened = val.opened;
            //            if (combo.Item1.openRate > 0) newOpened += val.me;
            //            if (combo.Item2.openRate > 0) newOpened += val.elephant;
            //            var next = (me: combo.Item1.loc, elephant: combo.Item2.loc, mins: val.mins + 1, val.open + combo.Item1.openRate + combo.Item2.openRate, val.released, newOpened);
            //            var nextKey = (next.me + next.elephant, newOpened);
            //            if (!visited.TryGetValue(nextKey, out var prev2) || prev2 > next.mins)
            //            {
            //                queue.Enqueue(next, 0);
            //                //visited[nextKey] = next.mins;
            //            }
            //        }
            //    }
            //});
            //logger.WriteLine($"{step} : best: {best} : checked {visited.Count} : queue: {queue.Count}");

            return 0;// best;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            //logger.WriteLine("- Pt2 - " + Part2(input, logger));
        }
    }
}