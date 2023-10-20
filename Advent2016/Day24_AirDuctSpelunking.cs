using AoC.Utils;
using AoC.Utils.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2016
{
    public class Day24 : IPuzzle
    {
        public string Name => "2016-24";

        static uint LocationCode(char c) => 1U << (c - '0');
        static readonly uint HOME = LocationCode('0');
        static uint GetKey(uint location, uint visited) => (location << 10) + visited;

        public class MapData : IMap<int>
        {
            readonly HashSet<int> Data = new();
            public readonly Dictionary<uint, int> Locations = new(), Paths = new();

            public readonly uint AllLocations = 0;
            public readonly int ShortestPath = 0;

            public MapData(string input)
            {
                var rawdata = Util.ParseSparseMatrix<char>(input, new Util.Convertomatic.SkipSpaces('#')).Select(kvp => (ch: kvp.Value, pos: kvp.Key.x + (kvp.Key.y << 16)));

                Data = rawdata.Select(v => v.pos).ToHashSet();
                Locations = rawdata.Where(v => v.ch is >= '0' and <= '9').ToDictionary(kvp => LocationCode(kvp.ch), kvp => kvp.pos);
                AllLocations = Locations.Keys.Sum();
                Paths = (from loc1 in Locations from loc2 in Locations
                         where loc1.Key < loc2.Key
                         select (loc1.Key + loc2.Key, this.FindPath(loc1.Value, loc2.Value).Length)).ToDictionary();

                ShortestPath = Paths.Min(kvp => kvp.Value);
            }

            readonly Dictionary<uint, uint[]> BitCache = new();
            public uint[] Bits(uint input) => BitCache.GetOrCalculate(input, _ => input.BitSequence().ToArray());

            static readonly int[] Neighbours = { 1, 1 << 16, -1, -(1 << 16) };
            public IEnumerable<int> GetNeighbours(int location) => Neighbours.Select(n => location + n).Where(n => Data.Contains(n));
        }

        public static int Solve(MapData map, bool returnHome)
        {
            var queue = new PriorityQueue<(uint location, uint visited, int distance), int>(new[] { ((HOME, HOME, 0), 0) });
            var cache = new Dictionary<uint, int>() { { GetKey(HOME, HOME), 0 } };

            int currentBest = int.MaxValue;

            queue.Operate(state =>
            {
                uint tryLocations = map.AllLocations - state.visited;
                if (tryLocations > 0)
                {   // check locations not visited
                    foreach (var location in map.Bits(tryLocations))
                    {
                        if (map.Paths.TryGetValue(state.location | location, out var pathDistance))
                        {
                            var next = (location, visited: state.visited + location, distance: state.distance + pathDistance);

                            int remainingCount = (map.AllLocations - next.visited).CountBits();
                            if (next.distance + (map.ShortestPath * remainingCount) > currentBest) continue;

                            var cacheId = GetKey(next.location, next.visited);
                            if (!cache.TryGetValue(cacheId, out int cachedBest) || cachedBest > next.distance)
                            {   // cache the new shorter distance, and add the new state to our job queue
                                cache[cacheId] = next.distance;
                                queue.Enqueue(next, next.distance * remainingCount);
                            }
                        }
                    }
                }
                else // we have visited all the locations, so this is a possible solution
                    currentBest = Math.Min(currentBest, state.distance + (returnHome ? map.Paths[state.location + HOME] : 0));
            });

            return currentBest;
        }

        public static int Part1(string input)
        {
            return Solve(new MapData(input), false);
        }

        public static int Part2(string input)
        {
            return Solve(new MapData(input), true);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}