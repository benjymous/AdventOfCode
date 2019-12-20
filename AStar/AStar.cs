using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.AStar
{
    public interface IMap
    {
        bool IsValidNeighbour(ManhattanVector2 location);
        IEnumerable<ManhattanVector2> GetNeighbours(ManhattanVector2 location);
    }

    public interface IIsWalkable<TCellDataType>
    {
        bool IsWalkable(TCellDataType cell);
    }

    public class GridMap<TCellDataType> : IMap
    {
        public Dictionary<string, TCellDataType> data = new Dictionary<string, TCellDataType>();

        public TCellDataType WallType;

        IIsWalkable<TCellDataType> Walkable;

        public GridMap(IIsWalkable<TCellDataType> walkable)
        {
            Walkable = walkable;
        }

        public IEnumerable<ManhattanVector2> GetNeighbours(ManhattanVector2 center)
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
    }

    public class RoomPathFinder
    {

        Dictionary<ManhattanVector2, bool> closedSet = new Dictionary<ManhattanVector2, bool>();
        Dictionary<ManhattanVector2, bool> openSet = new Dictionary<ManhattanVector2, bool>();

        //cost of start to this key node
        Dictionary<ManhattanVector2, int> gScore = new Dictionary<ManhattanVector2, int>();
        //cost of start to goal, passing through key node
        Dictionary<ManhattanVector2, int> fScore = new Dictionary<ManhattanVector2, int>();

        Dictionary<ManhattanVector2, ManhattanVector2> nodeLinks = new Dictionary<ManhattanVector2, ManhattanVector2>();

        private void Reset()
        {
            closedSet.Clear();
            openSet.Clear();
            gScore.Clear();
            fScore.Clear();
            nodeLinks.Clear();
        }

        public List<ManhattanVector2> FindPath(IMap graph, ManhattanVector2 start, ManhattanVector2 goal)
        {
            Reset();
            
            openSet[start] = true;
            gScore[start] = 0;
            fScore[start] = Heuristic(start, goal);

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
                    fScore[neighbor] = projectedG + Heuristic(neighbor, goal);

                }
            }


            return new List<ManhattanVector2>();
        }

        private int Heuristic(ManhattanVector2 start, ManhattanVector2 goal)
        {
            var dx = goal.X - start.X;
            var dy = goal.Y - start.Y;
            return Math.Abs(dx) + Math.Abs(dy);
        }

        private int getGScore(ManhattanVector2 pt)
        {
            int score = int.MaxValue;
            gScore.TryGetValue(pt, out score);
            return score;
        }


        private int getFScore(ManhattanVector2 pt)
        {
            int score = int.MaxValue;
            fScore.TryGetValue(pt, out score);
            return score;
        }

        private List<ManhattanVector2> Reconstruct(ManhattanVector2 current)
        {
            List<ManhattanVector2> path = new List<ManhattanVector2>();
            while (nodeLinks.ContainsKey(current))
            {
                path.Add(current);
                current = nodeLinks[current];
            }

            path.Reverse();
            return path;
        }

        private ManhattanVector2 nextBest()
        {
            int best = int.MaxValue;
            ManhattanVector2 bestPt = null;
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