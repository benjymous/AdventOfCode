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
            public PortalMap()
                : base (null)
            {

            }

            public Dictionary<string, ManhattanVector2> Portals {get;set;} = new Dictionary<string, ManhattanVector2>();

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
            var lines = Util.Split(input);

            var map = new PortalMap();

            var portals = new Dictionary<ManhattanVector2, string>();

            int height = lines.Length;
            int width = lines.First().Length;

            for (var y=0; y<lines.Length; ++y)
            {
                var line = lines[y];
                for(var x=0; x<line.Length; ++x)
                {
                    var c = line[x];
                    if (IsCapitalLetter(c))
                    {
                        // part of a portal

                        var leftNeighbour = map.data.GetStrKey($"{x-1},{y}");
                        var aboveNeighbour = map.data.GetStrKey($"{x},{y-1}");

                        string code = null;

                        if (IsCapitalLetter(leftNeighbour))
                        {
                            code = $"{leftNeighbour}{c}";

                            var leftLeftNeighbour = map.data.GetStrKey($"{x-2},{y}");
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
                            var aboveAboveNeighbour = map.data.GetStrKey($"{x},{y-2}");
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
                    map.data.PutStrKey($"{x},{y}", c);
                }
            }

            var groups = portals.GroupBy(kvp => kvp.Value);

            foreach (var group in groups)
            {
                if (group.Count()==2)
                {
                    map.Portals[group.First().Key.ToString()] = group.Last().Key;
                    map.Portals[group.Last().Key.ToString()] = group.First().Key;
                }
            }
            
            var start = portals.Where(kvp => kvp.Value == "AA").First().Key;
            var end = portals.Where(kvp => kvp.Value == "ZZ").First().Key;


            var jobqueue = new Queue<Tuple<ManhattanVector2,int>>();
            jobqueue.Enqueue(Tuple.Create(end,0));
            int best = int.MaxValue;
            var cache = new Dictionary<string, int>();

            cache[end.ToString()] = 0;

            while (jobqueue.Any())
            {
                var entry = jobqueue.Dequeue();

                if (entry.Item1 == start)
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

            //Console.WriteLine(Part1("         A           \n         A           \n  #######.#########  \n  #######.........#  \n  #######.#######.#  \n  #######.#######.#  \n  #######.#######.#  \n  #####  B    ###.#  \nBC...##  C    ###.#  \n  ##.##       ###.#  \n  ##...DE  F  ###.#  \n  #####    G  ###.#  \n  #########.#####.#  \nDE..#######...###.#  \n  #.#########.###.#  \nFG..#########.....#  \n  ###########.#####  \n             Z       \n             Z       "));


            //Console.WriteLine(Part1("                   A               \n                   A               \n  #################.#############  \n  #.#...#...................#.#.#  \n  #.#.#.###.###.###.#########.#.#  \n  #.#.#.......#...#.....#.#.#...#  \n  #.#########.###.#####.#.#.###.#  \n  #.............#.#.....#.......#  \n  ###.###########.###.#####.#.#.#  \n  #.....#        A   C    #.#.#.#  \n  #######        S   P    #####.#  \n  #.#...#                 #......VT\n  #.#.#.#                 #.#####  \n  #...#.#               YN....#.#  \n  #.###.#                 #####.#  \nDI....#.#                 #.....#  \n  #####.#                 #.###.#  \nZZ......#               QG....#..AS\n  ###.###                 #######  \nJO..#.#.#                 #.....#  \n  #.#.#.#                 ###.#.#  \n  #...#..DI             BU....#..LF\n  #####.#                 #.#####  \nYN......#               VT..#....QG\n  #.###.#                 #.###.#  \n  #.#...#                 #.....#  \n  ###.###    J L     J    #.#.###  \n  #.....#    O F     P    #.#...#  \n  #.###.#####.#.#####.#####.###.#  \n  #...#.#.#...#.....#.....#.#...#  \n  #.#####.###.###.#.#.#########.#  \n  #...#.#.....#...#.#.#.#.....#.#  \n  #.###.#####.###.###.#.#.#######  \n  #.#.........#...#.............#  \n  #########.###.###.#############  \n           B   J   C               \n           U   P   P               "));
            
            console.WriteLine("- Pt1 - "+Part1(input));
            //console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}