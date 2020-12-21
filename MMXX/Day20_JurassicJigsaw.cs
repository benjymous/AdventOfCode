using Advent.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
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
                ID = int.Parse(Title.Substring(5, 4));
                Grid = lines.Skip(1).ToArray();

                string left = "";
                string right = "";

                foreach (var line in Grid)
                {
                    left += line.First();
                    right += line.Last();
                }

                Edges = new string[]
                {
                    Grid.First(),
                    right,
                    Grid.Last().Reversed(),
                    left.Reversed(),
                };
            }

            public int ID;

            public int Orientation = 0;

            public int Borders = 0;

            public string Title;
            public string[] Grid;

            public string[] Edges;

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

                var top = Edges[(int)Edge.top];
                var right = Edges[(int)Edge.right];
                var bottom = Edges[(int)Edge.bottom];
                var left = Edges[(int)Edge.left];

                Edges[(int)Edge.top] = left;
                Edges[(int)Edge.right] = top;
                Edges[(int)Edge.bottom] = right;
                Edges[(int)Edge.left] = bottom;

                if (Orientation == 0 || Orientation == 4) // flip over
                {
                    for (var i = 0; i < 4; ++i)
                    {
                        Edges[i] = Edges[i].Reversed();
                    }
                    Edges = Edges.Reverse().ToArray();
                }

            }

            /*

            0    1    2    3    4    5    6    7

            0A1  3D0  2C3  1B2  1A0  2B1  3C2  0D3
            DXB  CXA  BXD  AXC  BXD  CXA  DXB  AXC
            3C2  2B1  1A0  0D3  2C3  3D0  0A1  1B2

            0A1  3D0  2C3  1B2  0D3  1A0  2B1  3C2
            DXB  CXA  BXD  AXC  AXC  BXD  CXA  DXB
            3C2  2B1  1A0  0D3  1B2  2C3  3D0  0A1

            */

            public char GetCellTransformed(int x, int y)
            {
                var h = Grid.Length - 1;
                var w = Grid[0].Length - 1;
                switch (Orientation)
                {
                    case 0:
                        return Grid[y][x];

                    case 1:
                        return Grid[w-x][y];

                    case 2:
                        return Grid[h-y][w-x];

                    case 3:
                        return Grid[x][h-y];

                    case 4:
                        return Grid[x][y];

                    case 5:
                        return Grid[y][w - x];

                    case 6:
                        return Grid[w - x][h - y];

                    case 7:
                        return Grid[h - y][x];
                }

                throw new Exception("Bad orientation");
            }

            public string Transformed()
            {
                StringBuilder sb = new StringBuilder();
                for (int y=0; y<Grid.Length; ++y)
                {
                    for (int x=0; x<Grid[0].Length; ++x)
                    {
                        sb.Append(GetCellTransformed(x, y));
                    }
                    sb.AppendLine();
                }
                return sb.ToString();
            }
        }

        public class JigsawSolver
        {
            public JigsawSolver(string input)
            {
                Tiles = Util.Parse<Tile>(input.Split("\n\n"));

                // Build map of all edges and which tiles they belong to
                Map = new Dictionary<string, List<Tile>>();
                foreach(var tile in Tiles)
                {
                    foreach(var edge in tile.Edges)
                    {
                        AddEdge(edge, tile);
                        AddEdge(edge.Reversed(), tile);
                    }
                }   

                // Find the tiles that exist on a border of the picture
                foreach(var tile in Tiles)
                {
                    tile.Borders = tile.Edges.Where(e => Map[e].Count == 1).Count();
                }
            }

            public List<Tile> Tiles;
            public Dictionary<string, List<Tile>> Map;

            void AddEdge(string edge, Tile tile)
            {
                if (!Map.ContainsKey(edge))
                {
                    Map[edge] = new List<Tile>();
                }

                Map[edge].Add(tile);
            }

            public Tile FindNeigbour(Tile nextTo, int position)
            {
                string edge = nextTo.Edges[position];
                string revEdge = edge.Reversed();

                var options = Map[edge];

                if (options.Count == 1)
                {
                    return null;
                }

                var neighbourTile = options.Where(x => x != nextTo).First();

                int pos2 = (position + 2) % 4;

                while (neighbourTile.Edges[pos2] != revEdge) // find orientation for next piece
                {
                    neighbourTile.Twizzle();
                }

                return neighbourTile;
            }

            public IEnumerable<Tile> Corners => Tiles.Where(t => t.Borders == 2);
        }

        private static HashSet<(int x, int y)> BuildNessieMap()
        {
            var nessie =
                "                  #\n" +
                "#    ##    ##    ###\n" +
                " #  #  #  #  #  #";

            var lines = nessie.Split("\n");

            var nessieMap = new HashSet<(int x, int y)>();
            for (var y = 0; y < lines.Length; ++y)
            {
                for (var x = 0; x < lines[y].Length; ++x)
                {
                    if (lines[y][x] == '#') nessieMap.Add((x, y));
                }
            }

            return nessieMap;
        }

        public static Int64 Part1(string input)
        {
            var solver = new JigsawSolver(input);

            return solver.Corners.Select(t => t.ID).Product();   
        }

        public static Int64 Part2(string input)
        {
            var solver = new JigsawSolver(input);

            var corners = solver.Corners.ToArray();

            int tryCorner = 0;

            while (true)
            {
                Tile corner = corners[tryCorner];

                // orient this corner so it'll fit in the top left corner
                while (!(solver.Map[corner.Edges[(int)Edge.top]].Count == 1 && 
                         solver.Map[corner.Edges[(int)Edge.left]].Count == 1))
                {
                    corner.Twizzle();
                }

                var solution = new Dictionary<(int x, int y), Tile>();

                (int x, int y) pos = (0, 0);

                solution[pos] = corner;

                var tile = corner;
                while (tile != null)
                {
                    tile = solver.FindNeigbour(tile, (int)Edge.right); // find right hand neighbour to last 
                    if (tile != null)
                    {
                        pos = (pos.x + 1, pos.y);
                        solution[pos] = tile;
                    }
                    else
                    {
                        // end of row, so loop back to beginning of next row
                        tile = solver.FindNeigbour(solution[(0, pos.y)], (int)Edge.bottom); // find lower neighbour
                        if (tile == null)
                        {
                            // no more results - this must be the last row
                            break;
                        }
                        pos = (0, pos.y + 1);
                        solution[pos] = tile;
                    }
                }


                // Assemble the final image
                var maxx = pos.x + 1;
                var maxy = pos.y + 1;
                char[,] bitmap = new char[maxx * 8, maxy * 8];

                for (var y = 0; y < maxy; ++y)
                {
                    for (var x = 0; x < maxx; ++x)
                    {
                        if (solution.TryGetValue((x, y), out Tile cell))
                        {
                            for (var ty = 0; ty < 8; ++ty)
                            {
                                for (var tx = 0; tx < 8; ++tx)
                                {
                                    bitmap[(x * 8) + tx, (y * 8) + ty] = cell.GetCellTransformed(tx + 1, ty + 1);
                                }
                            }
                        }
                        else
                        {
                            throw new Exception($"Missing {x},{y}");
                        }
                    }
                }

                // offsets we need to look for to check for monsters
                HashSet<(int x, int y)> nessieMap = BuildNessieMap();

                // do the monster scan
                int nessies = 0;
                for (var y = 0; (y < (maxy * 8) - 2); ++y)
                {
                    for (var x = 0; (x < (maxx * 8) - 19); ++x)
                    {
                        int nessieCount = 0;
                        foreach (var e in nessieMap)
                        {
                            if (bitmap[x + e.x, y + e.y] == '.') break;
                            nessieCount++;
                        }

                        if (nessieCount == nessieMap.Count)
                        {
                            foreach (var e in nessieMap)
                            {
                                bitmap[x + e.x, y + e.y] = 'O';
                                nessies++;
                            }
                        }
                    }
                }

                if (nessies > 0)
                {
                    // at least one nessie showed up in this orientation

                    //for (var y = 0; y < maxy * 8; ++y)
                    //{
                    //    //if (y % 8 == 0) Console.WriteLine();
                    //    for (var x = 0; x < maxx * 8; ++x)
                    //    {
                    //        //if (x % 8 == 0) Console.Write(" ");
                    //        Console.Write(bitmap[x, y]);
                    //    }
                    //    Console.WriteLine();
                    //}

                    //var bmp = new Bitmap(maxx * 8, maxy * 8);
                    
                    //for (var y = 0; y < maxy * 8; ++y)
                    //{
                    //    for (var x = 0; x < maxx * 8; ++x)
                    //    {
                    //        Color col = Color.MidnightBlue;
                    //        var c = bitmap[x, y];
                    //        if (c == '#') col = Color.MediumBlue;
                    //        if (c == 'O') col = Color.Chartreuse;

                    //        bmp.SetPixel(x, y, col);
                    //    }
                    //}

                    //bmp.Save(@"nessie.png");       

                    // return remaining waves
                    return bitmap.Values().Where(c => c == '#').Count();
                };
                tryCorner++;
            }
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - "+Part1(input)); 
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}