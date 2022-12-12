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

    public class GridMap<TCellDataType> : IMap<(int x, int y)>
    {
        public Dictionary<(int x, int y), TCellDataType> Data = new();

        public TCellDataType WallType;
        readonly IIsWalkable<TCellDataType> Walkable;

        public GridMap(IIsWalkable<TCellDataType> walkable)
        {
            Walkable = walkable ?? this as IIsWalkable<TCellDataType>;
        }

        public GridMap(IIsWalkable<TCellDataType> walkable, Dictionary<(int x, int y), TCellDataType> data)
        {
            Walkable = walkable ?? this as IIsWalkable<TCellDataType>;
            Data = data;
        }

        public virtual IEnumerable<(int x, int y)> GetNeighbours((int x, int y) center)
        {
            (int x, int y) pt;
            pt = (center.x - 1, center.y);
            if (IsValidNeighbour(pt))
                yield return pt;

            pt = (center.x + 1, center.y);
            if (IsValidNeighbour(pt))
                yield return pt;

            pt = (center.x, center.y + 1);
            if (IsValidNeighbour(pt))
                yield return pt;

            pt = (center.x, center.y - 1);
            if (IsValidNeighbour(pt))
                yield return pt;
        }

        public bool IsValidNeighbour((int x, int y) pt) => Data.TryGetValue(pt, out var room) && Walkable.IsWalkable(room);

        public (int x, int y) FindCell(TCellDataType val) => Data.Where(kvp => EqualityComparer<TCellDataType>.Default.Equals(kvp.Value, val)).First().Key;

        public int Heuristic((int x, int y) location1, (int x, int y) location2) => Math.Abs(location1.x - location2.x) + Math.Abs(location1.y - location2.y);

        public int GScore((int x, int y) location) => 1;
    }


    public static class AStar<TCoordinateType>
    {
        public static TCoordinateType[] FindPath(IMap<TCoordinateType> map, TCoordinateType start, TCoordinateType goal)
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

            return Array.Empty<TCoordinateType>();
        }

        public class State
        {
            public HashSet<TCoordinateType> closedSet = new();

            //cost of start to goal, passing through key node
            public HashSet<TCoordinateType> openSet = new();
            public PriorityQueue<TCoordinateType, int> taskQueue = new();

            //cost of start to this key node
            public Dictionary<TCoordinateType, int> gScore = new();

            public Dictionary<TCoordinateType, TCoordinateType> nodeLinks = new();

            internal int GetGScore(TCoordinateType pt) => gScore[pt];

            internal TCoordinateType[] Reconstruct(TCoordinateType current)
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