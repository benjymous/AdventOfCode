using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent.AStar;

namespace Advent.MMXIX
{
    public class Day18 : IPuzzle
    {
        public string Name { get { return "2019-18";} }

        public class Node : AStar.IRoom
        {
            public Node(char c)
            {
                data = c;
            }
            public char data;

            public int Data()
            {
                return data;
            }
        }

        public class Callback : AStar.ICanWalk
        {
            public HashSet<char> HeldKeys {get;set;}
            public bool IsWalkable(IRoom room)
            {
                int data = room.Data();
                return data == '.' || (data >='a' && data <='z') || HeldKeys.Contains((Char)data);
            }
        }

        // static Dictionary<string, AStar.IRoom> CloneMap(Dictionary<string, AStar.IRoom> map, char remove)
        // {
        //     var newMap = new Dictionary<string, AStar.IRoom>();

        //     foreach (var kvp in map)
        //     {
        //         var node = kvp.Value as Node;
        //         char c = node.data;
        //         if (remove == c) c = '.';
        //         newMap[kvp.Key] = new Node(c);
        //     }
        //     return newMap;
        // }

        static Dictionary<char, ManhattanVector2> CloneKeys(Dictionary<char, ManhattanVector2> keys, char remove)
        {
            var newKeys = new Dictionary<char, ManhattanVector2>();
            foreach (var k in keys)
            {
                if (k.Key != remove)
                {
                    newKeys[k.Key] = k.Value;
                }
            }
            return newKeys;
        }

         static int min = int.MaxValue;

        //         public static int Solve(Dictionary<string, AStar.IRoom> map, ManhattanVector2 position, Dictionary<char, ManhattanVector2> availableKeys, HashSet<char> allKeys, int totalScore)
        //         {
        //             if (availableKeys.Count == 0)
        //             {
        //                 min = Math.Min(totalScore, min);
        //                 Console.WriteLine($"[{min}] - Path found at {totalScore}");

        //                 return 0;
        //             }
        //             var finder = new AStar.RoomPathFinder();



        //             var callback = new Callback();
        //             callback.HeldKeys = new HashSet<char>(allKeys.Where(k => !availableKeys.ContainsKey(k)).Select(k => char.ToUpper(k)));

        //             var paths = availableKeys.Select(k => Tuple.Create(k, finder.FindPath(map, position, k.Value, callback))).Where(v => v.Item2.Count() > 0 && (v.Item2.Count() + totalScore) < min).OrderBy(v => v.Item2.Count());

        //             if (!paths.Any()) return 9999;

        // #if BLAH
        //             var shortestPath = paths.AsParallel().Select(tup => tup.Item2.Count + Solve(map, tup.Item1.Value, CloneKeys(availableKeys, tup.Item1.Key), allKeys, totalScore+tup.Item2.Count)).Min();
        // #else
        //             int shortestPath = int.MaxValue;
        //             foreach (var tup in paths)
        //             {           
        //                 var path = tup.Item2;
        //                 var key = tup.Item1;
        //                 if (path.Count < shortestPath)
        //                 {
        //                     int newVal = path.Count + Solve(map, key.Value, CloneKeys(availableKeys, key.Key), allKeys, totalScore+path.Count);
        //                     shortestPath = Math.Min(shortestPath, newVal);
        //                 }
        //             }
        // #endif

        //             // foreach (var key in availableKeys.OrderBy(k => k.Key))
        //             // {
        //             //     var path = finder.FindPath(map, position, key.Value);
        //             //     if (path.Any())
        //             //     {               
        //             //         if (path.Count < shortestPath)
        //             //         {
        //             //             int newVal = path.Count + Solve(CloneMap(map, char.ToUpper(key.Key)), key.Value, CloneKeys(availableKeys, key.Key), doors, totalScore+path.Count);
        //             //             shortestPath = Math.Min(shortestPath, newVal);
        //             //         }
        //             //     }
        //             // }

        //             return shortestPath;
        //         }

