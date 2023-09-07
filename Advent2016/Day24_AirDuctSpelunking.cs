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
        static uint GetKey(uint location, uint visited) => location << 10 | visited;

        public class MapData : IMap<int>
        {
            readonly HashSet<int> Data = new();
            public Dictionary<uint, int> Locations = new();
            public Dictionary<uint, int> paths = new();

            public uint AllLocations = 0;
            public int ShortestPath = 0;

            public MapData(string input) 
            {
                var lines = Util.Split(input);

                // find points of interest in the map
                for (var y = 0; y < lines.Length; ++y)
                {
                    var line = lines[y];
                    for (var x = 0; x < line.Length; ++x)
                    {
                        var c = line[x];
                        if (c is >= '0' and <= '9')
                        {
                            // location to visit
                            var code = LocationCode(c);
                            Locations[code] = x + (y << 16);
                            AllLocations += code;
                        }
                        if (c != '#') Data.Add(x + (y << 16));
                    }
                }

                BuildPaths();
            }

            public void BuildPaths()
            {
                foreach (var (loc1, loc2) in from loc1 in Locations
                                             from loc2 in Locations
                                             where loc1.Key < loc2.Key
                                             select (loc1, loc2))
                {
                    paths[loc1.Key + loc2.Key] = this.FindPath(loc1.Value, loc2.Value).Length;
                }

                ShortestPath = paths.Min(kvp => kvp.Value);
            }

            readonly Dictionary<uint, uint[]> BitCache = new();
            public uint[] Bits(uint input) => BitCache.GetOrCalculate(input, _ => input.BitSequence().ToArray());

            public IEnumerable<int> GetNeighbours(int location)
            {
                if (Data.Contains(location + 1)) yield return location + 1;
                if (Data.Contains(location + (1 << 16))) yield return location + (1 << 16);
                if (Data.Contains(location - 1)) yield return location - 1;
                if (Data.Contains(location - (1 << 16))) yield return location - (1 << 16);
            }
        }

        public static int Solve(MapData map, bool returnHome)
        {
            var queue = new PriorityQueue<(uint location, uint visited, int distance), int>();
            queue.Enqueue((HOME, HOME, 0), 0);

            var cache = new Dictionary<uint, int>() { { GetKey(HOME, HOME), 0 } };

            int currentBest = int.MaxValue;

            queue.Operate(state =>
            {
                uint tryLocations = map.AllLocations - state.visited;
                if (tryLocations > 0)
                {
                    // check locations not visited
                    foreach (var location in map.Bits(tryLocations))
                    {
                        if (map.paths.TryGetValue(state.location | location, out var pathDistance))
                        {
                            // create new state, at next location 
                            var next = (location, visited: state.visited + location, distance: state.distance + pathDistance);

                            int remainingCount = (map.AllLocations - next.visited).CountBits();
                            if (next.distance + (map.ShortestPath * remainingCount) > currentBest) continue;

                            // check if we've visited this position with this set of keys before
                            var cacheId = GetKey(next.location, next.visited);
                            if (!cache.TryGetValue(cacheId, out int cachedBest) || cachedBest > next.distance)
                            {
                                // cache the new shorter distance, and add the new state to our job queue
                                cache[cacheId] = next.distance;
                                queue.Enqueue(next, next.distance * remainingCount);
                            }
                        }
                    }
                }
                else
                {
                    // we have visited all the locations, so this is a possible solution
                    int distance = state.distance;
                    if (returnHome) distance += map.paths[state.location + HOME];
                    currentBest = Math.Min(currentBest, distance);
                    //Console.WriteLine(currentBest);
                }
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