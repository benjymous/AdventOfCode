﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVI
{
    public class Day13 : IPuzzle
    {
        public string Name { get { return "2016-13";} }
 
        static bool IsWall(int x, int y, int seed)
        {
            int v = (seed)+(x*x) + (3*x) + (2*x*y) + (y) + (y*y);
            var b = v.BitSequence();
            var c = b.Count();
            if (c%2 == 0) return false;
            return true;
        }

        public class CubicleMap : AStar.IMap<ManhattanVector2>
        {
            public Dictionary<string, bool> data = new Dictionary<string, bool>();

            int Seed = 0;

            public CubicleMap(int seed)
            {
                Seed = seed;
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
                if (pt.X < 0 || pt.Y < 0)
                {
                    return false;
                }

                if (!data.TryGetValue(pt.ToString(), out var isWall))
                {
                    isWall = IsWall(pt.X, pt.Y, Seed);    
                    data[pt.ToString()] = isWall;
                }

    
                return isWall == false;
            }

            public int Heuristic(ManhattanVector2 location1, ManhattanVector2 location2)
            {
                return location1.Distance(location2);
            }
        }

        public static int Part1(string input)
        {
            int seed = int.Parse(input);

            var map = new CubicleMap(seed);

            var finder = new AStar.RoomPathFinder<ManhattanVector2>();
            var route = finder.FindPath(map, new ManhattanVector2(1,1), new ManhattanVector2(31,39));
            return route.Count();
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, ILogger logger)
        {

            for (var y=0; y<10; ++y)
            {
                for (var x=0; x<10; ++x)
                {
                    Console.Write(IsWall(x,y,10)? "#" : " ");
                }
                Console.WriteLine();
            }

            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}