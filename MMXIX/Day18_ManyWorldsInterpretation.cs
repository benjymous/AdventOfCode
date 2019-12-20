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

        // turn key char into bit
        // a (or A) = 000001
        // b        = 000010
        // c        = 000100
        // etc
        public static int KeyCode(char c)
        {
            return (1 << char.ToLower(c)-'a');
        }

        public static int PlayerCode(int i)
        {
            return (1 << 26+i);
        }

        // set bit in given value for key added
        public static int AddKey(int v, char c)
        {
            return v | KeyCode(c);
        }

        // iterate over bits, returns sequence like 1,2,4,8 (only returning set bits in input)
        public static IEnumerable<int> Bits(int v)
        {
            for(int k=1; k<=v; k<<=1)
            {
                if ((v & k)>0) 
                    yield return k;
            }
        }

        public class RoomPath
        {
            public RoomPath(Dictionary<string, char> map, IEnumerable<ManhattanVector2> path)
            {
                List<char> seenDoors = new List<char>();

                // find all the doors this path passes through
                foreach (var pos in path)
                {
                    var c = map.GetObjKey(pos);              

                    if (c>='A' && c <='Z')
                    {
                        Doors = AddKey(Doors, c);
                    }                  
                }

                Count = path.Count();
            }

            public bool IsWalkable(int heldKeys) => Count > 0 && ((Doors & heldKeys) == Doors);

            public int Count {get;private set;}

            int Doors = 0;
        }

        public class MapData : AStar.GridMap<char>
        {
            List<ManhattanVector2> startPositions = new List<ManhattanVector2>();

            public int AllKeys {get; private set;} = 0;
            public int AllPlayers {get; private set;} = 0;

            Dictionary<int, ManhattanVector2> keyPositions = new Dictionary<int, ManhattanVector2>();
            Dictionary<int, ManhattanVector2> doors = new Dictionary<int, ManhattanVector2>();
            public Dictionary<int, RoomPath> paths {get; private set;} = new Dictionary<int, RoomPath>();

            class Walkable : AStar.IIsWalkable<char>
            {
                public bool IsWalkable(char cell)
                {
                    return cell != '#';
                }
            }

            public MapData(string input)
                :base(new Walkable())
            {
                var lines = Util.Split(input);                

                // find points of interest in the map
                for (var y=0; y<lines.Length; ++y)
                {
                    var line = lines[y];
                    for(var x=0; x<line.Length; ++x)
                    {
                        var c = line[x];
                        if (c=='@')
                        {
                            // player start position
                            startPositions.Add(new ManhattanVector2(x,y));
                            c= '.';
                        }
                        if (c>='a' && c <='z')
                        {
                            // key
                            keyPositions[KeyCode(c)] = new ManhattanVector2(x,y);
                            AllKeys|=KeyCode(c);
                        }
                        if (c>='A' && c <='Z')
                        {
                            // door
                            doors[KeyCode(c)] = new ManhattanVector2(x,y);
                        }
                        data.PutStrKey($"{x},{y}",c);
                    }
                }
            }

            public void AlterForPart2()
            {
                if (startPositions.Count == 1)
                {
                    var centrePoint = startPositions.First();
                    startPositions.Clear();

                    startPositions.Add(new ManhattanVector2(centrePoint.X-1, centrePoint.Y-1));
                    startPositions.Add(new ManhattanVector2(centrePoint.X+1, centrePoint.Y-1));
                    startPositions.Add(new ManhattanVector2(centrePoint.X-1, centrePoint.Y+1));
                    startPositions.Add(new ManhattanVector2(centrePoint.X+1, centrePoint.Y+1));

                    data[$"{centrePoint.X},{centrePoint.Y}"] = '#';

                    data[$"{centrePoint.X-1},{centrePoint.Y}"] = '#';
                    data[$"{centrePoint.X+1},{centrePoint.Y}"] = '#';
                    data[$"{centrePoint.X},{centrePoint.Y-1}"] = '#';
                    data[$"{centrePoint.X},{centrePoint.Y+1}"] = '#';
                }
            }

            public void DrawMap()
            {
                for (var y=0; y<7; ++y)
                {
                    for (var x=0; x<7; ++x)
                    {
                        var c = data.GetStrKey($"{x},{y}");

                        var pos = new ManhattanVector2(x,y);

                        if (startPositions.Contains(pos))
                        {
                            c = (char)('1'+startPositions.IndexOf(pos));
                        }

                        Console.Write(c);
                    }
                    Console.WriteLine();
                }
            }
        
            public void CalcPaths()
            {
                // precalculate all possible paths (ignoring doors)

                foreach (var player in startPositions)
                {
                    AllPlayers |= PlayerCode(startPositions.IndexOf(player));
                }
         
                var finder = new AStar.RoomPathFinder<ManhattanVector2>();
                foreach (var k1 in Bits(AllKeys))  
                {
                    // path from start to k1's location
                    foreach (var player in startPositions)
                    {
                        int playerId = PlayerCode(startPositions.IndexOf(player));
                        var path = finder.FindPath(this, player, keyPositions[k1]);
                        if (path.Any())
                        {
                            paths[playerId|k1] = new RoomPath(data, path);
                        }
                    }
                    foreach (var k2 in Bits(AllKeys))  
                    {
                        if (k2 > k1)
                        {
                            // path from k1 to k2
                            var path = finder.FindPath(this, keyPositions[k1], keyPositions[k2]);
                            if (path.Any())
                            {
                                paths[k1|k2] = new RoomPath(data,  path); // since we're using bitwise, we just store two bits for k1|k2 it doesn't matter which way around
                            }
                        }
                    }
                }
            }
        }
 
        public static int Solve(MapData map)
        {
            map.CalcPaths();

            var queue = new Queue<Tuple<int, int, int>>();

            queue.Enqueue(Tuple.Create(map.AllPlayers, 0, 0));
            var cache = new Dictionary<Int64,int>();

            int currentBest = int.MaxValue;

            while (queue.Any())
            {
                // take an item from the job queue
                var item = queue.Dequeue();

                var positions = item.Item1;
                var heldKeys = item.Item2;
                int distance = item.Item3;

                int tryKeys = map.AllKeys - heldKeys;

                if (tryKeys>0)
                {
                    var z = Bits(positions);
                    foreach (var position in Bits(positions))
                    {
                        // check keys not held
                        foreach (var key in Bits(tryKeys))
                        {
                            if (map.paths.TryGetValue(position|key, out var path))
                            {
                                if (path.IsWalkable(heldKeys))
                                {
                                    // path isn't blocked - state holds all necessary keys

                                    // create new state, at location of next key
                                    var next = Tuple.Create((positions-position)+(key), heldKeys|key, distance+path.Count);

                                    // check if we've visited this position with this set of keys before
                                    var cacheId  = (Int64)next.Item1 << 32 | (Int64)(uint)next.Item2;
                                    if (!cache.TryGetValue(cacheId, out int cachedBest))
                                    {
                                        cachedBest = int.MaxValue;
                                    }

                                    // we've not visited, or our new path is shorter
                                    if (cachedBest > next.Item3)
                                    {
                                        // cache the new shorter distance, and add the new state to our job queue
                                        cache[cacheId] = next.Item3;
                                        queue.Enqueue(next);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    // we have all the keys, so this is a possible solution
                    currentBest = Math.Min(currentBest, distance);
                }
            }

            return currentBest;
        }

        public static int Part1(string input)
        {
            var map = new MapData(input);
            return Solve(map);
        }



        public static int Part2(string input)
        {     
            var map = new MapData(input);
            map.AlterForPart2();
            return Solve(map);
        }



        public void Run(string input, System.IO.TextWriter console)
        {
            console.WriteLine("- Pt1 - "+Part1(input));
            console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}