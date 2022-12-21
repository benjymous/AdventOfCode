using AoC.Utils;
using AoC.Utils.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;

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
            foreach (var v in valves.OrderByDescending(val => val.Rate).WithIndex(1)) v.Value.IntId = (1U << v.Index);

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
            var queue = new PriorityQueue<(uint location, int mins, int open, int released, uint toVisit), int>();
            queue.Enqueue((1U, availableTime, 0, 0, availableNodes), 0);
            Dictionary<uint, int> seen = new();
            int best = 0;

            queue.Operate(entry =>
            {
                var potential = entry.released + (entry.open * entry.mins);
                if (potential > best)
                {
                    best = potential;
                }
                var key = entry.toVisit;
                if (!seen.TryGetValue(key, out var val) || val <= potential)
                {
                    seen[key] = potential;
                    foreach (var next in entry.toVisit.BitSequence())
                    {
                        var distance = routes[entry.location | next];
                        var nextState = (location: next, mins: entry.mins - (distance + 1), open: entry.open + lookup[next], released: entry.released + (entry.open * (distance + 1)), toVisit: entry.toVisit - next);
                        if (nextState.mins > 0)
                            queue.Enqueue(nextState, -(nextState.released + (nextState.open * nextState.mins)));
                    }
                }
            });

            return best;
        }

        public static int Part1(string input, ILogger logger)
        {
            Init(input, out IEnumerable<Valve> valves, out Dictionary<uint, int> routes, out Dictionary<uint, int> lookup);

            return Solve(routes, lookup, (uint)valves.Sum(v => v.IntId), 30);
        }

        public static int Part2(string input, ILogger logger)
        {
            Init(input, out IEnumerable<Valve> valves, out Dictionary<uint, int> routes, out Dictionary<uint, int> lookup);

            uint availableNodes = (uint)valves.Sum(v => v.IntId);

            int best = 0;
            Dictionary<uint, int> scores = new() { [0] = 0 };
            for (uint i = 2; i <= availableNodes; i+=2)
            {
                uint nodes1 = i & availableNodes;
                uint nodes2 = ~i & availableNodes;
                if (!scores.TryGetValue(nodes1, out int score1)) scores[nodes1] = score1 = Solve(routes, lookup, nodes1, 26);    
                if (!scores.TryGetValue(nodes2, out int score2)) scores[nodes2] = score2 = Solve(routes, lookup, nodes2, 26);
                best = Math.Max(best, score1 + score2);
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