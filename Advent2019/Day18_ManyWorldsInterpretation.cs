using AoC.Utils;
using AoC.Utils.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2019
{
    public class Day18 : IPuzzle
    {
        public string Name => "2019-18";

        // turn key char into bit
        // a (or A) = 000001
        // b        = 000010
        // c        = 000100
        // etc
        public static int KeyCode(char c) => 1 << char.ToLower(c) - 'a';

        public static int PlayerCode(int i) => 1 << 26 + i;

        public readonly struct RoomPath
        {
            public RoomPath(IEnumerable<char> path)
            {
                Doors = path.Where(c => c >= 'A' && c <= 'Z').Sum(KeyCode);
                Count = path.Count();
            }

            public bool IsWalkable(int heldKeys) => (Doors & heldKeys) == Doors;

            readonly int Doors = 0;

            public readonly int Count;
        }

        public class MapData : GridMap<char>
        {
            readonly List<(int x, int y)> startPositions = new();

            public int AllKeys { get; private set; } = 0;
            public int AllPlayers { get; private set; } = 0;

            readonly Dictionary<int, (int x, int y)> keyPositions = new();
            readonly Dictionary<int, (int x, int y)> doors = new();
            public Dictionary<int, RoomPath> Paths { get; private set; } = new Dictionary<int, RoomPath>();

            class Walkable : IIsWalkable<char>
            {
                public bool IsWalkable(char cell) => cell != '#';
            }

            readonly Dictionary<int, int[]> BitCache = new();
            public int[] Bits(int input) => BitCache.GetOrCalculate(input, _ => input.BitSequence().ToArray());

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
                        if (c == '@')
                        {
                            // player start position
                            startPositions.Add((x, y));
                            c = '.';
                        }
                        if (c >= 'a' && c <= 'z')
                        {
                            // key
                            keyPositions[KeyCode(c)] = (x, y);
                            AllKeys |= KeyCode(c);
                        }
                        if (c >= 'A' && c <= 'Z')
                        {
                            // door
                            doors[KeyCode(c)] = (x, y);
                        }
                        Data[(x,y)] = c;
                    }
                }
            }

            public void AlterForPart2()
            {
                if (startPositions.Count == 1)
                {
                    var (x, y) = startPositions.First();
                    startPositions.Clear();

                    startPositions.Add((x - 1, y - 1));
                    startPositions.Add((x + 1, y - 1));
                    startPositions.Add((x - 1, y + 1));
                    startPositions.Add((x + 1, y + 1));

                    Data[(x,y)] = '#';

                    Data[(x - 1,y)] = '#';
                    Data[(x + 1,y)] = '#';
                    Data[(x,y - 1)] = '#';
                    Data[(x,y + 1)] = '#';
                }
            }

            public void DrawMap()
            {
                for (var y = 0; y < 7; ++y)
                {
                    for (var x = 0; x < 7; ++x)
                    {
                        var c = Data[(x,y)];

                        var pos = (x, y);

                        if (startPositions.Contains(pos))
                        {
                            c = (char)('1' + startPositions.IndexOf(pos));
                        }

                        Console.Write(c);
                    }
                    Console.WriteLine();
                }
            }

            public void CalcPaths()
            {
                // precalculate all possible paths (ignoring doors)

                for (int i = 0; i < startPositions.Count; i++) AllPlayers |= PlayerCode(i);

                int[] keyIds = keyPositions.Keys.ToArray();
                for (int i = 0; i < keyIds.Length; i++)
                {
                    int k1 = keyIds[i];
                    // path from start to k1's location
                    foreach (var player in startPositions)
                    {
                        var path = this.FindPath(player, keyPositions[k1]);
                        int playerId = PlayerCode(startPositions.IndexOf(player));
                        if (path.Any())
                        {
                            Paths[playerId | k1] = new RoomPath(path.Select(pos => Data[pos]));
                        }
                    }
                    for(int j = i + 1; j < keyIds.Length; j++)
                    {
                        int k2 = keyIds[j];
    
                        // path from k1 to k2
                        var path = this.FindPath(keyPositions[k1], keyPositions[k2]);
                        if (path.Any())
                        {
                            Paths[k1 | k2] = new RoomPath(path.Select(pos => Data[pos])); // since we're using bitwise, we just store two bits for k1|k2 it doesn't matter which way around
                        }
                    }
                }
                Console.WriteLine($"Generated {Paths.Count} paths");
            }
        }

        static long GetKey(int players, int keys) => (long)players << 32 | (long)keys;

        public static int Solve(MapData map, ILogger logger)
        {
            map.CalcPaths();

            logger.WriteLine("2");

            var shortestPath = map.Paths.Values.Min(room => room.Count);

            var queue = new PriorityQueue<(int positions, int heldKeys, int distance), int>();

            queue.Enqueue((map.AllPlayers, 0, 0), 0);
            var cache = new Dictionary<long, int>() { { GetKey(map.AllPlayers, 0), 0 } };

            int currentBest = int.MaxValue;

            logger.WriteLine("3");

            queue.Operate((state, estimatedDistance) =>
            {
                var (positions, heldKeys, distance) = state;

                if (estimatedDistance < currentBest)
                {
                    foreach (var position in map.Bits(positions))
                    {
                        // check keys not held

                        var tryKeys = map.Bits(map.AllKeys - heldKeys);
                        foreach (var key in tryKeys)
                        {
                            int remainingCount = tryKeys.Length - 1;

                            if (map.Paths.TryGetValue(position | key, out var path) && path.IsWalkable(heldKeys) && (path.Count + distance) + (remainingCount * shortestPath) < currentBest)
                            {
                                // path isn't blocked - state holds all necessary keys

                                // create new state, at location of next key
                                var next = (positions: positions - position + key, heldKeys: heldKeys + key, distance: distance + path.Count);

                                // check if we've visited this position with this set of keys before
                                var cacheId = GetKey(next.positions, next.heldKeys);
                                if (!cache.TryGetValue(cacheId, out int cachedBest) || cachedBest > next.distance)
                                {
                                    // we've not visited, or our new path is shorter
                                    // cache the new shorter distance, and add the new state to our job queue
                                    cache[cacheId] = next.distance;

                                    var nextEstimatedDistance = next.distance + (remainingCount * shortestPath);

                                    if (remainingCount == 0)
                                    {
                                        // Collected all the keys, so this is a possible solution
                                        currentBest = Math.Min(currentBest, next.distance);
                                    }
                                    else if (nextEstimatedDistance < currentBest)
                                    {
                                        queue.Enqueue(next, nextEstimatedDistance);
                                    }
                                }
                            }
                        }
                    }
                }
            });

            Console.WriteLine($"examined {cache.Count} states");

            logger.WriteLine("4");

            return currentBest;
        }

        public static int Part1(string input, ILogger logger)
        {
            logger.WriteLine("0");
            var map = new MapData(input);
            logger.WriteLine("1");
            return Solve(map, logger);
        }

        public static int Part2(string input, ILogger logger)
        {
            logger.WriteLine("0");
            var map = new MapData(input);
            logger.WriteLine("1");
            map.AlterForPart2();
            return Solve(map, logger);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input, logger));
            logger.WriteLine("- Pt2 - " + Part2(input, logger));
        }
    }
}