using AoC.Utils;
using AoC.Utils.Pathfinding;
using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2019
{
    public class Day15 : IPuzzle
    {
        public string Name => "2019-15";

        public class RepairDrone : NPSA.ICPUInterrupt
        {
            readonly NPSA.IntCPU cpu;
            (int x, int y) position = (0, 0);

            (int x, int y) tryState;
            ((int x, int y), (int x, int y)) target;
            bool hasTarget = false;

            public int Steps { get; private set; } = 0;

            public int minx = 0, miny = 0;
            public int maxx = 0, maxy = 0;

            public const int OPEN = 0;
            public const int WALL = 1;
            public const int OXYGEN = 2;

            class Walkable : IIsWalkable<int>
            {
                public bool IsWalkable(int cell)
                {
                    return cell != WALL;
                }
            }

            public GridMap<int> map = new(new Walkable());

            public (int x, int y) FindCell(int num)
            {
                return map.FindCell(num);
            }

            readonly Stack<((int x, int y), (int x, int y))> unknowns = new();

            public enum Mode
            {
                Interactive = 0,
                Search = 2,
            }

            public Mode mode = Mode.Search;


            public RepairDrone(string input)
            {
                cpu = new NPSA.IntCPU(input)
                {
                    Interrupt = this
                };
            }

            public void Run()
            {
                cpu.Run();
                Console.WriteLine(cpu.Speed());
            }

            public void AddIfUnknown((int x, int y) current, (int x, int y) neighbour)
            {
                if (!map.Data.ContainsKey(neighbour))
                {
                    unknowns.Push((current, neighbour));
                }
            }

            public void AddUnknowns()
            {
                AddIfUnknown(position, (position.x - 1, position.y));
                AddIfUnknown(position, (position.x + 1, position.y));
                AddIfUnknown(position, (position.x, position.y - 1));
                AddIfUnknown(position, (position.x, position.y + 1));
            }

            public void HasPutOutput()
            {
                var val = cpu.Output.Dequeue();

                switch (val)
                {
                    case 0:
                        {
                            if (mode == Mode.Interactive) Console.WriteLine("Wall!");

                            map.Data[tryState] = WALL;
                        }
                        break;
                    case 1:
                        {
                            if (mode == Mode.Interactive) Console.WriteLine("Ok");
                            map.Data[tryState] = OPEN;
                            position = tryState;
                        }
                        break;
                    case 2:
                        {
                            if (mode == Mode.Interactive) Console.WriteLine("Found Oxygen system");
                            map.Data[tryState] = OXYGEN;
                            position = tryState;
                        }
                        break;
                    default:
                        {
                            if (mode == Mode.Interactive) Console.WriteLine("??? {val}");
                            map.Data[tryState] = (int)val;
                            position = tryState;
                        }
                        break;
                }
                minx = Math.Min(minx, tryState.x);
                maxx = Math.Max(maxx, tryState.x);

                miny = Math.Min(miny, tryState.y);
                maxy = Math.Max(maxy, tryState.y);
            }

            public int GetMapData(int x, int y)
            {
                var key = (x,y);
                if (map.Data.TryGetValue(key, out int value))
                {
                    return value;
                }
                else return 0;
            }

            public void DrawMap(ILogger logger)
            {
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
                                0 => ".",
                                1 => "#",
                                _ => "?",
                            };
                        }
                    }
                    logger.WriteLine(line);
                }
                logger.WriteLine();

            }
            public void WillReadInput()
            {
                Steps++;
                if (mode == Mode.Interactive)
                {
                    DrawMap(new ConsoleOut());
                    Console.WriteLine($"Located at {position}");
                    Console.WriteLine("north (1), south (2), west (3), and east (4) ?");

                    int code = 0;

                    while (code < 1 || code > 4)
                    {
                        Console.Write("> ");
                        var key = Console.ReadKey();
                        Console.WriteLine();

                        try
                        {
                            code = int.Parse($"{key.KeyChar}");

                            switch (code)
                            {
                                case 1: tryState = (position.x, position.y - 1); break;
                                case 2: tryState = (position.x, position.y + 1); break;
                                case 3: tryState = (position.x - 1, position.y); break;
                                case 4: tryState = (position.x + 1, position.y); break;
                            }
                        }
                        catch
                        {
                        }
                    }
                    cpu.Input.Enqueue(code);
                }
                else if (mode == Mode.Search)
                {
                    //DrawMap();

                    if (mode == Mode.Search)
                    {
                        AddUnknowns();
                    }

                    if (unknowns.Count == 0 && !hasTarget)
                    {
                        cpu.Input.Enqueue(0); // stop
                        return;
                    }

                    if (!hasTarget)
                    {
                        target = unknowns.Pop();
                        hasTarget = true;
                    }

                    if (target.Item2.Distance(position) == 1)
                    {
                        tryState = target.Item2;
                        hasTarget = false;
                    }
                    else if (target.Item1.Distance(position) == 1)
                    {
                        tryState = target.Item1;
                    }
                    else
                    {
                        // need to path find

                        var path = FindPath(position, target.Item1);
                        tryState = path.First();
                    }

                    int code = 0;

                    if (tryState.y < position.y) code = 1; // up
                    else if (tryState.y > position.y) code = 2; // down
                    else if (tryState.x < position.x) code = 3; // left
                    else if (tryState.x > position.x) code = 4; // right

                    cpu.Input.Enqueue(code); // stop
                }
            }

            public IEnumerable<(int x, int y)> FindPath((int x, int y) start, (int x, int y) end)
            {
                return AStar<(int x, int y)>.FindPath(map, start, end);
            }

        }

        public static int Part1(string input, ILogger logger = null)
        {
            var droid = new RepairDrone(input);
            droid.Run();

            droid.DrawMap(logger);

            logger?.WriteLine($"Map explored in {droid.Steps} steps");

            var oxygenSystemPosition = droid.FindCell(RepairDrone.OXYGEN);

            var path = droid.FindPath((0, 0), oxygenSystemPosition);

            return path.Count();
        }

        public static int Part2(string input)
        {
            var droid = new RepairDrone(input);
            droid.Run();
            var oxygenSystemPosition = droid.FindCell(2);

            var dist = 0;
            for (int y = droid.miny + 1; y < droid.maxy; ++y)
            {
                for (int x = droid.minx + 1; x < droid.maxx; ++x)
                {
                    if (droid.GetMapData(x, y) == 0)
                    {
                        int score = droid.GetMapData(x + 1, y) +
                                    droid.GetMapData(x - 1, y) +
                                    droid.GetMapData(x, y + 1) +
                                    droid.GetMapData(x, y - 1);

                        if (score == 3)
                        {
                            // dead end
                            var path = droid.FindPath(oxygenSystemPosition, (x, y));
                            dist = Math.Max(dist, path.Count());
                        }
                    }
                }
            }

            return dist;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input, logger));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}