using AoC.Advent2016.BunniTek;
using AoC.Utils;
using AoC.Utils.Pathfinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using static AoC.Advent2022.Day04;

namespace AoC.Advent2022
{
    public class Day16 : IPuzzle
    {
        public string Name => "2022-16";
        static readonly int AA = MakeId("AA");

        static int MakeId(string name) => name[0] + (name[1] << 8);

        class Valve
        {
            [Regex("Valve (..) has flow rate=(.+); tunnels? leads? to valves? (.+)")]
            public Valve(string id, int rate, string[] neighbours)
            {
                Id = MakeId(id);
                Rate = rate;
                Neighbours = neighbours.Select(n => MakeId(n.Trim())).ToArray();
            }

            public int Id;
            public uint IntId;
            public int Rate;
            public int[] Neighbours;
        }

        record MapData(Dictionary<int, int[]> Data) : IMap<int>
        {
            public IEnumerable<int> GetNeighbours(int location) => Data[location];
        }

        private static void Init(string input, out IEnumerable<Valve> valves, out Dictionary<uint, int> routes, out Dictionary<uint, int> lookup)
        {
            var data = Util.RegexParse<Valve>(input).ToArray();
            valves = data.Where(val => val.Rate > 0);
            foreach (var v in valves.WithIndex(1)) v.Value.IntId = (1U << v.Index);

            var mapAdaptor = new MapData(data.ToDictionary(val => val.Id, val => val.Neighbours));

            routes = new();
            foreach (var v1 in valves)
            {
                routes.Add(1 | v1.IntId, AStar<int>.FindPath(mapAdaptor, AA, v1.Id).Length);
                foreach (var v2 in valves.Where(v => v.IntId < v1.IntId))
                {
                    routes.Add(v1.IntId | v2.IntId, AStar<int>.FindPath(mapAdaptor, v1.Id, v2.Id).Length);
                }
            }

            lookup = valves.ToDictionary(val => val.IntId, val => val.Rate);
        }
        private static int Solve(Dictionary<uint, int> routes, Dictionary<uint, int> lookup, uint availableNodes, int availableTime)
        {
            var queue = new Queue<(uint location, int mins, int open, int released, uint toVisit)>(Util.Values((1U, availableTime, 0, 0, availableNodes)));
            Dictionary<uint, int> seen = new();
            int best = 0;

            queue.Operate(entry =>
            {
                if (entry.mins > 0)
                {
                    var potential = entry.released + (entry.open * (entry.mins));
                    if (potential > best)
                    {
                        best = potential;
                    }
                    var key = entry.location + entry.toVisit;// << 16;
                    if (!seen.TryGetValue(key, out var val) || val <= potential)
                    {
                        seen[key] = potential;
                        var visited = availableNodes - entry.toVisit;
                        foreach (var next in entry.toVisit.BitSequence())
                        {
                            var distance = routes[entry.location | next];
                            queue.Enqueue((location: next, mins: entry.mins - (distance + 1), open: entry.open + lookup[next], released: entry.released + (entry.open * (distance + 1)), toVisit: entry.toVisit - next));
                        }
                    }
                }
            });
            return best;
        }

        public static int Part1(string input, ILogger logger)
        {
            Init(input, out IEnumerable<Valve> valves, out Dictionary<uint, int> routes, out Dictionary<uint, int> lookup);

            uint availableNodes = (uint)valves.Sum(v => v.IntId);

            return Solve(routes, lookup, availableNodes, 30);
        }

        public static int Part2(string input, ILogger logger)
        {
            Init(input, out IEnumerable<Valve> valves, out Dictionary<uint, int> routes, out Dictionary<uint, int> lookup);

            uint availableNodes = (uint)valves.Sum(v => v.IntId);

            int best = 0;

            Dictionary<uint, int> scores = new();

            for (uint i = 0; i <= availableNodes; ++i)
            {
                uint nodes = i & availableNodes;
                if (nodes > 0 && !scores.ContainsKey(nodes))
                {
                    var route = Solve(routes, lookup, nodes, 26);
                    if (route > 0) scores[nodes] = route;
                }
            }

            for (uint i = 0; i <= availableNodes; ++i)
            {
                var nodes1 = i & availableNodes;
                var nodes2 = ~i & availableNodes;
                if (scores.TryGetValue(nodes1, out int value1) && scores.TryGetValue(nodes2, out int value2))
                {
                    best = Math.Max(best, value1 + value2);
                }
            }

            return best;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input, logger));
            logger.WriteLine("- Pt2 - " + Part2(input, logger));
        }
    }
}