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
                target = new ManhattanVector2(targetX, targetY);
                depth = caveDepth;
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
                else if (pos == target) result =  0;

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
                for(int y=0; y<=target.Y; ++y)
                {
                    for (int x=0; x<=target.X; ++x)
                    {
                        var c = MapAt( new ManhattanVector2(x,y) );
                        score += RiskLevel(c);
                    }
                }
                return score;
            }

            public ManhattanVector2 target;
            int depth;
        }
        
        public enum Tool
        {
            None,
            Torch,
            ClimbingGear, 
        }

        public class CaveStar : Astar.Astar<State>
        {
            public CaveStar(Cave cave)
            {
                this.cave = cave;
            }
            Cave cave;

            public override List<Astar.Node<State>> GetAdjacentNodes(Astar.Node<State> n)
            {
                return new List<Astar.Node<State>>();
            }
        }
        
        public class State : IVec
        {
            public ManhattanVector2 position;
            public int steps;

            public Tool tool = Tool.Torch;

            public bool Win(Cave cave)
            {
                return DistanceToTarget(cave)==0 && tool==Tool.Torch;
            }

            public int Sort(Cave cave)
            {
                return DistanceToTarget(cave)+steps;
            }

            public int DistanceToTarget(Cave cave)
            {
                return position.Distance(cave.target);
            }

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
                var newPos = position + new ManhattanVector2(dx, dy);
                var t = cave.MapAt(newPos);
                return ToolValid(t, tool);
            }

            bool CanSwitch(Cave cave, Tool newTool)
            {
                var t = cave.MapAt(position);
                return ToolValid(t, newTool);
            }

            State SetTool(Cave cave, Tool newTool)
            {
                if (newTool != tool && CanSwitch(cave, tool))
                {
                    var newState = this.MemberwiseClone() as State;
                    newState.tool = newTool;
                    newState.steps += 7;
                    return newState;
                }

                return null;
            }

            State Move(Cave cave, int dx, int dy)
            {
                if (CanMove(cave, dx, dy))
                {
                    var newState = this.MemberwiseClone() as State;
                    newState.position.Offset(dx, dy);
                    newState.steps++;
                    return newState;
                }

                return null;
            }

            public List<State> TryMoves(Cave cave)
            {
                List<State> newStates = new List<State>();

                newStates.Add(SetTool(cave, Tool.None));
                newStates.Add(SetTool(cave, Tool.Torch));
                newStates.Add(SetTool(cave, Tool.ClimbingGear));

                newStates.Add(Move(cave, 1, 0));
                newStates.Add(Move(cave, 0, 1));
                newStates.Add(Move(cave, -1, 0));
                newStates.Add(Move(cave, 0, -1));

                return newStates.Where(x => x!=null).ToList();
            }

            public int Distance(IVec other)
            {
                if (!(other is State)) return Int32.MaxValue;

                var man2 = other as State;
                return position.Distance(man2.position) + Math.Abs((int)tool-(int)man2.tool);
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

            var best = new Dictionary<string, int>();
            var tryStates = new List<State>() {new State()};

            
            int winCount = 500;
            while (tryStates.Count > 0)
            {
                var state = tryStates[0];
                tryStates.Remove(state);
                var key = GetKey(state.position.X, state.position.Y);

                if (!best.ContainsKey(key) || best[key] > state.steps)
                {
                    best[key] = state.steps;
                }

                if (state.Win(cave))
                {
                    winCount--;
                }

                if (winCount == 0)
                {
                    break;
                }

                tryStates.AddRange(state.TryMoves(cave));
                tryStates.Sort((a,b) => a.DistanceToTarget(cave).CompareTo(b.DistanceToTarget(cave)));
                
            }


            return best[GetKey(tx,ty)];
        }

        public static int Part2(string input)
        {
            var bits = input.Replace("\n", ",").Replace(" ",",").Split(',');
            return Part2(int.Parse(bits[3]), int.Parse(bits[4]), int.Parse(bits[1]));
        }

        public void Run(string input)
        {
            Console.WriteLine(Part2(10,10,510));

            Console.WriteLine("- Pt1 - "+Part1(input));
            //Console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}
