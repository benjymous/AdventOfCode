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

                            if (IsCapitalLetter(leftNeighbour))
                            {
                                code = $"{leftNeighbour}{c}";

                                var leftLeftNeighbour = data.GetStrKey($"{x-2},{y}");
                                if (leftLeftNeighbour == '.')
                                {
                                    portals[new ManhattanVector2(x-2, y)] = code;
                                }
                                else
                                {
                                    portals[new ManhattanVector2(x+1, y)] = code;
                                }
                            }
                            else if (IsCapitalLetter(aboveNeighbour))
                            {
                                code = $"{aboveNeighbour}{c}";
                                var aboveAboveNeighbour = data.GetStrKey($"{x},{y-2}");
                                if (aboveAboveNeighbour == '.')
                                {
                                    portals[new ManhattanVector2(x, y-2)] = code;
                                }
                                else
                                {
                                    portals[new ManhattanVector2(x, y+1)] = code;
                                }
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
                        Portals[group.First().Key.ToString()] = group.Last().Key;
                        Portals[group.Last().Key.ToString()] = group.First().Key;
                    }
                }
                
                Start = portals.Where(kvp => kvp.Value == "AA").First().Key;
                End = portals.Where(kvp => kvp.Value == "ZZ").First().Key;

            }

            public bool Part2 {get;set;} = false;

            public Dictionary<string, ManhattanVector2> Portals {get;private set;} = new Dictionary<string, ManhattanVector2>();

            public ManhattanVector2 Start {get;private set;}
            public ManhattanVector2 End {get;private set;}

            public int Width {get;private set;}
            public int Height {get;private set;}
        

            public override IEnumerable<ManhattanVector2> GetNeighbours(ManhattanVector2 center)
            {
                if (Portals.TryGetValue(center.ToString(), out var other))
                {
                    yield return other;
                }

                foreach (var v in base.GetNeighbours(center)) yield return v;
            }

            public bool IsWalkable(char cell)
            {
                return cell == '.';
            }
        }
 
        public static int Part1(string input)
        {
            var map = new PortalMap(input);

            var jobqueue = new Queue<Tuple<ManhattanVector2,int>>();
            jobqueue.Enqueue(Tuple.Create(map.End,0));
            int best = int.MaxValue;
            var cache = new Dictionary<string, int>();

            cache[map.End.ToString()] = 0;

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
                    if (entry.Item1.ToString() == "2,13")
                    {
                        Console.WriteLine("Stop!");
                    }

                    foreach (var neighbour in map.GetNeighbours(entry.Item1))
                    {
                        var key = neighbour.ToString();                        

                        if (key == "9,10")
                        {
                            Console.WriteLine("Stop!");
                        }

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
            return 0;
        }

        public void Run(string input, System.IO.TextWriter console)
        {       
            console.WriteLine("- Pt1 - "+Part1(input));
            //console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}