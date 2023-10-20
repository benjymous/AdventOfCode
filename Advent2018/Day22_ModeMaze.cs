using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day22 : IPuzzle
    {
        public string Name => "2018-22";

        public const char ROCKY = '.';
        public const char WET = '=';
        public const char NARROW = '|';

        public const char BLOCKED = '#';

        public class Cave
        {
            public Cave(int targetX, int targetY, int caveDepth)
            {
                target = new State((targetX, targetY));
                depth = caveDepth;
            }

            readonly Dictionary<(int x, int y), int> GeoCache = new();
            readonly Dictionary<(int x, int y), char> Map = new();

            public int MapSize => Map.Count;

            public char MapAt((int x, int y) pos) => Map.GetOrCalculate(pos, TypeChar);

            public int GeologicIndex((int x, int y) pos) => GeoCache.GetOrCalculate(pos, _ =>
            {
                // The region at 0,0 (the mouth of the cave) has a geologic index of 0.
                // The region at the coordinates of the target has a geologic index of 0.
                if (pos == (0, 0) || pos == target.position) return 0;

                // If the region's Y coordinate is 0, the geologic index is its X coordinate times 16807.
                else if (pos.y == 0) return pos.x * 16807;

                // If the region's X coordinate is 0, the geologic index is its Y coordinate times 48271
                else if (pos.x == 0) return pos.y * 48271;

                else return ErosionLevel((pos.x - 1, pos.y)) * ErosionLevel((pos.x, pos.y - 1));
            });

            // A region's erosion level is its geologic index plus the cave system's depth, all modulo 20183
            public int ErosionLevel((int x, int y) pos) => (GeologicIndex(pos) + depth) % 20183;

            public char TypeChar((int x, int y) pos)
            {
                if (pos.x < 0 || pos.y < 0 || pos.x > target.position.x + 20 || pos.y > target.position.y + 20) return BLOCKED;
                return (ErosionLevel(pos) % 3) switch
                {
                    0 => ROCKY,
                    1 => WET,
                    2 => NARROW,
                    _ => BLOCKED,
                };
            }

            public static int RiskLevel(char typeChar) => typeChar switch
            {
                ROCKY => 0,
                WET => 1,
                NARROW => 2,
                _ => throw new Exception("Unknown area type"),
            };

            public int GetScore()
            {
                int score = 0;
                for (int y = 0; y <= target.position.y; ++y)
                {
                    for (int x = 0; x <= target.position.x; ++x)
                    {
                        score += RiskLevel(MapAt((x, y)));
                    }
                }
                return score;
            }

            public State target;
            readonly int depth;
        }

        public enum Tool
        {
            None,
            Torch,
            ClimbingGear,
        }

        public readonly struct State
        {
            public readonly (int x, int y) position = (0, 0);

            public readonly Tool tool = Tool.Torch;

            public readonly int cost = 0;

            public readonly int key;

            public State((int x, int y) pos, Tool t = Tool.Torch, int c = 0)
            {
                position = pos;
                tool = t;
                cost = c;
                key = HashCode.Combine(position, tool);
            }

            public State() { }

            static bool ToolValid(char cell, Tool tool) => cell switch
            {
                ROCKY => tool != Tool.None,
                WET => tool != Tool.Torch,
                NARROW => tool != Tool.ClimbingGear,
                _ => false,
            };

            bool CanMove(Cave cave, int dx, int dy) => ToolValid(cave.MapAt(((int, int))(position.x + dx, position.y + dy)), tool);

            bool CanSwitch(Cave cave, Tool newTool) => ToolValid(cave.MapAt(position), newTool);

            bool TrySetTool(Cave cave, Tool newTool, out State newState)
            {
                if (newTool != tool && CanSwitch(cave, tool))
                {
                    newState = new State(position, newTool, 7);

                    return true;
                }

                newState = default;
                return false;
            }

            bool TryMove(Cave cave, int dx, int dy, out State newState)
            {
                if (CanMove(cave, dx, dy))
                {
                    newState = new State((position.x + dx, position.y + dy), tool, 1);

                    return true;
                }

                newState = default;
                return false;
            }

            public IEnumerable<State> GetNeighbours(Cave cave)
            {
                if (TryMove(cave, 1, 0, out State newstate)) yield return newstate;
                if (TryMove(cave, 0, 1, out newstate)) yield return newstate;
                if (TryMove(cave, -1, 0, out newstate)) yield return newstate;
                if (TryMove(cave, 0, -1, out newstate)) yield return newstate;

                if (TrySetTool(cave, Tool.None, out newstate)) yield return newstate;
                if (TrySetTool(cave, Tool.Torch, out newstate)) yield return newstate;
                if (TrySetTool(cave, Tool.ClimbingGear, out newstate)) yield return newstate;
            }

            public int Distance(State other) => position.Distance(other.position);// + (tool != other.tool ? 1 : 0);

            public override int GetHashCode() => key;
        }

        private class StateComparer : IEqualityComparer<(State, int, int)>
        {
            public bool Equals((State, int, int) x, (State, int, int) y) => x.Item1.key == y.Item1.key;

            public int GetHashCode([DisallowNull] (State, int, int) obj) => obj.Item1.key;
        }

        public static void DrawMap(char[][] map)
        {
            foreach (var line in map)
            {
                Console.WriteLine(string.Join("", line));
            }
        }

        public static int Part1(int tx, int ty, int depth)
        {
            var cave = new Cave(tx, ty, depth);
            return cave.GetScore();
        }

        public static int Part1(string input)
        {
            var bits = input.Replace("\n", ",").Replace(" ", ",").Split(',');
            return Part1(int.Parse(bits[3]), int.Parse(bits[4]), int.Parse(bits[1]));
        }

        public static int Part2(int tx, int ty, int depth)
        {
            var cave = new Cave(tx, ty, depth);

            var startPos = new State((0, 0), Tool.Torch, 0);


            IEnumerable<(State state, int time, int distance)> generation = new List<(State, int, int)> { (startPos, 0, 0) };

            int best = int.MaxValue;
            var cache = new Dictionary<int, int>() { { startPos.key, 0 } };
            var nextGen = new HashSet<(State state, int time, int distance)>(new StateComparer());

            while (generation.Any())
            {
                //Console.WriteLine($"{generation.Count()} - {generation.First().state.Distance(cave.target)}");
                nextGen.Clear();
                foreach (var (state, time, distance) in generation)
                {
                    if (state.key == cave.target.key)
                    {
                        if (time < best) best = time;
                    }
                    else
                    {
                        var neighbours = state.GetNeighbours(cave);
                        foreach (var neighbour in neighbours)
                        {
                            int newTime = time + neighbour.cost;

                            if ((newTime >= best) || cache.TryGetValue(neighbour.key, out var prevTime) && newTime >= prevTime) continue;

                            cache[neighbour.key] = newTime;

                            nextGen.Add((neighbour, newTime, neighbour.Distance(cave.target) + newTime * 100));
                        }
                    }
                }

                generation = nextGen.OrderBy(v => v.distance).Take(64).ToList();
            }

            Console.WriteLine($"{cache.Count} states seen, {cave.MapSize} locations visited");

            return best;
        }

        public static int Part2(string input)
        {
            var bits = input.Replace("\n", ",").Replace(" ", ",").Split(',');
            return Part2(int.Parse(bits[3]), int.Parse(bits[4]), int.Parse(bits[1]));
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }

    }
}
