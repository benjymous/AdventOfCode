using AoC.Utils;
using AoC.Utils.Pathfinding;
using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2016
{
    public class Day24 : IPuzzle
    {
        public string Name => "2016-24";

        static uint LocationCode(char c) => (1U << char.ToLower(c) - '0');
        static long GetKey(uint location, uint visited) => (long)location << 32 | visited;

        public class MapData : GridMap<char>
        {
            public Dictionary<uint, (int x, int y)> Locations = new();
            public Dictionary<uint, int> paths = new();

            public uint AllLocations = 0;

            public MapData(string input) : base(new Walkable())
            {
                var lines = Util.Split(input);

                // find points of interest in the map
                for (var y = 0; y < lines.Length; ++y)
                {
                    var line = lines[y];
                    for (var x = 0; x < line.Length; ++x)
                    {
                        var c = line[x];
                        if (c >= '0' && c <= '9')
                        {
                            // location to visit
                            var code = LocationCode(c);
                            Locations[code] = (x, y);
                            AllLocations += code;
                        }
                        Data[(x,y)] = c;
                    }
                }

                BuildPaths();
            }

            public void BuildPaths()
            {
                foreach (var loc1 in Locations)
                {
                    foreach (var loc2 in Locations)
                    {
                        if (loc1.Key != loc2.Key)
                        {
                            var path = AStar<(int x, int y)>.FindPath(this, loc1.Value, loc2.Value);
                            paths[loc1.Key + loc2.Key] = path.Count();
                        }
                    }
                }
            }

            readonly Dictionary<uint, IEnumerable<uint>> BitCache = new();
            public IEnumerable<uint> Bits(uint input)
            {
                if (BitCache.TryGetValue(input, out var output))
                {
                    return output;
                }

                var seq = input.BitSequence().ToArray();
                BitCache[input] = seq;
                return seq;
            }

            class Walkable : IIsWalkable<char>
            {
                public bool IsWalkable(char cell)
                {
                    return cell != '#';
                }
            }
        }

        public static int Solve(MapData map, bool returnHome)
        {
            var queue = new PriorityQueue<(uint location, uint visited, int distance), uint>();
            queue.Enqueue((LocationCode('0'), LocationCode('0'), 0), map.AllLocations);

            var cache = new Dictionary<long, int>() { { GetKey(LocationCode('0'), LocationCode('0')), 0 } };

            int currentBest = int.MaxValue;

            while (queue.Count > 0)
            {
                // take an item from the job queue
                var item = queue.Dequeue();

                uint tryLocations = map.AllLocations - item.visited;

                if (tryLocations > 0)
                {
                    // check locations not visited
                    foreach (var location in map.Bits(tryLocations))
                    {
                        if (map.paths.TryGetValue(item.location | location, out var pathDistance))
                        {
                            // create new state, at next location 
                            uint visited = item.visited + location;
                            int distance = item.distance + pathDistance;
                            var next = (location, visited, distance);

                            if (currentBest != int.MaxValue && distance > currentBest) continue;

                            // check if we've visited this position with this set of keys before
                            var cacheId = GetKey(next.location, next.visited);
                            if (!cache.TryGetValue(cacheId, out int cachedBest))
                            {
                                cachedBest = int.MaxValue;
                            }

                            // we've not visited, or our new path is shorter
                            if (cachedBest > next.distance)
                            {
                                // cache the new shorter distance, and add the new state to our job queue
                                cache[cacheId] = next.distance;
                                queue.Enqueue(next, map.AllLocations - next.visited);
                            }
                        }
                    }
                }
                else
                {
                    // we have visited all the locations, so this is a possible solution
                    int distance = item.distance;
                    if (returnHome)
                    {
                        distance += map.paths[item.location + LocationCode('0')];
                    }
                    currentBest = Math.Min(currentBest, distance);
                }
            }

            return currentBest;
        }

        public static int Part1(string input)
        {
            var map = new MapData(input);
            return Solve(map, false);
        }

        public static int Part2(string input)
        {
            var map = new MapData(input);
            return Solve(map, true);
        }

        public void Run(string input, ILogger logger)
        {
            //logger.WriteLine(Part1("###########\n#0.1.....2#\n#.#######.#\n#4.......3#\n###########"));

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}