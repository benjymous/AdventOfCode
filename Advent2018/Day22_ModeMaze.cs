using AoC.Utils;
using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;
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
                target = new State() { position = (targetX, targetY), tool = Tool.Torch };
                depth = caveDepth;
            }

            readonly Dictionary<(int x, int y), int> GeoCache = new();
            readonly Dictionary<(int x, int y), char> Map = new();

            public char MapAt((int x, int y) pos)
            {
                return Map.GetOrCalculate(pos, p => TypeChar(p));
            }

            public int GeologicIndex((int x, int y) pos)
            {
                if (GeoCache.TryGetValue(pos, out int value))
                {
                    return value;
                }

                if (pos.x < 0 || pos.y < 0)
                {
                    throw new Exception("Invalid coordinate");
                }


                int result;
                // The region at 0,0 (the mouth of the cave) has a geologic index of 0.
                if (pos == (0,0)) result = 0;

                // The region at the coordinates of the target has a geologic index of 0.
                else if (pos == target.position) result = 0;

                // If the region's Y coordinate is 0, the geologic index is its X coordinate times 16807.
                else if (pos.y == 0) result = pos.x * 16807;

                // If the region's X coordinate is 0, the geologic index is its Y coordinate times 48271
                else if (pos.x == 0) result = pos.y * 48271;

                else result = ErosionLevel((pos.x-1, pos.y)) * ErosionLevel((pos.x, pos.y-1));

                GeoCache[pos] = result;
                return result;
            }

            // A region's erosion level is its geologic index plus the cave system's depth, all modulo 20183
            public int ErosionLevel((int x, int y) pos) => (GeologicIndex(pos) + depth) % 20183;

            public char TypeChar((int x, int y) pos)
            {
                if (pos.x < 0 || pos.y < 0) return BLOCKED;
                var erosionLevel = ErosionLevel(pos);
                return (erosionLevel % 3) switch
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
                        var c = MapAt((x, y));
                        score += RiskLevel(c);
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

        public struct State
        {
            public (int x, int y) position = (0, 0);

            public Tool tool = Tool.Torch;

            public int cost = 0;

            public Int64 GetKey() => (position.x << 40) + (position.y << 20) + (int)tool;

            public State((int x, int y) pos, Tool t, int c)
            {
                position = pos;
                tool = t;
                cost = c;
            }

            public State() { }

            static bool ToolValid(char cell, Tool tool) => cell switch
            {
                ROCKY => tool != Tool.None,
                WET => tool != Tool.Torch,
                NARROW => tool != Tool.ClimbingGear,
                _ => false,
            };

            bool CanMove(Cave cave, int dx, int dy)
            {
                if ((position.x + dx > cave.target.position.x + 20) || (position.y + dy > cave.target.position.y + 20)) return false; // prevent the search space going crazy

                var newPos = (position.x + dx, position.y + dy);

                var t = cave.MapAt(newPos);
                return ToolValid(t, tool);
            }

            bool CanSwitch(Cave cave, Tool newTool)
            {
                var t = cave.MapAt(position);
                return ToolValid(t, newTool);
            }

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

            public int Distance(State other)
            {
                return position.Distance(other.position) + Math.Abs((int)tool - (int)other.tool);
            }

            public static bool operator ==(State v1, State v2)
            {
                return v1.Equals(v2);
            }

            public static bool operator !=(State v1, State v2)
            {
                return !v1.Equals(v2);
            }

            public bool Equals(State other)
            {
                return Distance(other) == 0;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(position, tool);
            }

            public override string ToString()
            {
                return $"{position} {tool}";
            }

            public override bool Equals(object obj)
            {
                throw new NotImplementedException();
            }
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

            var jobqueue = new Queue<(State state, int time)>();
            jobqueue.Enqueue((startPos, 0));
            int best = int.MaxValue;
            var cache = new Dictionary<Int64, int>() { { startPos.GetKey(), 0 } };

            while (jobqueue.Any())
            {
                var (state, time) = jobqueue.Dequeue();

                if (state == cave.target)
                {
                    if (time < best)
                    {
                        best = time;
                    }
                }
                else
                {
                    var neighbours = state.GetNeighbours(cave);
                    foreach (var neighbour in neighbours)
                    {
                        int newTime = time + neighbour.cost;

                        if (newTime > best) continue;

                        var key = neighbour.GetKey();
                        if (cache.TryGetValue(key, out var prevTime))
                        {
                            if (prevTime <= newTime)
                            {
                                continue;
                            }
                        }
                        cache[key] = newTime;
                        jobqueue.Enqueue((neighbour, newTime));
                    }
                }
            }

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
