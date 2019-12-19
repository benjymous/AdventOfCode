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
            public bool IsWalkable(IRoom room)
            {
                // we don't care about keys or doors, just the shortest possible path
                return room.Data() != '#';
            }
        }

        public static int KeyCode(char c)
        {
            return (1 << char.ToLower(c)-'a');
        }

        public static int AddKey(int v, char c)
        {
            return v | KeyCode(c);
        }

        public static IEnumerable<int> Bits(int v)
        {
            for(int k=1; k<=v; k<<=1)
            {
                if ((v & k)>0) yield return k;
            }
        }

        public class RoomPath
        {
            public RoomPath(Dictionary<string, AStar.IRoom> map, IEnumerable<ManhattanVector2> path)
            {
                Path = path;

                List<char> seenDoors = new List<char>();

                foreach (var pos in path)
                {
                    var node = map.GetObjKey(pos);              
                    var c = (node as Node).data;

                    if (c>='A' && c <='Z')
                    {
                        Doors = AddKey(Doors, c);
                    }                  
                }

                Count = path.Count();
            }

            public bool IsWalkable(int heldKeys) => (Doors & heldKeys) == Doors;

            public IEnumerable<ManhattanVector2> Path {get;private set;}

            public int Count {get;private set;}

            int Doors = 0;
        }
 
        public static int Part1(string input)
        {
            var lines = Util.Split(input);

            var map = new Dictionary<string, AStar.IRoom>();
            ManhattanVector2 startPosition = new ManhattanVector2(0,0);
            var keyPositions = new Dictionary<int, ManhattanVector2>();
            int allKeys = 0;
            var doors = new Dictionary<int, ManhattanVector2>();

            for (var y=0; y<lines.Length; ++y)
            {
                var line = lines[y];
                for(var x=0; x<line.Length; ++x)
                {
                    var c = line[x];
                    if (c=='@')
                    {
                        startPosition = new ManhattanVector2(x,y);
                        c= '.';
                    }
                    if (c>='a' && c <='z')
                    {
                        keyPositions[KeyCode(c)] = new ManhattanVector2(x,y);
                        allKeys|=KeyCode(c);
                    }
                    if (c>='A' && c <='Z')
                    {
                        doors[KeyCode(c)] = new ManhattanVector2(x,y);
                    }
                    map.PutStrKey($"{x},{y}", new Node(c));
                }
            }

            var paths = new Dictionary<int, RoomPath>();

            var finder = new AStar.RoomPathFinder();
            var callback = new Callback();

            foreach (var k1 in Bits(allKeys))  
            {
                paths[k1] = new RoomPath(map, finder.FindPath(map, startPosition, keyPositions[k1], callback));
                foreach (var k2 in Bits(allKeys))  
                {
                    if (k2 > k1)
                    {
                        var path = new RoomPath(map,  finder.FindPath(map, keyPositions[k1], keyPositions[k2], callback));
                        paths[k1|k2] = path;
                    }
                }
            }

            var queue = new Queue<Tuple<int, int, int>>();

            queue.Enqueue(Tuple.Create(0, 0, 0));

            var cache = new Dictionary<string,int>();

            int currentBest = int.MaxValue;

            while (queue.Any())
            {
                var item = queue.Dequeue();

                var position = item.Item1;
                var heldKeys = item.Item2;
                int distance = item.Item3;

                int tryKeys = allKeys - heldKeys;

                if (tryKeys>0)
                {
                    foreach (var key in Bits(tryKeys))
                    {
                        var path = paths[position|key];
                        if (path.IsWalkable(heldKeys))
                        {
                            var newKeys = heldKeys|key;
                            var next = Tuple.Create(key, newKeys, distance+path.Count);
                            var nextKey = $"{next.Item1}:{next.Item2}";
                            if (!cache.TryGetValue(nextKey, out int cachedBest))
                            {
                                cachedBest = int.MaxValue;
                            }
                            if (cachedBest > next.Item3)
                            {
                                cache[nextKey] = next.Item3;
                                queue.Enqueue(next);
                            }
                        }
                    }
                }
                else
                {
                    currentBest = Math.Min(currentBest, distance);
                }
            }

            return currentBest;
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
            console.WriteLine("- Pt1 - "+Part1(input));
            //console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}