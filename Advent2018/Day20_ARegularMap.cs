using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day20 : IPuzzle
    {
        public string Name => "2018-20";

        static readonly (int dx, int dy)[] Directions = { (0, -1), (1, 0), (0, 1), (-1, 0) };

        public class Cell
        {
            public bool[] Exits = { false, false, false, false };

            public int DoorDistance { get; set; } = int.MaxValue;
        }

        const int NORTH = 0;
        const int EAST = 1;
        const int SOUTH = 2;
        const int WEST = 3;

        static IEnumerable<IEnumerable<char>> SplitOptions(char[] input)
        {
            List<IEnumerable<char>> parts = new();

            int close = 1;
            int depth = 1;
            List<char> part = new();
            while (depth > 0)
            {
                var c = input[close++];
                switch (c)
                {
                    case '(': depth++; part.Add(c); break;
                    case ')': depth--; part.Add(c); break;
                    case '|':
                        {
                            if (depth == 1)
                            {
                                parts.Add(part.ToArray());
                                part.Clear();
                            }
                            else part.Add(c);
                            break;
                        }
                    default: part.Add(c); break;
                }
            }
            parts.Add(part);

            var rest = input[close..];

            foreach (var p in parts)
            {
                yield return p.Concat(rest);
            }
        }
        static Dictionary<(int x, int y), Cell> BuildMap(string input, ILogger logger = null)
        {
            var map = new Dictionary<(int x, int y), Cell>();

            Queue<((int x, int y) position, char[] tape)> queue = new();

            queue.Enqueue(((0, 0), input[1..].ToArray()));

            map[(0, 0)] = new() { DoorDistance = 0 };

            while (queue.TryDequeue(out var state))
            {
                var ch = state.tape[0];
                if (ch == '(')
                {
                    foreach (var option in SplitOptions(state.tape))
                    {
                        queue.Enqueue((state.position, option.ToArray()));
                    }
                }
                else
                {
                    var forward = ch switch
                    {
                        'N' => NORTH,
                        'E' => EAST,
                        'S' => SOUTH,
                        'W' => WEST,
                        _ => -1,
                    };
                    if (forward == -1) continue;

                    var newPos = state.position.OffsetBy(Directions[forward]);

                    map[state.position].Exits[forward] = true;
                    map.GetOrCalculate(newPos, _ => new Cell()).Exits[(forward + 2) % 4] = true;

                    queue.Enqueue((newPos, state.tape[1..]));

                }
            }

            logger?.WriteLine("Built map");

            CalcFurthestDistance(map, (0, 0));

            logger?.WriteLine("Calculated distances");

            return map;
        }

        static void Display(Dictionary<(int x, int y), Cell> map)
        {
            var minx = map.Min(kvp => kvp.Key.x);
            var miny = map.Min(kvp => kvp.Key.y);
            var maxx = map.Max(kvp => kvp.Key.x);
            var maxy = map.Max(kvp => kvp.Key.y);

            for (int y = miny; y <= maxy; y++)
            {
                Console.Write("#");
                for (int x = minx; x <= maxx; x++)
                {
                    if (map.TryGetValue((x, y), out var cell))
                    {
                        Console.Write(cell.Exits[NORTH] ? " #" : "##");
                    }
                    else
                    {
                        Console.Write("##");
                    }
                }
                Console.WriteLine();
                for (int x = minx; x <= maxx; x++)
                {
                    if (map.TryGetValue((x, y), out var cell))
                    {
                        Console.Write(cell.Exits[WEST] ? " " : "#");
                        Console.Write(" ");

                        if (x == maxx)
                        {
                            Console.Write(cell.Exits[EAST] ? " " : "#");
                        }
                    }
                    else
                    {

                        Console.Write("##");
                        if (x == maxx)
                        {
                            Console.Write('#');
                        }
                    }

                }
                Console.WriteLine();

                if (y == maxy)
                {
                    Console.Write("#");
                    for (int x = minx; x <= maxx; x++)
                    {
                        if (map.TryGetValue((x, y), out var cell))
                        {
                            Console.Write(cell.Exits[SOUTH] ? " #" : "##");
                        }
                        else
                        {
                            Console.Write("##");
                        }
                    }
                    Console.WriteLine();
                }

            }

            Console.WriteLine();

        }

        private static void CalcFurthestDistance(Dictionary<(int x, int y), Cell> map, (int x, int y) pos)
        {
            var cell = map[pos];

            foreach (var (dx, dy) in cell.Exits.WithIndex().Where(e => e.Value).Select(e => Directions[e.Index]))
            {
                var neighbourPos = (pos.x + dx, pos.y + dy);
                var neighbour = map[neighbourPos];
                if (neighbour.DoorDistance > cell.DoorDistance + 1)
                {
                    neighbour.DoorDistance = cell.DoorDistance + 1;
                    CalcFurthestDistance(map, neighbourPos);
                }
            }
        }

        public static int Part1(string input, Dictionary<(int x, int y), Cell> map = null)
        {
            map ??= BuildMap(input);

            return map.Values.Max(c => c.DoorDistance);
        }


        public static int Part2(string input, Dictionary<(int x, int y), Cell> map = null)
        {
            map ??= BuildMap(input);

            return map.Values.Count(c => c.DoorDistance >= 1000);
        }

        public void Run(string input, ILogger logger)
        {
            var map = BuildMap(input, logger);

            logger.WriteLine("- Pt1 - " + Part1(input, map));
            logger.WriteLine("- Pt2 - " + Part2(input, map));
        }
    }
}
