using Advent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXX
{
    public class Day20 : IPuzzle
    {
        public string Name { get { return "2020-20";} }
 
        enum Edge
        {
            top = 0,
            right,
            bottom,
            left
        }

        public class Tile
        {
            public Tile(string input)
            {
                var lines = input.Split("\n");
                Title = lines[0];
                ID = Int64.Parse(Title.Substring(5, 4));
                Grid = lines.Skip(1).ToArray();
                //Console.WriteLine(ID);

                string left = "";
                string right = "";

                foreach (var line in Grid)
                {
                    left += line.First();
                    right += line.Last();
                }

                edges = new string[]
                {
                    Grid.First(),
                    right,
                    Grid.Last().Reversed(),
                    left.Reversed(),
                };
            }

            public Int64 ID;

            public int Orientation = 0;

            public int Borders = 0;

            public string Title;
            public string[] Grid;

            public string[] edges;

            /*
            
            0 0 abc 1       1 3 jkl 0
              l     d         i     a
              k     e         h     b
              j     f         g     c
              3 ihg 2         2 fed 1
            */
              

            public void Twizzle()
            {
                Orientation = ((Orientation + 1) % 8);


                var top = edges[(int)Edge.top];
                var right = edges[(int)Edge.right];
                var bottom = edges[(int)Edge.bottom];
                var left = edges[(int)Edge.left];

                edges[(int)Edge.top] = left;
                edges[(int)Edge.right] = top;
                edges[(int)Edge.bottom] = right;
                edges[(int)Edge.left] = bottom;

                if (Orientation == 0 || Orientation == 4)
                {
                    for (var i = 0; i < 4; ++i)
                    {
                        edges[i] = edges[i].Reversed();
                    }
                    edges = edges.Reverse().ToArray();
                }

            }

        }

        public class JigsawSolver
        {
            public JigsawSolver(string input)
            {
                Tiles = Util.Parse<Tile>(input.Split("\n\n"));

                Map = new Dictionary<string, List<Tile>>();

                foreach(var tile in Tiles)
                {
                    foreach(var edge in tile.edges)
                    {
                        AddEdge(edge, tile);
                        AddEdge(edge.Reversed(), tile);
                    }
                    //Console.WriteLine("---");
                }

               

                foreach(var tile in Tiles)
                {
                    foreach(var e in tile.edges)
                    {
                        if (Map[e].Count == 1)
                        {
                            tile.Borders++;
                        }
                    }
                }
            }
            public List<Tile> Tiles;
            public Dictionary<string, List<Tile>> Map;
            void AddEdge(string edge, Tile tile)
            {
                //Console.WriteLine(edge);
                if (!Map.ContainsKey(edge))
                {
                    Map[edge] = new List<Tile>();
                }

                Map[edge].Add(tile);
            }

            public Tile FindNeigbour(Tile nextTo, int position)
            {
                string edge = nextTo.edges[position];
                string revEdge = edge.Reversed();
                //Console.WriteLine($"{nextTo.ID} : {position} : {edge}");

                var options = Map[edge];

                if (options.Count == 1)
                {
                    return null;
                }

                var n = options.Where(x => x != nextTo).First();

                int pos2 = (position + 2) % 4;

                while (n.edges[pos2] != revEdge)
                {
                    n.Twizzle();
                }

                //Console.WriteLine($"{n.ID} : {pos2} : {n.edges[pos2]}");

                return n;
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

        public static Int64 Part2(string input)
        {
            var solver = new JigsawSolver(input);

            var corners = solver.Tiles.Where(t => t.Borders == 2);

            Tile corner = corners.First();

            // orient so its in position to be top left corner
            while (!(solver.Map[corner.edges[(int)Edge.top]].Count==1 && solver.Map[corner.edges[(int)Edge.left]].Count == 1))
            {
                corner.Twizzle();
            }

            var solution = new Dictionary<(int x, int y), Tile>();

            (int x, int y) pos = (0, 0);

            Console.WriteLine($"Position{pos} = {corner.ID}:{corner.Orientation}");
            solution[pos] = corner;

            var tile = corner;
            while (tile != null)
            {
                tile = solver.FindNeigbour(tile, 1); // find right hand neighbour to last 
                if (tile != null)
                {
                    pos = (pos.x + 1, pos.y);
                    solution[pos] = tile;
                    Console.WriteLine($"Position{pos} = {tile.ID}:{tile.Orientation}");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("-end of row-");
                    Console.WriteLine();
                    tile = solver.FindNeigbour(solution[(0, pos.y)], 2); // find lower neighbour
                    if (tile == null)
                    {
                        break;
                    }
                    pos = (0, pos.y + 1);
                    solution[pos] = tile;
                    Console.WriteLine($"Position{pos} = {tile.ID}:{tile.Orientation}");

                }
            }

            return corner.ID;
        }

        public void Run(string input, ILogger logger)
        {

            //var tile = new Tile("Tile 0000:\n"+
            //    "0abc1\n" +
            //    "l   d\n" +
            //    "k   e\n" +
            //    "j   f\n" +
            //    "3ihg2");

            //for (int i=0; i<9; ++i)
            //{
            //    Console.WriteLine(tile.Orientation);
            //    foreach (var edge in tile.edges)
            //    {
            //        Console.WriteLine(edge);
            //    }
            //    Console.WriteLine();
            //    tile.Twizzle();
            //}

//            var test = "Tile 2311:\n" +
//"..##.#..#.\n" +
//"##..#.....\n" +
//"#...##..#.\n" +
//"####.#...#\n" +
//"##.##.###.\n" +
//"##...#.###\n" +
//".#.#.#..##\n" +
//"..#....#..\n" +
//"###...#.#.\n" +
//"..###..###\n" +
//"\n" +
//"Tile 1951:\n" +
//"#.##...##.\n" +
//"#.####...#\n" +
//".....#..##\n" +
//"#...######\n" +
//".##.#....#\n" +
//".###.#####\n" +
//"###.##.##.\n" +
//".###....#.\n" +
//"..#.#..#.#\n" +
//"#...##.#..\n" +
//"\n" +
//"Tile 1171:\n" +
//"####...##.\n" +
//"#..##.#..#\n" +
//"##.#..#.#.\n" +
//".###.####.\n" +
//"..###.####\n" +
//".##....##.\n" +
//".#...####.\n" +
//"#.##.####.\n" +
//"####..#...\n" +
//".....##...\n" +
//"\n" +
//"Tile 1427:\n" +
//"###.##.#..\n" +
//".#..#.##..\n" +
//".#.##.#..#\n" +
//"#.#.#.##.#\n" +
//"....#...##\n" +
//"...##..##.\n" +
//"...#.#####\n" +
//".#.####.#.\n" +
//"..#..###.#\n" +
//"..##.#..#.\n" +
//"\n" +
//"Tile 1489:\n" +
//"##.#.#....\n" +
//"..##...#..\n" +
//".##..##...\n" +
//"..#...#...\n" +
//"#####...#.\n" +
//"#..#.#.#.#\n" +
//"...#.#.#..\n" +
//"##.#...##.\n" +
//"..##.##.##\n" +
//"###.##.#..\n" +
//"\n" +
//"Tile 2473:\n" +
//"#....####.\n" +
//"#..#.##...\n" +
//"#.##..#...\n" +
//"######.#.#\n" +
//".#...#.#.#\n" +
//".#########\n" +
//".###.#..#.\n" +
//"########.#\n" +
//"##...##.#.\n" +
//"..###.#.#.\n" +
//"\n" +
//"Tile 2971:\n" +
//"..#.#....#\n" +
//"#...###...\n" +
//"#.#.###...\n" +
//"##.##..#..\n" +
//".#####..##\n" +
//".#..####.#\n" +
//"#..#.#..#.\n" +
//"..####.###\n" +
//"..#.#.###.\n" +
//"...#.#.#.#\n" +
//"\n" +
//"Tile 2729:\n" +
//"...#.#.#.#\n" +
//"####.#....\n" +
//"..#.#.....\n" +
//"....#..#.#\n" +
//".##..##.#.\n" +
//".#.####...\n" +
//"####.#.#..\n" +
//"##.####...\n" +
//"##..#.##..\n" +
//"#.##...##.\n" +
//"\n" +
//"Tile 3079:\n" +
//"#.#.#####.\n" +
//".#..######\n" +
//"..#.......\n" +
//"######....\n" +
//"####.#..#.\n" +
//".#...#.##.\n" +
//"#.#####.##\n" +
//"..#.###...\n" +
//"..#.......\n" +
//"..#.###...";

//            Part2(test);
            

            logger.WriteLine("- Pt1 - "+Part1(input));  // 8581320593371
            //logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}