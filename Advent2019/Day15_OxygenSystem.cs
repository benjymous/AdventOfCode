using AoC.Utils;
using AoC.Utils.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2019
{
    public class Day15 : IPuzzle
    {
        public string Name => "2019-15";

        public class RepairDrone : NPSA.ICPUInterrupt, IIsWalkable<int>
        {
            readonly NPSA.IntCPU cpu;
            (int x, int y) position = (0, 0);

            (int x, int y) tryState;
            readonly Queue<(int x, int y)> path = new();

            public int Steps { get; private set; } = 0;

            int minx = 0, miny = 0, maxx = 0, maxy = 0;

            public const int OPEN = 1, WALL = 0, OXYGEN = 2;

            readonly Dictionary<int, (int, int)> SpecialCells = new();

            public GridMap<int> map;
            public bool IsWalkable(int cell) => cell != WALL;

            public (int x, int y) FindCell(int num) => SpecialCells[num];

            readonly Stack<((int x, int y) position, (int x, int y) unknownNeighbour)> unknowns = new();

            public RepairDrone(string input)
            {
                map = new(this);
                cpu = new NPSA.IntCPU(input) { Interrupt = this };
            }

            public void Run()
            {
                cpu.Run();
                Console.WriteLine(cpu.Speed());
            }

            readonly (int dx, int dy)[] Neighbours = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

            public void AddUnknowns()
            {
                foreach (var n in Neighbours.Select(n => position.OffsetBy(n)).Where(n => !map.Data.ContainsKey(n))) unknowns.Push((position, n));
            }

            public void OutputReady()
            {
                var val = (int)cpu.Output.Dequeue();

                map.Data[tryState] = val;
                if (val > 1) SpecialCells[val] = tryState;
                if (val != WALL) position = tryState;
            }

            public int GetMapData(int x, int y) => map.Data.TryGetValue((x, y), out int value) ? value : -1;

            public void DrawMap(ILogger logger)
            {
                (minx, maxx) = map.Data.Keys.MinMax(v => v.x);
                (miny, maxy) = map.Data.Keys.MinMax(v => v.y);
                if (logger == null) return;
                logger.WriteLine();
                for (var y = miny; y <= maxy; ++y)
                {
                    var line = "";
                    for (int x = minx; x <= maxx; ++x)
                    {
                        if (x == position.x && y == position.y)
                        {
                            line += "@";
                        }
                        else if (x == 0 && y == 0)
                        {
                            line += "S";
                        }
                        else
                        {
                            line += GetMapData(x, y) switch
                            {
                                OPEN => " ",
                                WALL => "#",
                                OXYGEN => "o",
                                _ => "?",
                            };
                        }
                    }
                    logger.WriteLine(line);
                }
                logger.WriteLine();

            }

            public void RequestInput()
            {
                Steps++;
                
                AddUnknowns();

                if (!path.Any())
                {
                    if (unknowns.Count == 0)
                    {
                        cpu.Input.Enqueue(0); // stop
                        return;
                    }
                    PlotRoute(unknowns.Pop());
                }

                tryState = path.Dequeue();

                cpu.Input.Enqueue((tryState.x - position.x, tryState.y - position.y) switch
                {
                    (0, -1) => 1, // up
                    (0, 1) => 2,  // down
                    (-1, 0) => 3,  // left
                    (1, 0) => 4, // right
                    _ => 0
                });
            }

            private void PlotRoute(((int x, int y) position, (int x, int y) unknownNeighbour) target)
            {
                foreach (var pos in FindPath(position, target.position))
                {
                    path.Add(pos);
                    if (pos == target.unknownNeighbour) return;
                }
                path.Add(target.unknownNeighbour);
            }

            public IEnumerable<(int x, int y)> FindPath((int x, int y) start, (int x, int y) end) => map.FindPath(start, end);
        }

        public static int Part1(string input, ILogger logger = null)
        {
            var droid = new RepairDrone(input);
            droid.Run();

            droid.DrawMap(logger);

            logger?.WriteLine($"Map explored in {droid.Steps} steps");

            return droid.FindPath((0, 0), droid.FindCell(RepairDrone.OXYGEN)).Count();
        }

        static void FloodFill(int currentDistance, (int x, int y) position, GridMap<int> map, Dictionary<(int, int), int> distances)
        {
            if (!map.IsValidNeighbour(position) || distances.TryGetValue(position, out var prev) && prev <= currentDistance) return;

            distances[position] = currentDistance;

            FloodFill(currentDistance + 1, (position.x + 1, position.y), map, distances);
            FloodFill(currentDistance + 1, (position.x - 1, position.y), map, distances);
            FloodFill(currentDistance + 1, (position.x, position.y + 1), map, distances);
            FloodFill(currentDistance + 1, (position.x, position.y - 1), map, distances);
        }

        public static int Part2(string input)
        {
            var droid = new RepairDrone(input);
            droid.Run();
            var oxygenSystemPosition = droid.FindCell(2);

            Dictionary<(int, int), int> distance = new();
            FloodFill(0, oxygenSystemPosition, droid.map, distance);

            return distance.Values.Max();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input, logger));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}