using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXX
{
    public class Day20 : IPuzzle
    {
        public string Name { get { return "2020-20";} }
 
        public class Tile
        {
            public Tile(string input)
            {
                var lines = input.Split("\n");
                Title = lines[0];
                ID = Int64.Parse(Title.Substring(5,4));
                Grid = lines.Skip(1).ToArray();
                //Console.WriteLine(ID);

                edges.Add(Grid[0]);
                edges.Add(Grid[9]);

                string left = "";
                string right ="";

                foreach(var line in Grid)
                {
                    left += line[0];
                    right += line[9];
                }

                edges.Add(left);
                edges.Add(right);
            }

            public Int64 ID;

            public int Borders = 0;

            public string Title;
            public string[] Grid;

            public List<string> edges = new List<string>();
        }

        public class JigsawSolver
        {
            public JigsawSolver(string input)
            {
                Tiles = Util.Parse<Tile>(input.Split("\n\n"));

                map = new Dictionary<string, List<Tile>>();

                foreach(var tile in Tiles)
                {
                    foreach(var edge in tile.edges)
                    {
                        AddEdge(edge, tile);
                        AddEdge(new string(edge.Reverse().ToArray()), tile);
                    }
                    //Console.WriteLine("---");
                }

               

                foreach(var tile in Tiles)
                {
                    foreach(var e in tile.edges)
                    {
                        if (map[e].Count == 1)
                        {
                            tile.Borders++;
                        }
                    }
                }
            }
            public List<Tile> Tiles;
            Dictionary<string, List<Tile>> map;
            void AddEdge(string edge, Tile tile)
            {
                //Console.WriteLine(edge);
                if (!map.ContainsKey(edge))
                {
                    map[edge] = new List<Tile>();
                }

                map[edge].Add(tile);
            }
        }

 
        public static Int64 Part1(string input)
        {
            var solver = new JigsawSolver(input);

            Int64 result=1;
            
            foreach(var tile in solver.Tiles)
            {
                if (tile.Borders == 2) result *= tile.ID;
            }
            return result;
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}