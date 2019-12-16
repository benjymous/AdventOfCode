using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent.AStar;

namespace Advent.MMXIX
{
    public class Day15 : IPuzzle
    {
        public string Name { get { return "2019-15";} }

        public class Node : AStar.IRoom
        {
            public Node(int d)
            {
                data = d;
            }
            public int data;
            public bool IsWalkable()
            {
                return data != 1;
            }

            public int Data()
            {
                return data;
            }
        }

        public class RepairDrone : NPSA.ICPUInterrupt
        {
            NPSA.IntCPU cpu;
            ManhattanVector2 position = new ManhattanVector2(0,0);

            ManhattanVector2 tryState = null;
            Tuple<ManhattanVector2,ManhattanVector2> target = null;
            bool hasTarget = false;

            int minx = 0, miny = 0;
            int maxx = 0, maxy = 0;

            public Dictionary<string,AStar.IRoom> map = new Dictionary<string,AStar.IRoom>();

            public string FindCell(int num)
            {
                return map.Where(kvp => kvp.Value.Data() == num).First().Key;
            }

            HashSet<Tuple<ManhattanVector2,ManhattanVector2>> unknowns = new HashSet<Tuple<ManhattanVector2,ManhattanVector2>>();

            public enum Mode
            {
                Interactive = 0,
                Sweep = 2,
            }

            public Mode mode = Mode.Sweep;


            public RepairDrone(string input)
            {
                cpu = new NPSA.IntCPU(input);
                cpu.Interrupt = this;
            }

            public void Run()
            {
                cpu.Run();
            }

            public void AddIfUnknown(ManhattanVector2 current, ManhattanVector2 neighbour)
            {
                if (!map.ContainsKey(neighbour.ToString()))
                {
                    unknowns.Add(Tuple.Create(current, neighbour));
                }
            }

            public void AddUnknowns()
            {
                AddIfUnknown(position, new ManhattanVector2(position.X-1, position.Y));
                AddIfUnknown(position, new ManhattanVector2(position.X+1, position.Y));
                AddIfUnknown(position, new ManhattanVector2(position.X, position.Y-1));
                AddIfUnknown(position, new ManhattanVector2(position.X, position.Y+1));
            }

            public void HasPutOutput()
            {
                var val = cpu.Output.Dequeue();

                switch (val)
                {
                    case 0:
                    {
                        if (mode == Mode.Interactive) Console.WriteLine("Wall!");

                        map.PutObjKey(tryState, new Node(1));
                    }
                    break;
                    case 1: 
                    {
                        if (mode == Mode.Interactive) Console.WriteLine("Ok");
                        map.PutObjKey(tryState, new Node(0));
                        position = tryState;
                    }
                    break;
                    case 2: 
                    {
                        if (mode == Mode.Interactive) Console.WriteLine("Found Oxygen system"); 
                        map.PutObjKey(tryState, new Node(2));
                        position = tryState;
                    }
                    break;
                    default: 
                    {
                        if (mode == Mode.Interactive) Console.WriteLine("??? {val}"); 
                        map.PutObjKey(tryState, new Node((int)val));
                        position = tryState;
                    }
                    break;
                }
                minx = Math.Min(minx, tryState.X);
                maxx = Math.Max(maxx, tryState.X);

                miny = Math.Min(miny, tryState.Y);
                maxy = Math.Max(maxy, tryState.Y);
            }

            public void DrawMap(System.IO.TextWriter console)
            {
                console.WriteLine();
                for (var y=miny; y<=maxy; ++y)
                {
                    var line = "";
                    for (int x=minx; x <=maxx; ++x)
                    {
                
                        if (x == position.X && y == position.Y)
                        {
                            line += "@";
                        }
                        else if (x==0 && y==0)
                        {
                            line += "S";
                        }
                        else
                        {         
                            var key = $"{x},{y}";
                            if (map.ContainsKey(key))
                            {                  
                                var node = map.GetStrKey(key) as Node;
                                switch(node.data)
                                {
                                    case 0:
                                        line += "."; break;
                                    case 1:
                                        line += "#"; break;
                                    default:
                                        line += $"{node.data}"; break;
                                }
                            }
                            else
                            {
                                line += " ";
                            }
                        }
                    }
                    console.WriteLine(line);
                }
                console.WriteLine();

            }
            public void WillReadInput()
            {
                if (mode == Mode.Interactive)
                {
                    DrawMap(Console.Out);
                    Console.WriteLine($"Located at {position}");
                    Console.WriteLine("north (1), south (2), west (3), and east (4) ?");
                    
                    int code = 0;

                    while (code <1 || code > 4)
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
                else if (mode == Mode.Sweep)
                {
                    //DrawMap();

                    if (mode == Mode.Sweep)
                    {
                        AddUnknowns();
                    }

                    if (unknowns.Count == 0)
                    {
                        cpu.Input.Enqueue(0); // stop
                        return;
                    }

                    if (!hasTarget)
                    {
                        target = unknowns.OrderBy(p => p.Item2.Distance(position)).First();
                        unknowns.Remove(target);
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
                    else if (tryState.Y > position.Y)  code = 2; // down
                    else if (tryState.X < position.X) code = 3; // left
                    else if (tryState.X > position.X) code = 4; // right

                    cpu.Input.Enqueue(code); // stop
                }
            }

            public IEnumerable<ManhattanVector2> FindPath(ManhattanVector2 start, ManhattanVector2 end)
            {
                var finder = new RoomPathFinder();
                return finder.FindPath(map, start, end);
            }
          
        }
 
        public static int Part1(string input, System.IO.TextWriter console = null)
        {
            var droid = new RepairDrone(input);
            droid.Run();

            droid.DrawMap(console==null ? Console.Out : console);

            var oxygenSystemPosition = droid.FindCell(2);

            var path = droid.FindPath(new ManhattanVector2(0,0), new ManhattanVector2(oxygenSystemPosition));

            return path.Count();
        }

        public static int Part2(string input)
        {
            var droid = new RepairDrone(input);
            droid.Run();
            var oxygenSystemPosition = new ManhattanVector2(droid.FindCell(2));

            var dist = 0;
            foreach (var kvp in droid.map)
            {
                if (kvp.Value.Data()==0)
                {
                    var path = droid.FindPath(oxygenSystemPosition, new ManhattanVector2(kvp.Key));
                    dist = Math.Max(dist, path.Count());
                }
            }

            return dist;
        }

        public void Run(string input, System.IO.TextWriter console)
        {
            console.WriteLine("- Pt1 - "+Part1(input, console));
            console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}