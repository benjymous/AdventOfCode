using AoC.Advent2016.BunniTek;
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
        static readonly ushort AA = Util.MakeTwoCC("AA");

        class Valve
        {
            [Regex("Valve (..) has flow rate=(.+); tunnels? leads? to valves? (.+)")]
            public Valve(string id, int rate, string[] neighbours) => (Id, Rate, Neighbours) = (Util.MakeTwoCC(id), rate, neighbours.Select(n => Util.MakeTwoCC(n.Trim())).ToArray());

            public ushort Id;
            public uint IntId;
            public int Rate;
            public ushort[] Neighbours;
        }

        record MapData(Dictionary<ushort, ushort[]> Data) : IMap<ushort> { public IEnumerable<ushort> GetNeighbours(ushort location) => Data[location];}

        private static (Dictionary<uint, int> routes, Dictionary<uint, int> valveRates, uint availableNodes) Init(string input)
        {
            var data = Util.RegexParse<Valve>(input).ToArray();
            var valves = data.Where(val => val.Rate > 0).WithIndex(1).Select(i => { i.Value.IntId = 1U << i.Index; return i.Value; });
            return (routes: BuildRoutes(valves, new MapData(data.ToDictionary(val => val.Id, val => val.Neighbours))).ToDictionary(), valveRates: valves.ToDictionary(val => val.IntId, val => val.Rate), availableNodes: (uint)valves.Sum(v => v.IntId));
        }

        private static IEnumerable<(uint, int)> BuildRoutes(IEnumerable<Valve> valves, MapData map)
        {
            foreach (var v1 in valves)
            {
                yield return(1 | v1.IntId, AStar<ushort>.FindPath(map, AA, v1.Id).Length+1);
                foreach (var v2 in valves.Where(v => v.IntId < v1.IntId))
                    yield return(v1.IntId | v2.IntId, AStar<ushort>.FindPath(map, v1.Id, v2.Id).Length+1);
            }
        }

        private static int Solve(Dictionary<uint, int> routes, Dictionary<uint, int> valveRates, uint availableNodes, int availableTime)
        {
            var queue = new PriorityQueue<(uint location, int mins, int open, int released, uint toVisit), int>(Util.Values(((1U, availableTime, 0, 0, availableNodes), 0)));
            Dictionary<uint, int> seen = new();
            int best = 0;
            queue.Operate((entry, score) =>
            {
                var potential = -score;
                best = Math.Max(best, potential);
                if (!seen.TryGetValue(entry.toVisit, out var previous) || previous <= potential)
                {
                    seen[entry.toVisit] = potential;
                    foreach (var next in entry.toVisit.BitSequence())
                    {
                        var distance = routes[entry.location | next];
                        var nextState = (location: next, mins: entry.mins - distance, open: entry.open + valveRates[next], released: entry.released + (entry.open * distance), toVisit: entry.toVisit - next);
                        if (nextState.mins > 0)
                            queue.Enqueue(nextState, -(nextState.released + (nextState.open * nextState.mins)));
                    }
                }
            });

            return best;
        }

        public static int Part1(string input)
        {
            var (routes, valveRates, availableNodes) = Init(input);
            return Solve(routes, valveRates, availableNodes, 30);
        }

        public static int Part2(string input)
        {
            var (routes, valveRates, availableNodes) = Init(input);
            return Util.For<uint, (uint n1, uint n2)>(2, availableNodes, 2, i => Util.MinMax(i & availableNodes, ~i & availableNodes))
                        .Distinct().AsParallel().Select(p => Solve(routes, valveRates, p.n1, 26) + Solve(routes, valveRates, p.n2, 26)).Max();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}