using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2018
{
    public class Day20 : IPuzzle
    {
        public string Name => "2018-20";

        static (int dx, int dy)[] Directions = { (0, -1), (1, 0), (0, 1), (-1, 0) };

        public class Cell
        {
            public bool[] Exits = { false, false, false, false };
            public int doorDistance { get; set; } = int.MaxValue;
        }

        const int NORTH = 0;
        const int EAST = 1;
        const int SOUTH = 2;
        const int WEST = 3;

        static IEnumerable<string> SplitOptions(string input)
        {
            List<string> parts = new List<string>();

            int close = 1;
            int depth = 1;
            string part = "";
            while (depth > 0)
            {
                var c = input[close++];
                switch (c)
                {
                    case '(': depth++; part += c; break;
                    case ')': depth--; part += c; break;
                    case '|':
                        {
                            if (depth == 1)
                            {
                                parts.Add(part); part = "";
                            }
                            else part += c;
                            break;
                        }
                    default: part += c; break;
                }
            }
            parts.Add(part);

            var rest = input.Substring(close);

            foreach (var p in parts)
            {
                yield return $"{p}{rest}";
            }
        }
        static Dictionary<(int x, int y), Cell> BuildMap(string input, ILogger logger = null)
        {
            var map = new Dictionary<(int x, int y), Cell>();

            Queue<((int x, int y) position, string tape)> queue = new();

            queue.Enqueue(((0, 0), input.Substring(1)));

            map[(0, 0)] = new Cell();

            while (queue.TryDequeue(out var state))
            {
                var ch = state.tape.First();


                var cell = map[state.position];

                (int dx, int dy) move = (-1, -1);
                int forward = 0, backward = 0;

                if (ch == '(')
                {
                    var options = SplitOptions(state.tape);
                    foreach (var option in options)
                    {
                        queue.Enqueue((state.position, option));
                    }
                }
                else
                {
                    switch (ch)
                    {
                        case 'N':
                            move = Directions[0]; forward = NORTH; backward = SOUTH; break;
                        case 'E':
                            move = Directions[1]; forward = EAST; backward = WEST; break;
                        case 'S':
                            move = Directions[2]; forward = SOUTH; backward = NORTH; break;
                        case 'W':
                            move = Directions[3]; forward = WEST; backward = EAST; break;
                    }

                    if (move != (-1, -1))
                    {
                        var newPos = (state.position.x + move.dx, state.position.y + move.dy);
                        if (!map.TryGetValue(newPos, out var neighbour))
                        {
                            neighbour = new Cell();
                            map[newPos] = neighbour;
                        }
                        cell.Exits[forward] = true;
                        neighbour.Exits[backward] = true;

                        queue.Enqueue((newPos, state.tape[1..]));
                    }
                }
            }

            if (logger != null) logger.WriteLine("Built map");

            map[(0, 0)].doorDistance = 0;
            CalcFurthestDistance(map, (0, 0));

            if (logger != null) logger.WriteLine("Calculated distances");

            return map;
        }

        static void Display(Dictionary<(int x, int y), Cell> map)
        {
            var minx = map.Min(kvp => kvp.Key.x);
            var miny = map.Min(kvp => kvp.Key.y);
            var maxx = map.Max(kvp => kvp.Key.x);
            var maxy = map.Max(kvp => kvp.Key.y);

            for (int y=miny; y<=maxy; y++)
            {
                Console.Write("#");
                for (int x=minx; x<=maxx; x++)
                {
                    if (map.TryGetValue((x,y), out var cell))
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

            foreach (var dir in cell.Exits.WithIndex().Where(e => e.Value).Select(e => Directions[e.Index]))
            {
                var neighbourPos = (pos.x + dir.dx, pos.y + dir.dy);
                var neighbour = map[neighbourPos];
                if (neighbour.doorDistance > cell.doorDistance + 1)
                {
                    neighbour.doorDistance = cell.doorDistance + 1;
                    CalcFurthestDistance(map, neighbourPos);
                }
            }
        }

        public static int Part1(string input, Dictionary<(int x, int y), Cell> map = null)
        {
            if (map == null) map = BuildMap(input);

            //Display(map);

            return map.Values.Max(c => c.doorDistance);
        }


        public static int Part2(string input, Dictionary<(int x, int y), Cell> map = null)
        {
            if (map==null) map = BuildMap(input);

            return map.Values.Where(c => c.doorDistance >= 1000).Count();
        }
    
        public void Run(string input, ILogger logger)
        {
            var map = BuildMap(input, logger);

            logger.WriteLine("- Pt1 - " + Part1(input, map));
            logger.WriteLine("- Pt2 - " + Part2(input, map));
        }
    }
}
