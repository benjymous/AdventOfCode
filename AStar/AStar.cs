using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.AStar
{
    public interface IRoom
    {
        int Data();
    }

    public interface ICanWalk
    {
        bool IsWalkable(IRoom room);
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

        ICanWalk canWalk = null;

        private void Reset()
        {
            closedSet.Clear();
            openSet.Clear();
            gScore.Clear();
            fScore.Clear();
            nodeLinks.Clear();
            canWalk = null;
        }

        public List<ManhattanVector2> FindPath(Dictionary<string,IRoom> graph, ManhattanVector2 start, ManhattanVector2 goal, ICanWalk callback)
        {
            Reset();

            canWalk = callback;
            
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

                foreach (var neighbor in Neighbors(graph, current))
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

        IEnumerable<ManhattanVector2> Neighbors(Dictionary<string,IRoom> graph, ManhattanVector2 center)
        {

            // ManhattanVector2 pt = new ManhattanVector2(center.X - 1, center.Y - 1);
            // if (IsValidNeighbor(graph, pt))
            //     yield return pt;

            var pt = new ManhattanVector2(center.X, center.Y - 1);
            if (IsValidNeighbor(graph, pt))
                yield return pt;

            // pt = new ManhattanVector2(center.X + 1, center.Y - 1);
            // if (IsValidNeighbor(graph, pt))
            //     yield return pt;

            //middle row
            pt = new ManhattanVector2(center.X - 1, center.Y);
            if (IsValidNeighbor(graph, pt))
                yield return pt;

            pt = new ManhattanVector2(center.X + 1, center.Y);
            if (IsValidNeighbor(graph, pt))
                yield return pt;


            // //bottom row
            // pt = new ManhattanVector2(center.X - 1, center.Y + 1);
            // if (IsValidNeighbor(graph, pt))
            //     yield return pt;

            pt = new ManhattanVector2(center.X, center.Y + 1);
            if (IsValidNeighbor(graph, pt))
                yield return pt;

            // pt = new ManhattanVector2(center.X + 1, center.Y + 1);
            // if (IsValidNeighbor(graph, pt))
            //     yield return pt;
        }

        bool IsValidNeighbor(Dictionary<string,IRoom> matrix, ManhattanVector2 pt)
        {
            var key = pt.ToString();
            IRoom room;
            if (matrix.TryGetValue(key, out room))
            {
                return canWalk.IsWalkable(room);
            }

            return false;
        
            // int x = pt.X;
            // int y = pt.Y;
            // if (x < 0 || x >= matrix.Length)
            //     return false;

            // if (y < 0 || y >= matrix[x].Length)
            //     return false;

            // return matrix[x][y];

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