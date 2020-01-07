using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXIX
{
    public class Day20 : IPuzzle
    {
        public string Name { get { return "2019-20";} }

        public static bool IsCapitalLetter(char c)
        {
            return c>='A' && c <='Z';
        }

        class PortalMap : AStar.GridMap<char>, AStar.IIsWalkable<char>
        {
            public PortalMap(string input)
                : base (null)
            {
                var lines = Util.Split(input);
                var portals = new Dictionary<ManhattanVector2, string>();
                var innerPortals = new Dictionary<ManhattanVector2, bool>();

                Height = lines.Length;
                Width = lines.First().Length;

                for (var y=0; y<lines.Length; ++y)
                {
                    var line = lines[y];
                    for(var x=0; x<line.Length; ++x)
                    {
                        var c = line[x];
                        if (IsCapitalLetter(c))
                        {
                            // part of a portal

                            var leftNeighbour = data.GetStrKey($"{x-1},{y}");
                            var aboveNeighbour = data.GetStrKey($"{x},{y-1}");

                            string code = null;

                            ManhattanVector2 v = null;
                            bool isInner = false;

                            if (IsCapitalLetter(leftNeighbour))
                            {
                                code = $"{leftNeighbour}{c}";

                                var leftLeftNeighbour = data.GetStrKey($"{x-2},{y}");
                                
                                if (leftLeftNeighbour == '.')
                                {
                                    v = new ManhattanVector2(x-2, y);
                                    isInner = v.X != Width-3;
                                }
                                else
                                {
                                    v = new ManhattanVector2(x+1, y);
                                    isInner = v.X != 2;
                                }
                                
                            }
                            else if (IsCapitalLetter(aboveNeighbour))
                            {
                                code = $"{aboveNeighbour}{c}";
                                var aboveAboveNeighbour = data.GetStrKey($"{x},{y-2}");
                                if (aboveAboveNeighbour == '.')
                                {
                                    v = new ManhattanVector2(x, y-2);
                                    isInner = v.Y != Height-3;
                                }
                                else
                                {
                                    v = new ManhattanVector2(x, y+1);
                                    isInner = v.Y != 2;
                                }
                            }

                            if (!ReferenceEquals(v,null))
                            {
                                portals[v] = code;
                                innerPortals[v] = isInner;
                            }


                        }
                        data.PutStrKey($"{x},{y}", c);
                    }
                }

                var groups = portals.GroupBy(kvp => kvp.Value);

                foreach (var group in groups)
                {
                    if (group.Count()==2)
                    {
                        Portals[GetKey(group.First().Key)] = Tuple.Create(group.Last().Key, innerPortals[group.First().Key]);
                        Portals[GetKey(group.Last().Key)] = Tuple.Create(group.First().Key, innerPortals[group.Last().Key]);
                    }
                }
                
                Start = portals.Where(kvp => kvp.Value == "AA").First().Key;
                End = portals.Where(kvp => kvp.Value == "ZZ").First().Key;

            }

            public bool Part2 {get;set;} = false;

            public Dictionary<int, Tuple<ManhattanVector2, bool>> Portals {get;private set;} = new Dictionary<int, Tuple<ManhattanVector2, bool>>();

            public ManhattanVector2 Start {get;private set;}
            public ManhattanVector2 End {get;private set;}

            public int Width {get;private set;}
            public int Height {get;private set;}

            public int MaxDepth {get;set;} = 25;
        

            public override IEnumerable<ManhattanVector2> GetNeighbours(ManhattanVector2 centre)
            {
                if (Portals.TryGetValue(GetKey(centre), out var other))
                {
                    yield return other.Item1;
                }

                foreach (var v in base.GetNeighbours(centre)) yield return v;
            }

            public IEnumerable<Tuple<ManhattanVector2, int>> GetNeighbours2(ManhattanVector2 centre, int level)
            {
                if (Portals.TryGetValue(GetKey(centre), out var other))
                {
                    int newLevel = level;
                    if (other.Item2)
                    {
                        newLevel++;
                    }
                    else
                    {
                        newLevel--;
                    }
                    if (newLevel >=0 && newLevel <= MaxDepth)
                    {
                        yield return Tuple.Create(other.Item1, newLevel);
                    }
                }

                foreach (var v in base.GetNeighbours(centre)) yield return Tuple.Create(v, level);
            }

            public bool IsWalkable(char cell)
            {
                return cell == '.';
            }
        }

        public static int GetKey(ManhattanVector2 pos) => (pos.X << 8)+(pos.Y);
        public static int GetKey(ManhattanVector2 pos, int level) => (pos.X << 16)+(pos.Y << 8)+level;

        public static int Part1(string input)
        {
            var map = new PortalMap(input);

            var jobqueue = new Queue<Tuple<ManhattanVector2,int>>();
            jobqueue.Enqueue(Tuple.Create(map.End,0));
            int best = int.MaxValue;
            var cache = new Dictionary<int, int>();

            cache[GetKey(map.End)] = 0;

            while (jobqueue.Any())
            {
                var entry = jobqueue.Dequeue();

                if (entry.Item1 == map.Start)
                {
                    if (entry.Item2 < best)
                    {
                        best = entry.Item2;
                    }
                }
                else
                {
                    foreach (var neighbour in map.GetNeighbours(entry.Item1))
                    {
                        var key = GetKey(neighbour);                        

                        int newDistance = entry.Item2+1;
                        if (cache.TryGetValue(key, out var dist))
                        {
                            if (dist < newDistance)
                            {
                                continue;
                            }
                        }
                        cache[key] = newDistance;
                        jobqueue.Enqueue(Tuple.Create(neighbour, newDistance));
                    }
                }
            }

            // for (var y=0; y<height; ++y)
            // {
            //     for (var x=0; x<width; ++x)
            //     {
            //         var c = map.data[$"{x},{y}"];
            //         if (c == '.')
            //         {
            //             if (cache.TryGetValue($"{x},{y}", out int v))
            //             {
            //                 var d = v.ToString("X2");

            //                 if (v>255)
            //                 {
            //                     d = "!!";
            //                 }

            //                 Console.Write(d);
            //             }
            //             else
            //             {
            //                 Console.Write("..");
            //             }
            //         }
            //         else
            //         {
            //             Console.Write($"{c}{c}");
            //         }
            //     }
            //     Console.WriteLine();
            // }

            return best;
        }



  

        public static int Part2(string input)
        {
            var map = new PortalMap(input);

            var jobqueue = new Queue<Tuple<ManhattanVector2,int,int>>();
            jobqueue.Enqueue(Tuple.Create(map.End,0,0));
            int best = int.MaxValue;
            var cache = new Dictionary<int, int>();

            cache[GetKey(map.End, 0)] = 0;

            int deepest = 0;

            while (jobqueue.Any())
            {
                var entry = jobqueue.Dequeue();
                deepest = Math.Max(deepest, entry.Item2);

                //Console.WriteLine($"{entry.Item1}:{entry.Item2} - {entry.Item3}");

                if (entry.Item1 == map.Start && entry.Item2 == 0)
                {
                    if (entry.Item2 < best)
                    {
                        best = entry.Item3;
                    }
                }
                else
                {
                    var neighbours = map.GetNeighbours2(entry.Item1, entry.Item2);
                    foreach (var neighbour in neighbours)
                    {
                        var key = GetKey(neighbour.Item1, neighbour.Item2);

                        int newDistance = entry.Item3+1;
                        if (cache.TryGetValue(key, out var dist))
                        {
                            if (dist < newDistance)
                            {
                                continue;
                            }
                        }
                        cache[key] = newDistance;
                        jobqueue.Enqueue(Tuple.Create(neighbour.Item1, neighbour.Item2, newDistance));
                    }
                }
            }

            return best;
        }

        public void Run(string input, ILogger logger)
        {       
            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}