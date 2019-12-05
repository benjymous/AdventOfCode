using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.AStar
{
    public class Node<VecType> where VecType : class, IVec
    {
        public Node<VecType> Parent;
        public VecType Position;

        public float DistanceToTarget;
        public float Cost;
        public float F
        {
            get
            {
                if (DistanceToTarget != -1 && Cost != -1)
                    return DistanceToTarget + Cost;
                else
                    return -1;
            }
        }
        public bool Walkable;

        public Node(VecType pos, bool walkable)
        {
            Parent = null;
            Position = pos;
            DistanceToTarget = -1;
            Cost = 1;
            Walkable = walkable;
        }
    }

    public abstract class Astar<VecType> where VecType : class, IVec
    {
        List<List<Node<VecType>>> Grid;
        int GridRows
        {
            get
            {
               return Grid[0].Count;
            }
        }
        int GridCols
        {
            get
            {
                return Grid.Count;
            }
        }

        public Astar(List<List<Node<VecType>>> grid)
        {
            Grid = grid;
        }

        public Stack<Node<VecType>> FindPath(VecType Start, VecType End)
        {
            Node<VecType> start = new Node<VecType>(Start, true);
            Node<VecType> end = new Node<VecType>(End, true);

            Stack<Node<VecType>> Path = new Stack<Node<VecType>>();
            List<Node<VecType>> OpenList = new List<Node<VecType>>();
            List<Node<VecType>> ClosedList = new List<Node<VecType>>();
            List<Node<VecType>> adjacencies;
            Node<VecType> current = start;
           
            // add start node to Open List
            OpenList.Add(start);

            while(OpenList.Count != 0 && !ClosedList.Exists(x => x.Position == end.Position))
            {
                current = OpenList[0];
                OpenList.Remove(current);
                ClosedList.Add(current);
                adjacencies = GetAdjacentNodes(current);

 
                foreach(Node<VecType> n in adjacencies)
                {
                    if (!ClosedList.Contains(n) && n.Walkable)
                    {
                        if (!OpenList.Contains(n))
                        {
                            n.Parent = current;
                            n.DistanceToTarget = n.Position.Distance(end.Position);
                            n.Cost = 1 + n.Parent.Cost;
                            OpenList.Add(n);
                            OpenList = OpenList.OrderBy(node => node.F).ToList<Node<VecType>>();
                        }
                    }
                }
            }
            
            // construct path, if end was not closed return null
            if(!ClosedList.Exists(x => x.Position == end.Position))
            {
                return null;
            }

            // if all good, return path
            Node<VecType> temp = ClosedList[ClosedList.IndexOf(current)];
            while(temp.Parent != start && temp != null)
            {
                Path.Push(temp);
                temp = temp.Parent;
            }
            return Path;
        }
		
        public abstract List<Node<VecType>> GetAdjacentNodes(Node<VecType> n);

        // {
        //     List<Node<Vector2>> temp = new List<Node<Vector2>>();

        //     int row = (int)n.Position.Y;
        //     int col = (int)n.Position.X;

        //     if(row + 1 < GridRows)
        //     {
        //         temp.Add(Grid[col][row + 1]);
        //     }
        //     if(row - 1 >= 0)
        //     {
        //         temp.Add(Grid[col][row - 1]);
        //     }
        //     if(col - 1 >= 0)
        //     {
        //         temp.Add(Grid[col - 1][row]);
        //     }
        //     if(col + 1 < GridCols)
        //     {
        //         temp.Add(Grid[col + 1][row]);
        //     }

        //     return temp;
        // }
    }
}