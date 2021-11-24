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
        public string Name { get { return "2019-15"; } }

        public class RepairDrone : NPSA.ICPUInterrupt
        {
            NPSA.IntCPU cpu;
            ManhattanVector2 position = new ManhattanVector2(0, 0);

            ManhattanVector2 tryState = null;
            Tuple<ManhattanVector2, ManhattanVector2> target = null;
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

            public GridMap<int> map = new GridMap<int>(new Walkable());

            public string FindCell(int num)
            {
                return map.FindCell(num);
            }

            Stack<Tuple<ManhattanVector2, ManhattanVector2>> unknowns = new Stack<Tuple<ManhattanVector2, ManhattanVector2>>();

            public enum Mode
            {
                Interactive = 0,
                Search = 2,
            }

            public Mode mode = Mode.Search;


            public RepairDrone(string input)
            {
                cpu = new NPSA.IntCPU(input);
                cpu.Interrupt = this;
            }

            public void Run()
            {
                cpu.Run();
                Console.WriteLine(cpu.Speed());
            }

            public void AddIfUnknown(ManhattanVector2 current, ManhattanVector2 neighbour)
            {
                if (!map.data.ContainsKey(neighbour.ToString()))
                {
                    unknowns.Push(Tuple.Create(current, neighbour));
                }
            }

            public void AddUnknowns()
            {
                AddIfUnknown(position, new ManhattanVector2(position.X - 1, position.Y));
                AddIfUnknown(position, new ManhattanVector2(position.X + 1, position.Y));
                AddIfUnknown(position, new ManhattanVector2(position.X, position.Y - 1));
                AddIfUnknown(position, new ManhattanVector2(position.X, position.Y + 1));
            }

            public void HasPutOutput()
            {
                var val = cpu.Output.Dequeue();

                switch (val)
                {
                    case 0:
                        {
                            if (mode == Mode.Interactive) Console.WriteLine("Wall!");

                            map.data.PutObjKey(tryState, WALL);
                        }
                        break;
                    case 1:
                        {
                            if (mode == Mode.Interactive) Console.WriteLine("Ok");
                            map.data.PutObjKey(tryState, OPEN);
                            position = tryState;
                        }
                        break;
                    case 2:
                        {
                            if (mode == Mode.Interactive) Console.WriteLine("Found Oxygen system");
                            map.data.PutObjKey(tryState, OXYGEN);
                            position = tryState;
                        }
                        break;
                    default:
                        {
                            if (mode == Mode.Interactive) Console.WriteLine("??? {val}");
                            map.data.PutObjKey(tryState, (int)val);
                            position = tryState;
                        }
                        break;
                }
                minx = Math.Min(minx, tryState.X);
                maxx = Math.Max(maxx, tryState.X);

                miny = Math.Min(miny, tryState.Y);
                maxy = Math.Max(maxy, tryState.Y);
            }

            public int GetMapData(int x, int y)
            {
                var key = $"{x},{y}";
                if (map.data.ContainsKey(key))
                {
                    return map.data.GetStrKey(key);
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

                        if (x == position.X && y == position.Y)
                        {
                            line += "@";
                        }
                        else if (x == 0 && y == 0)
                        {
                            line += "S";
                        }
                        else
                        {
                            switch (GetMapData(x, y))
                            {
                                case 0:
                                    line += "."; break;
                                case 1:
                                    line += "#"; break;
                                default:
                                    line += "?"; break;
                            }

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
                                case 1: tryState = new ManhattanVector2(position.X, position.Y - 1); break;
                                case 2: tryState = new ManhattanVector2(position.X, position.Y + 1); break;
                                case 3: tryState = new ManhattanVector2(position.X - 1, position.Y); break;
                                case 4: tryState = new ManhattanVector2(position.X + 1, position.Y); break;
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

                    if (tryState.Y < position.Y) code = 1; // up
                    else if (tryState.Y > position.Y) code = 2; // down
                    else if (tryState.X < position.X) code = 3; // left
                    else if (tryState.X > position.X) code = 4; // right

                    cpu.Input.Enqueue(code); // stop
                }
            }

            public IEnumerable<ManhattanVector2> FindPath(ManhattanVector2 start, ManhattanVector2 end)
            {
                return AStar<ManhattanVector2>.FindPath(map, start, end);
            }

        }

        public static int Part1(string input, ILogger logger = null)
        {
            var droid = new RepairDrone(input);
            droid.Run();

            droid.DrawMap(logger);

            logger?.WriteLine($"Map explored in {droid.Steps} steps");

            var oxygenSystemPosition = droid.FindCell(RepairDrone.OXYGEN);

            var path = droid.FindPath(new ManhattanVector2(0, 0), new ManhattanVector2(oxygenSystemPosition));

            return path.Count();
        }

        public static int Part2(string input)
        {
            var droid = new RepairDrone(input);
            droid.Run();
            var oxygenSystemPosition = new ManhattanVector2(droid.FindCell(2));

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
                            var path = droid.FindPath(oxygenSystemPosition, new ManhattanVector2(x, y));
                            dist = Math.Max(dist, path.Count());
                        }
                    }
                }
            }


            // foreach (var kvp in droid.map)
            // {
            //     if (kvp.Value.Data()==0)
            //     {
            //         var path = droid.FindPath(oxygenSystemPosition, new ManhattanVector2(kvp.Key));
            //         dist = Math.Max(dist, path.Count());
            //     }
            // }

            return dist;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input, logger));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}