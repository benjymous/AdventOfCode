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
        public string Name { get { return "2016-24"; } }

        static int LocationCode(char c) => (1 << char.ToLower(c) - '0');
        static Int64 GetKey(int location, int visited) => (Int64)location << 32 | (Int64)(uint)visited;

        public class MapData : GridMap<char>
        {
            public Dictionary<int, ManhattanVector2> Locations = new Dictionary<int, ManhattanVector2>();
            public Dictionary<int, int> paths = new Dictionary<int, int>();

            public int AllLocations = 0;

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
                            Locations[code] = new ManhattanVector2(x, y);
                            AllLocations += code;
                        }
                        data.PutStrKey($"{x},{y}", c);
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
                            var path = AStar<ManhattanVector2>.FindPath(this, loc1.Value, loc2.Value);

                            var c1 = data.GetObjKey(loc1.Value);
                            var c2 = data.GetObjKey(loc2.Value);

                            paths[loc1.Key + loc2.Key] = path.Count();
                        }
                    }
                }
            }

            Dictionary<int, IEnumerable<int>> BitCache = new Dictionary<int, IEnumerable<int>>();
            public IEnumerable<int> Bits(int input)
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

            var queue = new Queue<(int location, int visited, int distance)>();

            queue.Enqueue((LocationCode('0'), LocationCode('0'), 0));
            var cache = new Dictionary<Int64, int>();
            cache[GetKey(LocationCode('0'), LocationCode('0'))] = 0;

            int currentBest = int.MaxValue;

            while (queue.Any())
            {
                // take an item from the job queue
                var item = queue.Dequeue();

                int tryLocations = map.AllLocations - item.visited;

                if (tryLocations > 0)
                {
                    // check locations not visited
                    foreach (var location in map.Bits(tryLocations))
                    {
                        if (map.paths.TryGetValue(item.location | location, out var pathDistance))
                        {
                            // create new state, at next location 
                            int visited = item.visited + location;
                            int distance = item.distance + pathDistance;
                            var next = (location, visited, distance);

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
                                queue.Enqueue(next);
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
                        distance += +map.paths[item.location + LocationCode('0')];
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