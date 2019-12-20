using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.AStar
{
    public interface IMap<TCoordinateType>
    {
        bool IsValidNeighbour(TCoordinateType location);
        IEnumerable<TCoordinateType> GetNeighbours(TCoordinateType location);
        int Heuristic(TCoordinateType location1, TCoordinateType location2);
    }

    public interface IIsWalkable<TCellDataType>
    {
        bool IsWalkable(TCellDataType cell);
    }

    public class GridMap<TCellDataType> : IMap<ManhattanVector2>
    {
        public Dictionary<string, TCellDataType> data = new Dictionary<string, TCellDataType>();

        public TCellDataType WallType;

        IIsWalkable<TCellDataType> Walkable;

        public GridMap(IIsWalkable<TCellDataType> walkable)
        {
            if (walkable != null)
            {
                Walkable = walkable;
            }
            else
            {        
                Walkable = this as IIsWalkable<TCellDataType>;
            }
        }

        public virtual IEnumerable<ManhattanVector2> GetNeighbours(ManhattanVector2 center)
        {
            ManhattanVector2 pt;
            pt = new ManhattanVector2(center.X - 1, center.Y);
            if (IsValidNeighbour(pt))
                yield return pt;

            pt = new ManhattanVector2(center.X + 1, center.Y);
            if (IsValidNeighbour(pt))
                yield return pt;

            pt = new ManhattanVector2(center.X, center.Y + 1);
            if (IsValidNeighbour(pt))
                yield return pt;

            pt = new ManhattanVector2(center.X, center.Y - 1);
            if (IsValidNeighbour(pt))
                yield return pt;

        }

        public bool IsValidNeighbour(ManhattanVector2 pt)
        {
            if (data.TryGetValue(pt.ToString(), out var room))
            {
                return Walkable.IsWalkable(room);
            }

            return false;
        }

        public string FindCell(TCellDataType val)
        {       
            return data.Where(kvp =>  EqualityComparer<TCellDataType>.Default.Equals(kvp.Value, val) ).First().Key;
        }

        public int Heuristic(ManhattanVector2 location1, ManhattanVector2 location2)
        {
            return location1.Distance(location2);
        }
    }

    public class RoomPathFinder<TCoordinateType> where TCoordinateType : class
    {

        Dictionary<TCoordinateType, bool> closedSet = new Dictionary<TCoordinateType, bool>();
        Dictionary<TCoordinateType, bool> openSet = new Dictionary<TCoordinateType, bool>();

        //cost of start to this key node
        Dictionary<TCoordinateType, int> gScore = new Dictionary<TCoordinateType, int>();
        //cost of start to goal, passing through key node
        Dictionary<TCoordinateType, int> fScore = new Dictionary<TCoordinateType, int>();

        Dictionary<TCoordinateType, TCoordinateType> nodeLinks = new Dictionary<TCoordinateType, TCoordinateType>();

        private void Reset()
        {
            closedSet.Clear();
            openSet.Clear();
            gScore.Clear();
            fScore.Clear();
            nodeLinks.Clear();
        }

        public List<TCoordinateType> FindPath(IMap<TCoordinateType> graph, TCoordinateType start, TCoordinateType goal)
        {
            Reset();
            
            openSet[start] = true;
            gScore[start] = 0;
            fScore[start] = graph.Heuristic(start, goal);

            while (openSet.Count > 0)
            {
                var current = nextBest();
                if (current.Equals(goal))
                {
                    return Reconstruct(current);
                }


                openSet.Remove(current);
                closedSet[current] = true;

                foreach (var neighbor in graph.GetNeighbours(current))
                {
                    if (closedSet.ContainsKey(neighbor))
                        continue;

                    var projectedG = getGScore(current) + 1;

                    if (!openSet.ContainsKey(neighbor))
                        openSet[neighbor] = true;
                    else if (projectedG >= getGScore(neighbor))
                        continue;

                    //record it
                    nodeLinks[neighbor] = current;
                    gScore[neighbor] = projectedG;
                    fScore[neighbor] = projectedG + graph.Heuristic(neighbor, goal);

                }
            }


            return new List<TCoordinateType>();
        }

        private int getGScore(TCoordinateType pt)
        {
            int score = int.MaxValue;
            gScore.TryGetValue(pt, out score);
            return score;
        }


        private int getFScore(TCoordinateType pt)
        {
            int score = int.MaxValue;
            fScore.TryGetValue(pt, out score);
            return score;
        }

        private List<TCoordinateType> Reconstruct(TCoordinateType current)
        {
            List<TCoordinateType> path = new List<TCoordinateType>();
            while (nodeLinks.ContainsKey(current))
            {
                path.Add(current);
                current = nodeLinks[current];
            }

            path.Reverse();
            return path;
        }

        private TCoordinateType nextBest()
        {
            int best = int.MaxValue;
            TCoordinateType bestPt = null;
            foreach (var node in openSet.Keys)
            {
                var score = getFScore(node);
                if (score < best)
                {
                    bestPt = node;
                    best = score;
                }
            }


            return bestPt;
        }

    }
}