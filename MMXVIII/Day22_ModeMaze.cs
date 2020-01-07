using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVIII
{
    public class Day22 : IPuzzle
    {
        public string Name { get { return "2018-22";} }

        public const char ROCKY = '.';
        public const char WET = '=';
        public const char NARROW = '|';

        public const char BLOCKED = '#';

        public static string GetKey(int x, int y) => $"{x},{y}";
        public static string GetKey(ManhattanVector2 pos) => $"{pos.X},{pos.Y}";

        public class Cave
        {
            public Cave(int targetX, int targetY, int caveDepth)
            {
                target = new State() {position = new ManhattanVector2(targetX, targetY), tool = Tool.Torch };
                depth = caveDepth;
                distanceLimit = (int)((targetX + targetY)*1.25);
            }

            Dictionary<string,int> GeoCache = new Dictionary<string, int>();
            Dictionary<string,char> Map = new Dictionary<string, char>();

            public char MapAt(ManhattanVector2 pos)
            {
                var key = GetKey(pos);

                if (!Map.ContainsKey(key))
                {
                    Map[key] = TypeChar(pos);
                }   
                return Map[key];               
            }

            public int GeologicIndex(ManhattanVector2 pos)
            {
                var key = GetKey(pos);

                if (GeoCache.ContainsKey(key))
                {
                    return GeoCache[key];
                }

                int result = 0;

                if (pos.X<0 || pos.Y<0)
                {
                    throw new Exception("Invalid coordinate");
                }

                // The region at 0,0 (the mouth of the cave) has a geologic index of 0.
                if (pos == ManhattanVector2.Zero) result =  0;

                // The region at the coordinates of the target has a geologic index of 0.
                else if (pos == target.position) result =  0;

                // If the region's Y coordinate is 0, the geologic index is its X coordinate times 16807.
                else if (pos.Y==0) result = pos.X*16807; 

                // If the region's X coordinate is 0, the geologic index is its Y coordinate times 48271
                else if (pos.X==0) result = pos.Y*48271;

                else result = ErosionLevel(pos - new ManhattanVector2(1,0)) * ErosionLevel(pos - new ManhattanVector2(0,1));

                GeoCache[key] = result;
                return result;
            }

            // A region's erosion level is its geologic index plus the cave system's depth, all modulo 20183
            public int ErosionLevel(ManhattanVector2 pos)
            {
                return (GeologicIndex(pos) + depth) % 20183;
            }

            public char TypeChar(ManhattanVector2 pos)
            {
                if (pos.X<0 || pos.Y<0) return BLOCKED;
                var erosionLevel = ErosionLevel(pos);
                switch (erosionLevel % 3)
                {
                    case 0:
                        return ROCKY;

                    case 1:
                        return WET;

                    case 2:
                        return NARROW;
                }

                return BLOCKED;
            }

            public static int RiskLevel(char typeChar)
            {
                switch(typeChar)
                {
                    case ROCKY:
                        return 0;

                    case WET:
                        return 1;

                    case NARROW:
                        return 2;
                }

                throw new Exception("Unknown area type");
            }

            public int GetScore()
            {
                int score = 0;
                for(int y=0; y<=target.position.Y; ++y)
                {
                    for (int x=0; x<=target.position.X; ++x)
                    {
                        var c = MapAt( new ManhattanVector2(x,y) );
                        score += RiskLevel(c);
                    }
                }
                return score;
            }

            public State target;
            int depth;

            public int distanceLimit;
        }
        
        public enum Tool
        {
            None,
            Torch,
            ClimbingGear, 
        }
        
        public class State : IVec
        {
            public ManhattanVector2 position = new ManhattanVector2(0,0);

            public Tool tool = Tool.Torch;

            public int cost = 0;

            public Int64 GetKey() => (position.X << 40)+(position.Y << 20)+(int)tool;

            public State(ManhattanVector2 pos, Tool t)
            {
                position = pos;
                tool = t;
            }

            public State() {}

            bool ToolValid(char cell, Tool tool)
            {
                switch (cell)
                {
                    case ROCKY:
                        return tool != Tool.None;

                    case WET:
                        return tool != Tool.Torch;

                    case NARROW:
                        return tool != Tool.ClimbingGear;

                    case BLOCKED:
                    default:
                        return false;
                }
            }

            bool CanMove(Cave cave, int dx, int dy)
            {
                if ((position.X+dx > cave.target.position.X+20) || (position.Y+dy > cave.target.position.Y+20)) return false; // prevent the search space going crazy

                var newPos = position + new ManhattanVector2(dx, dy);

                // if (newPos.Distance(ManhattanVector2.Zero) > cave.distanceLimit && 
                //     newPos.Distance(cave.target) > cave.distanceLimit)
                //     {
                //         return false;
                //     }

                var t = cave.MapAt(newPos);
                return ToolValid(t, tool);
            }

            bool CanSwitch(Cave cave, Tool newTool)
            {
                var t = cave.MapAt(position);
                return ToolValid(t, newTool);
            }

            bool TrySetTool(Cave cave, Tool newTool, out State newState)
            {
                if (newTool != tool && CanSwitch(cave, tool))
                {
                    newState = new State(position, newTool);
                    newState.cost = 7;

                    return true;
                }

                newState = null;
                return false;
            }

            bool TryMove(Cave cave, int dx, int dy, out State newState)
            {
                if (CanMove(cave, dx, dy))
                {
                    newState = new State(new ManhattanVector2(position.X+dx, position.Y+dy), tool);
                    newState.cost = 1;

                    return true;
                }

                newState = null;
                return false;
            }

            public IEnumerable<State> GetNeighbours(Cave cave)
            {
                State newstate;

                if (TryMove(cave, 1, 0, out newstate)) yield return newstate;
                if (TryMove(cave, 0, 1, out newstate)) yield return newstate;
                if (TryMove(cave, -1, 0, out newstate)) yield return newstate;
                if (TryMove(cave, 0, -1, out newstate)) yield return newstate;

                if (TrySetTool(cave, Tool.None, out newstate)) yield return newstate;
                if (TrySetTool(cave, Tool.Torch, out newstate)) yield return newstate;
                if (TrySetTool(cave, Tool.ClimbingGear, out newstate)) yield return newstate;
            }

            public int Distance(IVec other)
            {
                if (!(other is State)) return Int32.MaxValue;

                var man2 = other as State;
                return position.Distance(man2.position) + Math.Abs((int)tool-(int)man2.tool);
            }

            public static bool operator== (State v1, State v2)
            {
                if (object.ReferenceEquals(v1, null) && object.ReferenceEquals(v2, null)) return true;
                if (object.ReferenceEquals(v1, null) && !object.ReferenceEquals(v2, null)) return false;
                return v1.Equals(v2);
            }

            public static bool operator!= (State v1, State v2)
            {
                if (object.ReferenceEquals(v1, null) && object.ReferenceEquals(v2, null)) return true;
                if (object.ReferenceEquals(v1, null) && !object.ReferenceEquals(v2, null)) return false;
                return !v1.Equals(v2);
            }

            public override bool Equals(object other)
            {
                if (!(other is State)) return false;
                return Distance(other as State) == 0;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 31 + position.GetHashCode();
                    hash = hash * 31 + tool.GetHashCode();
                    return hash;
                }
            }

            public override string ToString()
            {
                return $"{position} {tool}";
            }
        }
        
        public void DrawMap(char[][] map)
        {
            foreach (var line in map)
            {
                Console.WriteLine(string.Join("", line));
            }
        }



        public static int Part1(int tx, int ty, int depth)
        {
            var cave = new Cave(tx, ty, depth);
            return cave.GetScore();
        }

        public static int Part1(string input)
        {
            var bits = input.Replace("\n", ",").Replace(" ",",").Split(',');
            return Part1(int.Parse(bits[3]), int.Parse(bits[4]), int.Parse(bits[1]));
        }

        public static int Part2(int tx, int ty, int depth)
        {
            var cave = new Cave(tx, ty, depth);

            var startPos = new State(new ManhattanVector2(0,0), Tool.Torch);

            var jobqueue = new Queue<Tuple<State,int>>();
            jobqueue.Enqueue(Tuple.Create(startPos,0));
            int best = int.MaxValue;
            var cache = new Dictionary<Int64, int>();

            cache[startPos.GetKey()] = 0;

            while (jobqueue.Any())
            {
                var entry = jobqueue.Dequeue();

                if (entry.Item1 == cave.target)
                {
                    if (entry.Item2 < best)
                    {
                        best = entry.Item2;
                    }
                }
                else
                {
                    var neighbours = entry.Item1.GetNeighbours(cave);
                    foreach (var neighbour in neighbours)
                    {
                        int newDistance = entry.Item2+neighbour.cost;

                        if (newDistance > best) continue;

                        var key = neighbour.GetKey();              
                        if (cache.TryGetValue(key, out var dist))
                        {
                            if (dist <= newDistance)
                            {
                                continue;
                            }
                        }
                        cache[key] = newDistance;
                        jobqueue.Enqueue(Tuple.Create(neighbour, newDistance));
                    }
                }
            }

            return best;
        }

        public static int Part2(string input)
        {
            var bits = input.Replace("\n", ",").Replace(" ",",").Split(',');
            return Part2(int.Parse(bits[3]), int.Parse(bits[4]), int.Parse(bits[1]));
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));

            //Console.WriteLine(Part2(10,10,510));
        }
    }
}