        public static int Solve(ManhattanVector2 position, Dictionary<string, AStar.IRoom> map, Dictionary<string, IEnumerable<ManhattanVector2>> paths, HashSet<char> allKeys, Dictionary<char, ManhattanVector2> availableKeys, int totalScore)
        {

            if (availableKeys.Count == 0)
            {
                min = Math.Min(totalScore, min);
                Console.WriteLine($"[{min}] - Path found at {totalScore}");
                return 0;
            }

            int shortestPath = int.MaxValue;
            var heldKeys = new HashSet<char>(allKeys.Where(k => !availableKeys.ContainsKey(k)).Select(k => char.ToUpper(k)));

            List<Tuple<char, ManhattanVector2, IEnumerable<ManhattanVector2>>> possiblePaths = new List<Tuple<char, ManhattanVector2, IEnumerable<ManhattanVector2>>>();

            foreach (var key in availableKeys.OrderBy(k => k.Key))
            {

                var pathKey = $"{position}:{key.Value}";

                IEnumerable<ManhattanVector2> path;

                if (!paths.TryGetValue(pathKey, out path))
                {
                    pathKey = $"{key.Value}:{position}";

                    if (!paths.TryGetValue(pathKey, out path))
                    {
                        throw new Exception("Couldn't find path!");
                    }
                }

                if (IsWalkable(map, path, heldKeys))
                {
                    //var newVal = path.Count() + Solve(key.Value, map, paths, allKeys, CloneKeys(availableKeys, key.Key), totalScore+path.Count());
                    //shortestPath = Math.Min(shortestPath, newVal);

                    possiblePaths.Add(Tuple.Create(key.Key, key.Value, path));
                }
            }

            if (totalScore == 0)
            { 
                return possiblePaths.OrderBy(t => t.Item3.Count()).AsParallel().Select(t => Solve(t.Item2, map, paths, allKeys, CloneKeys(availableKeys, t.Item1), totalScore + t.Item3.Count())).Min();
            }
            else
            {
                return possiblePaths.OrderBy(t => t.Item3.Count()).Select(t => Solve(t.Item2, map, paths, allKeys, CloneKeys(availableKeys, t.Item1), totalScore + t.Item3.Count())).Min();
            }
        }

        public static bool IsWalkable(Dictionary<string, AStar.IRoom> map, IEnumerable<ManhattanVector2> path, HashSet<char> heldKeys)
        {
            foreach (var pos in path)
            {
                var node = map.GetObjKey(pos);
                if (node==null) 
                {
                    return false;
                }
                var c = (node as Node).data;
                if (c != '.')
                {
                    if (c>='A' && c <='Z')
                    {
                        if (!heldKeys.Contains(c))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
 
        public static int Part1(string input)
        {
            min = int.MaxValue;

            var lines = Util.Split(input);

            var map = new Dictionary<string, AStar.IRoom>();
            ManhattanVector2 position = new ManhattanVector2(0,0);
            var availableKeys = new Dictionary<char, ManhattanVector2>();
            var allKeys = new HashSet<char>();
            var doors = new Dictionary<char, ManhattanVector2>();

            for (var y=0; y<lines.Length; ++y)
            {
                var line = lines[y];
                for(var x=0; x<line.Length; ++x)
                {
                    var c = line[x];
                    if (c=='@')
                    {
                        position = new ManhattanVector2(x,y);
                        c= '.';
                    }
                    if (c>='a' && c <='z')
                    {
                        availableKeys[c] = new ManhattanVector2(x,y);
                        allKeys.Add(c);
                    }
                    if (c>='A' && c <='Z')
                    {
                        doors[char.ToLower(c)] = new ManhattanVector2(x,y);
                    }
                    map.PutStrKey($"{x},{y}", new Node(c));
                }
            }

            var paths = new Dictionary<string, IEnumerable<ManhattanVector2>>();

            var finder = new AStar.RoomPathFinder();
            var callback = new Callback();
            callback.HeldKeys = new HashSet<char>(allKeys.Select(c => char.ToUpper(c)));

            foreach (var k1 in allKeys)
            {
                paths[$"{position}:{availableKeys[k1]}"] = finder.FindPath(map, position, availableKeys[k1], callback);

                foreach (var k2 in allKeys.Where(k => k > k1))
                {
                    paths[$"{availableKeys[k1]}:{availableKeys[k2]}"] = finder.FindPath(map, availableKeys[k1], availableKeys[k2], callback);
                }
            }

            // foreach (var p in paths)
            // {
            //     Console.WriteLine($"{p.Key} - {IsWalkable(map, p.Value, new HashSet<char>())}");
            // };

            return Solve(position, map, paths, allKeys, availableKeys, 0);

            //return Solve(map, position, availableKeys, allKeys, 0);
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Test(int actual, int expected)
        {
            if (expected != actual)
            {
                throw new Exception("FAIL");
            }
            Console.WriteLine(actual);
        }

        public void Run(string input, System.IO.TextWriter console)
        {

            //Test(Part1("#########\n#b.A.@.a#\n#########"), 8);

            //Test(Part1("########################\n#f.D.E.e.C.b.A.@.a.B.c.#\n######################.#\n#d.....................#\n########################"), 86);
        
            //Test(Part1("########################\n#...............b.C.D.f#\n#.######################\n#.....@.a.B.c.d.A.e.F.g#\n########################"), 132);

            //Test(Part1("#################\n#i.G..c...e..H.p#\n########.########\n#j.A..b...f..D.o#\n########@########\n#k.E..a...g..B.n#\n########.########\n#l.F..d...h..C.m#\n#################"), 136);

            //console.WriteLine("- Pt1 - "+Part1(input));
            //console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}