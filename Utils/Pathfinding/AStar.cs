using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Utils.Pathfinding
{
    public interface IMap<TCoordinateType>
    {
        IEnumerable<TCoordinateType> GetNeighbours(TCoordinateType location);
        int Heuristic(TCoordinateType location, TCoordinateType goal) => 1;
        int GScore(TCoordinateType location) => 1;
    }

    public interface IIsWalkable<TCellDataType>
    {
        bool IsWalkable(TCellDataType cell);
    }

    public class GridMap<TCellDataType> : IMap<ManhattanVector2>
    {
        public Dictionary<string, TCellDataType> Data = new();

        public TCellDataType WallType;
        readonly IIsWalkable<TCellDataType> Walkable;

        public GridMap(IIsWalkable<TCellDataType> walkable)
        {
            Walkable = walkable ?? this as IIsWalkable<TCellDataType>;
        }

        public GridMap(IIsWalkable<TCellDataType> walkable, Dictionary<string, TCellDataType> data)
        {
            Walkable = walkable ?? this as IIsWalkable<TCellDataType>;
            Data = data;
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

        public bool IsValidNeighbour(ManhattanVector2 pt) => Data.TryGetValue(pt.ToString(), out var room) && Walkable.IsWalkable(room);

        public string FindCell(TCellDataType val) => Data.Where(kvp => EqualityComparer<TCellDataType>.Default.Equals(kvp.Value, val)).First().Key;

        public int Heuristic(ManhattanVector2 location1, ManhattanVector2 location2) => location1.Distance(location2);

        public int GScore(ManhattanVector2 location) => 1;
    }


    public static class AStar<TCoordinateType>
    {
        public static IEnumerable<TCoordinateType> FindPath(IMap<TCoordinateType> map, TCoordinateType start, TCoordinateType goal)
        {
            var state = new State();

            state.gScore[start] = 0;
            state.openSet.Add(start);
            state.taskQueue.Enqueue(start, map.Heuristic(start, goal));

            while (state.openSet.Count > 0)
            {
                var current = state.taskQueue.Dequeue();
                if (current.Equals(goal))
                {
                    return state.Reconstruct(current);
                }

                state.openSet.Remove(current);
                state.closedSet.Add(current);

                foreach (var neighbour in map.GetNeighbours(current))
                {
                    if (state.closedSet.Contains(neighbour))
                        continue;

                    var projectedG = state.GetGScore(current) + map.GScore(neighbour);

                    if (state.openSet.Contains(neighbour) && (projectedG >= state.GetGScore(neighbour)))
                        continue;

                    //record it
                    state.nodeLinks[neighbour] = current;
                    state.gScore[neighbour] = projectedG;

                    var newScore = projectedG + map.Heuristic(neighbour, goal);
                    state.openSet.Add(neighbour);
                    state.taskQueue.Enqueue(neighbour, newScore);
                }
            }

            return Enumerable.Empty<TCoordinateType>();
        }

        class State
        {
            public HashSet<TCoordinateType> closedSet = new();

            //cost of start to goal, passing through key node
            public HashSet<TCoordinateType> openSet = new();
            public PriorityQueue<TCoordinateType, int> taskQueue = new();

            //cost of start to this key node
            public Dictionary<TCoordinateType, int> gScore = new();

            public Dictionary<TCoordinateType, TCoordinateType> nodeLinks = new();

            internal int GetGScore(TCoordinateType pt) => gScore[pt];

            internal IEnumerable<TCoordinateType> Reconstruct(TCoordinateType current)
            {
                List<TCoordinateType> path = new();
                while (nodeLinks.ContainsKey(current))
                {
                    path.Add(current);
                    current = nodeLinks[current];
                }

                return (path as IEnumerable<TCoordinateType>).Reverse().ToArray();
            }
        }

    }
}