using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            public bool IsWalkable()
            {
                return data == '.' || (data >='a' && data <='z');
            }

            public int Data()
            {
                return data;
            }
        }
        
        static Dictionary<string, AStar.IRoom> CloneMap(Dictionary<string, AStar.IRoom> map, char remove)
        {
            var newMap = new Dictionary<string, AStar.IRoom>();

            foreach (var kvp in map)
            {
                var node = kvp.Value as Node;
                char c = node.data;
                if (remove == c) c = '.';
                newMap[kvp.Key] = new Node(c);
            }
            return newMap;
        }

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

        public static int Solve(Dictionary<string, AStar.IRoom> map, ManhattanVector2 position, Dictionary<char, ManhattanVector2> availableKeys, Dictionary<char, ManhattanVector2> doors, int totalScore)
        {
            if (availableKeys.Count == 0)
            {
                min = Math.Min(totalScore, min);
                Console.WriteLine($"[{min}] - Path found at {totalScore}");
                
                return 0;
            }
            var finder = new AStar.RoomPathFinder();

            //int shortestPath = int.MaxValue;

            var paths = availableKeys.Select(k => Tuple.Create(k, finder.FindPath(map, position, k.Value))).Where(v => v.Item2.Count() > 0).OrderBy(v => v.Item2.Count());

            var shortestPath = paths.AsParallel().Select(tup => tup.Item2.Count + Solve(CloneMap(map, char.ToUpper(tup.Item1.Key)), tup.Item1.Value, CloneKeys(availableKeys, tup.Item1.Key), doors, totalScore+tup.Item2.Count)).Min();

            // foreach (var tup in paths)
            // {           
            //     var path = tup.Item2;
            //     var key = tup.Item1;
            //     if (path.Count < shortestPath)
            //     {
            //         int newVal = path.Count + Solve(CloneMap(map, char.ToUpper(key.Key)), key.Value, CloneKeys(availableKeys, key.Key), doors, totalScore+path.Count);
            //         shortestPath = Math.Min(shortestPath, newVal);
            //     }
            // }

            // foreach (var key in availableKeys.OrderBy(k => k.Key))
            // {
            //     var path = finder.FindPath(map, position, key.Value);
            //     if (path.Any())
            //     {               
            //         if (path.Count < shortestPath)
            //         {
            //             int newVal = path.Count + Solve(CloneMap(map, char.ToUpper(key.Key)), key.Value, CloneKeys(availableKeys, key.Key), doors, totalScore+path.Count);
            //             shortestPath = Math.Min(shortestPath, newVal);
            //         }
            //     }
            // }

            return shortestPath;
        }
 
        public static int Part1(string input)
        {
            min = int.MaxValue;
            var lines = Util.Split(input);

            var map = new Dictionary<string, AStar.IRoom>();
            ManhattanVector2 position = new ManhattanVector2(0,0);
            var availableKeys = new Dictionary<char, ManhattanVector2>();
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
                    }
                    if (c>='A' && c <='Z')
                    {
                        doors[char.ToLower(c)] = new ManhattanVector2(x,y);
                    }
                    map.PutStrKey($"{x},{y}", new Node(c));
                }
            }

            return Solve(map, position, availableKeys, doors, 0);
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

            // Test(Part1("#########\n#b.A.@.a#\n#########"), 8);

            // Test(Part1("########################\n#f.D.E.e.C.b.A.@.a.B.c.#\n######################.#\n#d.....................#\n########################"), 86);
        
            // Test(Part1("########################\n#...............b.C.D.f#\n#.######################\n#.....@.a.B.c.d.A.e.F.g#\n########################"), 132);

            // Test(Part1("#################\n#i.G..c...e..H.p#\n########.########\n#j.A..b...f..D.o#\n########@########\n#k.E..a...g..B.n#\n########.########\n#l.F..d...h..C.m#\n#################"), 136);

            //console.WriteLine("- Pt1 - "+Part1(input));
            //console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}